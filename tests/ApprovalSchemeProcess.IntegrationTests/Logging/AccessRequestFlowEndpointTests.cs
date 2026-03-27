using System.Net;
using System.Net.Http.Json;
using ApprovalSchemeProcess.Application.Access;
using ApprovalSchemeProcess.Application.Approval;
using ApprovalSchemeProcess.Application.Logging;
using ApprovalSchemeProcess.Application.Sessions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ApprovalSchemeProcess.IntegrationTests.Logging;

public class AccessRequestFlowEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AccessRequestFlowEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IAccessRequestFlowService>();
                services.AddSingleton<IAccessRequestFlowService>(new FakeAccessRequestFlowService());
            });
        });
    }

    [Fact]
    public async Task Submit_endpoint_returns_composed_access_request_result()
    {
        using var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/access/requests", new AccessRequestSubmissionRequest(
            null,
            42,
            7,
            "citizen",
            "CIT-3003",
            "historical-check",
            false,
            false,
            new DateTime(2026, 3, 27, 10, 15, 0, DateTimeKind.Utc),
            "10.10.1.25",
            "WS-REG-01"));

        var payload = await response.Content.ReadFromJsonAsync<AccessRequestSubmissionResult>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(payload);
        Assert.Equal(15, payload.QueryId);
        Assert.Equal(90, payload.ApprovalRequestId);
        Assert.NotNull(payload.Workflow);
        Assert.Equal(AccessDecisionOutcome.RequiresApproval, payload.Evaluation.Outcome);
        Assert.Equal(ApprovalWorkflowStatus.Pending, payload.Workflow.Status);
    }

    [Fact]
    public async Task Approval_decision_endpoint_returns_terminal_workflow_result()
    {
        using var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/access/requests/approval-decision", new ApprovalDecisionSubmissionRequest(
            90,
            1002,
            102,
            ApprovalStepDecision.Approved,
            "security-approved",
            new DateTime(2026, 3, 27, 10, 25, 0, DateTimeKind.Utc),
            "10.10.1.25",
            "WS-REG-01"));

        var payload = await response.Content.ReadFromJsonAsync<ApprovalDecisionSubmissionResult>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(payload);
        Assert.Equal(15, payload.QueryId);
        Assert.Equal(90, payload.ApprovalRequestId);
        Assert.Equal(ApprovalWorkflowStatus.Approved, payload.Workflow.Status);
        Assert.Null(payload.Workflow.CurrentStepOrder);
        Assert.Null(payload.Workflow.CurrentRoleCode);
    }

    private sealed class FakeAccessRequestFlowService : IAccessRequestFlowService
    {
        public Task<AccessRequestSubmissionResult> SubmitAsync(
            AccessRequestSubmissionRequest request,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new AccessRequestSubmissionResult(
                15,
                new AccessEvaluationResult(
                    AccessDecisionOutcome.RequiresApproval,
                    AccessRequestClassification.OutOfContext,
                    AccessFailureReason.None,
                    SessionContextFailureReason.TargetMismatch,
                    request.RequestedByUserId,
                    request.OperationTypeId,
                    5,
                    2,
                    true,
                    request.SessionId,
                    200),
                90,
                new ApprovalWorkflowState(
                    15,
                    90,
                    ApprovalWorkflowStatus.Pending,
                    2,
                    "SECURITY",
                    new ApprovalWorkflowStep[]
                    {
                        new(1001, 1, "SUPERVISOR", "approval", true, 240, ApprovalStepStatus.Approved, 101, "business-approved", new DateTime(2026, 3, 27, 10, 20, 0, DateTimeKind.Utc)),
                        new(1002, 2, "SECURITY", "approval", true, 240, ApprovalStepStatus.Pending, null, null, null)
                    })));
        }

        public Task<ApprovalDecisionSubmissionResult> RecordApprovalDecisionAsync(
            ApprovalDecisionSubmissionRequest request,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new ApprovalDecisionSubmissionResult(
                15,
                request.ApprovalRequestId,
                new ApprovalWorkflowState(
                    15,
                    90,
                    ApprovalWorkflowStatus.Approved,
                    null,
                    null,
                    new ApprovalWorkflowStep[]
                    {
                        new(1001, 1, "SUPERVISOR", "approval", true, 240, ApprovalStepStatus.Approved, 101, "business-approved", new DateTime(2026, 3, 27, 10, 20, 0, DateTimeKind.Utc)),
                        new(1002, 2, "SECURITY", "approval", true, 240, ApprovalStepStatus.Approved, request.ApproverUserId, request.Reason, request.DecidedAtUtc)
                    })));
        }
    }
}
