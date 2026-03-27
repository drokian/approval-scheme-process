using ApprovalSchemeProcess.Domain.Entities;

namespace ApprovalSchemeProcess.Application.Sessions;

public interface ISessionContextSessionReader
{
    Task<Session?> GetByIdAsync(long sessionId, CancellationToken cancellationToken = default);
}

public interface ISessionContextService
{
    Task<SessionContextEvaluation> IsInContextAsync(
        SessionContextRequest request,
        CancellationToken cancellationToken = default);
}

public sealed class SessionContextEvaluator : ISessionContextEvaluator
{
    public SessionContextEvaluation IsInContext(Session? session, SessionContextRequest request)
    {
        if (session is null)
        {
            return SessionContextEvaluation.Denied(SessionContextFailureReason.SessionNotFound);
        }

        if (session.Id != request.SessionId)
        {
            return SessionContextEvaluation.Denied(
                SessionContextFailureReason.SessionMismatch,
                session.Id,
                session.AppointmentId);
        }

        if (IsInvalidated(session))
        {
            return SessionContextEvaluation.Denied(
                SessionContextFailureReason.SessionInvalidated,
                session.Id,
                session.AppointmentId);
        }

        if (IsClosed(session))
        {
            return SessionContextEvaluation.Denied(
                SessionContextFailureReason.SessionClosed,
                session.Id,
                session.AppointmentId);
        }

        if (IsExpired(session, request.RequestedAtUtc))
        {
            return SessionContextEvaluation.Denied(
                SessionContextFailureReason.SessionExpired,
                session.Id,
                session.AppointmentId);
        }

        if (!string.Equals(session.Status, "active", StringComparison.OrdinalIgnoreCase))
        {
            return SessionContextEvaluation.Denied(
                SessionContextFailureReason.SessionNotActive,
                session.Id,
                session.AppointmentId);
        }

        if (session.Appointment is null)
        {
            return SessionContextEvaluation.Denied(
                SessionContextFailureReason.AppointmentContextMissing,
                session.Id,
                session.AppointmentId);
        }

        if (!string.Equals(session.Appointment.Status, "active", StringComparison.OrdinalIgnoreCase))
        {
            return SessionContextEvaluation.Denied(
                SessionContextFailureReason.AppointmentNotActive,
                session.Id,
                session.AppointmentId);
        }

        if (session.Appointment.OperationTypeId != request.OperationTypeId)
        {
            return SessionContextEvaluation.Denied(
                SessionContextFailureReason.OperationTypeMismatch,
                session.Id,
                session.AppointmentId);
        }

        if (!IsAssignedToRequester(session, request.RequestedByUserId))
        {
            return SessionContextEvaluation.Denied(
                SessionContextFailureReason.UserNotAssigned,
                session.Id,
                session.AppointmentId);
        }

        if (!MatchesAppointmentTarget(session.Appointment.Targets, request.TargetType, request.TargetIdentifier))
        {
            return SessionContextEvaluation.Denied(
                SessionContextFailureReason.TargetMismatch,
                session.Id,
                session.AppointmentId);
        }

        return SessionContextEvaluation.Allowed(session.Id, session.AppointmentId);
    }

    private static bool IsExpired(Session session, DateTime requestedAtUtc) =>
        string.Equals(session.Status, "expired", StringComparison.OrdinalIgnoreCase)
        || session.ExpiresAt is DateTime expiresAt && expiresAt <= requestedAtUtc;

    private static bool IsClosed(Session session) =>
        string.Equals(session.Status, "closed", StringComparison.OrdinalIgnoreCase)
        || session.ClosedAt is not null;

    private static bool IsInvalidated(Session session) =>
        string.Equals(session.Status, "invalidated", StringComparison.OrdinalIgnoreCase)
        || session.InvalidatedAt is not null;

    private static bool IsAssignedToRequester(Session session, long requestedByUserId)
    {
        if (session.CurrentAssignedUserId.HasValue)
        {
            return session.CurrentAssignedUserId.Value == requestedByUserId;
        }

        return session.Assignments.Any(assignment =>
            assignment.UserId == requestedByUserId
            && assignment.IsCurrent
            && assignment.ReleasedAt is null);
    }

    private static bool MatchesAppointmentTarget(
        IEnumerable<AppointmentTarget> targets,
        string targetType,
        string targetIdentifier) =>
        targets.Any(target =>
            string.Equals(target.TargetType, targetType, StringComparison.OrdinalIgnoreCase)
            && string.Equals(target.TargetIdentifier, targetIdentifier, StringComparison.OrdinalIgnoreCase));
}

public sealed class SessionContextService(
    ISessionContextSessionReader sessionReader,
    ISessionContextEvaluator evaluator) : ISessionContextService
{
    public async Task<SessionContextEvaluation> IsInContextAsync(
        SessionContextRequest request,
        CancellationToken cancellationToken = default)
    {
        var session = await sessionReader.GetByIdAsync(request.SessionId, cancellationToken);
        return evaluator.IsInContext(session, request);
    }
}
