# Security Levels - Approval Scheme Process

This document defines the security-level model used in the Approval Scheme Process. Security levels help the Access Engine evaluate out-of-context requests consistently and determine which approval controls apply before access can be granted.

[Turkce surum](security-levels.tr.md) | [Architecture](architecture.md) | [Flows](flows.md)

## 1. Overview

Every operation type in a government institution can carry a different level of sensitivity, legal exposure, and operational risk. To enforce consistent access governance, each operation type is assigned a security level.

Security levels define:

- Whether approval is required
- How many approval steps are needed
- Which roles must approve
- Whether extra compliance or policy checks apply

This model allows institutions to match approval rigor to the sensitivity of each operation.

## 2. Security Level Model

The default model defines five standard levels, ranging from context-based free access to highly controlled access.

### Level 0 - Context-Based Free Access

Description:
Queries performed within an active session that matches the appointment context.

Characteristics:

- No approval required
- Fast and low-friction
- Fully logged
- Limited to valid session and role constraints

Examples:

- An employee querying the citizen assigned to the current active session
- Accessing records directly related to the appointment workflow

### Level 1 - Low-Risk Out-of-Context Access

Description:
Out-of-context queries with limited sensitivity and low expected impact.

Approval requirements:

- 1 approval step
- Typically approved by a Supervisor or Team Lead

Examples:

- Basic identity verification outside the active session
- Non-sensitive record lookups
- Internal consistency checks with low privacy impact

### Level 2 - Medium-Risk Access

Description:
Out-of-context queries involving personal data or moderately sensitive operational information.

Approval requirements:

- 2 approval steps
- Typically:
  - Step 1: Supervisor
  - Step 2: Security or Compliance Officer

Examples:

- Accessing personal records outside the assigned session
- Reviewing historical transactions
- Cross-department data lookups

### Level 3 - High-Risk Access

Description:
Queries involving legally sensitive categories, protected personal data, or requests with significant institutional risk.

Approval requirements:

- 3 control points
- Typically:
  - Step 1: Supervisor
  - Step 2: Legal
  - Step 3: Data Protection or Compliance review

Examples:

- Accessing records of public officials or protected persons
- Sensitive demographic or health-related data
- Queries with legal, disciplinary, or reputational implications

### Level 4 - Critical Access

Description:
Exceptional or highly sensitive access that requires the strongest level of scrutiny and explicit justification.

Approval requirements:

- Multiple approvals plus special authorization
- Typically:
  - Step 1: Supervisor
  - Step 2: Security
  - Step 3: Legal or Data Protection
  - Step 4: Senior executive or specially authorized authority

Examples:

- Emergency access to highly sensitive citizen records
- Access tied to major investigations or crisis response
- Queries involving especially restricted datasets or high-profile cases

## 3. How Security Levels Influence Access Decisions

When an employee performs a query:

1. The Access Engine checks whether the request is in-context.
2. If the request is out-of-context, the Access Engine identifies the operation type.
3. The Access Engine resolves the security level for that operation.
4. The Access Engine determines whether approval is required.
5. If approval is required, the corresponding approval scheme is loaded and executed.
6. The query is allowed only if all required controls are satisfied.
7. The request, security level, approval result, and outcome are logged.

This keeps access-control behavior predictable across departments and institutions.

## 4. Design Notes

Security levels are most effective when they are:

### Consistent

The same operation type should resolve to the same baseline level unless an explicit policy exception exists.

### Explainable

Employees and approvers should understand why a request was classified at a given level.

### Configurable

Institutions may adapt role mappings, approval chains, and level thresholds to their own governance model.

### Auditable

Security-level decisions should be visible in logs, reports, and later investigations.

## 5. Extensibility

The model is designed to support:

- Institution-specific operation catalogs
- Custom approval chains per operation type
- Extra conditions such as working hours, requester role, or emergency justification
- Future risk scoring or anomaly-detection enhancements

## 6. Summary

Security levels provide a structured and institution-agnostic way to classify operation sensitivity and apply proportional controls.

They help ensure that:

- Low-risk access remains efficient
- Medium- and high-risk access receives stronger oversight
- Critical access is reviewed with the highest level of scrutiny

This model is a core part of secure, context-aware access governance in government systems.
