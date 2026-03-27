using ApprovalSchemeProcess.Application.Logging;
using ApprovalSchemeProcess.Domain.Entities;
using ApprovalEntity = ApprovalSchemeProcess.Domain.Entities.Approval;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSchemeProcess.Infrastructure.Persistence;

public sealed class AccessRequestFlowStore(ApprovalSchemeProcessDbContext dbContext) : IAccessRequestFlowStore
{
    public async Task CreateQueryAsync(Query query, CancellationToken cancellationToken = default)
    {
        dbContext.Queries.Add(query);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateQueryAsync(Query query, CancellationToken cancellationToken = default)
    {
        dbContext.Queries.Update(query);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task CreateApprovalRequestAsync(ApprovalRequest approvalRequest, CancellationToken cancellationToken = default)
    {
        dbContext.ApprovalRequests.Add(approvalRequest);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateApprovalRequestAsync(ApprovalRequest approvalRequest, CancellationToken cancellationToken = default)
    {
        dbContext.ApprovalRequests.Update(approvalRequest);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ApprovalRequestAggregate?> GetApprovalRequestAggregateAsync(
        long approvalRequestId,
        CancellationToken cancellationToken = default)
    {
        var approvalRequest = await dbContext.ApprovalRequests
            .AsNoTracking()
            .Include(request => request.Query)
            .Include(request => request.ApprovalScheme)
            .ThenInclude(scheme => scheme.Steps)
            .Include(request => request.Approvals)
            .FirstOrDefaultAsync(request => request.Id == approvalRequestId, cancellationToken);

        return approvalRequest is null
            ? null
            : new ApprovalRequestAggregate(
                approvalRequest,
                approvalRequest.Query,
                approvalRequest.ApprovalScheme,
                approvalRequest.Approvals.OrderBy(approval => approval.DecidedAt).ThenBy(approval => approval.Id).ToArray());
    }

    public async Task AddApprovalAsync(ApprovalEntity approval, CancellationToken cancellationToken = default)
    {
        dbContext.Approvals.Add(approval);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAuditLogsAsync(IReadOnlyCollection<AuditLog> auditLogs, CancellationToken cancellationToken = default)
    {
        if (auditLogs.Count == 0)
        {
            return;
        }

        dbContext.AuditLogs.AddRange(auditLogs);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
