using System.Text.Json;
using ApprovalSchemeProcess.Application.Access;
using ApprovalSchemeProcess.Application.Approval;
using ApprovalSchemeProcess.Domain.Entities;
using ApprovalEntity = ApprovalSchemeProcess.Domain.Entities.Approval;

namespace ApprovalSchemeProcess.Application.Logging;

public sealed class AccessRequestFlowService(
    IAccessEvaluationService accessEvaluationService,
    IApprovalWorkflowService approvalWorkflowService,
    IAccessRequestFlowStore accessRequestFlowStore) : IAccessRequestFlowService
{
    public async Task<AccessRequestSubmissionResult> SubmitAsync(
        AccessRequestSubmissionRequest request,
        CancellationToken cancellationToken = default)
    {
        var evaluation = await accessEvaluationService.EvaluateAsync(
            new AccessEvaluationRequest(
                request.SessionId,
                request.RequestedByUserId,
                request.OperationTypeId,
                request.TargetType,
                request.TargetIdentifier,
                request.Justification,
                request.IsEmergency,
                request.IsOverride,
                request.RequestedAtUtc),
            cancellationToken);

        Query? query = null;
        if (CanPersistQuery(evaluation))
        {
            query = BuildQuery(request, evaluation);
            await accessRequestFlowStore.CreateQueryAsync(query, cancellationToken);
        }

        ApprovalRequest? approvalRequest = null;
        ApprovalWorkflowState? workflow = null;

        if (evaluation.RequiresApproval && query is not null)
        {
            workflow = await approvalWorkflowService.StartAsync(
                new ApprovalWorkflowStartRequest(
                    query.Id,
                    query.OperationTypeId,
                    query.RequestedAt),
                cancellationToken);

            approvalRequest = new ApprovalRequest
            {
                QueryId = query.Id,
                ApprovalSchemeId = workflow.ApprovalSchemeId,
                Status = "pending",
                RequestedAt = request.RequestedAtUtc
            };

            await accessRequestFlowStore.CreateApprovalRequestAsync(approvalRequest, cancellationToken);
        }

        var auditLogs = BuildSubmissionAuditLogs(request, evaluation, query, approvalRequest, workflow);
        if (auditLogs.Count > 0)
        {
            await accessRequestFlowStore.AddAuditLogsAsync(auditLogs, cancellationToken);
        }

        return new AccessRequestSubmissionResult(
            query?.Id,
            evaluation,
            approvalRequest?.Id,
            workflow);
    }

    public async Task<ApprovalDecisionSubmissionResult> RecordApprovalDecisionAsync(
        ApprovalDecisionSubmissionRequest request,
        CancellationToken cancellationToken = default)
    {
        var aggregate = await accessRequestFlowStore.GetApprovalRequestAggregateAsync(request.ApprovalRequestId, cancellationToken)
            ?? throw new InvalidOperationException($"Approval request {request.ApprovalRequestId} could not be found.");

        var workflow = await approvalWorkflowService.StartAsync(
            new ApprovalWorkflowStartRequest(
                aggregate.Query.Id,
                aggregate.Query.OperationTypeId,
                aggregate.Query.RequestedAt),
            cancellationToken);

        foreach (var priorApproval in aggregate.Approvals.OrderBy(approval => approval.DecidedAt).ThenBy(approval => approval.Id))
        {
            workflow = approvalWorkflowService.ApplyDecision(
                workflow,
                new ApprovalStepDecisionRequest(
                    priorApproval.ApprovalSchemeStepId,
                    priorApproval.ApproverUserId,
                    ParseDecision(priorApproval.Decision),
                    priorApproval.Reason,
                    priorApproval.DecidedAt));
        }

        workflow = approvalWorkflowService.ApplyDecision(
            workflow,
            new ApprovalStepDecisionRequest(
                request.ApprovalSchemeStepId,
                request.ApproverUserId,
                request.Decision,
                request.Reason,
                request.DecidedAtUtc));

        await accessRequestFlowStore.AddApprovalAsync(
            new ApprovalEntity
            {
                ApprovalRequestId = request.ApprovalRequestId,
                ApprovalSchemeStepId = request.ApprovalSchemeStepId,
                ApproverUserId = request.ApproverUserId,
                Decision = ToPersistenceDecision(request.Decision),
                Reason = request.Reason,
                DecidedAt = request.DecidedAtUtc
            },
            cancellationToken);

        if (workflow.Status is not ApprovalWorkflowStatus.Pending)
        {
            aggregate.ApprovalRequest.Status = MapApprovalRequestStatus(workflow.Status);
            aggregate.ApprovalRequest.CompletedAt = request.DecidedAtUtc;
            await accessRequestFlowStore.UpdateApprovalRequestAsync(aggregate.ApprovalRequest, cancellationToken);

            aggregate.Query.Status = MapQueryStatus(workflow.Status);
            aggregate.Query.DecidedAt = request.DecidedAtUtc;
            await accessRequestFlowStore.UpdateQueryAsync(aggregate.Query, cancellationToken);
        }

        var auditLogs = BuildDecisionAuditLogs(request, aggregate.Query.Id, workflow);
        await accessRequestFlowStore.AddAuditLogsAsync(auditLogs, cancellationToken);

        return new ApprovalDecisionSubmissionResult(
            aggregate.Query.Id,
            request.ApprovalRequestId,
            workflow);
    }

    private static bool CanPersistQuery(AccessEvaluationResult evaluation) =>
        evaluation.OperationTypeId > 0 && evaluation.SecurityLevelId > 0;

    private static Query BuildQuery(
        AccessRequestSubmissionRequest request,
        AccessEvaluationResult evaluation)
    {
        var isAllowed = evaluation.Outcome is AccessDecisionOutcome.Allowed;
        var isDenied = evaluation.Outcome is AccessDecisionOutcome.Denied;

        return new Query
        {
            SessionId = request.SessionId,
            RequestedByUserId = request.RequestedByUserId,
            OperationTypeId = evaluation.OperationTypeId,
            SecurityLevelId = evaluation.SecurityLevelId,
            TargetType = request.TargetType,
            TargetIdentifier = request.TargetIdentifier,
            RequestClassification = MapClassification(evaluation.Classification),
            Justification = request.Justification,
            IsEmergency = request.IsEmergency,
            IsOverride = request.IsOverride,
            Status = evaluation.Outcome switch
            {
                AccessDecisionOutcome.RequiresApproval => "pending",
                AccessDecisionOutcome.Allowed => "executed",
                AccessDecisionOutcome.Denied => "denied",
                _ => throw new ArgumentOutOfRangeException(nameof(evaluation))
            },
            RequestedAt = request.RequestedAtUtc,
            DecidedAt = isAllowed || isDenied ? request.RequestedAtUtc : null,
            ExecutedAt = isAllowed ? request.RequestedAtUtc : null
        };
    }

    private static List<AuditLog> BuildSubmissionAuditLogs(
        AccessRequestSubmissionRequest request,
        AccessEvaluationResult evaluation,
        Query? query,
        ApprovalRequest? approvalRequest,
        ApprovalWorkflowState? workflow)
    {
        var logs = new List<AuditLog>
        {
            CreateAuditLog(
                "QUERY_REQUESTED",
                query is null ? "access_request" : "query",
                query?.Id ?? 0,
                request.RequestedByUserId,
                request.RequestedAtUtc,
                new AuditContext(request.IpAddress, request.DeviceIdentifier),
                new
                {
                    request.SessionId,
                    request.OperationTypeId,
                    request.TargetType,
                    request.TargetIdentifier,
                    evaluation.Outcome,
                    evaluation.Classification,
                    evaluation.FailureReason,
                    evaluation.SessionFailureReason
                })
        };

        if (approvalRequest is not null && workflow is not null)
        {
            logs.Add(CreateAuditLog(
                "APPROVAL_REQUEST_CREATED",
                "approval_request",
                approvalRequest.Id,
                request.RequestedByUserId,
                request.RequestedAtUtc,
                new AuditContext(request.IpAddress, request.DeviceIdentifier),
                new
                {
                    queryId = query!.Id,
                    workflow.ApprovalSchemeId,
                    workflow.CurrentStepOrder,
                    workflow.CurrentRoleCode
                }));

            return logs;
        }

        if (evaluation.Outcome is AccessDecisionOutcome.Allowed && query is not null)
        {
            logs.Add(CreateAuditLog(
                "QUERY_EXECUTED",
                "query",
                query.Id,
                request.RequestedByUserId,
                request.RequestedAtUtc,
                new AuditContext(request.IpAddress, request.DeviceIdentifier),
                new
                {
                    evaluation.Classification,
                    evaluation.SecurityLevelCode,
                    evaluation.RequiresApproval
                }));
        }
        else if (evaluation.Outcome is AccessDecisionOutcome.Denied)
        {
            logs.Add(CreateAuditLog(
                "QUERY_DENIED",
                query is null ? "access_request" : "query",
                query?.Id ?? 0,
                request.RequestedByUserId,
                request.RequestedAtUtc,
                new AuditContext(request.IpAddress, request.DeviceIdentifier),
                new
                {
                    evaluation.FailureReason,
                    evaluation.SessionFailureReason,
                    evaluation.Classification
                }));
        }

        return logs;
    }

    private static List<AuditLog> BuildDecisionAuditLogs(
        ApprovalDecisionSubmissionRequest request,
        long queryId,
        ApprovalWorkflowState workflow)
    {
        var logs = new List<AuditLog>
        {
            CreateAuditLog(
                "APPROVAL_DECISION_RECORDED",
                "approval_request",
                request.ApprovalRequestId,
                request.ApproverUserId,
                request.DecidedAtUtc,
                new AuditContext(request.IpAddress, request.DeviceIdentifier),
                new
                {
                    request.ApprovalSchemeStepId,
                    request.Decision,
                    request.Reason,
                    workflow.Status,
                    workflow.CurrentStepOrder,
                    workflow.CurrentRoleCode
                })
        };

        if (workflow.Status is ApprovalWorkflowStatus.Pending)
        {
            return logs;
        }

        logs.Add(CreateAuditLog(
            MapFinalQueryEvent(workflow.Status),
            "query",
            queryId,
            request.ApproverUserId,
            request.DecidedAtUtc,
            new AuditContext(request.IpAddress, request.DeviceIdentifier),
            new
            {
                workflow.Status,
                Steps = workflow.Steps.Select(step => new
                {
                    step.ApprovalSchemeStepId,
                    step.Status,
                    step.ApproverUserId,
                    step.DecidedAtUtc
                })
            }));

        return logs;
    }

    private static AuditLog CreateAuditLog(
        string eventType,
        string entityType,
        long entityId,
        long? actorUserId,
        DateTime occurredAt,
        AuditContext context,
        object details) =>
        new()
        {
            EventType = eventType,
            EntityType = entityType,
            EntityId = entityId,
            ActorUserId = actorUserId,
            OccurredAt = occurredAt,
            IpAddress = context.IpAddress,
            DeviceIdentifier = context.DeviceIdentifier,
            Details = JsonSerializer.Serialize(details)
        };

    private static string MapClassification(AccessRequestClassification classification) =>
        classification switch
        {
            AccessRequestClassification.InContext => "in_context",
            AccessRequestClassification.OutOfContext => "out_of_context",
            _ => throw new ArgumentOutOfRangeException(nameof(classification), classification, null)
        };

    private static ApprovalStepDecision ParseDecision(string decision) =>
        decision.ToLowerInvariant() switch
        {
            "approved" => ApprovalStepDecision.Approved,
            "denied" => ApprovalStepDecision.Denied,
            "expired" => ApprovalStepDecision.Expired,
            _ => throw new InvalidOperationException($"Unsupported approval decision '{decision}'.")
        };

    private static string ToPersistenceDecision(ApprovalStepDecision decision) =>
        decision switch
        {
            ApprovalStepDecision.Approved => "approved",
            ApprovalStepDecision.Denied => "denied",
            ApprovalStepDecision.Expired => "expired",
            _ => throw new ArgumentOutOfRangeException(nameof(decision), decision, null)
        };

    private static string MapApprovalRequestStatus(ApprovalWorkflowStatus status) =>
        status switch
        {
            ApprovalWorkflowStatus.Approved => "approved",
            ApprovalWorkflowStatus.Denied => "denied",
            ApprovalWorkflowStatus.Expired => "expired",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };

    private static string MapQueryStatus(ApprovalWorkflowStatus status) =>
        status switch
        {
            ApprovalWorkflowStatus.Approved => "approved",
            ApprovalWorkflowStatus.Denied => "denied",
            ApprovalWorkflowStatus.Expired => "expired",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };

    private static string MapFinalQueryEvent(ApprovalWorkflowStatus status) =>
        status switch
        {
            ApprovalWorkflowStatus.Approved => "QUERY_APPROVED",
            ApprovalWorkflowStatus.Denied => "QUERY_DENIED",
            ApprovalWorkflowStatus.Expired => "QUERY_EXPIRED",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
}
