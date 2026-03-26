# Development Workflow - Approval Scheme Process

This document defines the branch strategy, commit standard, pull request expectations, and test layers for implementation work in the Approval Scheme Process repository.

[Turkce surum](development-workflow.tr.md) | [Contributing](../CONTRIBUTING.md)

## 1. Purpose

The repository is moving from documentation-first design into structured implementation planning. To keep parallel work stable and reviewable, development should follow a common workflow.

This document provides the baseline workflow for branches, commits, pull requests, and tests.

## 2. Branch Strategy

### `main`

- Stable branch
- Represents the latest reviewed sprint output
- No direct push allowed
- Updated from `develop` at sprint boundaries

### `develop`

- Active integration branch
- Receives completed feature, fix, and documentation work
- Used as the working baseline for the current sprint

### `feature/[issue-no]-[short-description]`

- Used for new feature or implementation work linked to a single issue
- Target merge branch is `develop`

Example:

- `feature/12-session-engine-context-check`

### `fix/[issue-no]-[short-description]`

- Used for normal bug fixes discovered during active development
- Target merge branch is `develop`

### `hotfix/[issue-no]-[short-description]`

- Used only for production-critical fixes affecting the stable branch
- Created from `main`
- Merged back to `main`
- Then back-merged into `develop`

### `docs/[issue-no]-[short-description]`

- Used for documentation-only work
- Target merge branch is normally `develop`
- May be used for README, architecture, flow, governance, or diagram updates

## 3. Branch Flow Rules

- `feature/*` -> `develop`
- `fix/*` -> `develop`
- `docs/*` -> `develop`
- `develop` -> `main` at sprint end
- `hotfix/*` -> `main`, then back-merge `main` into `develop`
- No direct push to `main`

## 4. Commit Message Standard

Conventional Commits should be used.

Recommended types:

- `feat`
- `fix`
- `docs`
- `test`
- `refactor`
- `chore`
- `build`
- `ci`

Recommended format:

`type(scope): short description`

Examples:

- `feat(session): validate appointment context on query`
- `fix(access): resolve null session edge case on expired token`
- `test(approval): add step rejection flow unit tests`
- `docs(schema): align SessionAssignment FK references`
- `refactor(logging): extract audit writer to interface`

## 5. Pull Request Standard

All implementation work should be submitted through a pull request.

Each PR should include:

- Linked issue number
- Clear summary of the change
- Which acceptance criteria are completed
- Test coverage note
- Whether bilingual documentation was updated when required

## 6. Merge Guidance

Recommended merge behavior:

- `feature/*`, `fix/*`, and `docs/*` into `develop`: prefer squash merge
- `develop` into `main`: controlled sprint-end merge after review
- `hotfix/*` into `main`: direct reviewed merge, then immediate back-merge to `develop`

## 7. Test Layers

### Unit Tests

- Framework: `xUnit`
- Purpose: isolated verification of engine and domain logic
- Scope: session rules, access evaluation, approval flow decisions, expiry handling, logging helpers

### Integration Tests

- Framework: `WebApplicationFactory`
- Purpose: verify API behavior and endpoint contracts
- Scope: request handling, approval endpoints, audit endpoints, citizen log inquiry endpoints

### Architecture Tests

- Framework: `NetArchTest`
- Purpose: enforce layer dependency rules
- Scope: application, domain, infrastructure, and API boundaries

## 8. Minimum PR Checklist

Before merging, confirm at least:

- Build succeeds
- Relevant tests pass
- New or changed behavior is linked to an issue
- Acceptance criteria are reviewed
- Documentation is updated when behavior or policy changes
- English and Turkish docs are aligned when both exist

## 9. Sprint Usage Notes

- `develop` should stay releasable enough for sprint integration review
- Large work should be split into issue-sized branches
- Demo flows should map to linked issues and PRs
- Critical production fixes should never bypass the `hotfix/*` rule

## 10. Summary

This workflow keeps implementation controlled while the project grows from documentation into code. The goal is not process overhead, but traceability, review quality, and clean sprint delivery.
