using ApprovalSchemeProcess.Application.Sessions;
using ApprovalSchemeProcess.Domain.Entities;
using ApprovalSchemeProcess.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ApprovalSchemeProcess.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ApprovalSchemeProcess")
            ?? throw new InvalidOperationException("Connection string 'ApprovalSchemeProcess' is not configured.");

        services.AddDbContext<ApprovalSchemeProcessDbContext>(options => options
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention());
        services.AddScoped<ISessionContextEvaluator, SessionContextEvaluator>();
        services.AddScoped<ISessionContextSessionReader, SessionContextSessionReader>();
        services.AddScoped<ISessionContextService, SessionContextService>();

        return services;
    }

    public static async Task InitializeDatabaseAsync(
        this IServiceProvider services,
        IConfiguration configuration,
        string contentRootPath,
        CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var logger = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("DatabaseInitialization");
        var dbContext = scope.ServiceProvider.GetRequiredService<ApprovalSchemeProcessDbContext>();
        var shouldSeed = bool.TryParse(configuration["Database:SeedDemoDataOnStartup"], out var seedDemoData)
            && seedDemoData;

        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(cancellationToken);
        if (pendingMigrations.Any())
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
            logger.LogInformation("Applied pending EF Core migrations.");
        }
        else
        {
            await dbContext.Database.EnsureCreatedAsync(cancellationToken);
            logger.LogInformation("Ensured database exists without pending migrations.");
        }

        if (!shouldSeed)
        {
            return;
        }

        if (await dbContext.Users.AnyAsync(cancellationToken))
        {
            logger.LogInformation("Skipping seed because users already exist.");
            return;
        }

        var repositoryRoot = Path.GetFullPath(Path.Combine(contentRootPath, "..", ".."));
        var seedPath = Path.Combine(repositoryRoot, "db", "seed.sql");
        if (!File.Exists(seedPath))
        {
            throw new FileNotFoundException("Seed SQL file could not be found.", seedPath);
        }

        var seedSql = await File.ReadAllTextAsync(seedPath, cancellationToken);
        await dbContext.Database.ExecuteSqlRawAsync(seedSql, cancellationToken);
        logger.LogInformation("Applied demo seed SQL from {SeedPath}.", seedPath);
    }
}

public sealed class SessionContextSessionReader(ApprovalSchemeProcessDbContext dbContext) : ISessionContextSessionReader
{
    public Task<Session?> GetByIdAsync(long sessionId, CancellationToken cancellationToken = default) =>
        dbContext.Sessions
            .AsNoTracking()
            .Include(session => session.Appointment)
            .ThenInclude(appointment => appointment!.Targets)
            .Include(session => session.Assignments)
            .FirstOrDefaultAsync(session => session.Id == sessionId, cancellationToken);
}
