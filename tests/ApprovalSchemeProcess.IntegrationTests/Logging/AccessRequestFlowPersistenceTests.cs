using ApprovalSchemeProcess.Application.Access;
using ApprovalSchemeProcess.Application.Approval;
using ApprovalSchemeProcess.Application.Logging;
using ApprovalSchemeProcess.Application.Sessions;
using ApprovalSchemeProcess.Domain.Entities;
using ApprovalSchemeProcess.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSchemeProcess.IntegrationTests.Logging;

public class AccessRequestFlowPersistenceTests
{
    [Fact]
    public async Task Approval_flow_persists_query_approval_and_audit_records()
    {
        var databaseName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<ApprovalSchemeProcessDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        await using (var setupContext = new ApprovalSchemeProcessDbContext(options))
        {
            setupContext.ApprovalSchemes.Add(new ApprovalScheme
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
            });
            await setupContext.SaveChangesAsync();
        }

        AccessRequestSubmissionResult submission;
        await using (var submitContext = new ApprovalSchemeProcessDbContext(options))
        {
            var submitService = new AccessRequestFlowService(
                new FakeAccessEvaluationService(),
                new ApprovalWorkflowService(new ApprovalSchemeReader(submitContext)),
                new AccessRequestFlowStore(submitContext));

            submission = await submitService.SubmitAsync(new AccessRequestSubmissionRequest(
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
        }

        Assert.NotNull(submission.QueryId);
        Assert.NotNull(submission.ApprovalRequestId);

        ApprovalDecisionSubmissionResult decision;
        await using (var decisionContext = new ApprovalSchemeProcessDbContext(options))
        {
            var decisionService = new AccessRequestFlowService(
                new FakeAccessEvaluationService(),
                new ApprovalWorkflowService(new ApprovalSchemeReader(decisionContext)),
                new AccessRequestFlowStore(decisionContext));

            decision = await decisionService.RecordApprovalDecisionAsync(new ApprovalDecisionSubmissionRequest(
                submission.ApprovalRequestId!.Value,
                1001,
                101,
                ApprovalStepDecision.Approved,
                "business-approved",
                new DateTime(2026, 3, 27, 10, 20, 0, DateTimeKind.Utc),
                "10.10.1.25",
                "WS-REG-01"));
        }

        await using var assertContext = new ApprovalSchemeProcessDbContext(options);
        var persistedQuery = await assertContext.Queries.SingleAsync();
        var persistedApprovalRequest = await assertContext.ApprovalRequests.SingleAsync();
        var persistedApproval = await assertContext.Approvals.SingleAsync();
        var auditEvents = await assertContext.AuditLogs
            .OrderBy(log => log.Id)
            .Select(log => log.EventType)
            .ToListAsync();

        Assert.Equal(ApprovalWorkflowStatus.Approved, decision.Workflow.Status);
        Assert.Equal("approved", persistedQuery.Status);
        Assert.Equal("approved", persistedApprovalRequest.Status);
        Assert.Equal("approved", persistedApproval.Decision);
        Assert.Equal(
            ["QUERY_REQUESTED", "APPROVAL_REQUEST_CREATED", "APPROVAL_DECISION_RECORDED", "QUERY_APPROVED"],
            auditEvents);
    }

    private sealed class FakeAccessEvaluationService : IAccessEvaluationService
    {
        public Task<AccessEvaluationResult> EvaluateAsync(
            AccessEvaluationRequest request,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(new AccessEvaluationResult(
                AccessDecisionOutcome.RequiresApproval,
                AccessRequestClassification.OutOfContext,
                AccessFailureReason.None,
                SessionContextFailureReason.SessionMismatch,
                request.RequestedByUserId,
                request.OperationTypeId,
                2,
                2,
                true,
                request.SessionId,
                null));
    }
}
