namespace ApprovalSchemeProcess.Application.Access;

public interface IAccessEvaluationService
{
    Task<AccessEvaluationResult> EvaluateAsync(
        AccessEvaluationRequest request,
        CancellationToken cancellationToken = default);
}
