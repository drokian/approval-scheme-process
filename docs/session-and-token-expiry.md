# Session and Token Expiry Policy - Approval Scheme Process

This document defines the baseline policy for session lifetime, token lifetime, expiration behavior, and related control responses in the Approval Scheme Process.

[Turkce surum](session-and-token-expiry.tr.md) | [Architecture](architecture.md) | [Edge Cases](edge-cases.md)

## 1. Purpose

The Approval Scheme Process depends on reliable control of active work sessions and authentication or authorization tokens. Without a clear expiry policy, in-context access boundaries become weak, auditability becomes ambiguous, and unattended sessions can create misuse risk.

This document defines the minimum policy expectations before implementation.

## 2. Scope

This policy applies to:

- Appointment-backed work sessions
- User authentication sessions
- Access tokens and refresh tokens where token-based authentication is used
- Approval-review sessions for approvers
- Timeout and expiration handling in user interfaces and backend services

## 3. Core Principles

- Session validity must be time-bound, not indefinite
- Token lifetime should be proportional to risk
- Expired sessions and expired tokens must fail closed
- Re-authentication should be required for sensitive actions after inactivity or long duration
- Expiration events should be visible in logs and reviewable in audits

## 4. Session Types

### Appointment Work Session

The active business context used by an employee during an appointment-based interaction.

### User Authentication Session

The signed-in application session that proves the employee identity to institutional systems.

### Approval Review Session

The session used by an approver when reviewing pending approval requests.

## 5. Recommended Session Policy

### Appointment Work Session Rules

- A session should not become active before the allowed pre-start window
- A session should auto-close when the appointment ends unless a short controlled grace period is defined
- A session should close immediately when the appointment is cancelled or marked no-show
- A session should be invalid for in-context access once closed or expired

### Idle Timeout

- Employee-facing sessions should expire after a defined inactivity period
- Higher-risk interfaces should use shorter idle timeout values
- Returning from idle timeout should require re-authentication before more access is granted

### Absolute Lifetime

- Sessions should also have a maximum absolute lifetime even if the user remains active
- Long-running sessions should require renewal rather than remaining open indefinitely

## 6. Token Policy

If the implementation uses tokens, institutions should distinguish at least the following:

### Access Token

- Short-lived
- Used for ordinary request authorization
- Must expire automatically without manual revocation dependency

### Refresh Token

- Longer-lived than the access token
- Stored and protected more strictly
- Revocable independently
- Invalidated on logout, compromise, or policy-triggered re-authentication

### Step-Up or Re-Authentication Token

- Used for especially sensitive actions such as override, emergency access, or approval of high-risk requests
- Very short-lived
- Bound to a recent authentication event

## 7. Expiration Triggers

At minimum, the system should support expiration or invalidation on:

- Idle timeout
- Absolute session lifetime reached
- Appointment end time reached
- Manual logout
- Password reset or credential reset
- Administrative disablement
- Role removal where continued access would no longer be valid
- Token compromise or suspected session hijack

## 8. Expiration Behavior

When session or token expiration occurs:

1. The current request should fail safely if it can no longer be completed under valid credentials or context.
2. The interface should clearly tell the user whether the cause was idle timeout, absolute expiry, or appointment/session closure.
3. In-context access should not silently fall back to out-of-context access.
4. Any unfinished approval or data-access attempt should require a fresh authorized retry.
5. The event should be logged with enough detail for later review.

## 9. In-Context Access Implications

For appointment-based access, session expiry has direct control impact:

- If the work session expires, in-context classification should no longer be possible
- A new session should require fresh validation of appointment state and assignment
- Queries started after session expiry should be denied or reclassified according to policy, not allowed under stale context

## 10. Approval Workflow Implications

Approval-step timeout and user-session timeout are separate controls and should not be confused.

- Approval-step timeout governs how long a business approval may remain pending
- User-session timeout governs how long an approver may remain signed in without re-authentication
- An expired approver session should not implicitly expire the approval request itself
- An expired approval request should not be reopened just because the user signs in again

## 11. Logging Requirements

The system should log at least:

- Session creation
- Session closure
- Idle timeout expiration
- Absolute expiration
- Token renewal and token revocation events
- Re-authentication for sensitive actions
- Expired request attempts blocked by policy

## 12. Data Model and Implementation Notes

Before implementation, institutions should align this policy with:

- Session lifecycle handling in [flows.md](flows.md)
- Approval timeout behavior in [edge-cases.md](edge-cases.md)
- Draft schema design in [db/schema.sql](/d:/source/drokian/approval-scheme-process/db/schema.sql)

The current schema draft can now represent session expiry, last activity, invalidation reason, and assignment history. Further implementation may still need:

- Token or authentication event tracking
- Stronger token revocation evidence
- Session-authentication linkage for step-up verification

## 13. Summary

Session and token expiry policy is not just a technical security setting. In this model, it is part of the access-control boundary itself. Strong expiry handling helps keep in-context access trustworthy, prevents stale-session misuse, and improves audit defensibility.