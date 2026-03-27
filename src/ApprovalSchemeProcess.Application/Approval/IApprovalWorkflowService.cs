namespace ApprovalSchemeProcess.Application.Approval;

public interface IApprovalWorkflowService
{
    Task<ApprovalWorkflowState> StartAsync(
        ApprovalWorkflowStartRequest request,
        CancellationToken cancellationToken = default);

    ApprovalWorkflowState ApplyDecision(
        ApprovalWorkflowState workflow,
        ApprovalStepDecisionRequest decision);
}
