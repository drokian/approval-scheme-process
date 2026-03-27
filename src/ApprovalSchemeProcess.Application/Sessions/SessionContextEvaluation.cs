namespace ApprovalSchemeProcess.Application.Sessions;

public sealed record SessionContextEvaluation(
    bool IsInContext,
    SessionContextFailureReason FailureReason,
    long? SessionId,
    long? AppointmentId)
{
    public static SessionContextEvaluation Allowed(long sessionId, long appointmentId) =>
        new(true, SessionContextFailureReason.None, sessionId, appointmentId);

    public static SessionContextEvaluation Denied(
        SessionContextFailureReason failureReason,
        long? sessionId = null,
        long? appointmentId = null) =>
        new(false, failureReason, sessionId, appointmentId);
}
