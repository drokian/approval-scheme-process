# Governance - Approval Scheme Process

This document defines the governance model required to operate the Approval Scheme Process in an institutional environment. It focuses on policy ownership, change control, accountability, and oversight.

[Turkce surum](governance.tr.md)

## 1. Purpose

The Approval Scheme Process is not only a technical architecture. It is also a governance model. For the system to be trustworthy, institutions must define who owns the rules, who can change them, who reviews their effectiveness, and how misuse is handled.

## 2. Governance Objectives

The governance model should ensure that:

- Access rules are approved by authorized stakeholders
- Security levels are assigned consistently
- Approval schemes are reviewed and maintained
- Emergency and override paths are tightly controlled
- Logs are reviewable and defensible during audits
- Policy changes are traceable over time

## 3. Governance Bodies

### Business Process Owner

Responsible for defining the meaning and scope of operation types, and for validating business need.

### Information Security Function

Responsible for risk classification, security control requirements, monitoring expectations, and review of high-risk access models.

### Legal and Compliance Function

Responsible for reviewing legal basis, data protection implications, and policy alignment with applicable regulation.

### System Administration Function

Responsible for implementing approved configuration changes in controlled environments and maintaining operational integrity.

### Internal Audit or Oversight Function

Responsible for independent review of logs, policy adherence, and misuse indicators.

## 4. Governance Scope

Governance must cover at least:

- Operation type catalog
- Security level assignments
- Approval scheme definitions
- Role mappings
- Emergency access policy
- Override policy
- Logging and retention policy
- Review and audit cadence

## 5. Change Control

Any change to the control model should follow a documented process:

1. Change request is submitted with business justification
2. Risk and compliance impact is assessed
3. Required approvers review the proposed change
4. Approved changes are implemented in a controlled release process
5. The change is logged with who approved it and when
6. Post-change review confirms that expected controls remain effective

Examples of controlled changes:

- Adding a new operation type
- Changing the security level of an existing operation
- Updating an approval chain
- Adding a new emergency override role

## 6. Review Cadence

Institutions should define recurring reviews for:

- Security-level assignments
- Approval scheme effectiveness
- Dormant or rarely used approval paths
- Emergency access events
- Override usage
- Repeated denied requests
- Access patterns outside working hours

Recommended minimum cadence:

- Monthly operational review
- Quarterly policy review
- Annual control effectiveness review

## 7. Segregation of Duties

No single role should control the entire chain of classification, approval, execution, and review.

At minimum:

- The requester must not approve their own request
- Policy authors should not be the sole reviewers of policy outcomes
- Emergency access should require post-event review by an independent function
- Audit reviewers should have read-only review authority, not operational override authority

## 8. Exception Management

Exceptions should be explicit, time-bound, and reviewable.

Every exception should include:

- Business justification
- Requested duration
- Scope of access
- Named approver
- Post-event review requirement

## 9. Evidence and Auditability

The governance model should require evidence for:

- Why a security level was assigned
- Why an approval scheme was approved
- Who changed a rule
- Who approved an exception
- Whether a review was performed on schedule

## 10. Summary

The Approval Scheme Process can only function reliably in government institutions if the technical model is backed by a clear governance model.

Governance turns the architecture from a conceptual control pattern into an accountable institutional process.
