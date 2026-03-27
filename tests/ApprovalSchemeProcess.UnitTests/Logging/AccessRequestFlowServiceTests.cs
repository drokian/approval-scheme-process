using ApprovalSchemeProcess.Application.Access;
using ApprovalSchemeProcess.Application.Approval;
using ApprovalSchemeProcess.Application.Logging;
using ApprovalSchemeProcess.Application.Sessions;
using ApprovalSchemeProcess.Domain.Entities;
using ApprovalEntity = ApprovalSchemeProcess.Domain.Entities.Approval;

namespace ApprovalSchemeProcess.UnitTests.Logging;

public class AccessRequestFlowServiceTests
{
    [Fact]
    public async Task SubmitAsync_creates_query_approval_request_and_audit_logs_when_approval_is_required()
    {
        var store = new FakeAccessRequestFlowStore();
        var service = new AccessRequestFlowService(
            new FakeAccessEvaluationService(new AccessEvaluationResult(
                AccessDecisionOutcome.RequiresApproval,
                AccessRequestClassification.OutOfContext,
                AccessFailureReason.None,
                SessionContextFailureReason.SessionMismatch,
                42,
                7,
                2,
                2,
                true,
                null,
                null)),
            new ApprovalWorkflowService(new FakeApprovalSchemeReader(CreateSingleStepScheme())),
            store);

        var result = await service.SubmitAsync(new AccessRequestSubmissionRequest(
            null,
            42,
            7,
            "citizen",
            "CIT-3003",
            "historical-check",
            false,
            false,
            new DateTime(2026, 3, 27, 10, 15, 0, DateTimeKind.Utc),
            "10.10.1.25",
            "WS-REG-01"));

        Assert.Equal(1, result.QueryId);
        Assert.Equal(1, result.ApprovalRequestId);
        Assert.NotNull(result.Workflow);
        Assert.Equal("pending", store.CreatedQueries.Single().Status);
        Assert.Equal("pending", store.CreatedApprovalRequests.Single().Status);
        Assert.Collection(
            store.AuditLogs,
            log => Assert.Equal("QUERY_REQUESTED", log.EventType),
            log => Assert.Equal("APPROVAL_REQUEST_CREATED", log.EventType));
    }

    [Fact]
    public async Task SubmitAsync_creates_executed_query_and_outcome_logs_when_access_is_allowed()
    {
        var store = new FakeAccessRequestFlowStore();
        var service = new AccessRequestFlowService(
            new FakeAccessEvaluationService(new AccessEvaluationResult(
                AccessDecisionOutcome.Allowed,
                AccessRequestClassification.InContext,
                AccessFailureReason.None,
                SessionContextFailureReason.None,
                42,
                7,
                2,
                2,
                false,
                100,
                200)),
            new ApprovalWorkflowService(new FakeApprovalSchemeReader(CreateSingleStepScheme())),
            store);

        var result = await service.SubmitAsync(new AccessRequestSubmissionRequest(
            100,
            42,
            7,
            "citizen",
            "CIT-1001",
            "appointment-lookup",
            false,
            false,
            new DateTime(2026, 3, 27, 10, 15, 0, DateTimeKind.Utc),
            "10.10.1.25",
            "WS-REG-01"));

        Assert.Equal(1, result.QueryId);
        Assert.Null(result.ApprovalRequestId);
        Assert.Null(result.Workflow);
        Assert.Equal("executed", store.CreatedQueries.Single().Status);
        Assert.Collection(
            store.AuditLogs,
            log => Assert.Equal("QUERY_REQUESTED", log.EventType),
            log => Assert.Equal("QUERY_EXECUTED", log.EventType));
    }

