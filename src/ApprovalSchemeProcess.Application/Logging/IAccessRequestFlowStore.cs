using ApprovalSchemeProcess.Domain.Entities;
using ApprovalEntity = ApprovalSchemeProcess.Domain.Entities.Approval;

namespace ApprovalSchemeProcess.Application.Logging;

public interface IAccessRequestFlowStore
{
    Task CreateQueryAsync(Query query, CancellationToken cancellationToken = default);

    Task UpdateQueryAsync(Query query, CancellationToken cancellationToken = default);

    Task CreateApprovalRequestAsync(ApprovalRequest approvalRequest, CancellationToken cancellationToken = default);

    Task UpdateApprovalRequestAsync(ApprovalRequest approvalRequest, CancellationToken cancellationToken = default);

    Task<ApprovalRequestAggregate?> GetApprovalRequestAggregateAsync(
        long approvalRequestId,
        CancellationToken cancellationToken = default);

    Task AddApprovalAsync(ApprovalEntity approval, CancellationToken cancellationToken = default);

    Task AddAuditLogsAsync(IReadOnlyCollection<AuditLog> auditLogs, CancellationToken cancellationToken = default);
}
