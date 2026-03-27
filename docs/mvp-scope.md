# MVP Scope - Approval Scheme Process

This document defines the first implementation target for the project. The goal of the MVP is to prove the core control model end to end with the smallest meaningful backend slice.

[Turkce surum](mvp-scope.tr.md)

## 1. MVP Goal

The MVP should demonstrate one complete backend flow:

`Unauthorized query -> Level 2 approval chain -> Audit log`

This is the smallest slice that proves the architecture is executable, testable, and reviewable.

## 2. Included in MVP

The MVP includes:

- Session Engine context check
- Access Engine out-of-context detection
- Security level resolution for the requested operation
- Approval Engine with a Level 2 scheme
- Two approval steps: `Supervisor` and `Security Officer`
- Audit log write for the final outcome
- Minimal REST API for demo and test execution

## 3. Excluded from MVP

The MVP does not include:

- Emergency access
- Citizen self-service log access
- Frontend user interface
- Token and session expiry enforcement
- Delegation support
- Repeated denial detection
- Advanced notification or escalation behavior

These items remain valid roadmap targets, but they should not block the first working backend release.

## 4. Expected User Story

A government employee attempts a query outside the active appointment context.

The system:

1. Detects that the request is out of context
2. Resolves the security level for the requested operation
3. Routes the request to the Level 2 approval scheme
4. Executes the approval steps in order
5. Produces an allow or deny decision
6. Writes the outcome to the audit log

## 5. MVP Success Criteria

The MVP is considered complete when:

- The full flow works through API calls
- The approval chain is data-driven rather than hard-coded per request
- The final decision is written to the audit log
- Unit and integration tests cover the core path
- The architecture boundaries remain clean

## 6. Technical Boundaries

Target backend stack:

- `C#`
- `.NET 9`
- REST API
- EF Core

Suggested architecture:

- `Domain`: entities, enums, value objects, core rules
- `Application`: engines, use cases, interfaces
- `Infrastructure`: EF Core, persistence, repository implementations
- `Api`: minimal endpoints and transport concerns

## 7. Why This Scope Is Correct

This scope is intentionally narrow. It proves the central promise of the project without mixing in secondary features too early.

If this flow is stable, later phases can safely add expiry enforcement, emergency access, citizen transparency flows, and frontend experience on top of a tested backend foundation.
