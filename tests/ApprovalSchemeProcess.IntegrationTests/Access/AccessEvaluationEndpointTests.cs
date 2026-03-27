using System.Net;
using System.Net.Http.Json;
using ApprovalSchemeProcess.Application.Access;
using ApprovalSchemeProcess.Application.Sessions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ApprovalSchemeProcess.IntegrationTests.Access;

public class AccessEvaluationEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AccessEvaluationEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IAccessEvaluationService>();
                services.AddSingleton<IAccessEvaluationService>(new FakeAccessEvaluationService());
            });
        });
    }

    [Fact]
    public async Task Evaluate_endpoint_returns_access_decision()
    {
        using var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/access/evaluate", new
        {
            sessionId = 100L,
            requestedByUserId = 42L,
            operationTypeId = 7L,
            targetType = "citizen",
            targetIdentifier = "CIT-1001",
            justification = "supporting request",
            isEmergency = false,
            isOverride = false,
            requestedAtUtc = "2026-03-27T10:15:00Z"
        });

        var payload = await response.Content.ReadFromJsonAsync<AccessEvaluationResult>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(payload);
        Assert.Equal(AccessDecisionOutcome.RequiresApproval, payload.Outcome);
        Assert.Equal(AccessRequestClassification.OutOfContext, payload.Classification);
        Assert.True(payload.RequiresApproval);
        Assert.Equal(2, payload.SecurityLevelCode);
    }

    private sealed class FakeAccessEvaluationService : IAccessEvaluationService
    {
        public Task<AccessEvaluationResult> EvaluateAsync(
            AccessEvaluationRequest request,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new AccessEvaluationResult(
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
                200));
        }
    }
}
