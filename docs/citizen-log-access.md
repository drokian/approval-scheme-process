# Citizen Log Access - Approval Scheme Process

This document defines how a citizen or data subject can request and review access logs related to their own records within the Approval Scheme Process.

[Turkce surum](citizen-log-access.tr.md) | [Compliance](compliance.md) | [Flows](flows.md)

## 1. Purpose

Government institutions may need to provide a controlled way for citizens to see who accessed their records, when access occurred, and under which process the access was performed.

This capability supports transparency, misuse detection, complaint handling, and data-subject rights where local regulation requires it.

## 2. Scope

This flow applies to citizen-facing access-log inquiries, not to internal employee audit review.

The model should support:

- Requests by the citizen for their own access history
- Identity verification before disclosure
- Filtering and masking of sensitive internal details
- Logging of the citizen inquiry itself
- Escalation when the request reveals suspicious access patterns

## 3. Core Principles

- A citizen should only see logs related to their own records
- Disclosure should be policy-controlled and legally bounded
- Internal security-sensitive details should be masked where required
- The institution should preserve a defensible record of what was disclosed
- Suspicious patterns should be reviewable by oversight functions

## 4. Actors

### Citizen or Data Subject

Requests access to their own access history.

### Citizen Service Channel

Receives the request through a portal, call center, branch office, or formal application process.

### Identity Verification Function

Validates that the requester is the correct person or a legally authorized representative.

### Transparency or Audit Review Function

Handles exceptions, complaints, and suspicious results when additional review is required.

## 5. Citizen Log Access Flow

Citizen -> Service Channel -> Identity Verification -> Log Access Service -> Logging and Audit -> Citizen Response

### Steps

1. The citizen submits a request to view access history related to their own records.
2. The service channel verifies identity using the institution's approved verification method.
3. The system checks whether the requester is the data subject or a valid legal representative.
4. The request scope is validated:
   - Requested date range
   - Relevant record or case scope
   - Whether the request exceeds policy limits
5. The Log Access Service queries the underlying audit records for matching events.
6. Disclosure rules are applied before response:
   - Allowed fields are shown
   - Restricted internal notes are masked
   - Security-sensitive technical metadata is omitted when policy requires it
7. The citizen receives the filtered result through the approved response channel.
8. The inquiry itself is logged as a separate auditable event.
9. If the result contains suspicious or disputed access, the case may be escalated for formal review.

## 6. Minimum Citizen-Facing Result Set

The response should usually include:

- Date and time of access
- Institution or unit name
- Operation or purpose category
- Whether the access was in-context, approved, emergency, or overridden
- Complaint or review contact path

The response should usually exclude or restrict:

- Full internal justifications where disclosure is not legally appropriate
- Security-control internals
- Sensitive network or device identifiers
- Internal reviewer-only notes

## 7. Identity and Authorization Rules

- The requester must be strongly identified before disclosure
- Legal representatives should require documented authority
- Bulk disclosure across unrelated people must be blocked
- The request itself should be rate-limited and monitored

## 8. Escalation and Complaint Handling

If the citizen believes access was improper, the institution should support:

- Complaint registration
- Linking the complaint to relevant audit events
- Review by compliance, oversight, or inspection functions
- Preservation of evidence for later investigation

## 9. Logging Requirements

The system should log:

- Who requested the citizen log inquiry
- Which identity-verification method was used
- Which records or period were requested
- What disclosure scope was returned
- Whether escalation or complaint handling was triggered

## 10. Data Model and Policy Notes

Before implementation, institutions should align this flow with:

- Audit-log entity design in [db/schema.sql](/d:/source/drokian/approval-scheme-process/db/schema.sql)
- Retention policy in [compliance.md](compliance.md)
- Oversight and complaint ownership in [governance.md](governance.md)
- Core operational request flows in [flows.md](flows.md)

Institutions may later add dedicated entities for citizen requests, representative authority, complaint cases, and disclosure packages.

## 11. Summary

Citizen log access extends the Approval Scheme Process from internal control to accountable transparency. It should be implemented as a controlled disclosure capability, not as unrestricted raw log access.
