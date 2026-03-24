# Intranet Portal Web App Plan

Building a mobile-friendly progressive web internal portal using ASP.NET Core Web API, MySQL, and Next.js PWA.

## Approved Plan Features
- **Database Name**: `IntranetPortal`
- **Design System**: Tailwind CSS
- **Core Models**: `Employees`, `Departments`, `Announcements`, and `Locations` (to represent office, factory, warehouse, etc.).

## Proposed Architecture

We will structure the repository inside `d:/DEV/IntranetPortal` into two main directories: `backend` and `frontend`.

### `backend/` - ASP.NET Core Web API
We will create a RESTful API to serve data to the frontend.
- **Framework**: `dotnet new webapi -n IntranetPortal.Api`
- **Database Access**: We will use Entity Framework Core (`Pomelo.EntityFrameworkCore.MySql`) for MySQL integration.
- **Authentication**: Set up basic CORS to allow the frontend to communicate with the API.

### `frontend/` - Next.js PWA
We will create a Next.js application that can be installed on mobile devices.
- **Framework**: `npx create-next-app@latest frontend --ts --tailwind --eslint --app --src-dir --import-alias "@/*"`
- **PWA Capabilities**: Install the `next-pwa` plugin to generate a Web App Manifest and Service Workers, allowing employees to "Install App" to their home screens.
- **UI Architecture**: A responsive layout featuring a sidebar navigation for desktop browsers, which collapses into a hamburger menu or bottom-tab bar on mobile devices.

### Database: MySQL
We will design a simple initial schema suitable for an internal portal (e.g., `Users` or `Tasks` tables) to demonstrate the connection between the Next.js frontend, ASP.NET backend, and MySQL database.

## Verification Plan

### Automated Tests
- Run `dotnet build` and `dotnet run` on the API to verify it compiles and runs locally.
- Run `npm run lint` and `npm run dev` on the frontend to ensure no build errors exist.
- Verify Swagger UI is accessible from the browser.

### Manual Verification
- We will instruct you how to test the PWA installation in your browser (e.g., using Google Chrome to click "Install App").
- We will verify that data flows correctly from the MySQL database, through the ASP.NET API, and appears beautifully on the Next.js frontend.

---

## Phase 2: Authentication & RBAC Integration

We will transition the static UI into a fully secure system.

### Backend (ASP.NET Core Web API)
- **Token System**: Integrate JWT (JSON Web Token) Bearer Authentication.
- **Identity Model**: Extend the current `Employee` model to include security fields (e.g., `PasswordHash`, `Role`) so employees can log in using their email.
- **Endpoints**: Create an `AuthController` exposing `/api/auth/login`. Protect existing and future endpoints using `[Authorize(Roles = "Admin, Staff")]`.

### Frontend (Next.js)
- **Session Management**: Implement a lightweight custom JWT Cookie strategy. This gives maximum flexibility for deeply integrating with .NET APIs without the forced opinions of heavy external libraries.
- **UI Components**: Build a stunning, corporate-style `/login` page completely separate from the dashboard layer.
- **Route Protection**: Use Next.js Edge Middleware to automatically redirect unauthenticated users trying to access `/` back to `/login`.
- **Dynamic RBAC**: Update `Sidebar.tsx` and `page.tsx` to read the user's role from the decoded token and conditionally render Admin-only features.

## Approved Authentication Architecture
- **Identity Model**: Separate `UserAccount` table mapped to `Employee`.
- **Frontend Session**: Custom, lightweight JWT cookie implementation (no external JS libraries).

---

## Phase 3: Enterprise Permission Matrix (Advanced RBAC)

Since the system will scale across multiple isolated modules (HR, Purchase, Assets Management), the current single `Role` string on the `UserAccount` is functionally insufficient. We must architect a granular Permission system *before* building the modules to prevent severe technical debt.

### 1. Database Design Evolution (Site-Scoped Roles)
To support managers overseeing specific locations vs global bosses, we will create a **Resource-Scoped Permission Model**. By adding an optional `SiteId` boundary directly to a user's role assignment, we achieve perfect hierarchical scaling.
- **`Roles` Table**: Defines organizational hats (e.g., "HR Manager", "Purchasing Agent").
- **`Permissions` Table**: Defines exact actions (e.g., `HR.Employee.Edit`, `Purchase.Invoice.Approve`).
- **`RolePermissions`**: Maps specific permissions to a role.
- **`UserRoles`**: Maps a `UserAccount` to an organizational Role with an optional **`SiteId`** constraint.
  *(Example: Alice is assigned `HR Manager` for `SiteId = 1`. Bob is assigned `HR Manager` with `SiteId = NULL`, granting him global HR authority across all sites).*

