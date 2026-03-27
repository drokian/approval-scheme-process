using ApprovalSchemeProcess.Domain.Entities;

namespace ApprovalSchemeProcess.Application.Approval;

public interface IApprovalSchemeReader
{
    Task<ApprovalScheme?> GetActiveSchemeAsync(long operationTypeId, CancellationToken cancellationToken = default);
}
