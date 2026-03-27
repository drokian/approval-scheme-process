using ApprovalSchemeProcess.Application.Access;
using ApprovalSchemeProcess.Application.Approval;

namespace ApprovalSchemeProcess.Application.Logging;

public sealed record AccessRequestSubmissionResult(
    long? QueryId,
    AccessEvaluationResult Evaluation,
    long? ApprovalRequestId,
    ApprovalWorkflowState? Workflow);
