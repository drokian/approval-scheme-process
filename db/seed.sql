-- Example seed data for the Approval Scheme Process.
-- This dataset is intended for demonstrations and early development only.
-- Run after db/schema.sql.

INSERT INTO roles (code, name, description) VALUES
    ('EMPLOYEE', 'Employee', 'Standard employee role'),
    ('SUPERVISOR', 'Supervisor', 'Business approval authority'),
    ('SECURITY', 'Security Officer', 'Security review authority'),
    ('LEGAL', 'Legal Reviewer', 'Legal review authority'),
    ('COMPLIANCE', 'Compliance Officer', 'Data protection and compliance review authority'),
    ('SENIOR_AUTH', 'Senior Authorizing Authority', 'Critical access authority'),
    ('SYS_ADMIN', 'System Administrator', 'Platform administration role'),
    ('AUDITOR', 'Auditor', 'Independent oversight and review role');

INSERT INTO users (username, employee_number, full_name, email, status) VALUES
    ('ayse.yilmaz', 'EMP-1001', 'Ayse Yilmaz', 'ayse.yilmaz@example.gov', 'active'),
    ('mehmet.demir', 'EMP-1002', 'Mehmet Demir', 'mehmet.demir@example.gov', 'active'),
    ('selin.kaya', 'EMP-1003', 'Selin Kaya', 'selin.kaya@example.gov', 'active'),
    ('murat.arslan', 'EMP-1004', 'Murat Arslan', 'murat.arslan@example.gov', 'active'),
    ('ece.ozkan', 'EMP-1005', 'Ece Ozkan', 'ece.ozkan@example.gov', 'active'),
    ('zeynep.kurt', 'EMP-1006', 'Zeynep Kurt', 'zeynep.kurt@example.gov', 'active'),
    ('kemal.sahin', 'EMP-1007', 'Kemal Sahin', 'kemal.sahin@example.gov', 'active'),
    ('deniz.celik', 'EMP-1008', 'Deniz Celik', 'deniz.celik@example.gov', 'active');

INSERT INTO user_roles (user_id, role_id, valid_from, is_active)
SELECT u.id, r.id, CURRENT_TIMESTAMP, TRUE
FROM users u
JOIN roles r ON r.code = 'EMPLOYEE'
WHERE u.username = 'ayse.yilmaz'
UNION ALL
SELECT u.id, r.id, CURRENT_TIMESTAMP, TRUE
FROM users u
JOIN roles r ON r.code = 'SUPERVISOR'
WHERE u.username = 'mehmet.demir'
UNION ALL
SELECT u.id, r.id, CURRENT_TIMESTAMP, TRUE
FROM users u
JOIN roles r ON r.code = 'SECURITY'
WHERE u.username = 'selin.kaya'
UNION ALL
SELECT u.id, r.id, CURRENT_TIMESTAMP, TRUE
FROM users u
JOIN roles r ON r.code = 'LEGAL'
WHERE u.username = 'murat.arslan'
UNION ALL
SELECT u.id, r.id, CURRENT_TIMESTAMP, TRUE
FROM users u
JOIN roles r ON r.code = 'COMPLIANCE'
WHERE u.username = 'ece.ozkan'
UNION ALL
SELECT u.id, r.id, CURRENT_TIMESTAMP, TRUE
FROM users u
JOIN roles r ON r.code = 'SENIOR_AUTH'
WHERE u.username = 'zeynep.kurt'
UNION ALL
SELECT u.id, r.id, CURRENT_TIMESTAMP, TRUE
FROM users u
JOIN roles r ON r.code = 'SYS_ADMIN'
WHERE u.username = 'kemal.sahin'
UNION ALL
SELECT u.id, r.id, CURRENT_TIMESTAMP, TRUE
FROM users u
JOIN roles r ON r.code = 'AUDITOR'
WHERE u.username = 'deniz.celik';

