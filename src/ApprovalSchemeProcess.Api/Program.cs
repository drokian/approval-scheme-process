using ApprovalSchemeProcess.Application.Access;
using ApprovalSchemeProcess.Application.Approval;
using ApprovalSchemeProcess.Application.Sessions;
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
    version = "s4-approval-engine",
    status = "running"
}));

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy"
}));

app.MapPost("/api/session-context/evaluate", async (
    SessionContextRequest request,
    ISessionContextService sessionContextService,
    CancellationToken cancellationToken) =>
{
    var evaluation = await sessionContextService.IsInContextAsync(request, cancellationToken);

    return Results.Ok(new SessionContextEvaluationResponse(
        evaluation.IsInContext,
        evaluation.FailureReason.ToString(),
        evaluation.SessionId,
        evaluation.AppointmentId));
});

app.MapPost("/api/access/evaluate", async (
    AccessEvaluationRequest request,
    IAccessEvaluationService accessEvaluationService,
    CancellationToken cancellationToken) =>
{
    var evaluation = await accessEvaluationService.EvaluateAsync(request, cancellationToken);
    return Results.Ok(evaluation);
});

app.MapPost("/api/approval/workflows/start", async (
    ApprovalWorkflowStartRequest request,
    IApprovalWorkflowService approvalWorkflowService,
    CancellationToken cancellationToken) =>
{
    var workflow = await approvalWorkflowService.StartAsync(request, cancellationToken);
    return Results.Ok(workflow);
});

app.MapPost("/api/approval/workflows/decision", (
    ApprovalWorkflowDecisionEnvelope request,
    IApprovalWorkflowService approvalWorkflowService) =>
{
    var workflow = approvalWorkflowService.ApplyDecision(request.Workflow, request.Decision);
    return Results.Ok(workflow);
});

app.Run();

public partial class Program;

public sealed record SessionContextEvaluationResponse(
    bool IsInContext,
    string FailureReason,
    long? SessionId,
    long? AppointmentId);

public sealed record ApprovalWorkflowDecisionEnvelope(
    ApprovalWorkflowState Workflow,
    ApprovalStepDecisionRequest Decision);
