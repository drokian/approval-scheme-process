# Edge Cases - Approval Scheme Process

This document defines the operational edge-case flows that must be handled explicitly in the Approval Scheme Process. These cases are especially important in government environments because they affect legality, accountability, and continuity of service.

[Turkish version](edge-cases.tr.md) | [Architecture](architecture.md) | [Flows](flows.md) | [Session and Token Expiry](session-and-token-expiry.md)

## 1. Purpose

Core flows alone are not enough for a production-ready control model. Institutions must also define what happens when normal approval or session assumptions break down.

This document covers the minimum edge-case set that should be documented before implementation:

- Emergency access
- Approval timeout or expiration
- Approver unavailable or delegated review
- Manual override
- Repeated denied requests
- High-risk access outside working hours

## 2. Common Control Rules

All edge-case flows should follow these baseline rules:

- Normal in-context access remains the default path
- Edge-case handling must be exceptional, not routine
- Every exception path requires stronger logging than normal access
- Time limits, justification, and responsible actors must be explicit
- Post-event review should be mandatory for high-risk exceptions

## 3. Emergency Access Flow

This flow applies when access is needed immediately to prevent harm, maintain continuity, or support a legally authorized urgent public duty.

### Trigger Conditions

- Immediate operational risk exists
- Waiting for the normal approval chain would create unacceptable harm or delay
- The requester states an emergency justification

### Flow

1. The requester submits an out-of-context request and marks it as emergency access.
2. The Access Engine validates whether the requester role is allowed to initiate emergency access at all.
3. The request is classified using the normal operation type and security level model.
4. Emergency policy rules are applied:
   - Some operation types may still be blocked entirely
   - Some levels may require a shortened emergency approval chain
   - Some levels may allow immediate temporary access with mandatory later review
5. If emergency access is allowed, the system records the justification, emergency flag, requester identity, and timestamp before execution.
6. The request is executed only for the minimum necessary target scope.
7. A mandatory post-event review is created automatically.

### Required Controls

- Explicit emergency justification
- Restricted role eligibility
- Limited target scope
- Stronger log retention and review priority
- Mandatory post-event review by an independent function

## 4. Approval Timeout or Expiration Flow

This flow applies when one or more approval steps are not completed in time.

### Trigger Conditions

- A step has a configured expiry window
- The assigned approver does not respond before the deadline

### Flow

1. The Approval Engine tracks timeout values per approval step.
2. If the deadline passes without a decision, the step is marked as expired.
3. The workflow stops unless a policy-defined fallback exists.
4. The Approval Engine returns a deny or expired outcome to the Access Engine.
5. The request is not executed.
6. The request, timeout event, and final outcome are logged.

### Required Controls

- Expiry time defined per step or per scheme
- Clear expired status in logs and reports
- Optional resubmission path instead of silent retry
- No implicit approval by inactivity

## 5. Approver Unavailable or Delegation Flow

This flow applies when the designated approver cannot review the request because of leave, outage, reassignment, or other justified unavailability.

### Trigger Conditions

- Approver is unavailable
- A valid delegation rule exists

### Flow

1. The Approval Engine identifies that the designated approver is unavailable.
2. The system checks whether a valid pre-approved delegate exists for that role and period.
3. If a valid delegate exists, the step is reassigned to the delegate.
4. If no valid delegate exists, one of the following policy outcomes should apply:
   - The request remains pending until timeout
   - The request is reassigned by an authorized administrator
   - The request is denied for lack of available control authority
5. Delegation details are logged with original approver, delegate, time range, and reason.

### Required Controls

- Delegation must be explicit and time-bound
- Delegates must have equal or higher review authority
- Requesters must not select their own delegates
- Delegation changes must be auditable

## 6. Manual Override Flow

This flow applies when the system allows a specially authorized actor to override the normal decision path.

### Trigger Conditions

- Normal approval path cannot satisfy urgent legal or operational need
- Override authority is granted by institutional policy

### Flow

1. A specially authorized actor initiates an override action.
2. The system validates whether override rights exist for the requester and operation type.
3. Override justification and legal or administrative basis are recorded.
4. The request is executed with override markers attached to the transaction.
5. A mandatory post-review case is created automatically.
6. Override usage is included in periodic oversight reporting.

### Required Controls

- Override rights limited to specific roles
- Strong justification requirement
- Automatic alerting or escalation
- Mandatory independent post-review
- No hidden override path

## 7. Repeated Denied Requests Flow

This flow applies when the same requester repeatedly attempts access after denial.

### Trigger Conditions

- Multiple denied or expired requests for the same target, operation, or period
- Policy-defined retry threshold exceeded

### Flow

1. The Access Engine or monitoring layer detects repeated denied attempts.
2. The system evaluates whether the attempts exceed alert thresholds.
3. If thresholds are exceeded, additional controls may apply:
   - Temporary request throttling
   - Manager or security notification
   - Manual review before any new approval request is accepted
4. Repeated attempts are grouped in audit reporting.

### Required Controls

- Threshold definitions
- Pattern-based alerting
- Separation between legitimate retry and suspicious behavior
- Reviewable evidence trail

## 8. High-Risk Access Outside Working Hours Flow

This flow applies when high-risk or critical requests are made outside standard operating hours.

### Trigger Conditions

- Security level is high-risk or critical
- Request occurs outside policy-defined working hours

### Flow

1. The Access Engine resolves the operation type and security level.
2. Working-hours policy is evaluated using the request timestamp.
3. If the request is outside allowed hours, one of the following policies should apply:
   - Additional approval step is required
   - Emergency justification becomes mandatory
   - The request is blocked unless an exception policy applies
4. The outside-hours condition is recorded in the final log entry and review reports.

### Required Controls

- Working-hours policy per institution
- Time-zone aware evaluation
- Additional scrutiny for high-risk levels
- Separate reporting for off-hours activity

## 9. Minimum Data and Logging Requirements

All edge-case flows should record at least:

- Requester identity
- Target and operation type
- Security level
- Triggering edge-case type
- Justification
- Decision path taken
- Responsible reviewer or delegate
- Timestamps for request, decision, execution, and post-review

## 10. Implementation Notes

Before implementation, institutions should align these edge-case flows with:

- Session and token expiry policy
- Schema support for delegation and assignment history
- Compliance reporting requirements
- Governance review cadence

The dedicated baseline for session and token lifetime handling is documented in [session-and-token-expiry.md](session-and-token-expiry.md).

## 11. Summary

Edge-case flows define how the model behaves when normal assumptions fail. Without them, the Approval Scheme Process remains incomplete for real institutional use.

These flows should be treated as mandatory design inputs for schema stabilization, policy design, and audit reporting.
