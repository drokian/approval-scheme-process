namespace ApprovalSchemeProcess.Application.Approval;

public sealed record ApprovalWorkflowState(
    long QueryId,
    long ApprovalSchemeId,
    ApprovalWorkflowStatus Status,
    int? CurrentStepOrder,
    string? CurrentRoleCode,
    IReadOnlyList<ApprovalWorkflowStep> Steps);
