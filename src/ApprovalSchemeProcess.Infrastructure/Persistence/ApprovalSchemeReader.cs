using ApprovalSchemeProcess.Application.Approval;
using ApprovalSchemeProcess.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSchemeProcess.Infrastructure.Persistence;

public sealed class ApprovalSchemeReader(ApprovalSchemeProcessDbContext dbContext) : IApprovalSchemeReader
{
    public Task<ApprovalScheme?> GetActiveSchemeAsync(long operationTypeId, CancellationToken cancellationToken = default) =>
        dbContext.ApprovalSchemes
            .AsNoTracking()
            .Include(scheme => scheme.Steps)
            .Where(scheme => scheme.OperationTypeId == operationTypeId && scheme.IsActive)
            .OrderByDescending(scheme => scheme.VersionNo)
            .FirstOrDefaultAsync(cancellationToken);
}
