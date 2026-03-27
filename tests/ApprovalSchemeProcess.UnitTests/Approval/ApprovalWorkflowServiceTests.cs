using ApprovalSchemeProcess.Application.Approval;
using ApprovalSchemeProcess.Domain.Entities;

namespace ApprovalSchemeProcess.UnitTests.Approval;

public class ApprovalWorkflowServiceTests
{
    [Fact]
    public async Task StartAsync_loads_steps_in_order_and_marks_first_step_pending()
    {
        var service = new ApprovalWorkflowService(new FakeApprovalSchemeReader(CreateLevel2Scheme()));

        var workflow = await service.StartAsync(new ApprovalWorkflowStartRequest(15, 7, new DateTime(2026, 3, 27, 10, 15, 0, DateTimeKind.Utc)));

        Assert.Equal(ApprovalWorkflowStatus.Pending, workflow.Status);
        Assert.Equal(1, workflow.CurrentStepOrder);
        Assert.Equal("SUPERVISOR", workflow.CurrentRoleCode);
        Assert.Collection(workflow.Steps,
            step => Assert.Equal(ApprovalStepStatus.Pending, step.Status),
            step => Assert.Equal(ApprovalStepStatus.Waiting, step.Status));
    }

    [Fact]
    public async Task ApplyDecision_moves_to_security_step_after_supervisor_approval()
    {
        var service = new ApprovalWorkflowService(new FakeApprovalSchemeReader(CreateLevel2Scheme()));
        var workflow = await service.StartAsync(new ApprovalWorkflowStartRequest(15, 7, new DateTime(2026, 3, 27, 10, 15, 0, DateTimeKind.Utc)));

        var updated = service.ApplyDecision(
            workflow,
            new ApprovalStepDecisionRequest(
                workflow.Steps[0].ApprovalSchemeStepId,
                101,
                ApprovalStepDecision.Approved,
                "business-approved",
                new DateTime(2026, 3, 27, 10, 20, 0, DateTimeKind.Utc)));

        Assert.Equal(ApprovalWorkflowStatus.Pending, updated.Status);
        Assert.Equal(2, updated.CurrentStepOrder);
        Assert.Equal("SECURITY", updated.CurrentRoleCode);
        Assert.Equal(ApprovalStepStatus.Approved, updated.Steps[0].Status);
        Assert.Equal(ApprovalStepStatus.Pending, updated.Steps[1].Status);
    }

    [Fact]
    public async Task ApplyDecision_marks_workflow_approved_when_last_step_is_approved()
    {
        var service = new ApprovalWorkflowService(new FakeApprovalSchemeReader(CreateLevel2Scheme()));
        var workflow = await service.StartAsync(new ApprovalWorkflowStartRequest(15, 7, new DateTime(2026, 3, 27, 10, 15, 0, DateTimeKind.Utc)));
        var afterSupervisor = service.ApplyDecision(
            workflow,
            new ApprovalStepDecisionRequest(
                workflow.Steps[0].ApprovalSchemeStepId,
                101,
                ApprovalStepDecision.Approved,
                null,
                new DateTime(2026, 3, 27, 10, 20, 0, DateTimeKind.Utc)));

        var approved = service.ApplyDecision(
            afterSupervisor,
            new ApprovalStepDecisionRequest(
                afterSupervisor.Steps[1].ApprovalSchemeStepId,
                102,
                ApprovalStepDecision.Approved,
                null,
                new DateTime(2026, 3, 27, 10, 25, 0, DateTimeKind.Utc)));

        Assert.Equal(ApprovalWorkflowStatus.Approved, approved.Status);
        Assert.Null(approved.CurrentStepOrder);
        Assert.Null(approved.CurrentRoleCode);
        Assert.All(approved.Steps, step => Assert.Equal(ApprovalStepStatus.Approved, step.Status));
    }

    [Fact]
    public async Task ApplyDecision_marks_workflow_denied_when_mandatory_step_is_denied()
    {
        var service = new ApprovalWorkflowService(new FakeApprovalSchemeReader(CreateLevel2Scheme()));
        var workflow = await service.StartAsync(new ApprovalWorkflowStartRequest(15, 7, new DateTime(2026, 3, 27, 10, 15, 0, DateTimeKind.Utc)));

        var denied = service.ApplyDecision(
            workflow,
            new ApprovalStepDecisionRequest(
                workflow.Steps[0].ApprovalSchemeStepId,
                101,
                ApprovalStepDecision.Denied,
                "insufficient-business-need",
                new DateTime(2026, 3, 27, 10, 20, 0, DateTimeKind.Utc)));

        Assert.Equal(ApprovalWorkflowStatus.Denied, denied.Status);
        Assert.Null(denied.CurrentStepOrder);
        Assert.Equal(ApprovalStepStatus.Denied, denied.Steps[0].Status);
        Assert.Equal(ApprovalStepStatus.Waiting, denied.Steps[1].Status);
    }

