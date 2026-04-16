# Copilot Workspace Instructions - Intranet Portal

These rules are mandatory when generating, refactoring, or modifying code in this workspace.
They are derived from `.geminirules`, `docs/dev_architecture-guidelines.md`, and `docs/dev_architecture-modular-monolith.md`.

## 1) Architecture Decision Rules

- Follow the Hybrid Modular Architecture model.
- Build inside this monolith when the feature:
  - mutates foundational Core/System/HR data,
  - needs strict transactional consistency and native SQL joins,
  - is mission-critical and deeply integrated with core platform behavior.
- Prefer standalone microservices (outside this repo) for organically isolated domains that can use loose coupling.

## 2) Monolith Module Boundaries

- Keep strict domain boundaries in frontend, backend, and data layers.
- Use module-oriented structure rather than mixing unrelated features in shared root folders.
- For medium/large features in the monolith, mirror the Assets module style:
  - dedicated route group/route space,
  - dedicated component grouping,
  - dedicated backend controller grouping,
  - dedicated database table prefix.

## 3) Database Naming and Schema Rules

- Every table must use a module prefix.
- Core domain tables use `Core_`.
- Assets domain tables use `AM_`.
- New monolith modules must define and consistently use their own prefix.
- Configure explicit table names in EF Core via entity configuration classes using `.ToTable("Prefix_TableName")`.

## 4) EF Core Configuration Rules

- Do not place inline entity configuration in `ApplicationDbContext.OnModelCreating` unless unavoidable.
- Use `IEntityTypeConfiguration<T>` per entity.
- Group configurations by module under `IntranetPortal.Data/Data/Configurations/{ModuleName}/`.
- Rely on assembly scanning (`ApplyConfigurationsFromAssembly`) to apply mappings.

## 5) Backend API Rules (.NET)

- Create dedicated module controllers; do not overload unrelated controllers.
- Keep controller organization module-scoped (for example, Core vs Assets folders/namespaces).
- Never expose EF entities directly over API responses.
- Always use module-specific DTOs for input and output contracts.
- Enforce scope constraints (`SiteId`, `DepartmentId`) and enterprise permission checks on protected endpoints.

## 6) RBAC and Permission Rules

- Follow enterprise RBAC design in `docs/enterprise-permissions.md`.
- Integrate new secured functionality with existing `UserAccount`, `Role`, `RoleDelegation`, and `Permission` models.
- Do not create isolated security systems per module.
- Add/extend permissions through the central permission matrix.

## 7) Audit Logging Rules

- Do not create or use a single generic global audit log table for module audit trails.
- Implement module-local, strongly typed audit tables (for example `AM_AssetAuditLogs`).
- Use explicit foreign keys to the parent domain entity (for example `AssetId` FK to assets table).

## 8) Frontend Routing and Module Layout (Next.js)

- Use App Router module segregation (for example route groups such as `(core)` and `(assets)` where applicable).
- Place server actions by module under `src/app/actions/{ModuleName}.ts`.
- Place module views under dedicated route directories rather than monolithic tab screens.
- Prefer deep/nested routing over large in-page state tab systems for major navigation.
- Register module route namespace and active state behavior in `src/components/layout/Sidebar.tsx`.

## 9) Frontend UI and Styling Rules

- Do not install or use third-party React icon libraries (for example `lucide-react`, `@heroicons/react`, Font Awesome).
- Use inline SVG icons only.
- Default SVG style should follow project conventions:
  - `fill="none"`
  - `stroke="currentColor"`
  - `strokeWidth={2}` (or `2.5` for emphasis when appropriate)
- For major module root `layout.tsx` and `page.tsx`, use:
  - `<div className="max-w-7xl mx-auto space-y-8">`
- Avoid arbitrary root-level padding/structure overrides (for example `p-8`, `px-4`, `min-h-[600px] bg-white`) unless explicitly required.

## 10) Frontend Imports and Code Organization

- Prefer absolute alias imports (`@/...`) over brittle deep relative imports.
- Keep components and actions segmented by domain/module to avoid cross-contamination.

## 11) Practical Implementation Checklist (For Copilot)

Before coding:
- Identify whether the requested feature belongs to monolith or external microservice.
- Identify target module and table prefix.
- Identify required RBAC permissions and scope constraints.

During coding:
- Keep API and UI files in module-specific locations.
- Use DTOs at API boundaries.
- Add/update EF configuration classes under module configuration folders.
- Keep layout and icon conventions compliant.

Before finishing:
- Verify no third-party icon package was introduced.
- Verify no EF entity is leaked via API.
- Verify table names are prefixed and mapped in configuration classes.
- Verify sidebar integration for new module navigation when applicable.

## 12) Priority and Conflict Resolution

- If instructions conflict, apply this priority order:
  1. Explicit user request for current task
  2. Security/safety requirements
  3. This `copilot-instructions.md`
  4. Existing local style and conventions in touched files

- If a requested change would violate architecture constraints (for example monolith vs microservice placement), flag the conflict and propose the compliant path.
