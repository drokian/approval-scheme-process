# Backend Sprint Plan - Approval Scheme Process

This document defines the initial backend implementation sequence for the MVP.

[Turkce surum](backend-sprints.tr.md) | [MVP Scope](mvp-scope.md)

## 1. Planning Assumptions

- Sprint size is intentionally small
- Each sprint should fit into `1-2` focused hours
- Each sprint should usually map to `3-5` issues
- The primary objective is traceable progress, not large batches of code

## 2. Target Solution Structure

```text
ApprovalSchemeProcess.sln
├── src/
│   ├── ApprovalSchemeProcess.Domain
│   ├── ApprovalSchemeProcess.Application
│   ├── ApprovalSchemeProcess.Infrastructure
│   └── ApprovalSchemeProcess.Api
└── tests/
    ├── ApprovalSchemeProcess.UnitTests
    ├── ApprovalSchemeProcess.IntegrationTests
    └── ApprovalSchemeProcess.ArchitectureTests
```

Layer rules:

- `Domain` depends on nothing
- `Application` depends only on `Domain`
- `Infrastructure` implements application-facing contracts
- `Api` handles HTTP and composition concerns

## 3. Sprint Sequence

## S1 - Solution Setup and Persistence Foundation

Scope:

- Create the solution and projects
- Configure EF Core
- Add the initial schema mapping
- Add seed migration support

Output:

- Compiling solution
- Working database bootstrap
- Entities mapped to persistence

## S2 - Session Engine

Scope:

- Implement session-related domain and application logic
- Add `IsInContext()` behavior
- Cover main context-validation cases with unit tests

Output:

- Session context evaluation service
- Unit-tested appointment/session validation path

## S3 - Access Engine

Scope:

- Detect out-of-context requests
- Resolve security level for requested operation
- Produce decision-ready access evaluation results

Output:

- Access evaluation pipeline
- Decision object suitable for approval routing
- Unit tests for context and security-level combinations

## S4 - Approval Engine

Scope:

- Load approval scheme definitions
- Execute a Level 2 chain
- Support sequential `Supervisor` and `Security Officer` steps

Output:

- Data-driven approval scheme execution
- Deterministic step-order handling
- Tests for approved and denied outcomes

## S5 - Logging Layer

Scope:

- Write audit records for the end-to-end flow
- Capture request, decision, approval, and outcome metadata
- Connect logging to the engines

Output:

- Persisted audit log entries
- End-to-end backend flow without UI
- Integration coverage for the core scenario

## S6 - API Layer

Scope:

- Expose minimal REST endpoints for the MVP flow
- Wire the engines into request handlers
- Make the core scenario runnable as a demo

Output:

- Minimal API surface
- Integration tests using `WebApplicationFactory`
- Demo-ready backend slice

## 4. Recommended Test Distribution

- `UnitTests`: engine logic, rule evaluation, approval-step behavior
- `IntegrationTests`: endpoint contracts, database-backed flow verification
- `ArchitectureTests`: layer dependency enforcement with `NetArchTest`

## 5. Recommended Definition of Done

Each sprint should finish with:

- Code committed through the documented branch workflow
- Tests added or updated
- Relevant English and Turkish docs updated when behavior changes
- A clear demo or verification note in the PR

## 6. Next Planning Step

After these backend sprints are accepted, the next planning task should be to split each sprint into issue-sized demo slices with explicit acceptance criteria.
