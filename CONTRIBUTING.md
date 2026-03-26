# Contributing

Thank you for contributing to `approval-scheme-process`.

This repository is currently documentation-first. Contributions may affect architecture, process flows, compliance assumptions, schema design, seed data, diagrams, and future implementation planning. Please keep changes scoped, explicit, and traceable.

## Working Principles

- Keep English and Turkish documentation aligned when both versions exist.
- Prefer small, reviewable changes over broad mixed updates.
- If a change affects schema, seed data, or core flows, update the related documentation in the same change when possible.
- Use issue templates so requests, defects, and documentation gaps remain structured.

## Issue Template Maintenance

The repository currently uses multiple GitHub issue forms under `.github/ISSUE_TEMPLATE/`.

When updating shared fields across templates, keep them aligned intentionally. In particular:

- The impacted-area checkbox list is intentionally similar across templates.
- If a new domain area is added later, such as `Anomaly Detection`, update all relevant issue forms together.
- If labels, severity handling, or triage rules change, update both the templates and this document.

## Severity Definitions

Severity in issue forms is currently descriptive guidance for triage. It does not yet trigger automatic labeling, routing, or assignment behavior.

### Low

Minor issue, low ambiguity, limited impact, or cosmetic documentation inconsistency.

### Medium

Clear issue or request with moderate impact on documentation, schema understanding, or planned implementation.

### High

Issue or request affecting core flows, governance assumptions, data-model consistency, or multi-document alignment.

### Critical

Issue or request that could invalidate the control model, create a major compliance or security misunderstanding, or block core implementation planning.

When implementation work becomes active, severity handling may later expand to include:

- automated labels
- escalation rules
- assignment conventions
- sprint prioritization guidance

## Branch and Sprint Readiness

This repository is expected to move toward branch-based implementation and sprint planning in later phases. When that starts:

- define branch strategy before parallel feature work grows
- keep issue scope small enough for sprint planning
- make acceptance criteria explicit in issue descriptions
- link implementation branches and pull requests back to the originating issue
