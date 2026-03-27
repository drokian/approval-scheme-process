namespace ApprovalSchemeProcess.Application.Access;

public sealed record AccessEvaluationRequest(
    long? SessionId,
    long RequestedByUserId,
    long OperationTypeId,
    string TargetType,
    string TargetIdentifier,
    string? Justification,
    bool IsEmergency,
    bool IsOverride,
    DateTime RequestedAtUtc);
