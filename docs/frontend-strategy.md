# Frontend Strategy - Approval Scheme Process

This document captures the current frontend decision and the reasoning behind it.

[Turkce surum](frontend-strategy.tr.md) | [MVP Scope](mvp-scope.md)

## 1. Current Decision

Frontend work should not begin before backend Sprint `S5` is complete.

Reason:

- The MVP is backend-first
- The main risk is domain and workflow correctness, not interface design
- Starting UI work too early would create avoidable rework while engines and contracts are still moving

## 2. Candidate Options

### Option A - Blazor

Strengths:

- Strong alignment with a `.NET`-centric stack
- Good fit if the long-term product will stay inside a Microsoft-heavy public-sector environment
- Shared language and tooling may simplify team operations later

Tradeoffs:

- Smaller frontend ecosystem than React
- Demo polish and rapid UI experimentation may be slower

### Option B - React + Vite + Tailwind

Strengths:

- Fastest path for a polished demo UI
- Strong ecosystem for component composition and testing
- Clear separation between API and client

Tradeoffs:

- Adds a second primary language and toolchain
- Increases architectural surface area earlier

## 3. Recommendation

For the current phase:

- Keep the official MVP backend-only
- Defer the final frontend stack decision until backend Sprint `S5`

If the immediate next goal is a fast demo, prefer `React + Vite + Tailwind`.

If the immediate next goal is long-term institutional alignment with a `.NET`-heavy delivery model, prefer `Blazor`.

## 4. Frontend Start Gate

Frontend planning should begin only when the following are stable:

- Session context contract
- Access evaluation response shape
- Approval action endpoints
- Audit-log output model for demo visibility

## 5. Proposed First Frontend Slice

When frontend work starts, the first UI should stay minimal:

- Query request screen
- Out-of-context decision display
- Approval chain status view
- Final audit-log result view

That keeps frontend work aligned with the same single MVP flow already defined for the backend.
