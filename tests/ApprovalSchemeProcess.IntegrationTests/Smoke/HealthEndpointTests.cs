using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ApprovalSchemeProcess.IntegrationTests.Smoke;

public class HealthEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HealthEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(_ => { });
    }

    [Fact]
    public async Task Health_endpoint_returns_ok()
    {
        using var client = _factory.CreateClient();

        var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
