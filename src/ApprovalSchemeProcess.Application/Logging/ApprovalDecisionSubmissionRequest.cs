using ApprovalSchemeProcess.Application.Approval;

namespace ApprovalSchemeProcess.Application.Logging;

public sealed record ApprovalDecisionSubmissionRequest(
    long ApprovalRequestId,
    long ApprovalSchemeStepId,
    long ApproverUserId,
    ApprovalStepDecision Decision,
    string? Reason,
    DateTime DecidedAtUtc,
    string? IpAddress,
    string? DeviceIdentifier);
