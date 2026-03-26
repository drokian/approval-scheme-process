-- Draft relational schema for the Approval Scheme Process.
-- This schema is intended as a conceptual baseline and should be adapted
-- to institutional, legal, and platform-specific requirements before use.

CREATE TABLE users (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    username VARCHAR(100) NOT NULL UNIQUE,
    employee_number VARCHAR(100) NOT NULL UNIQUE,
    full_name VARCHAR(200) NOT NULL,
    email VARCHAR(200),
    status VARCHAR(50) NOT NULL DEFAULT 'active',
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE roles (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    code VARCHAR(100) NOT NULL UNIQUE,
    name VARCHAR(200) NOT NULL,
    description VARCHAR(1000)
);

CREATE TABLE user_roles (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    user_id BIGINT NOT NULL,
    role_id BIGINT NOT NULL,
    valid_from TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    valid_to TIMESTAMP,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    CONSTRAINT fk_user_roles_user
        FOREIGN KEY (user_id) REFERENCES users (id),
    CONSTRAINT fk_user_roles_role
        FOREIGN KEY (role_id) REFERENCES roles (id),
    CONSTRAINT uq_user_roles_unique
        UNIQUE (user_id, role_id, valid_from)
);

CREATE TABLE security_levels (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    level_code INTEGER NOT NULL UNIQUE,
    name VARCHAR(200) NOT NULL,
    description VARCHAR(2000),
    requires_approval BOOLEAN NOT NULL DEFAULT FALSE,
    min_approval_steps INTEGER NOT NULL DEFAULT 0,
    requires_compliance_review BOOLEAN NOT NULL DEFAULT FALSE,
    requires_special_authorization BOOLEAN NOT NULL DEFAULT FALSE,
    CONSTRAINT ck_security_levels_level_code
        CHECK (level_code BETWEEN 0 AND 4),
    CONSTRAINT ck_security_levels_min_steps
        CHECK (min_approval_steps >= 0)
);

CREATE TABLE operation_types (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    code VARCHAR(100) NOT NULL UNIQUE,
    name VARCHAR(200) NOT NULL,
    description VARCHAR(2000),
    security_level_id BIGINT NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_operation_types_security_level
        FOREIGN KEY (security_level_id) REFERENCES security_levels (id)
);

CREATE TABLE approval_schemes (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    operation_type_id BIGINT NOT NULL,
    name VARCHAR(200) NOT NULL,
    version_no INTEGER NOT NULL DEFAULT 1,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    retired_at TIMESTAMP,
    CONSTRAINT fk_approval_schemes_operation_type
        FOREIGN KEY (operation_type_id) REFERENCES operation_types (id),
    CONSTRAINT uq_approval_schemes_unique_version
        UNIQUE (operation_type_id, version_no)
);

CREATE TABLE approval_scheme_steps (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    approval_scheme_id BIGINT NOT NULL,
    step_order INTEGER NOT NULL,
    role_code VARCHAR(100) NOT NULL,
    review_type VARCHAR(100) NOT NULL DEFAULT 'approval',
    is_mandatory BOOLEAN NOT NULL DEFAULT TRUE,
    timeout_minutes INTEGER,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_approval_scheme_steps_scheme
        FOREIGN KEY (approval_scheme_id) REFERENCES approval_schemes (id),
    CONSTRAINT uq_approval_scheme_steps_order
        UNIQUE (approval_scheme_id, step_order),
    CONSTRAINT ck_approval_scheme_steps_order
        CHECK (step_order > 0),
    CONSTRAINT ck_approval_scheme_steps_timeout
        CHECK (timeout_minutes IS NULL OR timeout_minutes > 0)
);

CREATE TABLE appointments (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    operation_type_id BIGINT NOT NULL,
    citizen_identifier VARCHAR(200) NOT NULL,
    citizen_display_name VARCHAR(200),
    scheduled_start_at TIMESTAMP NOT NULL,
    scheduled_end_at TIMESTAMP NOT NULL,
    status VARCHAR(50) NOT NULL DEFAULT 'scheduled',
    created_by_user_id BIGINT,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_appointments_operation_type
        FOREIGN KEY (operation_type_id) REFERENCES operation_types (id),
    CONSTRAINT fk_appointments_created_by
        FOREIGN KEY (created_by_user_id) REFERENCES users (id),
    CONSTRAINT ck_appointments_schedule
        CHECK (scheduled_end_at >= scheduled_start_at)
);

CREATE TABLE appointment_targets (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    appointment_id BIGINT NOT NULL,
    target_type VARCHAR(100) NOT NULL,
    target_identifier VARCHAR(200) NOT NULL,
    is_primary BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_appointment_targets_appointment
        FOREIGN KEY (appointment_id) REFERENCES appointments (id)
);

CREATE TABLE sessions (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    appointment_id BIGINT NOT NULL UNIQUE,
    assigned_user_id BIGINT NOT NULL,
    status VARCHAR(50) NOT NULL DEFAULT 'active',
    activated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    closed_at TIMESTAMP,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_sessions_appointment
        FOREIGN KEY (appointment_id) REFERENCES appointments (id),
    CONSTRAINT fk_sessions_assigned_user
        FOREIGN KEY (assigned_user_id) REFERENCES users (id),
    CONSTRAINT ck_sessions_close_time
        CHECK (closed_at IS NULL OR closed_at >= activated_at)
);

CREATE TABLE queries (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    session_id BIGINT,
    requested_by_user_id BIGINT NOT NULL,
    operation_type_id BIGINT NOT NULL,
    security_level_id BIGINT NOT NULL,
    target_type VARCHAR(100) NOT NULL,
    target_identifier VARCHAR(200) NOT NULL,
    request_classification VARCHAR(50) NOT NULL,
    justification TEXT,
    is_emergency BOOLEAN NOT NULL DEFAULT FALSE,
    is_override BOOLEAN NOT NULL DEFAULT FALSE,
    status VARCHAR(50) NOT NULL DEFAULT 'pending',
    requested_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    decided_at TIMESTAMP,
    executed_at TIMESTAMP,
    CONSTRAINT fk_queries_session
        FOREIGN KEY (session_id) REFERENCES sessions (id),
    CONSTRAINT fk_queries_requested_by
        FOREIGN KEY (requested_by_user_id) REFERENCES users (id),
    CONSTRAINT fk_queries_operation_type
        FOREIGN KEY (operation_type_id) REFERENCES operation_types (id),
    CONSTRAINT fk_queries_security_level
        FOREIGN KEY (security_level_id) REFERENCES security_levels (id),
    CONSTRAINT ck_queries_classification
        CHECK (request_classification IN ('in_context', 'out_of_context')),
    CONSTRAINT ck_queries_status
        CHECK (status IN ('pending', 'approved', 'denied', 'expired', 'executed'))
);

CREATE TABLE approval_requests (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    query_id BIGINT NOT NULL UNIQUE,
    approval_scheme_id BIGINT NOT NULL,
    status VARCHAR(50) NOT NULL DEFAULT 'pending',
    requested_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    completed_at TIMESTAMP,
    CONSTRAINT fk_approval_requests_query
        FOREIGN KEY (query_id) REFERENCES queries (id),
    CONSTRAINT fk_approval_requests_scheme
        FOREIGN KEY (approval_scheme_id) REFERENCES approval_schemes (id),
    CONSTRAINT ck_approval_requests_status
        CHECK (status IN ('pending', 'approved', 'denied', 'expired'))
);

CREATE TABLE approvals (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    approval_request_id BIGINT NOT NULL,
    approval_scheme_step_id BIGINT NOT NULL,
    approver_user_id BIGINT NOT NULL,
    decision VARCHAR(50) NOT NULL,
    reason TEXT,
    decided_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_approvals_request
        FOREIGN KEY (approval_request_id) REFERENCES approval_requests (id),
    CONSTRAINT fk_approvals_step
        FOREIGN KEY (approval_scheme_step_id) REFERENCES approval_scheme_steps (id),
    CONSTRAINT fk_approvals_approver
        FOREIGN KEY (approver_user_id) REFERENCES users (id),
    CONSTRAINT uq_approvals_unique_step
        UNIQUE (approval_request_id, approval_scheme_step_id),
    CONSTRAINT ck_approvals_decision
        CHECK (decision IN ('approved', 'denied', 'expired'))
);

CREATE TABLE audit_logs (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    event_type VARCHAR(100) NOT NULL,
    entity_type VARCHAR(100) NOT NULL,
    entity_id BIGINT NOT NULL,
    actor_user_id BIGINT,
    occurred_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ip_address VARCHAR(100),
    device_identifier VARCHAR(200),
    details TEXT,
    CONSTRAINT fk_audit_logs_actor
        FOREIGN KEY (actor_user_id) REFERENCES users (id)
);

CREATE INDEX idx_user_roles_user ON user_roles (user_id);
CREATE INDEX idx_user_roles_role ON user_roles (role_id);
CREATE INDEX idx_operation_types_security_level ON operation_types (security_level_id);
CREATE INDEX idx_approval_schemes_operation_type ON approval_schemes (operation_type_id);
CREATE INDEX idx_approval_scheme_steps_scheme ON approval_scheme_steps (approval_scheme_id);
CREATE INDEX idx_appointments_operation_type ON appointments (operation_type_id);
CREATE INDEX idx_appointment_targets_appointment ON appointment_targets (appointment_id);
CREATE INDEX idx_sessions_assigned_user ON sessions (assigned_user_id);
CREATE INDEX idx_queries_requested_by ON queries (requested_by_user_id);
CREATE INDEX idx_queries_session ON queries (session_id);
CREATE INDEX idx_queries_operation_type ON queries (operation_type_id);
CREATE INDEX idx_queries_security_level ON queries (security_level_id);
CREATE INDEX idx_approval_requests_scheme ON approval_requests (approval_scheme_id);
CREATE INDEX idx_approvals_request ON approvals (approval_request_id);
CREATE INDEX idx_audit_logs_entity ON audit_logs (entity_type, entity_id);
CREATE INDEX idx_audit_logs_occurred_at ON audit_logs (occurred_at);
