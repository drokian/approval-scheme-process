# Flows - Approval Scheme Process

This document describes the core operational flows of the Approval Scheme Process. It expands the high-level architecture with step-by-step request handling, approval processing, and logging behavior.

[Turkce surum](flows.tr.md) | [Architecture](architecture.md)

## 1. Scope

These flows are aligned with the control order defined in the architecture document:

1. Session and context validation
2. Employee and role authorization
3. Operation type lookup
4. Security level resolution
5. Approval requirement decision
6. Allow, deny, or send to approval workflow
7. Logging of both request and outcome

## 2. Appointment-Based Transaction Flow (In-Context Flow)

This flow applies when a citizen has a valid appointment and the employee is assigned to the corresponding active session.

Citizen -> Appointment System -> Session Engine -> Employee -> Access Engine -> Logging -> Allowed

### Steps

1. Appointment is created with operation type and initial context, such as citizen, record, or asset.
2. When the appointment time arrives, the Session Engine creates an active session and assigns it to the responsible employee.
3. The employee performs a query through the session-aware interface.
4. The Access Engine validates that:
   - An active session exists
   - The employee is assigned to that session
   - The query target matches the session context
   - The employee role is authorized for the requested operation
5. The request is classified as in-context.
6. No additional approval is required.
7. The query is executed and the request outcome is logged.

## 3. Out-of-Context Query Flow

This flow applies when an employee attempts to query data without a valid session or outside the assigned session context.

Employee -> Access Engine -> Approval Engine -> Logging -> Allowed or Denied

### Steps

1. The employee initiates a query without a valid session, or with a target that does not match the active session context.
2. The Access Engine classifies the request as out-of-context.
3. The Access Engine checks whether the employee role is allowed to request this operation at all.
4. The Access Engine identifies the operation type.
5. The Access Engine resolves the security level, using Security Level Manager data as needed.
6. The Access Engine decides whether approval is required.
7. If approval is required, the request is sent to the Approval Engine.
8. The Approval Engine loads the approval scheme for the operation type and starts the workflow.
9. If all required approvals are granted, the query is allowed.
10. If any step is rejected, expired, or fails policy checks, the query is denied.
11. The request, approval history, and final outcome are logged.

## 4. Approval Workflow Flow

This flow describes how approval steps are executed after the Access Engine determines that approval is required.

Access Engine -> Approval Engine -> Approver(s) -> Approval Engine -> Access Engine

### Steps

1. The Access Engine creates an approval request and passes the request context to the Approval Engine.
2. The Approval Engine loads the approval scheme for the resolved operation type.
3. The Approval Engine creates approval steps in the required order and routes each step to the relevant role.
4. Each approver reviews:
   - Query details
   - Employee justification
   - Operation type
   - Security level
   - Relevant session or context information
5. Each approver approves, rejects, or lets the request expire.
6. The Approval Engine records every decision and determines whether the workflow can continue.
7. If all required steps are approved, the Approval Engine returns an allow decision to the Access Engine.
8. If any required step is rejected or expires, the Approval Engine returns a deny decision.
9. The final decision is logged with the complete approval history.

## 5. Logging and Audit Flow

Any Request or Approval Action -> Logging and Audit Layer -> Reports and Analysis

### Logged Data

- Employee ID
- Timestamp
- Session ID, if any
- Query target
- Operation type
- Security level
- Approval status
- Approval history
- Device and network metadata when available

### Audit Capabilities

- Out-of-context query reports
- High-risk access reports
- Employee-based risk scoring
- Institution-wide access statistics
- Support for later anomaly-detection features

## 6. Edge Case Notes

Operational flows should also define handling for:

- Emergency access with justification
- Approval timeout or expiration
- Approver delegation or unavailability
- Manual override with mandatory post-review
- Repeated denied access attempts
- High-risk access outside working hours

## 7. Summary

These flows define the operational behavior of the Approval Scheme Process:

- In-context queries are fast and low-friction
- Out-of-context queries are controlled through policy and approvals
- Approval workflows are dynamic and role-based
- Logging supports traceability, oversight, and later analysis

Together, these flows provide a consistent and auditable access-governance model for government institutions.
