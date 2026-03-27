namespace ApprovalSchemeProcess.Application.Approval;

public sealed record ApprovalWorkflowStartRequest(
    long QueryId,
    long OperationTypeId,
    DateTime RequestedAtUtc);
