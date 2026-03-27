using ApprovalSchemeProcess.Application.Sessions;

namespace ApprovalSchemeProcess.Application.Access;

public sealed record AccessEvaluationResult(
    AccessDecisionOutcome Outcome,
    AccessRequestClassification Classification,
    AccessFailureReason FailureReason,
    SessionContextFailureReason SessionFailureReason,
    long RequestedByUserId,
    long OperationTypeId,
    long SecurityLevelId,
    int SecurityLevelCode,
    bool RequiresApproval,
    long? SessionId,
    long? AppointmentId);
