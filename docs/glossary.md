# Glossary - Approval Scheme Process

This document defines the core terms used across the Approval Scheme Process documentation set. It is intended to keep architecture, process, and policy documents consistent.

[Turkce surum](glossary.tr.md)

## Core Terms

### Access Engine

The primary decision-making component that evaluates whether a request is in-context or out-of-context, resolves the security level, and determines whether approval is required.

### Approval

A formal decision recorded by an authorized approver as part of an approval workflow. An approval may result in allow, deny, or expiration depending on policy.

### Approval Engine

The component that executes approval workflows, routes approval steps to the correct roles, records decisions, and returns a final result to the Access Engine.

### Approval Request

A request created when an employee attempts an out-of-context action that requires one or more approval steps before the action can proceed.

### Approval Scheme

A configured workflow that defines which approval steps apply to a specific operation type or risk level.

### Approval Step

A single checkpoint within an approval scheme, usually assigned to a role such as Supervisor, Security, Legal, or Data Protection.

### Appointment

A scheduled citizen interaction that provides the initial business context for a transaction, including operation type and relevant target information.

### Audit

The activity of reviewing logged events, approvals, and access decisions to verify compliance, detect misuse, and support investigation.

### Context

The set of business conditions that make a request legitimate without extra approval, such as a valid appointment, an active session, an assigned employee, and a matching query target.

### Employee

An authorized staff member who performs queries or operational actions within institutional systems.

### Emergency Access

A controlled exception path used when urgent access is needed outside normal approval timing. Emergency access must always require justification and post-review.

### In-Context Request

A request performed within a valid session and matching the active transaction context. In-context requests do not require additional approval if all policy checks pass.

### Logging and Audit Layer

The component or service responsible for storing request, approval, and access outcome data for traceability and later review.

### Operation Type

A business-defined category of work such as land sale, tax audit, birth registration, or social assistance review. Operation type is used to resolve security level and approval scheme.

### Out-of-Context Request

A request made without a valid session, with mismatched target context, or outside the normally permitted transaction boundary. Such requests are subject to additional controls.

### Override

A policy-controlled action that bypasses the standard path under explicit authority. Overrides must be strictly logged and reviewed after execution.

### Query

A request to access, retrieve, or inspect data in an institutional system.

### Role

A defined responsibility or authority class assigned to a user, such as Employee, Supervisor, Security Officer, Legal Reviewer, or Data Protection Officer.

### Security Level

A classification assigned to an operation type that determines the required strictness of approval and oversight for out-of-context access.

### Security Level Manager

The configuration domain responsible for maintaining security levels, mapping them to operation types, and supporting risk classification logic.

### Session

An active work context created from an appointment and assigned to an employee. A session establishes the boundary for in-context access.

### Session Engine

The component responsible for creating, maintaining, and closing sessions tied to appointment-based work.

## Usage Note

When a term appears in multiple documents, the meaning defined here should be treated as the default interpretation unless a document explicitly narrows the scope for a specific use case.
