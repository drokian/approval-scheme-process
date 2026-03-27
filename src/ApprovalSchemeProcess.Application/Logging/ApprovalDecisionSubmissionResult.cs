using ApprovalSchemeProcess.Application.Approval;

namespace ApprovalSchemeProcess.Application.Logging;

public sealed record ApprovalDecisionSubmissionResult(
    long QueryId,
    long ApprovalRequestId,
    ApprovalWorkflowState Workflow);
