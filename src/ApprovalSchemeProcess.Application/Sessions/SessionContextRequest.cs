namespace ApprovalSchemeProcess.Application.Sessions;

public sealed record SessionContextRequest(
    long SessionId,
    long RequestedByUserId,
    long OperationTypeId,
    string TargetType,
    string TargetIdentifier,
    DateTime RequestedAtUtc);
