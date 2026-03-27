using ApprovalSchemeProcess.Domain.Entities;

namespace ApprovalSchemeProcess.Application.Access;

public interface IAccessEvaluationReader
{
    Task<User?> GetRequesterAsync(long userId, CancellationToken cancellationToken = default);

    Task<OperationType?> GetOperationTypeAsync(long operationTypeId, CancellationToken cancellationToken = default);
}
