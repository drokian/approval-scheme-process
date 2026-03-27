using System.Net;
using System.Net.Http.Json;
using ApprovalSchemeProcess.Application.Approval;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ApprovalSchemeProcess.IntegrationTests.Approval;

public class ApprovalWorkflowEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ApprovalWorkflowEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IApprovalWorkflowService>();
                services.AddSingleton<IApprovalWorkflowService>(new FakeApprovalWorkflowService());
            });
        });
    }

    [Fact]
    public async Task Start_endpoint_returns_pending_workflow()
    {
        using var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/approval/workflows/start", new ApprovalWorkflowStartRequest(
            15,
            7,
            new DateTime(2026, 3, 27, 10, 15, 0, DateTimeKind.Utc)));

        var payload = await response.Content.ReadFromJsonAsync<ApprovalWorkflowState>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(payload);
        Assert.Equal(ApprovalWorkflowStatus.Pending, payload.Status);
        Assert.Equal(1, payload.CurrentStepOrder);
        Assert.Equal("SUPERVISOR", payload.CurrentRoleCode);
    }

    [Fact]
    public async Task Decision_endpoint_returns_approved_workflow()
    {
        using var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/api/approval/workflows/decision",
            new ApprovalWorkflowDecisionEnvelope(
                new ApprovalWorkflowState(
                    15,
                    90,
                    ApprovalWorkflowStatus.Pending,
                    2,
                    "SECURITY",
                    new ApprovalWorkflowStep[]
                    {
                        new(1001, 1, "SUPERVISOR", "approval", true, 240, ApprovalStepStatus.Approved, 101, null, new DateTime(2026, 3, 27, 10, 20, 0, DateTimeKind.Utc)),
                        new(1002, 2, "SECURITY", "approval", true, 240, ApprovalStepStatus.Pending, null, null, null)
                    }),
                new ApprovalStepDecisionRequest(
                    1002,
                    102,
                    ApprovalStepDecision.Approved,
                    "security-approved",
                    new DateTime(2026, 3, 27, 10, 25, 0, DateTimeKind.Utc))));

        var payload = await response.Content.ReadFromJsonAsync<ApprovalWorkflowState>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(payload);
        Assert.Equal(ApprovalWorkflowStatus.Approved, payload.Status);
        Assert.Null(payload.CurrentStepOrder);
        Assert.Null(payload.CurrentRoleCode);
    }

    private sealed class FakeApprovalWorkflowService : IApprovalWorkflowService
    {
        public Task<ApprovalWorkflowState> StartAsync(ApprovalWorkflowStartRequest request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new ApprovalWorkflowState(
                request.QueryId,
                90,
                ApprovalWorkflowStatus.Pending,
                1,
                "SUPERVISOR",
                new[]
                {
                    new ApprovalWorkflowStep(1001, 1, "SUPERVISOR", "approval", true, 240, ApprovalStepStatus.Pending, null, null, null),
                    new ApprovalWorkflowStep(1002, 2, "SECURITY", "approval", true, 240, ApprovalStepStatus.Waiting, null, null, null)
                }));
        }

        public ApprovalWorkflowState ApplyDecision(ApprovalWorkflowState workflow, ApprovalStepDecisionRequest decision)
        {
            return workflow with
            {
                Status = ApprovalWorkflowStatus.Approved,
                CurrentStepOrder = null,
                CurrentRoleCode = null,
                Steps = workflow.Steps
                    .Select(step => step.ApprovalSchemeStepId == decision.ApprovalSchemeStepId
                        ? step with
                        {
                            Status = ApprovalStepStatus.Approved,
                            ApproverUserId = decision.ApproverUserId,
                            Reason = decision.Reason,
                            DecidedAtUtc = decision.DecidedAtUtc
                        }
                        : step)
                    .ToArray()
            };
        }
    }
}