    [Fact]
    public async Task ApplyDecision_marks_workflow_expired_when_pending_step_expires()
    {
        var service = new ApprovalWorkflowService(new FakeApprovalSchemeReader(CreateLevel2Scheme()));
        var workflow = await service.StartAsync(new ApprovalWorkflowStartRequest(15, 7, new DateTime(2026, 3, 27, 10, 15, 0, DateTimeKind.Utc)));

        var expired = service.ApplyDecision(
            workflow,
            new ApprovalStepDecisionRequest(
                workflow.Steps[0].ApprovalSchemeStepId,
                101,
                ApprovalStepDecision.Expired,
                "timeout",
                new DateTime(2026, 3, 27, 14, 20, 0, DateTimeKind.Utc)));

        Assert.Equal(ApprovalWorkflowStatus.Expired, expired.Status);
        Assert.Null(expired.CurrentRoleCode);
        Assert.Equal(ApprovalStepStatus.Expired, expired.Steps[0].Status);
    }

    [Fact]
    public async Task StartAsync_throws_when_scheme_has_no_steps()
    {
        var service = new ApprovalWorkflowService(new FakeApprovalSchemeReader(new ApprovalScheme
        {
            Id = 91,
            OperationTypeId = 7,
            Name = "Broken Approval Scheme",
            VersionNo = 1,
            IsActive = true
        }));

        var action = () => service.StartAsync(new ApprovalWorkflowStartRequest(15, 7, new DateTime(2026, 3, 27, 10, 15, 0, DateTimeKind.Utc)));

        await Assert.ThrowsAsync<InvalidOperationException>(action);
    }

    [Fact]
    public async Task ApplyDecision_continues_when_optional_step_is_denied()
    {
        var service = new ApprovalWorkflowService(new FakeApprovalSchemeReader(CreateOptionalFirstStepScheme()));
        var workflow = await service.StartAsync(new ApprovalWorkflowStartRequest(15, 7, new DateTime(2026, 3, 27, 10, 15, 0, DateTimeKind.Utc)));

        var updated = service.ApplyDecision(
            workflow,
            new ApprovalStepDecisionRequest(
                workflow.Steps[0].ApprovalSchemeStepId,
                101,
                ApprovalStepDecision.Denied,
                "optional-review-rejected",
                new DateTime(2026, 3, 27, 10, 20, 0, DateTimeKind.Utc)));

        Assert.Equal(ApprovalWorkflowStatus.Pending, updated.Status);
        Assert.Equal(2, updated.CurrentStepOrder);
        Assert.Equal("SECURITY", updated.CurrentRoleCode);
        Assert.Equal(ApprovalStepStatus.Denied, updated.Steps[0].Status);
        Assert.Equal(ApprovalStepStatus.Pending, updated.Steps[1].Status);
    }

    private static ApprovalScheme CreateLevel2Scheme() =>
        new()
        {
            Id = 90,
            OperationTypeId = 7,
            Name = "Land Sale Standard Approval",
            VersionNo = 1,
            IsActive = true,
            Steps =
            [
                new ApprovalSchemeStep
                {
                    Id = 1001,
                    ApprovalSchemeId = 90,
                    StepOrder = 1,
                    RoleCode = "SUPERVISOR",
                    ReviewType = "approval",
                    IsMandatory = true,
                    TimeoutMinutes = 240
                },
                new ApprovalSchemeStep
                {
                    Id = 1002,
                    ApprovalSchemeId = 90,
                    StepOrder = 2,
                    RoleCode = "SECURITY",
                    ReviewType = "approval",
                    IsMandatory = true,
                    TimeoutMinutes = 240
                }
            ]
        };

    private static ApprovalScheme CreateOptionalFirstStepScheme() =>
        new()
        {
            Id = 92,
            OperationTypeId = 7,
            Name = "Optional Supervisor Review",
            VersionNo = 1,
            IsActive = true,
            Steps =
            [
                new ApprovalSchemeStep
                {
                    Id = 1001,
                    ApprovalSchemeId = 92,
                    StepOrder = 1,
                    RoleCode = "SUPERVISOR",
                    ReviewType = "approval",
                    IsMandatory = false,
                    TimeoutMinutes = 240
                },
                new ApprovalSchemeStep
                {
                    Id = 1002,
                    ApprovalSchemeId = 92,
                    StepOrder = 2,
                    RoleCode = "SECURITY",
                    ReviewType = "approval",
                    IsMandatory = true,
                    TimeoutMinutes = 240
                }
            ]
        };

    private sealed class FakeApprovalSchemeReader(ApprovalScheme scheme) : IApprovalSchemeReader
    {
        public Task<ApprovalScheme?> GetActiveSchemeAsync(long operationTypeId, CancellationToken cancellationToken = default) =>
            Task.FromResult<ApprovalScheme?>(scheme);
    }
}
