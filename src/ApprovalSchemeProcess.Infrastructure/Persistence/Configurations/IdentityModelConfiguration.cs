using ApprovalSchemeProcess.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSchemeProcess.Infrastructure.Persistence.Configurations;

internal static class IdentityModelConfiguration
{
    public static void ApplyIdentityModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Username).HasMaxLength(100).IsRequired();
            builder.Property(x => x.EmployeeNumber).HasMaxLength(100).IsRequired();
            builder.Property(x => x.FullName).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(200);
            builder.Property(x => x.Status).HasMaxLength(50).IsRequired().HasDefaultValue("active");
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.HasIndex(x => x.Username).IsUnique();
            builder.HasIndex(x => x.EmployeeNumber).IsUnique();
        });

        modelBuilder.Entity<Role>(builder =>
        {
            builder.ToTable("roles");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Code).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.HasIndex(x => x.Code).IsUnique();
        });

        modelBuilder.Entity<UserRole>(builder =>
        {
            builder.ToTable("user_roles", table =>
            {
                table.HasCheckConstraint("ck_user_roles_validity", "valid_to IS NULL OR valid_to >= valid_from");
            });

            builder.HasKey(x => x.Id);
            builder.Property(x => x.ValidFrom).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.HasIndex(x => x.UserId).HasDatabaseName("idx_user_roles_user");
            builder.HasIndex(x => x.RoleId).HasDatabaseName("idx_user_roles_role");
            builder.HasIndex(x => new { x.UserId, x.RoleId, x.ValidFrom }).IsUnique().HasDatabaseName("uq_user_roles_unique");
            builder.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.Role).WithMany(x => x.UserRoles).HasForeignKey(x => x.RoleId);
        });

        modelBuilder.Entity<SecurityLevel>(builder =>
        {
            builder.ToTable("security_levels", table =>
            {
                table.HasCheckConstraint("ck_security_levels_level_code", "level_code BETWEEN 0 AND 4");
                table.HasCheckConstraint("ck_security_levels_min_steps", "min_approval_steps >= 0");
            });

            builder.HasKey(x => x.Id);
            builder.Property(x => x.LevelCode).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(2000);
            builder.Property(x => x.RequiresApproval).HasDefaultValue(false);
            builder.Property(x => x.MinApprovalSteps).HasDefaultValue(0);
            builder.Property(x => x.RequiresComplianceReview).HasDefaultValue(false);
            builder.Property(x => x.RequiresSpecialAuthorization).HasDefaultValue(false);
            builder.HasIndex(x => x.LevelCode).IsUnique();
        });

        modelBuilder.Entity<OperationType>(builder =>
        {
            builder.ToTable("operation_types");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Code).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(2000);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.HasIndex(x => x.Code).IsUnique();
            builder.HasIndex(x => x.SecurityLevelId).HasDatabaseName("idx_operation_types_security_level");
            builder.HasOne(x => x.SecurityLevel).WithMany(x => x.OperationTypes).HasForeignKey(x => x.SecurityLevelId);
        });
    }
}