INSERT INTO security_levels (
    level_code,
    name,
    description,
    requires_approval,
    min_approval_steps,
    requires_compliance_review,
    requires_special_authorization
) VALUES
    (0, 'Context-Based Free Access', 'In-context access with valid session and role alignment', FALSE, 0, FALSE, FALSE),
    (1, 'Low-Risk Out-of-Context Access', 'Low-risk out-of-context access with limited sensitivity', TRUE, 1, FALSE, FALSE),
    (2, 'Medium-Risk Access', 'Out-of-context access involving personal data or moderate operational sensitivity', TRUE, 2, FALSE, FALSE),
    (3, 'High-Risk Access', 'Legally sensitive or protected data requiring elevated review', TRUE, 3, TRUE, FALSE),
    (4, 'Critical Access', 'Exceptional or highly sensitive access requiring maximum scrutiny', TRUE, 4, TRUE, TRUE);

INSERT INTO operation_types (code, name, description, security_level_id, is_active)
SELECT 'BIRTH_REGISTRATION', 'Birth Registration', 'Civil registry operation for birth-related records', sl.id, TRUE
FROM security_levels sl
WHERE sl.level_code = 1
UNION ALL
SELECT 'LAND_SALE', 'Land Sale', 'Property transfer and land registry review operation', sl.id, TRUE
FROM security_levels sl
WHERE sl.level_code = 2
UNION ALL
SELECT 'TAX_AUDIT', 'Tax Audit', 'Investigation and audit of tax-related records', sl.id, TRUE
FROM security_levels sl
WHERE sl.level_code = 3
UNION ALL
SELECT 'CRITICAL_INVESTIGATION', 'Critical Investigation', 'Exceptional high-sensitivity investigation workflow', sl.id, TRUE
FROM security_levels sl
WHERE sl.level_code = 4;

INSERT INTO approval_schemes (operation_type_id, name, version_no, is_active)
SELECT ot.id, 'Birth Registration Standard Approval', 1, TRUE
FROM operation_types ot
WHERE ot.code = 'BIRTH_REGISTRATION'
UNION ALL
SELECT ot.id, 'Land Sale Standard Approval', 1, TRUE
FROM operation_types ot
WHERE ot.code = 'LAND_SALE'
UNION ALL
SELECT ot.id, 'Tax Audit High-Risk Approval', 1, TRUE
FROM operation_types ot
WHERE ot.code = 'TAX_AUDIT'
UNION ALL
SELECT ot.id, 'Critical Investigation Escalated Approval', 1, TRUE
FROM operation_types ot
WHERE ot.code = 'CRITICAL_INVESTIGATION';

INSERT INTO approval_scheme_steps (
    approval_scheme_id,
    step_order,
    role_code,
    review_type,
    is_mandatory,
    timeout_minutes
)
SELECT aps.id, 1, 'SUPERVISOR', 'approval', TRUE, 240
FROM approval_schemes aps
WHERE aps.name = 'Birth Registration Standard Approval'
UNION ALL
SELECT aps.id, 1, 'SUPERVISOR', 'approval', TRUE, 240
FROM approval_schemes aps
WHERE aps.name = 'Land Sale Standard Approval'
UNION ALL
SELECT aps.id, 2, 'SECURITY', 'approval', TRUE, 240
FROM approval_schemes aps
WHERE aps.name = 'Land Sale Standard Approval'
UNION ALL
SELECT aps.id, 1, 'SUPERVISOR', 'approval', TRUE, 240
FROM approval_schemes aps
WHERE aps.name = 'Tax Audit High-Risk Approval'
UNION ALL
SELECT aps.id, 2, 'LEGAL', 'approval', TRUE, 240
FROM approval_schemes aps
WHERE aps.name = 'Tax Audit High-Risk Approval'
UNION ALL
SELECT aps.id, 3, 'COMPLIANCE', 'compliance_review', TRUE, 240
FROM approval_schemes aps
WHERE aps.name = 'Tax Audit High-Risk Approval'
UNION ALL
SELECT aps.id, 1, 'SUPERVISOR', 'approval', TRUE, 120
FROM approval_schemes aps
WHERE aps.name = 'Critical Investigation Escalated Approval'
UNION ALL
SELECT aps.id, 2, 'SECURITY', 'approval', TRUE, 120
FROM approval_schemes aps
WHERE aps.name = 'Critical Investigation Escalated Approval'
UNION ALL
SELECT aps.id, 3, 'LEGAL', 'approval', TRUE, 120
FROM approval_schemes aps
WHERE aps.name = 'Critical Investigation Escalated Approval'
UNION ALL
SELECT aps.id, 4, 'SENIOR_AUTH', 'special_authorization', TRUE, 120
FROM approval_schemes aps
WHERE aps.name = 'Critical Investigation Escalated Approval';

