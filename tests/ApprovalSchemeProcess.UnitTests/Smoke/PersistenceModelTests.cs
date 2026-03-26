using ApprovalSchemeProcess.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSchemeProcess.UnitTests.Smoke;

public class PersistenceModelTests
{
    [Fact]
    public void DbContext_model_maps_all_core_tables()
    {
        var options = new DbContextOptionsBuilder<ApprovalSchemeProcessDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=approval_scheme_process_test;Username=postgres;Password=postgres")
            .Options;

        using var dbContext = new ApprovalSchemeProcessDbContext(options);

        var tableNames = dbContext.Model
            .GetEntityTypes()
            .Select(entityType => entityType.GetTableName())
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(name => name!)
            .OrderBy(name => name)
            .ToArray();

        Assert.Equal(15, tableNames.Length);
        Assert.Contains("users", tableNames);
        Assert.Contains("roles", tableNames);
        Assert.Contains("user_roles", tableNames);
        Assert.Contains("security_levels", tableNames);
        Assert.Contains("operation_types", tableNames);
        Assert.Contains("approval_schemes", tableNames);
        Assert.Contains("approval_scheme_steps", tableNames);
        Assert.Contains("appointments", tableNames);
        Assert.Contains("appointment_targets", tableNames);
        Assert.Contains("sessions", tableNames);
        Assert.Contains("session_assignments", tableNames);
        Assert.Contains("queries", tableNames);
        Assert.Contains("approval_requests", tableNames);
        Assert.Contains("approvals", tableNames);
        Assert.Contains("audit_logs", tableNames);
    }
}
