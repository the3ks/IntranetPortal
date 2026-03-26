# Implementation Plan: Multi-Tenant RBAC Site Isolation

The current capability matrix authorizes users globally (e.g., if a manager has `Announcements.View`, they can view all announcements across the entire organization). The objective is to rigidly bind entities to physical `SiteIds` and rewire the capabilities engine so users can only manipulate resources inside their specifically authorized Sites.

## Proposed Changes

### 1. Schema Expansion
**Goal:** Guarantee every structural element traces back to a geographic boundary.
#### [MODIFY] `IntranetPortal.Data/Models/Employee.cs`
- Retain existing `int SiteId` mapping.

#### [MODIFY] `IntranetPortal.Data/Models/Department.cs`
- Introduce `int? SiteId { get; set; }` to support globally shared departments vs isolated site departments.
- Add `public Site? Site { get; set; }` foreign key.

#### [MODIFY] `IntranetPortal.Data/Models/Announcement.cs`
- Introduce `int? SiteId { get; set; }` allowing announcements to target a specific corporate building or blast globally (`null`).
- Add `public Site? Site { get; set; }` foreign key.

#### [NEW] Database Migration
- Add standard Entity Framework migrations (`dotnet ef migrations add AddSiteIdScopeLimits`) to inject `SiteId` into both the `Departments` and `Announcements` primary SQL tables.
- Run `dotnet ef database update`.

### 2. Authorization Engine Refactor
**Goal:** `[Authorize(Policy="...")]` acts as a global entry gate. True multi-tenant verification must filter horizontally at the database level!
#### [MODIFY] `IntranetPortal.Api/Security/PermissionService.cs`
- Inject a core helper method to extract native bounded scopes: `Task<List<int?>> GetAuthorizedSitesForCapabilityAsync(int userId, string permission)`.
- A returned `null` inside the list means the user holds Global Scope (`System.FullAccess` or a Role spanning all sites).
- If the list contains specific integers like `[2, 4]`, the user's authority strictly ends there.

### 3. API Controller Hardening
**Goal:** Map the PermissionService scopes directly into the EF Core `IQueryable` loops natively!
#### [MODIFY] `AnnouncementsController.cs`, `EmployeesController.cs`, `DepartmentsController.cs`
- **GET (Read Scopes):** Intercept the `IQueryable`. Sweep the user's token scopes, and dynamically append `.Where(x => allowedSites.Contains(null) || allowedSites.Contains(x.SiteId))` directly into the SQL response!
- **POST/PUT/DELETE (Write Mutations):** Before modifying or injecting an entity into the database, forcefully crash the pipeline with an `HTTP 403 Forbidden` if the manager targets a `SiteId` they don't explicitly own.

## User Review Required
> [!WARNING]
> This is a deep structural shift. It will perfectly isolate Tokyo managers from accidentally modifying London documents! Does this Multi-Tenant horizontal architecture align precisely with your vision before I execute these schemas?

---

# Implementation Plan: Rapid Bulk CSV Employee Importer

## Goal
Implement a single-click CSV bulk injection utility within the **Quick Setup** interface allowing enterprise managers to mass-deploy employees. The system will aggressively evaluate the CSV columns and mathematically auto-generate any missing underlying structures (Sites, Departments, Teams, Positions) on the fly!

## Proposed Changes

### 1. Backend CSV Auto-Generation Engine
#### [MODIFY] `IntranetPortal.Api/Controllers/SetupController.cs`
- Expose a new REST API endpoint: `[HttpPost("import-employees")]`.
- The endpoint will natively accept an `IFormFile` (the uploaded `.csv` document).
- **Format Expected:** `FullName, Email, JobTitle, Department, SubTeam, SiteName`.
- **Intelligent Matrix Mapping:** For every single row, the backend will sequentially query the database:
  - Find or Create the `Site`
  - Find or Create the `Department` physically locked to that `SiteId`.
  - Find or Create the `Team` locked to that `DepartmentId`.
  - Find or Create the `Position`.
  - Validate Employee isolation bounds and mass-insert the new `Employee` natively!

### 2. Frontend "Rapid Config" GUI
#### [MODIFY] `frontend/src/app/admin/quick-setup/QuickSetupForm.tsx` (OR parallel component)
- Construct an isolated, aesthetically distinct **"Batch Employee Importer (CSV)"** Dropzone/Button below the dictionary textareas.
- Integrate a hidden `<input type="file" accept=".csv">` mapped securely to a Next.js `FormData` HTTP fetch router pushing natively straight to the C# endpoint.
- Provide a downloadable "CSV Template" string helper natively so administrators understand the exact column arrays globally expected.

