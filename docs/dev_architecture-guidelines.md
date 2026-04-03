---
title: "[DEV] Architecture & Development Guidelines"
description: "To understand the architecture and development guidelines of the Intranet Portal."
date: "2026-04-04"
author: "Architecture Team"
---

# Architecture & Development Guidelines

This document serves as the single source of truth for architectural standards, module layout, and design conventions within the Intranet Portal platform. All developers (and automated AI tooling) must follow these rules when extending the application.

---

## 1. Modular Database Schema & Naming

As the Intranet Portal grows, it must maintain a strict level of separation between distinct operational domains.

- **Table Prefixing**: Every database table MUST possess a prefix aligning with its module.
  - **Core Module (`Core_`)**: Used for the central foundation of the application including Sites, Employees, user authentication, and basic hierarchy. (E.g. `Core_Employees`, `Core_UserAccounts`).
  - **Future Modules**: When creating an entirely new domain context such as "Human Resources" or "Information Technology", choose an appropriate short prefix and stick to it (e.g. `HR_Leaves`, `IT_Tickets`).

This ensures that our database remains highly organized, avoids sudden schema name collision, and makes targeted backups or database sharding significantly easier down the line.

---

## 2. Entity Framework Code Structure

To prevent our `ApplicationDbContext.cs` from becoming an unmanageable monolithic file, we rely on `IEntityTypeConfiguration<T>` to segregate mappings.

- **No Inline Configurations**: You should generally NOT use `modelBuilder.Entity<T>()` explicitly inside `OnModelCreating`.
- **Grouping Configurations**: For any new domain model, create an explicit configuration file and group it by its associated module:
  - Inside the backend data project, place configuration classes into folders under `/Data/Configurations/{ModuleName}/`.
  - Example: `IntranetPortal.Data/Data/Configurations/Core/EmployeeConfiguration.cs`.
- The `ApplicationDbContext` will automatically harvest and apply all configurations via `modelBuilder.ApplyConfigurationsFromAssembly`.

---

## 3. RBAC Design Specifications

The platform is heavily reliant on a specialized Enterprise Role-Based Access Control matrix.

- **Reference Material**: Refer to `docs/enterprise-permissions.md` for a comprehensive breakdown of user inheritance, context scoping, and hierarchical permission checks.
- **Rules for New Modules**: 
  - Do NOT implement isolated security roles for individual modules. Instead, add appropriate new `Permissions` into the core matrix.
  - New controllers or server actions must restrict usage adhering to geographic (`SiteId`) and organizational (`DepartmentId`) borders, making sure delegates (`RoleDelegation`) are fully factored into data retrieval.

---

## 4. Frontend Application Layer Integration (Next.js)

Because the UI is built with Next.js App Router, new UI features must respect the modularity of the backend.

- **Server Actions**: Group them exclusively by domain/module inside the `src/app/actions/` directory (e.g. `src/app/actions/{ModuleName}.ts`). Avoid generic "helper" files that become dumping grounds.
- **Directories**: Sub-modules should reside in their own route directories (e.g. `src/app/{ModuleName}/`). Tightly bound custom components should either sit directly next to the page (`src/app/{ModuleName}/components/`) or be distinctly identifiable in a global components folder.

---

## 5. Backend HTTP API Layer

When bridging the gap between the database layer and frontend, stick to module-oriented APIs.

- **Controllers**: Group HTTP endpoints by logical module. Create dedicated controllers rather than modifying expansive existing ones (use `IntranetPortal.Api/Controllers/{ModuleName}Controller.cs`).
- **Data Transfer Objects (DTOs)**: NEVER leak EF Database Entities directly through the API to the Frontend. You must serialize them to domain-specific DTOs to avoid circular reference loops and accidental security leaks.
