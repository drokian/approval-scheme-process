namespace ApprovalSchemeProcess.Application.Logging;

public sealed record AuditContext(
    string? IpAddress,
    string? DeviceIdentifier);