## Verification Plan
1. Fabricate a diverse CSV payload mathematically spanning overlapping Sites and missing Departments.
2. Submit the CSV through the frontend application UI.
3. Refresh the `/employees` screen to visually verify the Employees natively span their independent Geographic nodes gracefully.

---

# Implementation Plan: Dynamic Geographic Scoping Filters

## Goal
Because global administrators (or Multi-Site Managers) can view employees seamlessly spanning 10 locations, both the `/employees` and `/departments` frontend UI panels must dynamically inject an explicit "Site Switcher" dropdown. 

The filter strictly enforces Next.js UX Standards:
1. **Dynamic Options:** The dropdown maps exclusively to the Sites the active User mathematically possesses permissions for via their JWT.
2. **Locked State UX:** If a user only possesses explicit permission for 1 physical Site, the dropdown aggressively locks (disables) mitigating any navigation confusion natively.
3. **Server-Side Interpolation:** Switching sites automatically injects a Next.js `?siteId=X` search parameter seamlessly into the URL Router, preserving the pure Server-Component rendering loop natively.

## Proposed Frontend Pipeline Changes

#### [MODIFY] `src/app/employees/page.tsx`
- Refactor the Server Component `EmployeesPage({ searchParams })` to natively extract the `?siteId=` URL parameter synchronously.
- Loop across the extracted `Site` database arrays generating an isolated `<select>` dropdown interactively mapped to `router.push("?siteId=X")`.
- Measure the lengths of explicitly authorized sites derived natively from `user.ScopedPerm`: If length is exactly 1, rigidly inject the `disabled` property onto the `<select>` node securely.
- Structurally filter the `employees.filter()` array rendering completely organically based on the active `siteId` switch.

#### [MODIFY] `src/app/departments/page.tsx`
- Duplicate the precise dropdown UI standard mechanically across the structural Departments grid identically enforcing identical mathematical JWT isolation matrices.

---

# Implementation Plan: Hybrid Access Management & Roles Panel

## Objective
Merge Automated Account Provisioning with strict Role Access segregation natively.
1. **Provisioning:** When an Employee is created (via UI or CSV), an `Allow Login` flag (default: Yes) immediately creates a baseline `UserAccount` granting "Basic Access" (no explicitly elevated Security Roles).
2. **Access Security Dashboard:** A dedicated `/admin/users` UI strictly manages these accounts securely.
3. **Smart Matrix Filtering:** The Access Panel splits users natively via a structural UI Filter separating "Basic Staff" (default access only) from "Elevated Rights" (assigned specific multi-tenant `[UserRoles]`).

## Proposed Changes

### 1. Account Auto-Provisioning & CSV Injection
**Goal:** Mathematically generate Security Accounts transparently during HR creation.
#### [MODIFY] `EmployeesController.cs` & `SetupController.cs`
- Add an `AllowLogin` boolean explicitly mapped to incoming UI payloads and the CSV Parser Native Engine.
- If `AllowLogin == true`, explicitly hash a default standard password and inject a `UserAccount` bound perfectly to the new `EmployeeId`.

### 2. The Granular Security Dashboard (`/admin/users`)
**Goal:** Build a flawless UI datagrid safely filtering and searching the structural User graph.
#### [NEW] `frontend/src/app/admin/users/page.tsx`
- Build the Users React Datagrid listing active `UserAccount` rows physically joined to `Employee.FullName`.
- **Search Module:** Implement a text input `?search=` parameter mechanically filtering the `IQueryable` securely.
- **Security Scope Filter:** Implement a dropdown safely filtering users:
  - `Elevated Rights` (Default) -> Only shows Users possessing explicit Security Roles.
  - `Basic Staff` -> Only shows Users carrying zero explicit Roles seamlessly.
- **RBAC Assignment Modal:** Click "Manage Security Roles" to visually bind explicit Geographically-isolated `UserRoles` mathematically to an account!

### 3. Employee Grid Search Index
**Goal:** Scale the text-search search box natively to the primary HR datagrid organically.
#### [MODIFY] `frontend/src/app/employees/page.tsx`
- Intercept Next.js query parameters injecting a `?search=` URL text router physically interpolating the string directly against the backend EF Core `Employees` stream!

## User Review Required
> [!IMPORTANT]
> This restores the perfectly pristine mathematical separation between HR layers and RBAC capability arrays cleanly! You can provision software access separately from elevated Role Access flawlessly natively. Does this blueprint align precisely with your expectations?