INSERT INTO appointments (
    operation_type_id,
    citizen_identifier,
    citizen_display_name,
    scheduled_start_at,
    scheduled_end_at,
    status,
    created_by_user_id
)
SELECT
    ot.id,
    'CIT-1001',
    'Ali Veli',
    CURRENT_TIMESTAMP,
    CURRENT_TIMESTAMP + INTERVAL '30 minutes',
    'active',
    u.id
FROM operation_types ot
JOIN users u ON u.username = 'ayse.yilmaz'
WHERE ot.code = 'LAND_SALE'
UNION ALL
SELECT
    ot.id,
    'CIT-2002',
    'Fatma Kaya',
    CURRENT_TIMESTAMP,
    CURRENT_TIMESTAMP + INTERVAL '45 minutes',
    'scheduled',
    u.id
FROM operation_types ot
JOIN users u ON u.username = 'ayse.yilmaz'
WHERE ot.code = 'TAX_AUDIT';

INSERT INTO appointment_targets (appointment_id, target_type, target_identifier, is_primary)
SELECT a.id, 'citizen', a.citizen_identifier, TRUE
FROM appointments a;

INSERT INTO sessions (
    appointment_id,
    current_assigned_user_id,
    status,
    activated_at,
    expires_at,
    last_activity_at
)
SELECT
    a.id,
    u.id,
    'active',
    CURRENT_TIMESTAMP,
    CURRENT_TIMESTAMP + INTERVAL '30 minutes',
    CURRENT_TIMESTAMP
FROM appointments a
JOIN users u ON u.username = 'ayse.yilmaz'
WHERE a.citizen_identifier = 'CIT-1001';

INSERT INTO session_assignments (
    session_id,
    user_id,
    assignment_type,
    assignment_reason,
    assigned_by_user_id,
    assigned_at,
    is_current
)
SELECT
    s.id,
    assigned_user.id,
    'primary',
    'Primary appointment owner assigned at session activation.',
    assigned_by.id,
    s.activated_at,
    TRUE
FROM sessions s
JOIN users assigned_user ON assigned_user.username = 'ayse.yilmaz'
JOIN users assigned_by ON assigned_by.username = 'kemal.sahin'
JOIN appointments a ON a.id = s.appointment_id
WHERE a.citizen_identifier = 'CIT-1001';

INSERT INTO queries (
    session_id,
    requested_by_user_id,
    operation_type_id,
    security_level_id,
    target_type,
    target_identifier,
    request_classification,
    justification,
    is_emergency,
    is_override,
    status,
    requested_at,
    decided_at,
    executed_at
)
SELECT
    s.id,
    u.id,
    ot.id,
    sl.id,
    'citizen',
    'CIT-1001',
    'in_context',
    'Citizen record lookup during active appointment workflow.',
    FALSE,
    FALSE,
    'executed',
    CURRENT_TIMESTAMP,
    CURRENT_TIMESTAMP,
    CURRENT_TIMESTAMP
FROM sessions s
JOIN users u ON u.username = 'ayse.yilmaz'
JOIN operation_types ot ON ot.code = 'LAND_SALE'
JOIN security_levels sl ON sl.level_code = 0
WHERE s.status = 'active'
UNION ALL
SELECT
    NULL,
    u.id,
    ot.id,
    sl.id,
    'citizen',
    'CIT-3003',
    'out_of_context',
    'Historical property review requested to resolve a transfer discrepancy.',
    FALSE,
    FALSE,
    'approved',
    CURRENT_TIMESTAMP,
    CURRENT_TIMESTAMP,
    NULL
FROM users u
JOIN operation_types ot ON ot.code = 'LAND_SALE'
JOIN security_levels sl ON sl.level_code = 2
WHERE u.username = 'ayse.yilmaz'
UNION ALL
SELECT
    NULL,
    u.id,
    ot.id,
    sl.id,
    'citizen',
    'CIT-4004',
    'out_of_context',
    'Requested external tax record inspection related to suspected fraud.',
    FALSE,
    FALSE,
    'denied',
    CURRENT_TIMESTAMP,
    CURRENT_TIMESTAMP,
    NULL
