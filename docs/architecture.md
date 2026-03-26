# Architecture - Approval Scheme Process

This document describes the high-level architecture of the Approval Scheme Process, including the main components, decision flow, and extension points.

[Turkce surum](architecture.tr.md) | [Flows](flows.md) | [Edge Cases](edge-cases.md) | [Session and Token Expiry](session-and-token-expiry.md)

## 1. Overview

Approval Scheme Process is a context-aware access-governance model designed for government institutions.

It is built around four ideas:

- Free access is allowed only within a valid appointment-backed session context
- Out-of-context access is evaluated using security levels
- Approval workflows are defined per operation type
- Every decision is logged for traceability and auditability

The model is institution-agnostic and can be adapted to domains such as land registry, population systems, tax administration, and social services.

## 2. Core Principles

### 2.1 Context-Based Access

Employees may freely query data only when all of the following are true:

- A valid appointment exists
- The appointment has been activated as a session
- The employee is assigned to that session
- The query target matches the session context

### 2.2 Security-Level-Driven Access

Every operation type, such as land sale, birth registration, or tax audit, is assigned a security level.

Security levels determine:

- Whether approval is required
- How many approval steps are needed
- Which roles must approve
- Whether additional controls apply

### 2.3 Dynamic Approval Schemes

Each operation type can define its own approval workflow.

Example patterns:

- Level 1 -> Supervisor approval
- Level 2 -> Supervisor and Security approval
- Level 3 -> Supervisor, Legal, and Data Protection approval
- Level 4 -> Multiple approvals plus special authorization

### 2.4 Full Logging and Auditing

Every query should record:

- Who performed it
- When it was performed
- Session or request context
- Operation type and security level
- Approval status and approval history
- Device and network metadata when available

## 3. High-Level Architecture

User -> Appointment System -> Session Engine -> Access Engine -> Approval Engine -> Logging and Audit

### 3.1 Appointment System

Responsibilities:

- Manage citizen appointments
- Define the requested operation type
- Provide initial business context
- Trigger session creation at appointment time

### 3.2 Session Engine

Responsibilities:

- Convert appointments into active sessions
- Assign sessions to employees
- Maintain session lifecycle such as active, paused, and closed
- Expose context for access validation

Session lifetime and expiration expectations are documented in [session-and-token-expiry.md](session-and-token-expiry.md).

### 3.3 Access Engine

This is the primary decision-making component.

Responsibilities:

- Check whether a query is in-context or out-of-context
- Evaluate role and session constraints
- Determine the security level for the requested operation
- Decide whether approval is required
- Route approval-required requests to the Approval Engine
- Fail fast when required context or authorization is missing

### 3.4 Approval Engine

Responsibilities:

- Retrieve the approval scheme for the operation
- Start approval steps in the defined order
- Notify approvers
- Record approval decisions
- Return allow or deny outcomes to the Access Engine

### 3.5 Security Level Manager

Responsibilities:

- Create and update security levels
- Map security levels to operation types
- Maintain risk classification logic

### 3.6 Approval Scheme Manager

Responsibilities:

- Define approval steps, roles, and ordering
- Support conditional branches where needed
- Validate scheme consistency
- Provide approval chains to the Approval Engine

### 3.7 Logging and Audit Layer

Responsibilities:

- Store access and approval logs
- Support audit reports
- Enable later anomaly-detection extensions
- Help meet KVKK and GDPR-like compliance needs

## 4. Decision Order

To keep the control model predictable, the Access Engine should evaluate requests in this order:

1. Session and context validation
2. Employee and role authorization
3. Operation type lookup
4. Security level resolution
5. Approval requirement decision
6. Allow, deny, or send to approval workflow
7. Logging of both request and outcome

This ordering makes it clear which controls are mandatory before approval can even be considered.

## 5. Component Interaction

### 5.1 In-Context Query Flow

Employee -> Session Engine -> Access Engine -> Logging -> Allowed

Steps:

1. Employee performs a query within an active session
2. Session Engine provides the current session context
3. Access Engine verifies that the target matches the session and role constraints
4. The request is classified as in-context
5. The query is allowed without additional approval
6. The request and outcome are logged

### 5.2 Out-of-Context Query Flow

Employee -> Access Engine -> Approval Engine -> Logging -> Allowed or Denied

Steps:

1. Employee performs a query without a valid session, or with mismatched context
2. Access Engine classifies the request as out-of-context
3. The operation type and security level are resolved
4. The approval scheme is loaded
5. Approval workflow is executed
6. If approved, the query is allowed
7. If rejected or expired, the query is denied
8. The request, approval result, and final outcome are logged

## 6. Exception and Edge Cases

The architecture should explicitly define how to handle:

- Emergency access with elevated justification
- Approval timeout or expiration
- Approver unavailable or delegation scenarios
- Manual override with mandatory post-review
- Repeated denied requests
- High-risk access outside working hours

These cases are especially important in public-sector environments.

Detailed operational handling is documented in [edge-cases.md](edge-cases.md).

## 7. Data Model Status

The high-level entity set currently includes:

- User
- Appointment
- Session
- OperationType
- SecurityLevel
- ApprovalScheme
- ApprovalStep
- Query
- Approval

The repository now includes a draft relational schema in [db/schema.sql](../db/schema.sql). It should be treated as an evolving implementation draft aligned with the conceptual model.

## 8. Extensibility

The architecture is designed to be:

### Modular

Each component can be replaced or extended independently.

### Institution-Agnostic

Each institution can define:

- Its own operation types
- Its own security levels
- Its own approval schemes

### Technology-Agnostic

The backend can be implemented in:

- Python
- Node.js
- Java
- Go
- .NET

### Future Extensions

- Anomaly detection
- Real-time risk scoring
- Cross-institution access federation
- Zero-trust integration

## 9. Summary

Approval Scheme Process provides:

- A unified access-governance model
- Stronger personal-data protection
- Prevention of unauthorized access
- Flexible institution-specific approval workflows
- Full traceability and auditability

This document defines the conceptual foundation for implementing secure, context-aware access control in government systems.
