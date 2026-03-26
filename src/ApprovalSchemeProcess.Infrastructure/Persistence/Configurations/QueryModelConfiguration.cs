using ApprovalSchemeProcess.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSchemeProcess.Infrastructure.Persistence.Configurations;

internal static class QueryModelConfiguration
{
    public static void ApplyQueryModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Query>(builder =>
        {
            builder.ToTable("queries", table =>
            {
                table.HasCheckConstraint("ck_queries_classification", "request_classification IN ('in_context', 'out_of_context')");
                table.HasCheckConstraint("ck_queries_status", "status IN ('pending', 'approved', 'denied', 'expired', 'executed')");
            });

            builder.HasKey(x => x.Id);
            builder.Property(x => x.TargetType).HasMaxLength(100).IsRequired();
            builder.Property(x => x.TargetIdentifier).HasMaxLength(200).IsRequired();
            builder.Property(x => x.RequestClassification).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Status).HasMaxLength(50).IsRequired().HasDefaultValue("pending");
            builder.Property(x => x.RequestedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.HasIndex(x => x.RequestedByUserId).HasDatabaseName("idx_queries_requested_by");
            builder.HasIndex(x => x.SessionId).HasDatabaseName("idx_queries_session");
            builder.HasIndex(x => x.OperationTypeId).HasDatabaseName("idx_queries_operation_type");
            builder.HasIndex(x => x.SecurityLevelId).HasDatabaseName("idx_queries_security_level");
            builder.HasOne(x => x.Session).WithMany(x => x.Queries).HasForeignKey(x => x.SessionId);
            builder.HasOne(x => x.RequestedByUser).WithMany(x => x.RequestedQueries).HasForeignKey(x => x.RequestedByUserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.OperationType).WithMany(x => x.Queries).HasForeignKey(x => x.OperationTypeId);
            builder.HasOne(x => x.SecurityLevel).WithMany(x => x.Queries).HasForeignKey(x => x.SecurityLevelId);
        });

        modelBuilder.Entity<AuditLog>(builder =>
        {
            builder.ToTable("audit_logs");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.EventType).HasMaxLength(100).IsRequired();
            builder.Property(x => x.EntityType).HasMaxLength(100).IsRequired();
            builder.Property(x => x.OccurredAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(x => x.IpAddress).HasMaxLength(100);
            builder.Property(x => x.DeviceIdentifier).HasMaxLength(200);
            builder.HasIndex(x => new { x.EntityType, x.EntityId }).HasDatabaseName("idx_audit_logs_entity");
            builder.HasIndex(x => x.OccurredAt).HasDatabaseName("idx_audit_logs_occurred_at");
            builder.HasOne(x => x.ActorUser).WithMany(x => x.AuditLogs).HasForeignKey(x => x.ActorUserId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
