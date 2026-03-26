using ApprovalSchemeProcess.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Configuration.GetValue<bool>("Database:ApplyMigrationsOnStartup"))
{
    await app.Services.InitializeDatabaseAsync(
        app.Configuration,
        app.Environment.ContentRootPath,
        app.Lifetime.ApplicationStopping);
}

app.MapGet("/", () => Results.Ok(new
{
    service = "ApprovalSchemeProcess.Api",
    version = "s1-foundation",
    status = "running"
}));

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy"
}));

app.Run();

public partial class Program;
