using ApprovalSchemeProcess.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApprovalSchemeProcess.Infrastructure.Persistence.Configurations;

internal static class SchedulingModelConfiguration
{
    public static void ApplySchedulingModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(builder =>
        {
            builder.ToTable("appointments", table =>
            {
                table.HasCheckConstraint("ck_appointments_schedule", "scheduled_end_at >= scheduled_start_at");
            });

            builder.HasKey(x => x.Id);
            builder.Property(x => x.CitizenIdentifier).HasMaxLength(200).IsRequired();
            builder.Property(x => x.CitizenDisplayName).HasMaxLength(200);
            builder.Property(x => x.Status).HasMaxLength(50).IsRequired().HasDefaultValue("scheduled");
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.HasIndex(x => x.OperationTypeId).HasDatabaseName("idx_appointments_operation_type");
            builder.HasOne(x => x.OperationType).WithMany(x => x.Appointments).HasForeignKey(x => x.OperationTypeId);
            builder.HasOne(x => x.CreatedByUser).WithMany(x => x.CreatedAppointments).HasForeignKey(x => x.CreatedByUserId);
        });

        modelBuilder.Entity<AppointmentTarget>(builder =>
        {
            builder.ToTable("appointment_targets");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.TargetType).HasMaxLength(100).IsRequired();
            builder.Property(x => x.TargetIdentifier).HasMaxLength(200).IsRequired();
            builder.Property(x => x.IsPrimary).HasDefaultValue(false);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.HasIndex(x => x.AppointmentId).HasDatabaseName("idx_appointment_targets_appointment");
            builder.HasOne(x => x.Appointment).WithMany(x => x.Targets).HasForeignKey(x => x.AppointmentId);
        });

        modelBuilder.Entity<Session>(builder =>
        {
            builder.ToTable("sessions", table =>
            {
                table.HasCheckConstraint("ck_sessions_status", "status IN ('pending_activation', 'active', 'paused', 'closed', 'expired', 'invalidated')");
                table.HasCheckConstraint("ck_sessions_expiry_time", "expires_at IS NULL OR expires_at >= activated_at");
                table.HasCheckConstraint("ck_sessions_last_activity", "last_activity_at >= activated_at");
                table.HasCheckConstraint("ck_sessions_close_time", "closed_at IS NULL OR closed_at >= activated_at");
                table.HasCheckConstraint("ck_sessions_invalidation_time", "invalidated_at IS NULL OR invalidated_at >= activated_at");
                table.HasCheckConstraint("ck_sessions_invalidation_reason", "(invalidated_at IS NULL AND invalidation_reason IS NULL) OR (invalidated_at IS NOT NULL AND invalidation_reason IS NOT NULL)");
            });

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Status).HasMaxLength(50).IsRequired().HasDefaultValue("active");
            builder.Property(x => x.ActivatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(x => x.LastActivityAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(x => x.InvalidationReason).HasMaxLength(100);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.HasIndex(x => x.AppointmentId).IsUnique();
            builder.HasIndex(x => x.CurrentAssignedUserId).HasDatabaseName("idx_sessions_current_assigned_user");
            builder.HasOne(x => x.Appointment).WithOne(x => x.Session).HasForeignKey<Session>(x => x.AppointmentId);
            builder.HasOne(x => x.CurrentAssignedUser).WithMany(x => x.CurrentAssignedSessions).HasForeignKey(x => x.CurrentAssignedUserId);
        });

        modelBuilder.Entity<SessionAssignment>(builder =>
        {
            builder.ToTable("session_assignments", table =>
            {
                table.HasCheckConstraint("ck_session_assignments_type", "assignment_type IN ('primary', 'delegate', 'temporary', 'reassigned')");
                table.HasCheckConstraint("ck_session_assignments_release_time", "released_at IS NULL OR released_at >= assigned_at");
                table.HasCheckConstraint("ck_session_assignments_current", "(is_current = TRUE AND released_at IS NULL) OR (is_current = FALSE AND released_at IS NOT NULL)");
            });

            builder.HasKey(x => x.Id);
            builder.Property(x => x.AssignmentType).HasMaxLength(50).IsRequired().HasDefaultValue("primary");
            builder.Property(x => x.AssignmentReason).HasMaxLength(500);
            builder.Property(x => x.AssignedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(x => x.IsCurrent).HasDefaultValue(true);
            builder.HasIndex(x => x.SessionId).HasDatabaseName("idx_session_assignments_session");
            builder.HasIndex(x => x.UserId).HasDatabaseName("idx_session_assignments_user");
            builder.HasIndex(x => new { x.SessionId, x.IsCurrent }).HasDatabaseName("idx_session_assignments_current");
            builder.HasIndex(x => new { x.SessionId, x.UserId, x.AssignedAt }).IsUnique().HasDatabaseName("uq_session_assignments_unique");
            builder.HasOne(x => x.Session).WithMany(x => x.Assignments).HasForeignKey(x => x.SessionId);
            builder.HasOne(x => x.User).WithMany(x => x.SessionAssignments).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.AssignedByUser).WithMany(x => x.AssignedSessionActions).HasForeignKey(x => x.AssignedByUserId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