    [Fact]
    public async Task RecordApprovalDecisionAsync_updates_terminal_statuses_and_audit_logs()
    {
        var store = new FakeAccessRequestFlowStore
        {
            Aggregate = new ApprovalRequestAggregate(
                new ApprovalRequest
                {
                    Id = 90,
                    QueryId = 15,
                    ApprovalSchemeId = 12,
                    Status = "pending",
                    RequestedAt = new DateTime(2026, 3, 27, 10, 15, 0, DateTimeKind.Utc)
                },
                new Query
                {
                    Id = 15,
                    RequestedByUserId = 42,
                    OperationTypeId = 7,
                    SecurityLevelId = 2,
                    TargetType = "citizen",
                    TargetIdentifier = "CIT-3003",
                    RequestClassification = "out_of_context",
                    Status = "pending",
                    RequestedAt = new DateTime(2026, 3, 27, 10, 15, 0, DateTimeKind.Utc)
                },
                CreateSingleStepScheme(),
                [])
        };

        var service = new AccessRequestFlowService(
            new FakeAccessEvaluationService(new AccessEvaluationResult(
                AccessDecisionOutcome.RequiresApproval,
                AccessRequestClassification.OutOfContext,
                AccessFailureReason.None,
                SessionContextFailureReason.None,
                42,
                7,
                2,
                2,
                true,
                null,
                null)),
            new ApprovalWorkflowService(new FakeApprovalSchemeReader(CreateSingleStepScheme())),
            store);

        var result = await service.RecordApprovalDecisionAsync(new ApprovalDecisionSubmissionRequest(
            90,
            1001,
            101,
            ApprovalStepDecision.Approved,
            "business-approved",
            new DateTime(2026, 3, 27, 10, 20, 0, DateTimeKind.Utc),
            "10.10.1.25",
            "WS-REG-01"));

        Assert.Equal(15, result.QueryId);
        Assert.Equal(ApprovalWorkflowStatus.Approved, result.Workflow.Status);
        Assert.Equal("approved", store.UpdatedApprovalRequest!.Status);
        Assert.Equal("approved", store.UpdatedQuery!.Status);
        Assert.Collection(
            store.AuditLogs,
            log => Assert.Equal("APPROVAL_DECISION_RECORDED", log.EventType),
            log => Assert.Equal("QUERY_APPROVED", log.EventType));
    }

    private static ApprovalScheme CreateSingleStepScheme() =>
        new()
        {
            Id = 12,
            OperationTypeId = 7,
            Name = "Single Step Approval",
            VersionNo = 1,
            IsActive = true,
            Steps =
            [
                new ApprovalSchemeStep
                {
                    Id = 1001,
                    ApprovalSchemeId = 12,
                    StepOrder = 1,
                    RoleCode = "SUPERVISOR",
                    ReviewType = "approval",
                    IsMandatory = true,
                    TimeoutMinutes = 240
                }
            ]
        };

    private sealed class FakeAccessEvaluationService(AccessEvaluationResult result) : IAccessEvaluationService
    {
        public Task<AccessEvaluationResult> EvaluateAsync(
            AccessEvaluationRequest request,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(result);
    }

    private sealed class FakeApprovalSchemeReader(ApprovalScheme scheme) : IApprovalSchemeReader
    {
        public Task<ApprovalScheme?> GetActiveSchemeAsync(long operationTypeId, CancellationToken cancellationToken = default) =>
            Task.FromResult<ApprovalScheme?>(scheme);
    }

    private sealed class FakeAccessRequestFlowStore : IAccessRequestFlowStore
    {
        private long _nextQueryId = 1;
        private long _nextApprovalRequestId = 1;

        public List<Query> CreatedQueries { get; } = [];
        public List<ApprovalRequest> CreatedApprovalRequests { get; } = [];
        public List<ApprovalEntity> Approvals { get; } = [];
        public List<AuditLog> AuditLogs { get; } = [];
        public ApprovalRequestAggregate? Aggregate { get; set; }
        public Query? UpdatedQuery { get; private set; }
        public ApprovalRequest? UpdatedApprovalRequest { get; private set; }

        public Task CreateQueryAsync(Query query, CancellationToken cancellationToken = default)
        {
            query.Id = _nextQueryId++;
            CreatedQueries.Add(query);
            return Task.CompletedTask;
        }

        public Task UpdateQueryAsync(Query query, CancellationToken cancellationToken = default)
        {
            UpdatedQuery = query;
            return Task.CompletedTask;
        }

        public Task CreateApprovalRequestAsync(ApprovalRequest approvalRequest, CancellationToken cancellationToken = default)
        {
            approvalRequest.Id = _nextApprovalRequestId++;
            CreatedApprovalRequests.Add(approvalRequest);
            return Task.CompletedTask;
        }

        public Task UpdateApprovalRequestAsync(ApprovalRequest approvalRequest, CancellationToken cancellationToken = default)
        {
            UpdatedApprovalRequest = approvalRequest;
            return Task.CompletedTask;
        }

        public Task<ApprovalRequestAggregate?> GetApprovalRequestAggregateAsync(
            long approvalRequestId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(Aggregate);

        public Task AddApprovalAsync(ApprovalEntity approval, CancellationToken cancellationToken = default)
        {
            Approvals.Add(approval);
            return Task.CompletedTask;
        }

        public Task AddAuditLogsAsync(IReadOnlyCollection<AuditLog> auditLogs, CancellationToken cancellationToken = default)
        {
            AuditLogs.AddRange(auditLogs);
            return Task.CompletedTask;
        }
    }
}
