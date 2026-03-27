using ApprovalSchemeProcess.Domain.Entities;

namespace ApprovalSchemeProcess.Application.Approval;

public sealed class ApprovalWorkflowService(IApprovalSchemeReader approvalSchemeReader) : IApprovalWorkflowService
{
    public async Task<ApprovalWorkflowState> StartAsync(
        ApprovalWorkflowStartRequest request,
        CancellationToken cancellationToken = default)
    {
        var scheme = await approvalSchemeReader.GetActiveSchemeAsync(request.OperationTypeId, cancellationToken)
            ?? throw new InvalidOperationException($"Active approval scheme could not be found for operation type {request.OperationTypeId}.");

        var orderedSteps = scheme.Steps
            .OrderBy(step => step.StepOrder)
            .Select((step, index) => new ApprovalWorkflowStep(
                step.Id,
                step.StepOrder,
                step.RoleCode,
                step.ReviewType,
                step.IsMandatory,
                step.TimeoutMinutes,
                index == 0 ? ApprovalStepStatus.Pending : ApprovalStepStatus.Waiting,
                null,
                null,
                null))
            .ToArray();

        var firstStep = orderedSteps.FirstOrDefault()
            ?? throw new InvalidOperationException($"Active approval scheme {scheme.Id} does not define any approval steps.");

        return new ApprovalWorkflowState(
            request.QueryId,
            scheme.Id,
            ApprovalWorkflowStatus.Pending,
            firstStep.StepOrder,
            firstStep.RoleCode,
            orderedSteps);
    }

    public ApprovalWorkflowState ApplyDecision(
        ApprovalWorkflowState workflow,
        ApprovalStepDecisionRequest decision)
    {
        if (workflow.Status is not ApprovalWorkflowStatus.Pending)
        {
            throw new InvalidOperationException("Approval workflow is already completed.");
        }

        var currentStep = workflow.Steps.SingleOrDefault(step => step.ApprovalSchemeStepId == decision.ApprovalSchemeStepId)
            ?? throw new InvalidOperationException($"Approval step {decision.ApprovalSchemeStepId} could not be found.");

        if (currentStep.Status is not ApprovalStepStatus.Pending)
        {
            throw new InvalidOperationException($"Approval step {decision.ApprovalSchemeStepId} is not awaiting a decision.");
        }

        var updatedSteps = workflow.Steps
            .Select(step => step.ApprovalSchemeStepId == decision.ApprovalSchemeStepId
                ? step with
                {
                    Status = MapDecision(decision.Decision),
                    ApproverUserId = decision.ApproverUserId,
                    Reason = decision.Reason,
                    DecidedAtUtc = decision.DecidedAtUtc
                }
                : step)
            .ToArray();

        if (decision.Decision is ApprovalStepDecision.Denied && currentStep.IsMandatory)
        {
            return workflow with
            {
                Status = ApprovalWorkflowStatus.Denied,
                CurrentStepOrder = null,
                CurrentRoleCode = null,
                Steps = updatedSteps
            };
        }

        if (decision.Decision is ApprovalStepDecision.Expired && currentStep.IsMandatory)
        {
            return workflow with
            {
                Status = ApprovalWorkflowStatus.Expired,
                CurrentStepOrder = null,
                CurrentRoleCode = null,
                Steps = updatedSteps
            };
        }

        var nextPendingIndex = Array.FindIndex(updatedSteps, step => step.Status == ApprovalStepStatus.Waiting);
        if (nextPendingIndex < 0)
        {
            return workflow with
            {
                Status = ApprovalWorkflowStatus.Approved,
                CurrentStepOrder = null,
                CurrentRoleCode = null,
                Steps = updatedSteps
            };
        }

        updatedSteps[nextPendingIndex] = updatedSteps[nextPendingIndex] with
        {
            Status = ApprovalStepStatus.Pending
        };

        return workflow with
        {
            Status = ApprovalWorkflowStatus.Pending,
            CurrentStepOrder = updatedSteps[nextPendingIndex].StepOrder,
            CurrentRoleCode = updatedSteps[nextPendingIndex].RoleCode,
            Steps = updatedSteps
        };
    }

    private static ApprovalStepStatus MapDecision(ApprovalStepDecision decision) =>
        decision switch
        {
            ApprovalStepDecision.Approved => ApprovalStepStatus.Approved,
            ApprovalStepDecision.Denied => ApprovalStepStatus.Denied,
            ApprovalStepDecision.Expired => ApprovalStepStatus.Expired,
            _ => throw new ArgumentOutOfRangeException(nameof(decision), decision, null)
        };
}
