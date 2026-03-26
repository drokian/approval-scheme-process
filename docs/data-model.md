# Data Model - Approval Scheme Process

This document describes the conceptual data model behind the Approval Scheme Process. It is intended to bridge the gap between the architecture documents and the relational schema draft in `db/schema.sql`.

[Turkce surum](data-model.tr.md) | [Citizen Log Access](citizen-log-access.md) | [Session and Token Expiry](session-and-token-expiry.md)

## 1. Purpose

The data model captures the minimum set of entities required to support:

- Appointment-based session context
- Out-of-context access evaluation
- Security-level resolution
- Dynamic approval workflows
- Logging and auditability

## 2. Core Entities

### User

Represents an employee or institutional actor using the system.

Key examples of attributes:

- Employee identity
- Display name
- Contact information
- Status

### Role

Represents an authority class assigned to a user, such as Employee, Supervisor, Security Officer, or Auditor.

### UserRole

Maps users to roles and supports time-bounded role assignments where needed.

### OperationType

Represents a business operation category such as land sale, tax audit, birth registration, or social assistance review.

### SecurityLevel

Represents the classification assigned to an operation type for determining approval strictness.

### ApprovalScheme

Defines the approval chain for a specific operation type.

### ApprovalSchemeStep

Defines an individual step in an approval chain, including role, order, and requirement flags.

### Appointment

Represents a scheduled citizen-facing interaction that creates the initial business context.

### AppointmentTarget

Represents the data target associated with an appointment, such as a citizen, asset, record, or case.

### Session

Represents the active work context derived from an appointment. A session may keep a current assigned employee together with lifecycle and expiry metadata.

### SessionAssignment

Represents the assignment history of a session to employees over time, including primary assignment, reassignment, temporary coverage, or delegation-style handoff.

### Query

Represents a data access request made by an employee. A query may be in-context or out-of-context.

### ApprovalRequest

Represents the approval workflow instance created for a query that requires approval.

### Approval

Represents an individual approver decision within an approval workflow.

### AuditLog

Represents the immutable-style evidence trail of access, approval, and administrative events.

## 3. Relationship Overview

The main relationships are:

- A User can hold multiple Roles
- An OperationType maps to one SecurityLevel
- An OperationType can have one or more ApprovalSchemes over time
- An ApprovalScheme contains one or more ApprovalSchemeSteps
- An Appointment can have one or more AppointmentTargets
- An Appointment can produce one Session
- A Session can have one or more SessionAssignments over time
- A Session may have one current assigned User at a given moment
- A Query is requested by one User
- A Query may reference one Session
- A Query may create one ApprovalRequest
- An ApprovalRequest contains one or more Approval records

## 4. Lifecycle Notes

### Appointment to Session

An appointment begins as planned work and becomes an active session when the interaction starts.

Assignment and reassignment events should be preserved as session history rather than overwritten as a single opaque field.

### Query to Approval

An in-context query can be executed directly if policy checks pass.

An out-of-context query is classified, assigned a security level, and may trigger an approval request.

### Approval to Audit

Every approval action and every final access decision should be reflected in the audit trail.

## 5. Integrity Rules

The data model should enforce rules such as:

- A query cannot be marked in-context if its target does not match the session context
- A requester cannot also be the approver of the same approval step
- Approval steps should be unique within a scheme order
- A session should not have multiple current assignees at once
- In-context queries should match the current active session assignment
- A security level must exist before it can be assigned to an operation type

## 6. Extensibility Notes

Institutions may later add entities for:

- Emergency access
- Override review
- Incident management
- Risk scoring
- Citizen log inquiry
- Disclosure package
- External system connectors

## 7. Schema Mapping

The draft relational implementation of this conceptual model is provided in [db/schema.sql](/d:/source/drokian/approval-scheme-process/db/schema.sql).

An example baseline dataset for demonstrations and early testing is provided in [db/seed.sql](/d:/source/drokian/approval-scheme-process/db/seed.sql).