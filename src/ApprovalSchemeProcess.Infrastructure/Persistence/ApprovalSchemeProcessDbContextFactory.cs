using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ApprovalSchemeProcess.Infrastructure.Persistence;

public class ApprovalSchemeProcessDbContextFactory : IDesignTimeDbContextFactory<ApprovalSchemeProcessDbContext>
{
    public ApprovalSchemeProcessDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__ApprovalSchemeProcess")
            ?? "Host=localhost;Port=5432;Database=approval_scheme_process;Username=postgres;Password=postgres";

        var optionsBuilder = new DbContextOptionsBuilder<ApprovalSchemeProcessDbContext>();
        optionsBuilder
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention();

        return new ApprovalSchemeProcessDbContext(optionsBuilder.Options);
    }
}
