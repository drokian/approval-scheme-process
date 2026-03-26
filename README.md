# Approval Scheme Process

A context-aware access governance and multi-level approval framework for government institutions.

[Turkce README](README.tr.md) | [Architecture](docs/architecture.md) | [Turkce Mimari](docs/architecture.tr.md)

## Project Status

This repository is currently in the concept and documentation phase.

The current scope includes:

- Vision and problem definition
- High-level system architecture
- Draft approval and access-control model
- Initial roadmap for implementation

The repository now includes a draft schema in `db/schema.sql` and example seed data in `db/seed.sql`.

## Overview

This project proposes a unified access-governance model for public-sector systems that:

- Allows employees to make free queries only within the context of appointment-based transactions
- Requires approval for out-of-context queries based on security levels defined per operation type
- Supports dynamic approval schemes for different operation types
- Provides full logging, traceability, and auditability

The goal is to reduce unauthorized access to personal data, prevent misuse, and move institutions from reactive auditing to proactive control.

## Why This Project Exists

Many government information systems still allow broad internal access and rely mostly on after-the-fact review. This creates risks such as:

- Unauthorized access to personal data
- Curiosity-driven or politically motivated queries
- Inconsistent approval workflows
- Uneven security practices across institutions

Approval Scheme Process introduces a context-based and security-level-driven access model that can be adapted across agencies.

## Repository Structure

- `README.md`: English project overview
- `README.tr.md`: Turkish project overview
- `docs/`: Bilingual architecture, flow, governance, compliance, and data-model documents
- `db/`: Draft relational schema and example seed data

## Core Capabilities

- Context-based access control
- Security levels defined per operation type
- Dynamic approval scheme definition
- Multi-level approval workflows
- Full logging and audit support
- Institution-agnostic architecture

## High-Level Architecture

User -> Appointment System -> Session Engine -> Access Engine -> Approval Engine -> Logging and Audit

## Documentation

- English architecture: [docs/architecture.md](docs/architecture.md)
- Turkish architecture: [docs/architecture.tr.md](docs/architecture.tr.md)
- English flows: [docs/flows.md](docs/flows.md)
- Turkish flows: [docs/flows.tr.md](docs/flows.tr.md)
- English security levels: [docs/security-levels.md](docs/security-levels.md)
- Turkish security levels: [docs/security-levels.tr.md](docs/security-levels.tr.md)
- English glossary: [docs/glossary.md](docs/glossary.md)
- Turkish glossary: [docs/glossary.tr.md](docs/glossary.tr.md)
- English governance: [docs/governance.md](docs/governance.md)
- Turkish governance: [docs/governance.tr.md](docs/governance.tr.md)
- English roles and responsibilities: [docs/roles-and-responsibilities.md](docs/roles-and-responsibilities.md)
- Turkish roles and responsibilities: [docs/roles-and-responsibilities.tr.md](docs/roles-and-responsibilities.tr.md)
- English compliance: [docs/compliance.md](docs/compliance.md)
- Turkish compliance: [docs/compliance.tr.md](docs/compliance.tr.md)
- English data model: [docs/data-model.md](docs/data-model.md)
- Turkish data model: [docs/data-model.tr.md](docs/data-model.tr.md)
- Draft schema: [db/schema.sql](db/schema.sql)
- Example seed data: [db/seed.sql](db/seed.sql)

## Visual Diagrams

- Diagram index: [docs/diagrams/README.md](docs/diagrams/README.md)
- Turkish diagram index: [docs/diagrams/README.tr.md](docs/diagrams/README.tr.md)
- English access-control flow: [docs/diagrams/access-control-flow.svg](docs/diagrams/access-control-flow.svg)
- Turkish access-control flow: [docs/diagrams/access-control-flow.tr.svg](docs/diagrams/access-control-flow.tr.svg)
- English approval workflow: [docs/diagrams/approval-workflow.svg](docs/diagrams/approval-workflow.svg)
- Turkish approval workflow: [docs/diagrams/approval-workflow.tr.svg](docs/diagrams/approval-workflow.tr.svg)
- English architecture overview: [docs/diagrams/architecture-overview.svg](docs/diagrams/architecture-overview.svg)
- Turkish architecture overview: [docs/diagrams/architecture-overview.tr.svg](docs/diagrams/architecture-overview.tr.svg)
- English security levels: [docs/diagrams/security-levels.svg](docs/diagrams/security-levels.svg)
- Turkish security levels: [docs/diagrams/security-levels.tr.svg](docs/diagrams/security-levels.tr.svg)

## Roadmap

- Phase 1: Core documentation and data-model draft
- Phase 2: Access Engine prototype
- Phase 3: Approval Engine implementation
- Phase 4: Logging and audit layer
- Phase 5: Example integrations for public-sector domains

## License

MIT License. Free to use, modify, and distribute.

