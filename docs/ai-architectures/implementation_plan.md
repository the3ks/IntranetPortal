# Intranet Portal: Master Implementation Blueprint
*Persistent architectural foundation for the Multi-Tenant RBAC ecosystem, from initial database scaffolding through advanced horizontal geo-scoping metrics.*

---

## 1. Core Architecture & Stack Foundation
**Goal:** Establish a robust architecture isolating the backend API from the interactive frontend.
- **Backend Infrastructure:** .NET 10.0 ASP.NET Web API configured with Swagger/OpenAPI.
- **Data Engine:** Entity Framework Core (EF Core 9) connected to a MySQL database utilizing EF structural migrations.
- **Frontend Infrastructure:** Next.js 16 (App Router) architected with Tailwind CSS and Next.js Server Actions.

---

## 2. Enterprise RBAC & Identity Schema
**Goal:** Isolate HR structural elements from software access layers.
- **`UserAccount` Module:** Stores the `[Email]` and `BCrypt`-hashed `[PasswordHash]` securely validating HTTP boundaries.
- **`Role` & `Permission` Engine:** Highly granular many-to-many lookup structures bridging explicit capabilities (e.g., `HR.Employee.Create`) onto Role templates.
- **Geographic Nodes (`Site`):** Represents rigid physical facilities. Ensures database rows are horizontally partitioned across geographic borders.
- **Hybrid Cross-Binding (`UserRole`):** Solves enterprise scale by binding distinct Roles to specific `SiteId` constraints.

---

## 3. HR Organizational Matrix Definition
**Goal:** Construct the foundational business logic entities mapping corporate dependency flows.
- **`Employee` Entity:** Stores core HR fields. Possesses explicit Foreign Keys binding it to a singular `Position`, `Department`, `Team`, and geometric `Site`.
- **Structural Constraints:** `Sites` contain `Departments`; `Departments` contain formal `Teams`.

---

## 4. Authentication Pipeline & JWT Cryptography
**Goal:** Synthesize Identity graphs over REST HTTP Headers efficiently avoiding redundant database queries on every HTTP request.
- **`AuthController.cs`:** The entrypoint executing `BCrypt` hashing bounds and validating user credentials.
- **Granular Token Schema:** JWT payload containing Claims formatted as `PermissionName:SiteId` strings (e.g., `HR.Employee.View:4`).
- **`PermissionService.cs`:** A bespoke C# utility evaluating the `ScopedPerm` claim array, effectively blocking 403 HTTP violations across structurally bound mutations.

---

## 5. Multi-Tenant Horizontal Scoping (ISiteScoped)
**Goal:** Mathematically guarantee identical data filtering vertically across the entire Application layer APIs.
- **`ISiteScoped` Interface:** A globally standard C# Interface enforcing the existence of a `SiteId` integer on conforming Entity Framework Models.
- **`SiteScopeExtensions.cs` IQueryable Hook:** Automatically intercepts `IQueryable` flows (e.g. `_context.Employees.AsQueryable()`) dynamically appending `.Where()` clauses evaluating the JWT scopes array.

---

## 6. Bulk System Seeding Engine (CSV)
**Goal:** Rapidly ingest massive structural spreadsheets seamlessly into relational databases.
- **`SetupController.cs` CSV Router:** Parses `IFormFile` memory buffers utilizing `CsvHelper` logically tracking `Created`, `Skipped`, and `Updated` metrics to provide frontend feedback.
- **Auto-Computation Matrices:** If a generic string column contains "London Hub", the C# dictionary loop will explicitly locate or immediately construct the missing `Site` model.

---

## 7. Hybrid Auto-Provisioning & Security UI
**Goal:** Bridge the gap between bare-metal HR entry creation and explicit Software Credential issuance flexibly.
- **Basic Provisioning Flow:** An `AllowLogin` boolean is submitted via `/employees/new`. This flags `EmployeesController.cs` to instantly generate a matching `UserAccount` object with an active hash.
- **Security Dashboard UI:** Build `frontend/src/app/admin/users/page.tsx` slicing `UserAccount` grids upon parameter segments separating `Elevated Rights` from `Basic Staff` users.
- **RBAC Formal Geo-Role Assigner Module:** Real-time `UserAccount` capability assignment modal allowing Admins to attach a `Role` against a `SiteScope`.
- **EF Core Search Intercepts:** Map dynamic `?search=` HTTP parameters directly against C# SQL streams identically across the core Employee mapping and Admin Security tables.
