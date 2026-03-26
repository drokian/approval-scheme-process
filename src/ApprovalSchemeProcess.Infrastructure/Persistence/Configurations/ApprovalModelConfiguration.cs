using ApprovalSchemeProcess.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSchemeProcess.Infrastructure.Persistence.Configurations;

internal static class ApprovalModelConfiguration
{
    public static void ApplyApprovalModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApprovalScheme>(builder =>
        {
            builder.ToTable("approval_schemes");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.VersionNo).HasDefaultValue(1);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.HasIndex(x => x.OperationTypeId).HasDatabaseName("idx_approval_schemes_operation_type");
            builder.HasIndex(x => new { x.OperationTypeId, x.VersionNo }).IsUnique().HasDatabaseName("uq_approval_schemes_unique_version");
            builder.HasOne(x => x.OperationType).WithMany(x => x.ApprovalSchemes).HasForeignKey(x => x.OperationTypeId);
        });

        modelBuilder.Entity<ApprovalSchemeStep>(builder =>
        {
            builder.ToTable("approval_scheme_steps", table =>
            {
                table.HasCheckConstraint("ck_approval_scheme_steps_order", "step_order > 0");
                table.HasCheckConstraint("ck_approval_scheme_steps_timeout", "timeout_minutes IS NULL OR timeout_minutes > 0");
            });

            builder.HasKey(x => x.Id);
            builder.Property(x => x.RoleCode).HasMaxLength(100).IsRequired();
            builder.Property(x => x.ReviewType).HasMaxLength(100).IsRequired().HasDefaultValue("approval");
            builder.Property(x => x.IsMandatory).HasDefaultValue(true);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.HasIndex(x => x.ApprovalSchemeId).HasDatabaseName("idx_approval_scheme_steps_scheme");
            builder.HasIndex(x => new { x.ApprovalSchemeId, x.StepOrder }).IsUnique().HasDatabaseName("uq_approval_scheme_steps_order");
            builder.HasOne(x => x.ApprovalScheme).WithMany(x => x.Steps).HasForeignKey(x => x.ApprovalSchemeId);
        });

        modelBuilder.Entity<ApprovalRequest>(builder =>
        {
            builder.ToTable("approval_requests", table =>
            {
                table.HasCheckConstraint("ck_approval_requests_status", "status IN ('pending', 'approved', 'denied', 'expired')");
            });

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Status).HasMaxLength(50).IsRequired().HasDefaultValue("pending");
            builder.Property(x => x.RequestedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.HasIndex(x => x.QueryId).IsUnique();
            builder.HasIndex(x => x.ApprovalSchemeId).HasDatabaseName("idx_approval_requests_scheme");
            builder.HasOne(x => x.Query).WithOne(x => x.ApprovalRequest).HasForeignKey<ApprovalRequest>(x => x.QueryId);
            builder.HasOne(x => x.ApprovalScheme).WithMany(x => x.ApprovalRequests).HasForeignKey(x => x.ApprovalSchemeId);
        });

        modelBuilder.Entity<Approval>(builder =>
        {
            builder.ToTable("approvals", table =>
            {
                table.HasCheckConstraint("ck_approvals_decision", "decision IN ('approved', 'denied', 'expired')");
            });

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Decision).HasMaxLength(50).IsRequired();
            builder.Property(x => x.DecidedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.HasIndex(x => x.ApprovalRequestId).HasDatabaseName("idx_approvals_request");
            builder.HasIndex(x => new { x.ApprovalRequestId, x.ApprovalSchemeStepId }).IsUnique().HasDatabaseName("uq_approvals_unique_step");
            builder.HasOne(x => x.ApprovalRequest).WithMany(x => x.Approvals).HasForeignKey(x => x.ApprovalRequestId);
            builder.HasOne(x => x.ApprovalSchemeStep).WithMany(x => x.Approvals).HasForeignKey(x => x.ApprovalSchemeStepId);
            builder.HasOne(x => x.ApproverUser).WithMany(x => x.Approvals).HasForeignKey(x => x.ApproverUserId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
