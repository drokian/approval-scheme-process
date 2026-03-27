namespace ApprovalSchemeProcess.Application.Logging;

public interface IAccessRequestFlowService
{
    Task<AccessRequestSubmissionResult> SubmitAsync(
        AccessRequestSubmissionRequest request,
        CancellationToken cancellationToken = default);

    Task<ApprovalDecisionSubmissionResult> RecordApprovalDecisionAsync(
        ApprovalDecisionSubmissionRequest request,
        CancellationToken cancellationToken = default);
}
