namespace ApprovalSchemeProcess.Application.Logging;

public sealed record AccessRequestSubmissionRequest(
    long? SessionId,
    long RequestedByUserId,
    long OperationTypeId,
    string TargetType,
    string TargetIdentifier,
    string? Justification,
    bool IsEmergency,
    bool IsOverride,
    DateTime RequestedAtUtc,
    string? IpAddress,
    string? DeviceIdentifier);
