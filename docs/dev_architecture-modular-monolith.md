---
title: "[DEV] Architecture & Guidelines: The Modular Monolith"
description: "Guidelines and structure for the Intranet Portal's internal Modular Monolith."
date: "2026-04-08"
author: "Architecture Team"
---

# The Modular Monolith: Architecture & Structure

## The Big Picture: The "Central Sun"
The Intranet Portal uses a **Hybrid Architecture**, where this Modular Monolith serves as the "Central Sun." It inherently owns the absolute source of truth for global Authentication (JWTs), HR Employee Data, and the Enterprise RBAC Matrix. All other standalone Microservices operate as satellites orbiting this core codebase and database.

**When to Build in the Monolith:**
You MUST build your new feature inside this Modular Monolith if:
1. It actively mutates or tightly orchestrates foundational System/HR data.
2. It requires immediate, native, transactional SQL consistency (e.g., hard `INNER JOINS`) against core HR/Site/Role/User tables.
3. It represents a mission-critical, deeply integrated platform feature (e.g., Assets Management, Main Hub).

*(If your feature does NOT meet these strict criteria, you should instead scaffold it as a detached satellite. Refer to `dev_architecture-microservices.md` for guidance).*

---

As part of the Intranet Portal's Hybrid Architecture strategy, tightly-coupled core components and foundational legacy systems proudly reside within this central **Modular Monolith.** 

While these components exist in the exact same Git repository and connect to a completely shared SQL database, they **must** strictly enforce logical, domain-driven boundaries to prevent massive tangles of spaghetti code as the company scales.

---

## 1. Frontend Structure (Next.js)

The Next.js Application strictly utilizes App Router **Route Groups** to logically segment the application's domains without magically altering the user-facing URLs.

- **Route Group Isolation**: The `src/app/` directory is split into domains:
  - `src/app/(core)/`: Houses the central backbone (e.g., `/login`, `/dashboard`, `/admin/users`, `/admin/roles`).
  - `src/app/(assets)/`: Houses the massive Assets Management domain (e.g., `/assets`, `/admin/assets-management/`).
- **Component Segregation**: UI Components securely belong to their respective domains:
  - `src/components/core/`
  - `src/components/assets/`
- **Server Actions**: Server Actions follow the exact same pattern and must be exclusively grouped by module functionality inside the `src/app/actions/` directory (e.g., `auth.ts`, `assets.ts`, `employees.ts`).
- **Import Aliasing**: To seamlessly withstand inevitable folder restructuring, all internal module imports must utilize absolute aliases (`@/app/...`, `@/components/...`). Do not use brittle relative `../../` paths.

## 2. Backend Structure (.NET API)

The backend API remains a natively single compiled process but structurally isolates modules.

- **Controller Namespaces**: REST output endpoints are physically isolated into domain folders:
  - `IntranetPortal.Api/Controllers/Core/` (e.g., `AuthController.cs`, `UsersController.cs`)
  - `IntranetPortal.Api/Controllers/Assets/` (e.g., `AssetsController.cs`)
- **Data Transfer Objects (DTOs)**: DTOs must live alongside or strictly within their domain namespaces to completely prevent cross-contamination.

## 3. Database Layer (Entity Framework)

The monolith shares a single database schema (`ApplicationDbContext`), but the modeling code is fragmented to uniquely ensure maintainability.

- **Entity Segregation**: Models are cleanly stored in separated directories:
  - `IntranetPortal.Data/Models/Core/`
  - `IntranetPortal.Data/Models/Assets/`
- **Configuration Isolation**: We explicitly forbid massive inline model building inside `OnModelCreating`. Every entity must define its database schema in an isolated `IEntityTypeConfiguration<T>` file logically mapped inside:
  - `IntranetPortal.Data/Data/Configurations/Core/`
  - `IntranetPortal.Data/Data/Configurations/Assets/`
- **Table Naming Rules**: Even though the tables seamlessly share a database, they must use explicit domain prefixes (e.g., `Core_Employees`, `AM_AssetInventories`) to instantly identify table ownership natively in SQL.

## 4. Rule of Thumb for Developers

If you are building a moderately sized new feature and your team has decided it belongs in the Modular Monolith (rather than a Standalone Microservice), **mimic the Assets module design**. Make sure it gets its own Next.js Route Group, its own component folder, its own `Controllers/Module/` block, and its own database prefix. No feature is allowed to bleed indiscriminately into the root folder structure.
