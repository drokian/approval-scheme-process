using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ApprovalSchemeProcess.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "security_levels",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    level_code = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    requires_approval = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    min_approval_steps = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    requires_compliance_review = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    requires_special_authorization = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_security_levels", x => x.id);
                    table.CheckConstraint("ck_security_levels_level_code", "level_code BETWEEN 0 AND 4");
                    table.CheckConstraint("ck_security_levels_min_steps", "min_approval_steps >= 0");
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    employee_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    full_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "active"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "operation_types",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    security_level_id = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_operation_types", x => x.id);
                    table.ForeignKey(
                        name: "fk_operation_types_security_levels_security_level_id",
                        column: x => x.security_level_id,
                        principalTable: "security_levels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "audit_logs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    event_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    entity_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    entity_id = table.Column<long>(type: "bigint", nullable: false),
                    actor_user_id = table.Column<long>(type: "bigint", nullable: true),
                    occurred_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ip_address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    device_identifier = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    details = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit_logs", x => x.id);
                    table.ForeignKey(
                        name: "fk_audit_logs_users_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    role_id = table.Column<long>(type: "bigint", nullable: false),
                    valid_from = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    valid_to = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => x.id);
                    table.CheckConstraint("ck_user_roles_validity", "valid_to IS NULL OR valid_to >= valid_from");
                    table.ForeignKey(
                        name: "fk_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "appointments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    operation_type_id = table.Column<long>(type: "bigint", nullable: false),
                    citizen_identifier = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    citizen_display_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    scheduled_start_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    scheduled_end_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "scheduled"),
                    created_by_user_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_appointments", x => x.id);
                    table.CheckConstraint("ck_appointments_schedule", "scheduled_end_at >= scheduled_start_at");
                    table.ForeignKey(
                        name: "fk_appointments_operation_types_operation_type_id",
                        column: x => x.operation_type_id,
                        principalTable: "operation_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_appointments_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "approval_schemes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    operation_type_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    version_no = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    retired_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_approval_schemes", x => x.id);
                    table.ForeignKey(
                        name: "fk_approval_schemes_operation_types_operation_type_id",
                        column: x => x.operation_type_id,
                        principalTable: "operation_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "appointment_targets",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    appointment_id = table.Column<long>(type: "bigint", nullable: false),
                    target_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    target_identifier = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_appointment_targets", x => x.id);
                    table.ForeignKey(
                        name: "fk_appointment_targets_appointments_appointment_id",
                        column: x => x.appointment_id,
                        principalTable: "appointments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    appointment_id = table.Column<long>(type: "bigint", nullable: false),
                    current_assigned_user_id = table.Column<long>(type: "bigint", nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "active"),
                    activated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_activity_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    closed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    invalidated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    invalidation_reason = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sessions", x => x.id);
                    table.CheckConstraint("ck_sessions_close_time", "closed_at IS NULL OR closed_at >= activated_at");
                    table.CheckConstraint("ck_sessions_expiry_time", "expires_at IS NULL OR expires_at >= activated_at");
                    table.CheckConstraint("ck_sessions_invalidation_reason", "(invalidated_at IS NULL AND invalidation_reason IS NULL) OR (invalidated_at IS NOT NULL AND invalidation_reason IS NOT NULL)");
                    table.CheckConstraint("ck_sessions_invalidation_time", "invalidated_at IS NULL OR invalidated_at >= activated_at");
                    table.CheckConstraint("ck_sessions_last_activity", "last_activity_at >= activated_at");
                    table.CheckConstraint("ck_sessions_status", "status IN ('pending_activation', 'active', 'paused', 'closed', 'expired', 'invalidated')");
                    table.ForeignKey(
                        name: "fk_sessions_appointments_appointment_id",
                        column: x => x.appointment_id,
                        principalTable: "appointments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_sessions_users_current_assigned_user_id",
                        column: x => x.current_assigned_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "approval_scheme_steps",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    approval_scheme_id = table.Column<long>(type: "bigint", nullable: false),
                    step_order = table.Column<int>(type: "integer", nullable: false),
                    role_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    review_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "approval"),
                    is_mandatory = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    timeout_minutes = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_approval_scheme_steps", x => x.id);
                    table.CheckConstraint("ck_approval_scheme_steps_order", "step_order > 0");
                    table.CheckConstraint("ck_approval_scheme_steps_timeout", "timeout_minutes IS NULL OR timeout_minutes > 0");
                    table.ForeignKey(
                        name: "fk_approval_scheme_steps_approval_schemes_approval_scheme_id",
                        column: x => x.approval_scheme_id,
                        principalTable: "approval_schemes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "queries",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    session_id = table.Column<long>(type: "bigint", nullable: true),
                    requested_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    operation_type_id = table.Column<long>(type: "bigint", nullable: false),
                    security_level_id = table.Column<long>(type: "bigint", nullable: false),
                    target_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    target_identifier = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    request_classification = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    justification = table.Column<string>(type: "text", nullable: true),
                    is_emergency = table.Column<bool>(type: "boolean", nullable: false),
                    is_override = table.Column<bool>(type: "boolean", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "pending"),
                    requested_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    decided_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    executed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_queries", x => x.id);
                    table.CheckConstraint("ck_queries_classification", "request_classification IN ('in_context', 'out_of_context')");
                    table.CheckConstraint("ck_queries_status", "status IN ('pending', 'approved', 'denied', 'expired', 'executed')");
                    table.ForeignKey(
                        name: "fk_queries_operation_types_operation_type_id",
                        column: x => x.operation_type_id,
                        principalTable: "operation_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_queries_security_levels_security_level_id",
                        column: x => x.security_level_id,
                        principalTable: "security_levels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_queries_sessions_session_id",
                        column: x => x.session_id,
                        principalTable: "sessions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_queries_users_requested_by_user_id",
                        column: x => x.requested_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "session_assignments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    session_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    assignment_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "primary"),
                    assignment_reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    assigned_by_user_id = table.Column<long>(type: "bigint", nullable: true),
                    assigned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    released_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_current = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_session_assignments", x => x.id);
                    table.CheckConstraint("ck_session_assignments_current", "(is_current = TRUE AND released_at IS NULL) OR (is_current = FALSE AND released_at IS NOT NULL)");
                    table.CheckConstraint("ck_session_assignments_release_time", "released_at IS NULL OR released_at >= assigned_at");
                    table.CheckConstraint("ck_session_assignments_type", "assignment_type IN ('primary', 'delegate', 'temporary', 'reassigned')");
                    table.ForeignKey(
                        name: "fk_session_assignments_sessions_session_id",
                        column: x => x.session_id,
                        principalTable: "sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_session_assignments_users_assigned_by_user_id",
                        column: x => x.assigned_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_session_assignments_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "approval_requests",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    query_id = table.Column<long>(type: "bigint", nullable: false),
                    approval_scheme_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "pending"),
                    requested_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_approval_requests", x => x.id);
                    table.CheckConstraint("ck_approval_requests_status", "status IN ('pending', 'approved', 'denied', 'expired')");
                    table.ForeignKey(
                        name: "fk_approval_requests_approval_schemes_approval_scheme_id",
                        column: x => x.approval_scheme_id,
                        principalTable: "approval_schemes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_approval_requests_queries_query_id",
                        column: x => x.query_id,
                        principalTable: "queries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "approvals",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    approval_request_id = table.Column<long>(type: "bigint", nullable: false),
                    approval_scheme_step_id = table.Column<long>(type: "bigint", nullable: false),
                    approver_user_id = table.Column<long>(type: "bigint", nullable: false),
                    decision = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    reason = table.Column<string>(type: "text", nullable: true),
                    decided_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_approvals", x => x.id);
                    table.CheckConstraint("ck_approvals_decision", "decision IN ('approved', 'denied', 'expired')");
                    table.ForeignKey(
                        name: "fk_approvals_approval_requests_approval_request_id",
                        column: x => x.approval_request_id,
                        principalTable: "approval_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_approvals_approval_scheme_steps_approval_scheme_step_id",
                        column: x => x.approval_scheme_step_id,
                        principalTable: "approval_scheme_steps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_approvals_users_approver_user_id",
                        column: x => x.approver_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "idx_appointment_targets_appointment",
                table: "appointment_targets",
                column: "appointment_id");

            migrationBuilder.CreateIndex(
                name: "idx_appointments_operation_type",
                table: "appointments",
                column: "operation_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_appointments_created_by_user_id",
                table: "appointments",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "idx_approval_requests_scheme",
                table: "approval_requests",
                column: "approval_scheme_id");

            migrationBuilder.CreateIndex(
                name: "ix_approval_requests_query_id",
                table: "approval_requests",
                column: "query_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_approval_scheme_steps_scheme",
                table: "approval_scheme_steps",
                column: "approval_scheme_id");

            migrationBuilder.CreateIndex(
                name: "uq_approval_scheme_steps_order",
                table: "approval_scheme_steps",
                columns: new[] { "approval_scheme_id", "step_order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_approval_schemes_operation_type",
                table: "approval_schemes",
                column: "operation_type_id");

            migrationBuilder.CreateIndex(
                name: "uq_approval_schemes_unique_version",
                table: "approval_schemes",
                columns: new[] { "operation_type_id", "version_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_approvals_request",
                table: "approvals",
                column: "approval_request_id");

            migrationBuilder.CreateIndex(
                name: "ix_approvals_approval_scheme_step_id",
                table: "approvals",
                column: "approval_scheme_step_id");

            migrationBuilder.CreateIndex(
                name: "ix_approvals_approver_user_id",
                table: "approvals",
                column: "approver_user_id");

            migrationBuilder.CreateIndex(
                name: "uq_approvals_unique_step",
                table: "approvals",
                columns: new[] { "approval_request_id", "approval_scheme_step_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_audit_logs_entity",
                table: "audit_logs",
                columns: new[] { "entity_type", "entity_id" });

            migrationBuilder.CreateIndex(
                name: "idx_audit_logs_occurred_at",
                table: "audit_logs",
                column: "occurred_at");

            migrationBuilder.CreateIndex(
                name: "ix_audit_logs_actor_user_id",
                table: "audit_logs",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "idx_operation_types_security_level",
                table: "operation_types",
                column: "security_level_id");

            migrationBuilder.CreateIndex(
                name: "ix_operation_types_code",
                table: "operation_types",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_queries_operation_type",
                table: "queries",
                column: "operation_type_id");

            migrationBuilder.CreateIndex(
                name: "idx_queries_requested_by",
                table: "queries",
                column: "requested_by_user_id");

            migrationBuilder.CreateIndex(
                name: "idx_queries_security_level",
                table: "queries",
                column: "security_level_id");

            migrationBuilder.CreateIndex(
                name: "idx_queries_session",
                table: "queries",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "ix_roles_code",
                table: "roles",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_security_levels_level_code",
                table: "security_levels",
                column: "level_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_session_assignments_current",
                table: "session_assignments",
                columns: new[] { "session_id", "is_current" });

            migrationBuilder.CreateIndex(
                name: "idx_session_assignments_session",
                table: "session_assignments",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "idx_session_assignments_user",
                table: "session_assignments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_session_assignments_assigned_by_user_id",
                table: "session_assignments",
                column: "assigned_by_user_id");

            migrationBuilder.CreateIndex(
                name: "uq_session_assignments_unique",
                table: "session_assignments",
                columns: new[] { "session_id", "user_id", "assigned_at" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_sessions_current_assigned_user",
                table: "sessions",
                column: "current_assigned_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_sessions_appointment_id",
                table: "sessions",
                column: "appointment_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_user_roles_role",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "idx_user_roles_user",
                table: "user_roles",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "uq_user_roles_unique",
                table: "user_roles",
                columns: new[] { "user_id", "role_id", "valid_from" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_employee_number",
                table: "users",
                column: "employee_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_username",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appointment_targets");

            migrationBuilder.DropTable(
                name: "approvals");

            migrationBuilder.DropTable(
                name: "audit_logs");

            migrationBuilder.DropTable(
                name: "session_assignments");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "approval_requests");

            migrationBuilder.DropTable(
                name: "approval_scheme_steps");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "queries");

            migrationBuilder.DropTable(
                name: "approval_schemes");

            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "appointments");

            migrationBuilder.DropTable(
                name: "operation_types");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "security_levels");
        }
    }
}
