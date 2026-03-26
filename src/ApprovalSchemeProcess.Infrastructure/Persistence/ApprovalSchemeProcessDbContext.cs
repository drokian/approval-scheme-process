using ApprovalSchemeProcess.Domain.Entities;
using ApprovalSchemeProcess.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSchemeProcess.Infrastructure.Persistence;

public class ApprovalSchemeProcessDbContext(DbContextOptions<ApprovalSchemeProcessDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<SecurityLevel> SecurityLevels => Set<SecurityLevel>();
    public DbSet<OperationType> OperationTypes => Set<OperationType>();
    public DbSet<ApprovalScheme> ApprovalSchemes => Set<ApprovalScheme>();
    public DbSet<ApprovalSchemeStep> ApprovalSchemeSteps => Set<ApprovalSchemeStep>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<AppointmentTarget> AppointmentTargets => Set<AppointmentTarget>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<SessionAssignment> SessionAssignments => Set<SessionAssignment>();
    public DbSet<Query> Queries => Set<Query>();
    public DbSet<ApprovalRequest> ApprovalRequests => Set<ApprovalRequest>();
    public DbSet<Approval> Approvals => Set<Approval>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyIdentityModel();
        modelBuilder.ApplyApprovalModel();
        modelBuilder.ApplySchedulingModel();
        modelBuilder.ApplyQueryModel();
    }
}
