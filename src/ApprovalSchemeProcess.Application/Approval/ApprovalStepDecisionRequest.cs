namespace ApprovalSchemeProcess.Application.Approval;

public sealed record ApprovalStepDecisionRequest(
    long ApprovalSchemeStepId,
    long ApproverUserId,
    ApprovalStepDecision Decision,
    string? Reason,
    DateTime DecidedAtUtc);
