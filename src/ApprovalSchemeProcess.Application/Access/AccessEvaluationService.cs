using ApprovalSchemeProcess.Application.Sessions;
using ApprovalSchemeProcess.Domain.Entities;

namespace ApprovalSchemeProcess.Application.Access;

public sealed class AccessEvaluationService(
    ISessionContextService sessionContextService,
    IAccessEvaluationReader accessEvaluationReader) : IAccessEvaluationService
{
    public async Task<AccessEvaluationResult> EvaluateAsync(
        AccessEvaluationRequest request,
        CancellationToken cancellationToken = default)
    {
        var requester = await accessEvaluationReader.GetRequesterAsync(request.RequestedByUserId, cancellationToken);
        if (requester is null)
        {
            return Denied(
                request,
                AccessFailureReason.RequesterNotFound,
                SessionContextFailureReason.None);
        }

        if (!string.Equals(requester.Status, "active", StringComparison.OrdinalIgnoreCase))
        {
            return Denied(
                request,
                AccessFailureReason.RequesterInactive,
                SessionContextFailureReason.None);
        }

        if (!HasActiveEmployeeRole(requester, request.RequestedAtUtc))
        {
            return Denied(
                request,
                AccessFailureReason.RequesterNotAuthorized,
                SessionContextFailureReason.None);
        }

        var operationType = await accessEvaluationReader.GetOperationTypeAsync(request.OperationTypeId, cancellationToken);
        if (operationType is null || operationType.SecurityLevel is null)
        {
            return Denied(
                request,
                AccessFailureReason.OperationTypeNotFound,
                SessionContextFailureReason.None);
        }

        if (!operationType.IsActive)
        {
            return Denied(
                request,
                AccessFailureReason.OperationTypeInactive,
                SessionContextFailureReason.None,
                operationType);
        }

        var (classification, accessFailureReason, sessionFailureReason, appointmentId) =
            await ClassifyRequestAsync(request, cancellationToken);

        if (accessFailureReason is not AccessFailureReason.None)
        {
            return Denied(
                request,
                accessFailureReason,
                sessionFailureReason,
                operationType,
                appointmentId);
        }

        if (classification is AccessRequestClassification.InContext)
        {
            return new AccessEvaluationResult(
                AccessDecisionOutcome.Allowed,
                AccessRequestClassification.InContext,
                AccessFailureReason.None,
                SessionContextFailureReason.None,
                request.RequestedByUserId,
                operationType.Id,
                operationType.SecurityLevelId,
                operationType.SecurityLevel.LevelCode,
                false,
                request.SessionId,
                appointmentId);
        }

        var requiresApproval = operationType.SecurityLevel.RequiresApproval;
        return new AccessEvaluationResult(
            requiresApproval ? AccessDecisionOutcome.RequiresApproval : AccessDecisionOutcome.Allowed,
            AccessRequestClassification.OutOfContext,
            AccessFailureReason.None,
            sessionFailureReason,
            request.RequestedByUserId,
            operationType.Id,
            operationType.SecurityLevelId,
            operationType.SecurityLevel.LevelCode,
            requiresApproval,
            request.SessionId,
            appointmentId);
    }

    private async Task<(AccessRequestClassification Classification, AccessFailureReason FailureReason, SessionContextFailureReason SessionFailureReason, long? AppointmentId)>
        ClassifyRequestAsync(
            AccessEvaluationRequest request,
            CancellationToken cancellationToken)
    {
        if (!request.SessionId.HasValue)
        {
            return (AccessRequestClassification.OutOfContext, AccessFailureReason.None, SessionContextFailureReason.None, null);
        }

        var sessionEvaluation = await sessionContextService.IsInContextAsync(
            new SessionContextRequest(
                request.SessionId.Value,
                request.RequestedByUserId,
                request.OperationTypeId,
                request.TargetType,
                request.TargetIdentifier,
                request.RequestedAtUtc),
            cancellationToken);

        if (sessionEvaluation.IsInContext)
        {
            return (AccessRequestClassification.InContext, AccessFailureReason.None, SessionContextFailureReason.None, sessionEvaluation.AppointmentId);
        }

        return sessionEvaluation.FailureReason switch
        {
            SessionContextFailureReason.SessionMismatch or
            SessionContextFailureReason.UserNotAssigned or
            SessionContextFailureReason.OperationTypeMismatch or
            SessionContextFailureReason.TargetMismatch =>
                (AccessRequestClassification.OutOfContext, AccessFailureReason.None, sessionEvaluation.FailureReason, sessionEvaluation.AppointmentId),
            _ =>
                (AccessRequestClassification.OutOfContext, AccessFailureReason.InvalidSessionContext, sessionEvaluation.FailureReason, sessionEvaluation.AppointmentId)
        };
    }

    private static bool HasActiveEmployeeRole(User requester, DateTime requestedAtUtc) =>
        requester.UserRoles.Any(userRole =>
            userRole.IsActive
            && string.Equals(userRole.Role.Code, "EMPLOYEE", StringComparison.OrdinalIgnoreCase)
            && userRole.ValidFrom <= requestedAtUtc
            && (userRole.ValidTo is null || userRole.ValidTo >= requestedAtUtc));

    private static AccessEvaluationResult Denied(
        AccessEvaluationRequest request,
        AccessFailureReason failureReason,
        SessionContextFailureReason sessionFailureReason,
        OperationType? operationType = null,
        long? appointmentId = null) =>
        new(
            AccessDecisionOutcome.Denied,
            AccessRequestClassification.OutOfContext,
            failureReason,
            sessionFailureReason,
            request.RequestedByUserId,
            request.OperationTypeId,
            operationType?.SecurityLevelId ?? 0,
            operationType?.SecurityLevel?.LevelCode ?? -1,
            false,
            request.SessionId,
            appointmentId);
}
