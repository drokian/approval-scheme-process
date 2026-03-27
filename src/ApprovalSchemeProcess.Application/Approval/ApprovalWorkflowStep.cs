namespace ApprovalSchemeProcess.Application.Approval;

public sealed record ApprovalWorkflowStep(
    long ApprovalSchemeStepId,
    int StepOrder,
    string RoleCode,
    string ReviewType,
    bool IsMandatory,
    int? TimeoutMinutes,
    ApprovalStepStatus Status,
    long? ApproverUserId,
    string? Reason,
    DateTime? DecidedAtUtc);
