---
title: "[DEV] Module Development Lifecycle"
description: "How to build and configure a new feature module for the Intranet Portal."
date: "2026-03-19"
author: "Architecture Team"
---

# Module Development Lifecycle

This document outlines the standard "Design-First" workflow to follow when building new feature modules for the Intranet Portal. Following this structured process ensures architectural rules are obeyed, minimizes technical debt, and prevents costly rewrites.

---

## The 4-Step Framework

### Step 1: Requirements Gathering ("Brain Dump")
Before writing any code, formulate a raw blueprint of the module's core functioning. Answer these three core questions:
1. **The Goal:** What business problem does this solve? *(e.g., "An IT Helpdesk ticketing system")*
2. **The Data Ecosystem:** What specific entities and fields need to be tracked? *(e.g., "Ticket title, description, priority, IT staff assignment, completion status")*
3. **The Security/RBAC Constraints:** Who executes tasks? *(e.g., "Any employee can submit a ticket, but only IT staff assigned to the same `SiteId` can close them")*

### Step 2: The Architecture Proposal
With the brain dump complete, immediately draft an **Implementation Plan**. **Stop here and do not write code yet.** 

First, determine the **Deployment Strategy**: Evaluate the module's nature and decide if it belongs inside the **Core Monolith** or if it should be built as a **Standalone Microservice** (refer to `dev_architecture-guidelines.md` and `dev_architecture-microservices.md`).

If the module will live inside the monolith backend, also review `backend/IntranetPortal.Api/README.md` before implementation so the module design accounts for the current API configuration surface and environment expectations.

Translate the raw requirements into the chosen architectural paradigm:
- **Deployment Context:** Explicitly state whether this will be segregated via a Next.js Route Group in the Monolith or scaffolded in a distinct external Git repository.
- **Database Architecture:** If monolithic, define the exact tables and map out their modular `Prefix_` names (e.g., `IT_Tickets`). If a microservice, map out the initial entities of the standalone DB.
- **REST API Map:** Define the API Controller boundaries and strictly encapsulate the Request/Response payloads into secure DTOs.
- **Frontend Map:** Determine where the Server Actions will live and map either the internal Monolith UI hierarchy (`src/app/({module})/`) or the external Micro-Frontend routes.

For backend-facing modules, the API README should be treated as the source of truth for configuration keys and deployment notes, including:
- authentication and token settings under `JwtSettings`
- internal service authentication via `InternalApiSettings:Secret`
- CORS and host restrictions via `AllowedOrigins` and `AllowedHosts`
- optional security and infrastructure keys such as `Security:*` and `ChallengeEncryption:*`

### Step 3: Iteration and Refinement
Subject the Implementation Plan to peer review (or AI iteration). 
- Continually poke holes in the proposal.
- Scrutinize the data models to ensure they plug safely into the global enterprise RBAC matrix.
- Ensure cross-site or cross-department behaviors are fully accounted for.

*Do not advance until you are absolutely confident in the theoretical design.*

### Step 4: Systematic Execution
Once the Implementation Plan is "green-lit", break down the execution into precise technical phases:
1. **Infrastructure Scaffolding:** Skip if monolithic. If building a microservice, provision the new independent Git repository. **Crucial Step:** You must manually copy the `docs/dev_architecture-microservice-cheat-sheet.md` document from the Core codebase natively into your new isolated workspace *before* requesting any AI assistance. This mathematically ensures the agent remembers all JWT integration rules, structural CSS properties, and cross-application database constraints.
2. **Database Layer:** Implement the Entities and their `IEntityTypeConfiguration<T>` files. Generate and successfully apply the Entity Framework core migrations.
3. **API/Backend Layer:** Build out the explicit Module Controllers and their associated DTO serialization patterns. Verify JWT token pipelines if communicating across services. Before shipping, cross-check `backend/IntranetPortal.Api/README.md` so any required configuration keys for the module and its deployment path are documented and aligned with the existing API environment model. **Crucial Integration:** Ensure your APIs explicitly support "System.FullAccess" RBAC fallbacks, and strictly structure a resilient `IHostedService` retry-loop to auto-register any custom modular RBAC Permissions to the core Monolith synchronously upon deployment.
4. **Frontend Layer:** Scaffold the Next.js internal Route Groups (or external Micro-Frontend application layers), wrap up UI components, and bind them to the specific Server Actions.
