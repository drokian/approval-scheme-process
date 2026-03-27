using ApprovalSchemeProcess.Application.Access;
using ApprovalSchemeProcess.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSchemeProcess.Infrastructure.Persistence;

public sealed class AccessEvaluationReader(ApprovalSchemeProcessDbContext dbContext) : IAccessEvaluationReader
{
    public Task<User?> GetRequesterAsync(long userId, CancellationToken cancellationToken = default) =>
        dbContext.Users
            .AsNoTracking()
            .Include(user => user.UserRoles)
            .ThenInclude(userRole => userRole.Role)
            .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);

    public Task<OperationType?> GetOperationTypeAsync(long operationTypeId, CancellationToken cancellationToken = default) =>
        dbContext.OperationTypes
            .AsNoTracking()
            .Include(operationType => operationType.SecurityLevel)
            .FirstOrDefaultAsync(operationType => operationType.Id == operationTypeId, cancellationToken);
}
