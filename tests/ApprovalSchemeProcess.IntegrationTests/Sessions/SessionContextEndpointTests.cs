using System.Net;
using System.Net.Http.Json;
using ApprovalSchemeProcess.Application.Sessions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ApprovalSchemeProcess.IntegrationTests.Sessions;

public class SessionContextEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public SessionContextEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<ISessionContextService>();
                services.AddSingleton<ISessionContextService>(new FakeSessionContextService());
            });
        });
    }

    [Fact]
    public async Task Evaluate_endpoint_returns_context_evaluation()
    {
        using var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/session-context/evaluate", new
        {
            sessionId = 100L,
            requestedByUserId = 42L,
            operationTypeId = 7L,
            targetType = "citizen",
            targetIdentifier = "CIT-1001",
            requestedAtUtc = "2026-03-27T10:15:00Z"
        });

        var payload = await response.Content.ReadFromJsonAsync<SessionContextEvaluationResponse>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(payload);
        Assert.True(payload.IsInContext);
        Assert.Equal("None", payload.FailureReason);
        Assert.Equal(100, payload.SessionId);
        Assert.Equal(200, payload.AppointmentId);
    }

    private sealed class FakeSessionContextService : ISessionContextService
    {
        public Task<SessionContextEvaluation> IsInContextAsync(
            SessionContextRequest request,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(SessionContextEvaluation.Allowed(request.SessionId, 200));
        }
    }
}
