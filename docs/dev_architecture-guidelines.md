---
title: "[DEV] Architecture & Development Guidelines"
description: "To understand the architecture and development guidelines of the Intranet Portal."
date: "2026-04-08"
author: "Architecture Team"
---

# Architecture & Development Guidelines

This document serves as the single source of truth for architectural standards, module layout, and design conventions within the Intranet Portal platform. All developers (and automated AI tooling) must follow these rules when extending the application.

---

## 1. Hybrid Modular Architecture Strategy

The Intranet Portal utilizes a **Hybrid Modular Architecture** bridging a Modular Monolith with future Microservices. When a new module is proposed, the development team must evaluate its nature, scope, and coupling to decide whether it belongs inside the Modular Monolith or outside as a Standalone Microservice.

- **The Core Monolith:** Modules that are deeply coupled with existing core functionalities (like Core and Assets Management) reside in the single main repository. They share a frontend deployment and a backend API, but their codebase must be strictly segregated into domain folders under `(core)` and `(assets)` in Next.js, and respective namespace folders in the `.NET` backend. 
- **Standalone Microservices:** Modules that are naturally isolated (e.g., Drink Ordering) or have distinct scaling requirements should be developed as independent repositories. They will contain their own Next.js frontend, .NET backend API, and a completely separate database, integrating with the Core Monolith for authentication and shared state via APIs. *(For a complete breakdown of this module structure, refer to [docs/dev_architecture-microservices.md](dev_architecture-microservices.md)).*

---

## 2. Modular Database Schema & Naming

As the unified Intranet Portal Monolith grows, it must maintain a strict level of separation between distinct operational domains. 

- **Table Prefixing (For Internal Modules)**: Every database table within the monolithic database MUST possess a prefix aligning with its module.
  - **Core Module (`Core_`)**: Used for the central foundation of the application including Sites, Employees, user authentication, and basic hierarchy. (E.g. `Core_Employees`, `Core_UserAccounts`).
  - **Assets Module (`AM_`)**: Used for the Assets Management domain.
- **Separate Databases (For External Modules)**: When creating an entirely new module (e.g. "Drink Ordering" or "Human Resources"), it must exist in an external repository and connect to its own entirely **separate database**.

---

## 3. Entity Framework Code Structure

To prevent our `ApplicationDbContext.cs` from becoming an unmanageable monolithic file, we rely on `IEntityTypeConfiguration<T>` to segregate mappings.

- **No Inline Configurations**: You should generally NOT use `modelBuilder.Entity<T>()` explicitly inside `OnModelCreating`.
- **Grouping Configurations**: For any domain model, create an explicit configuration file and group it by its associated module:
  - Inside the backend data project, place configuration classes into folders under `/Data/Configurations/{ModuleName}/`.
  - Example: `IntranetPortal.Data/Data/Configurations/Core/EmployeeConfiguration.cs`.
- The `ApplicationDbContext` will automatically harvest and apply all configurations via `modelBuilder.ApplyConfigurationsFromAssembly`.

---

## 4. RBAC Design Specifications

The platform is heavily reliant on a specialized Enterprise Role-Based Access Control matrix.

- **Reference Material**: Refer to `docs/enterprise-permissions.md` for a comprehensive breakdown of user inheritance, context scoping, and hierarchical permission checks.
- **Rules for New Modules**: 
  - Do NOT implement isolated security roles for individual modules. Instead, add appropriate new `Permissions` into the core matrix.
  - New controllers or server actions must restrict usage adhering to geographic (`SiteId`) and organizational (`DepartmentId`) borders, making sure delegates (`RoleDelegation`) are fully factored into data retrieval.

---

## 5. Frontend Application Layer Integration (Next.js)

Because the UI is built with Next.js App Router, new features must respect the modularity of the backend.

- **Route Groups for Internal Modules**: Inside the main Monolith, major modules are segregated using visual boundary Next.js Route Groups (e.g., `src/app/(core)/` and `src/app/(assets)/`).
- **Server Actions**: Group them exclusively by domain/module inside the `src/app/actions/` directory (e.g. `src/app/actions/{ModuleName}.ts`). Avoid generic "helper" files.
- **Micro-Frontend Integration**: Standalone modules hosted in external repositories will be linked back into the Monolith frontend via subdomains or Reverse Proxies, but they will have their own independent Next.js architectures.
- **Sidebar Integration**: The global interface integrates heavily with `Sidebar.tsx`. When building a new module (even external ones), you must register its route prefix within the `activeModule` state in the unified Sidebar.

---

## 6. Backend HTTP API Layer

When bridging the gap between the database layer and frontend, stick to module-oriented APIs.

- **Monolith Controllers**: Inside the main repository, group HTTP endpoints by logical module using namespace folders (use `IntranetPortal.Api/Controllers/Core/` and `IntranetPortal.Api/Controllers/Assets/`).
- **Data Transfer Objects (DTOs)**: NEVER leak EF Database Entities directly through the API to the Frontend. You must serialize them to domain-specific DTOs.

---

## 7. Audit Logging

When creating modules that require tracking of state changes or historical ledgers (e.g., tracking when an asset is moved or returned), **do not** use a generic, centralized generic audit table (e.g., `Core_AuditLogs` with string-based `EntityId`s). 

- **Module-Specific Audit Tables:** Implement strongly-typed audit tables localized to the specific module (e.g., `AM_AssetAuditLogs`). 
- **Referential Integrity:** Ensure the log table uses explicit Foreign Keys pointing back to the core transactional table (e.g., `AssetId` INT FK).

---

## 8. Frontend Iconography

To maintain a lightweight dependency footprint, flawless client-side rendering speed, and strict visual consistency across the entire Intranet Portal, do **not** install 3rd-party React components for icons.

- **No Third-Party Icon Packages:** Packages like `lucide-react`, `@heroicons/react`, or `font-awesome` are strictly prohibited within the `package.json`.
- **Inline SVGs ONLY:** UI Modules must use raw, inline `<svg>` elements exact to the Heroicons specification.
- **Styling:** By default, SVGs should use `fill="none" stroke="currentColor"` and `strokeWidth={2}` (or `strokeWidth={2.5}` for prominent icons) seamlessly inheriting text styles from their parent Tailwind classes.

---

## 9. Frontend Layout Standardization

In order to guarantee that module boundaries behave consistently across the platform irrespective of the domain, enforce standardized root wrappers for any nested module entry point (`layout.tsx` or `page.tsx`).

- **Consistent Container Dimensions:** Every major module (e.g., The Hub, Assets Management, Administration) must wrap its contextual endpoints within a standardized responsive boundary: `<div className="max-w-7xl mx-auto space-y-8">`.
- **No Arbitrary Padding Overrides:** Do not inject stray native paddings (like `p-8` or `px-4`) or nested structural boxes (`min-h-[600px] bg-white`) explicitly into the module's absolute root layout tree unless absolutely necessary for a distinct visual split. Adhering to the universal wrapper ensures identically aligned white spaces and scroll-bars universally across the application ecosystem.
