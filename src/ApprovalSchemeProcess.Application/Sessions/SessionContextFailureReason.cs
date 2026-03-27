namespace ApprovalSchemeProcess.Application.Sessions;

public enum SessionContextFailureReason
{
    None = 0,
    SessionNotFound = 1,
    SessionMismatch = 2,
    SessionNotActive = 3,
    SessionExpired = 4,
    SessionClosed = 5,
    SessionInvalidated = 6,
    AppointmentContextMissing = 7,
    AppointmentNotActive = 8,
    UserNotAssigned = 9,
    OperationTypeMismatch = 10,
    TargetMismatch = 11
}
