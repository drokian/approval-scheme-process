using ApprovalSchemeProcess.Domain.Entities;
using ApprovalEntity = ApprovalSchemeProcess.Domain.Entities.Approval;

namespace ApprovalSchemeProcess.Application.Logging;

public sealed record ApprovalRequestAggregate(
    ApprovalRequest ApprovalRequest,
    Query Query,
    ApprovalScheme ApprovalScheme,
    IReadOnlyList<ApprovalEntity> Approvals);