### 2. API Security (ASP.NET Core Policies)
We will replace the basic `[Authorize(Roles="Admin")]` with dynamic ASP.NET Core Policy-Based Authorization:
- Create a custom `IAuthorizationHandler` or Middleware that intercepts API requests and verifies the user's granular matrix permissions.
- Controller endpoints will be protected dynamically like: `[HasPermission("Assets.Management.Write")]`.

### 3. Frontend Dynamic UI
The simplistic `isAdmin` boolean currently acting as a UI toggle will be upgraded into a robust React wrapper such as `<RequirePermission capability="Purchase.View">`. This ensures the Sidebar and Route Middleware dynamically hide/show modules matching their precise database rights.

---

## Phase 4: Employee Management Module
With the authentication matrix capable of handling location-based scopes, we will now construct the focal Employee operational module.

### 1. Backend REST API
- **`EmployeesController.cs`**: Implement foundational `GET`, `POST`, `PUT`, `DELETE` routes. Queries will eagerly `.Include()` referenced Database tables to return complete graphical views (e.g. returning the literal name "New York Office" instead of just `SiteId: 1`).
- **`SitesController.cs` & `DepartmentsController.cs`**: Implement read-only `GET` endpoints to seamlessly provision data arrays for frontend `<select>` dropdown fields.
- **Security Check**: Enforce generic `[Authorize]` JWT validation across endpoints instantly preventing unauthenticated access. 

### 2. Frontend Next.js Interface
- **Data Table Application (`/employees`)**: Construct a beautifully modernized, responsive dataset UI utilizing Next.js server components to render employees graphically alongside dynamic Badges denoting their Department and Location assignments.
- **Creation Routing (`/employees/new`)**: Build a dedicated application sub-page featuring a highly polished submission interface. It will autonomously ping the Backend API reference routes to populate its Site/Department dropdown boxes dynamically.
- **Authenticated Fetching**: Engineer a Next.js Server-Component utility that safely extracts the Secure HTTP-only JWT Cookie session and attaches it natively into API Header bearer queries, guaranteeing rapid data resolution.

---