FROM users u
JOIN operation_types ot ON ot.code = 'TAX_AUDIT'
JOIN security_levels sl ON sl.level_code = 3
WHERE u.username = 'ayse.yilmaz';

INSERT INTO approval_requests (
    query_id,
    approval_scheme_id,
    status,
    requested_at,
    completed_at
)
SELECT q.id, aps.id, 'approved', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
FROM queries q
JOIN operation_types ot ON ot.id = q.operation_type_id
JOIN approval_schemes aps ON aps.operation_type_id = ot.id AND aps.is_active = TRUE
WHERE q.target_identifier = 'CIT-3003'
UNION ALL
SELECT q.id, aps.id, 'denied', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
FROM queries q
JOIN operation_types ot ON ot.id = q.operation_type_id
JOIN approval_schemes aps ON aps.operation_type_id = ot.id AND aps.is_active = TRUE
WHERE q.target_identifier = 'CIT-4004';

INSERT INTO approvals (
    approval_request_id,
    approval_scheme_step_id,
    approver_user_id,
    decision,
    reason,
    decided_at
)
SELECT ar.id, ass.id, u.id, 'approved', 'Business need validated by supervisor.', CURRENT_TIMESTAMP
FROM approval_requests ar
JOIN queries q ON q.id = ar.query_id
JOIN approval_scheme_steps ass ON ass.approval_scheme_id = ar.approval_scheme_id AND ass.step_order = 1
JOIN users u ON u.username = 'mehmet.demir'
WHERE q.target_identifier = 'CIT-3003'
UNION ALL
SELECT ar.id, ass.id, u.id, 'approved', 'Security review completed with no blocking issue.', CURRENT_TIMESTAMP
FROM approval_requests ar
JOIN queries q ON q.id = ar.query_id
JOIN approval_scheme_steps ass ON ass.approval_scheme_id = ar.approval_scheme_id AND ass.step_order = 2
JOIN users u ON u.username = 'selin.kaya'
WHERE q.target_identifier = 'CIT-3003'
UNION ALL
SELECT ar.id, ass.id, u.id, 'approved', 'Initial business necessity confirmed.', CURRENT_TIMESTAMP
FROM approval_requests ar
JOIN queries q ON q.id = ar.query_id
JOIN approval_scheme_steps ass ON ass.approval_scheme_id = ar.approval_scheme_id AND ass.step_order = 1
JOIN users u ON u.username = 'mehmet.demir'
WHERE q.target_identifier = 'CIT-4004'
UNION ALL
SELECT ar.id, ass.id, u.id, 'denied', 'Legal basis insufficient for requested out-of-context access.', CURRENT_TIMESTAMP
FROM approval_requests ar
JOIN queries q ON q.id = ar.query_id
JOIN approval_scheme_steps ass ON ass.approval_scheme_id = ar.approval_scheme_id AND ass.step_order = 2
JOIN users u ON u.username = 'murat.arslan'
WHERE q.target_identifier = 'CIT-4004';

INSERT INTO audit_logs (
    event_type,
    entity_type,
    entity_id,
    actor_user_id,
    occurred_at,
    ip_address,
    device_identifier,
    details
)
SELECT
    'QUERY_EXECUTED',
    'query',
    q.id,
    u.id,
    CURRENT_TIMESTAMP,
    '10.10.1.25',
    'WS-REG-01',
    'In-context query executed during active session.'
FROM queries q
JOIN users u ON u.username = 'ayse.yilmaz'
WHERE q.target_identifier = 'CIT-1001'
UNION ALL
SELECT
    'APPROVAL_REQUEST_CREATED',
    'approval_request',
    ar.id,
    u.id,
    CURRENT_TIMESTAMP,
    '10.10.1.25',
    'WS-REG-01',
    'Out-of-context request routed to approval workflow.'
FROM approval_requests ar
JOIN queries q ON q.id = ar.query_id
JOIN users u ON u.username = 'ayse.yilmaz'
WHERE q.target_identifier = 'CIT-3003'
UNION ALL
SELECT
    'QUERY_DENIED',
    'query',
    q.id,
    u.id,
    CURRENT_TIMESTAMP,
    '10.10.1.25',
    'WS-REG-01',
    'Out-of-context query denied after approval workflow.'
FROM queries q
JOIN users u ON u.username = 'ayse.yilmaz'
WHERE q.target_identifier = 'CIT-4004';

