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

Translate the raw requirements into our specific architectural paradigm (refer to `architecture-guidelines.md`):
- **Database Architecture:** Define the exact tables and map out their `Prefix_` names (e.g., `IT_Tickets`).
- **REST API Map:** Define the API Controller boundaries and strictly encapsulate the Request/Response payloads into secure DTOs.
- **Frontend Map:** Determine where the Server Actions will live (`src/app/actions/{Module}.ts`) and the primary UI route hierarchy (`src/app/{Module}/`).

### Step 3: Iteration and Refinement
Subject the Implementation Plan to peer review (or AI iteration). 
- Continually poke holes in the proposal.
- Scrutinize the data models to ensure they plug safely into the global enterprise RBAC matrix.
- Ensure cross-site or cross-department behaviors are fully accounted for.

*Do not advance until you are absolutely confident in the theoretical design.*

### Step 4: Systematic Execution
Once the Implementation Plan is "green-lit", break down the execution into precise technical phases:
1. **Database Layer:** Implement the Entities and their `IEntityTypeConfiguration<T>` files. Generate and successfully apply the Entity Framework core migrations.
2. **API/Backend Layer:** Build out the explicit Module Controllers and their associated DTO serialization patterns. Run isolated endpoints tests if needed.
3. **Frontend Layer:** Scaffold the Next.js routes, wrap up your UI components, and bind them to the module's dedicated Server Actions.