## Phase 5: Organizational Architecture & Security Management
The user rightfully distinguished that an enterprise requires a physical separation between Security Roles (which govern data access limits) and HR Positions (which represent a staff member's real-life job title). 

To accomplish this beautifully, we will introduce a formal Administration interface.

### 1. Database Schema Evolution
- **Create `Position.cs`**: A new relational table storing corporate job titles (e.g. `Id`, `Name`, `Description`).
- **Refactor `Employee.cs`**: Replace the primitive `string? JobTitle` field with a strongly-typed `int? PositionId` foreign key.
- Execute an **Entity Framework Migration** to construct the SQL framework without data loss.

### 2. Backend REST API Expansion
- **`PositionsController.cs`**: Develop generic CRUD endpoints (`GET/POST/PUT/DELETE`) allowing HR managers to manage official job titles. 
- **`RolesController.cs`**: Develop a sophisticated administration endpoint fetching the full list of `Roles` and their nested `RolePermissions`, allowing administrators to construct brand-new system capabilities. 
- **Employee Endpoints Update**: Modify `EmployeesController.cs` to `.Include(e => e.Position)`.

### 3. Frontend Next.js Interface
- **Administration Suite (`/admin/positions` & `/admin/roles`)**: Build two dedicated Administrator data tables. 
  - The *Positions* view will let HR dynamically add new structural corporate titles. 
  - The *Roles* view will physically visualize the complex matrices binding permissions together.
- **Refactor Employee Onboarding (`/employees/new`)**: Remove the plain text `jobTitle` input and replace it with a dynamically-populated `<select>` dropdown sourcing from the `/api/positions` endpoint.

---

## Phase 6: Rapid Infrastructure Setup Utility
To drastically accelerate initial deployment timelines, we will construct a dedicated bulk-entry interface empowering administrators to literally copy-paste text arrays of dictionaries directly into the databases.

### 1. Backend Bulk UPSERT Endpoint
- **`SetupController.cs`**: Implement a new `[HttpPost("quick-setup")]` secure route ingesting a `QuickSetupDto` formatted with standard structural string arrays. It will iteratively perform efficient EF Core `.AnyAsync()` existence checks before securely bulk inserting any missing entities.

### 2. Frontend Next.js Administrator Utility
- **`/admin/quick-setup` Form**: Build an elegant Administration UI rendering four multi-line `textarea` domains simultaneously. The `Sites` block will be logically hardcoded to default to "1 Head Office".
- **Action Execution Logic**: A dedicated Next.js UI parser will autonomously intercept the form payload, slice the raw text by newline constraints (`\n`), purge trailing whitespace, and fire the compiled API packet synchronously.

---

## Phase 9: Organizational Sub-Teams & Channels Matrix

To support granular operational units like "B2B Sales" vs "Retail Sales" within the identical "Sales" Department, we must evolve the organizational data structures.

### Proposed Architecture Options

#### Option A: Dedicated `Team` Data Entity (Recommended for Trading & Services)
- **Architecture**: Create a distinct `Team` (or `Channel`) SQL Table referencing a specific `DepartmentId`.
- **Employee Record Update**: Overhaul the `Employee` model exposing a generic `TeamId` foreign key.
- **Benefit**: Extremely fast relational querying and enables cross-positioning (e.g., an Accountant and a Sales Rep can both belong to a singular multidisciplinary "Channel Apollo").

#### Option B: Recursive Hierarchical Departments
- **Architecture**: Modify the `Department` model to allow an optional `ParentDepartmentId` pointing recursively to itself.
- **Employee Record Update**: The employee simply belongs to a deeply nested department (e.g., assigning an employee to "B2B Outreach" instead of "Sales").
- **Benefit**: Practically infinite depth, natively nesting structure, but forces complex backend SQL CTE operations to retrieve full divisional trees.

### Implementation Plan
1. **Database Expansion**: Create the `Team` model mapping back to `Department` securely via EF Core Migrations.
2. **Backend API Overhaul**: Build `TeamsController` for CRUD operations and update `EmployeesController` to eagerly `.Include()` team definition strings on the Data payload.
3. **UI Interlocks**: Add `Teams Administration` globally, and dynamically interlock dropdowns within `/employees/new` so that selecting "Sales" unlocks the respective Sub-Teams securely.

---

## Phase 10: Unified Organizational Administration
To efficiently process the creation and editing of classical sub-structures, we will implement a dedicated Administration application linking operations securely.

### 1. Backend REST API Expansion
- **`DepartmentsController.cs`**: Appending fully authenticated POST, PUT, DELETE endpoints enforcing `[Authorize(Policy = "Perm:Admin")]` logically.
- **`TeamsController.cs`**: Appending comprehensive CRUD handlers processing the `DepartmentId` arrays accurately safely creating sub-teams natively.

### 2. Frontend Next.js Graphical Dashboard
- **`/admin/departments` Matrix**: Construct an elegant UI that loops directly through standard Departments, graphically opening inline drawers to instantly perform CRUD editing on child Teams transparently without full-page reloads!

---

## Phase 11: Multi-Tenant RBAC Evolution
To achieve enterprise multi-tenant scaling securely, we will implement Resource-Based Authorization enforcing localized Site scopes natively to prevent internal Privilege Escalation.

### 1. Backend: Scoped Authorization Services
- **`IPermissionService` Engine**: Build a lightweight native dependency that dynamically queries the database to verify if an animated user has Global (*Null SiteId*) access OR localized access to a `TargetSiteId`.
- **Controller Hardening**: In operations like `UpdateEmployee`, inject the engine to execute: `await _permService.RequireSitePermissionAsync("HR.Employee.Edit", targetSiteId)`. If unauthorized, it natively throws a `403 Forbid`.
- **SQL Query Masking**: Extend GET endpoints to automatically intercept dataset lookups, appending SQL `Where(e => ...)` clauses matching the user's allowed `SiteId` subset efficiently mapping multi-tenant boundaries.

### 2. Frontend: Decoding Localized Scopes
- **JWT Encoding**: Upgrade `AuthController` to serialize `ScopedPermissions` directly into the JWT token payload (e.g., `"HR.Employee.Edit:1"`, or `"HR.Employee.Edit:Global"`).
- **Next.js UI Masking**: Upgrade application rendering architectures. `EmployeeTable` will parse exact localized contexts. If an Employee row belongs to Tokyo (Site 2) and the user only possesses `HR.Employee.Edit:1` (Singapore), the frontend will securely hide the interactive Edit/Delete buttons natively!

---

## Phase 12: Global Sites Administration
To successfully control the geographic Multi-Tenant array, we will deploy an administrative control panel dynamically managing the Company's Sites/Branches natively.

### 1. Backend REST Endpoints
- **`SitesController.cs`**: Expand the Controller to natively interpret POST, PUT, and DELETE HTTP commands authenticated aggressively under `[Authorize(Policy = "Perm:Admin")]`.
- **EF Core Database Validation**: Ensure `Site.cs` safely drops into MySQL and that constraints are respected globally across the tenant mappings.

### 2. Next.js Hub Configurations
- **Server Actions Wiring**: Script `/actions/sites.ts` explicitly formatting immutable form data safely against the API matrix.
- **Graphical Application**: Construct an immersive grid GUI inside `/admin/sites` natively utilizing premium standard Tailwind matrices capturing "Headquarters", "Operational Nodes", and localized structural data explicitly without full-page reloads.

---

## Phase 13: Contextual App Launcher & Universal Portal OS
As the Intranet expanded rapidly with Departments, Employees, and Sites functionality, the unified global sidebar suffered from cognitive overload. We successfully re-architected the frontend into a scalable "Application OS" paradigm.

### 1. The App Grid (Homepage)
- **Architecture**: Redesigned `src/app/page.tsx` into a modern App Launcher dropping the global sidebar out of view.
- **Tiles**: Users are greeted with massive application tiles. The two primary engines are **The Hub** (grouping all standard employee data and documents) and **Administration** (grouping system settings and permission gating).

### 2. Context-Aware Navigation
- **`Sidebar.tsx` Refactor**: Implemented `usePathname()` logic to assess the active routing cluster. 
- **Dynamic Gating**: When a user is inside The Hub, the Sidebar *exclusively* renders Corporate Directory structures and Knowledge Base menus. If a user clicks into the Administration app, the sidebar natively purges the directory data to render structural settings, resulting in infinite horizontal scalability.

### 3. Department & Facilities Modular Realignment
To safely respect the new The Hub context, organizational subsets structurally tied to Administration were explicitly released to global staff natively.
- **Departments Integration**: Deployed `/departments` with an interactive CRUD interface and dynamic glassmorphic Tailwind modals.
- **Sites Migration**: Successfully relocated the `/admin/sites` GUI natively to `/sites`. The **Corporate Directory** is now perfectly unified sequentially: `Employees`, `Departments`, and `Geographic Sites`.

---

## Phase 14: Corporate Announcements Engine
To fully activate **The Hub**, employees require a dedicated interface to broadcast and read internal news, policies, and updates.

### 1. Backend REST API (`AnnouncementsController.cs`)
- **GET All**: Retrieve announcements ordered by `CreatedAt` descending playfully including the `Author` object to display who posted it visually.
- **POST/PUT/DELETE**: Restrict creation and mutations structurally using `[Authorize(Policy = "Perm:Admin")]` (or a dedicated `Communications` permission).

### 2. Frontend Realization (`/announcements`)
- **Server Component Fetching**: Fetch the data directly bypassing client-side loaders.
- **Data UI**: Construct a visually stunning "News Feed" layout using Tailwind cards with rich typography and date formatting instead of a rigid data Grid.
- **Sidebar Integration**: Wire the existing empty `Announcements` link in `Sidebar.tsx` dynamically to `/announcements`.

---

## Phase 16: Multi-Tenant Horizontal Scoping
To achieve true Multi-Tenant isolation, the backend MUST dynamically filter SQL arrays horizontally so that an HR Manager in Japan cannot view or modify Human Resource files stored in Canada.

### 1. The Database Architecture
- `Department.cs`, `Announcement.cs`, and `Employee.cs` receive `int? SiteId` properties. 
- A Global resource has `SiteId = null`. A localized resource locks rigidly onto geographical branch IDs.

### 2. Capabilities Extraction
- `PermissionService` was expanded natively to extract exactly which Site bounds an actor possesses internally. 

### 3. API Controller Traps
- **POST/PUT/DELETE**: Modifying target arrays executes `_permissionService.ValidateSiteScope()`, which automatically throws an `HTTP 403 Forbidden` flag if unauthorized.
- **GET**: Polling the database runs `query.ApplySiteScope()` automatically limiting `WHERE SiteId == ...` to intercept unauthorized row reads mathematically.

---

## Phase 17: Structural Security Scaffolding (ISiteScoped)
Because relying on future developers to remember to explicitly type the `Where(x => ...)` filters into their newly constructed API Controllers is a severe security vulnerability, we successfully implemented physical interfaces globally standardizing the logic.

### 1. The Native Boundary
- Forged `IntranetPortal.Data/Models/ISiteScoped.cs`.
- Tagged `Employee`, `Department`, and `Announcement` mechanically.

### 2. Explicit C# Extensions
- Constructed `SiteScopeExtensions.cs`.
- Deployed a heavily optimized Extension Method `ApplySiteScope(this IQueryable<T> ...)` and `.ValidateSiteScope(...)`.
- This definitively guarantees rapid enterprise scalability natively across the pipeline while structurally bypassing the standard Entity Framework Core Global Query Filter blindspot.
