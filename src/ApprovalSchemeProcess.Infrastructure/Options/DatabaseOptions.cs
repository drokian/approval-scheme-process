namespace ApprovalSchemeProcess.Infrastructure.Options;

public class DatabaseOptions
{
    public const string SectionName = "Database";

    public bool ApplyMigrationsOnStartup { get; set; }
    public bool SeedDemoDataOnStartup { get; set; }
}
