# Intranet Portal Application Task List

- [x] Project Setup & Architecture Planning
  - [x] Define implementation plan
  - [x] Request user review
- [x] Backend Initialization (ASP.NET Core Web API)
  - [x] Initialize ASP.NET Core Web API project
  - [x] Set up MySQL connection (EF Core + Pomelo)
  - [x] Set up basic authentication/CORS
- [x] Frontend Initialization (Next.js PWA)
  - [x] Initialize Next.js project
  - [x] Configure PWA support (using next-pwa)
  - [x] Set up basic responsive layout
- [x] Core Features Implementation
  - [x] Create basic internal portal UI (Sidebar, Header, Dashboard)
- [x] Architecture Refactoring
  - [x] Create IntranetPortal.Data class library
  - [x] Move Models and DbContext into Data project
  - [x] Update API project references and namespaces
  - [x] Recreate EF Core Migrations
- [/] Authentication & RBAC System
  - [x] Backend: Configure JWT Authentication and Role schemas
  - [/] Backend: Create Authentication Controller (Login API)
  - [x] Frontend: Implement JWT Session Context / Middleware
  - [x] Frontend: Build mobile-friendly Login Page (`/login`)
  - [x] Frontend: Apply RBAC conditionals to Sidebar components

- [x] Enterprise Permissions Database Migration
  - [x] Create `Role`, `Permission`, `RolePermission`, and `UserRole` Models
  - [x] Update `UserAccount` and `DbContext` relationships
  - [x] Apply Entity Framework Migrations
  - [x] Update `AuthController` JWT emission logic to inject dynamic matrix permissions

- [x] Internal Documentation Hub (Wiki)
  - [x] Install `react-markdown` and `typography` styling plugins
  - [x] Build markdown file reader utility (`lib/docs.ts`)
  - [x] Build `/docs` and `/docs/[slug]` UI routes
  - [x] Integrate Wiki access into the Dashboard Sidebar
  - [x] Generate sample architectural and setup wiki pages

- [x] Phase 4: Employee Management Module
  - [x] Build Backend API Endpoints (`Employees`, `Sites`, `Departments` controllers)
  - [x] Engineer native Next.js JWT Server-Fetch secure utility
  - [x] Construct Frontend `/employees` interactive Data Table UI
  - [x] Construct Frontend `/employees/new` dynamic form UI

- [x] Phase 5: Organizational Architecture & Administration
  - [x] Implement `Position` EF Core Database Model & Migration
  - [x] Refactor `Employee.JobTitle` primitive to `PositionId` relationship
  - [x] Build Backend `Positions` and `Roles` API CRUD endpoints
  - [x] Build Frontend `/admin/positions` Platform & UI
  - [x] Build Frontend `/admin/roles` Platform & UI
  - [x] Refactor Frontend `/employees/new` UI incorporating dynamic Position endpoints

- [x] Phase 6: Rapid Infrastructure Setup Utility
  - [x] Build `SetupController` backend Bulk UPSERT endpoint
  - [x] Develop `/admin/quick-setup` Frontend TextArea submission utility
  - [x] Implement line-splitting Server Actions payload processing

- [x] Phase 7: Dynamic Policy Authorization Engine
  - [x] Refactor QuickSetup Payload to enforce Developer Constants
  - [x] Seed EF Core Dictionary array natively inside DatabaseSeeder.cs
  - [x] Architect ASP.NET `IAuthorizationRequirement` & `IAuthorizationHandler` capabilities
  - [x] Initialize `builder.Services` Registry for Providers
  - [x] Construct Read-Only `/api/permissions` endpoint
  - [x] Develop Read-Only Next.js `/admin/permissions` GUI

- [x] Phase 8: Securing the HR Employee Module
  - [x] Enforce ASP.NET Core Policies upon `EmployeesController`
  - [x] Construct Backend API `PUT` and `DELETE` execution endpoints
  - [x] Implement robust JWT Permission extraction locally in Next.js Server Components
  - [x] Dynamically conditionally render Navigation Links natively mirroring security matrices
  - [x] Program interactive `Edit` UI logic across `/employees` data tables

- [x] Phase 9: Organizational Sub-Teams & Channels
  - [x] Construct `Team.cs` Entity mapped to `DepartmentId`
  - [x] Update `Employee.cs` to execute optional `TeamId` foreign keys
  - [x] Build EF Core Migrations natively reconstructing schemas safely
  - [x] Provision `TeamsController` for backend REST API fetching
  - [x] Expand Frontend Quick Setup / Forms to dynamically query Teams

- [x] Phase 10: Unified Organizational Administration
  - [x] Implement Backend `DepartmentsController` CRUD endpoints
  - [x] Implement Backend `TeamsController` CRUD endpoints
  - [x] Engineer Frontend Server Actions for Departments and Teams
  - [x] Build Next.js `/admin/departments` Interactive UI Application

- [x] Phase 11: Multi-Tenant RBAC Evolution
  - [x] Implement Backend `IPermissionService` Scoped Authorization Engine
  - [x] Harden `EmployeesController` with `SiteId` Target Context checks
  - [x] Implement Backend SQL Data Masking across GET dataset logic
  - [x] Upgrade `AuthController` formatting `ScopedPermissions` on JWT tokens
  - [x] React-ify Client UI parsing conditional inline buttons seamlessly

- [x] Phase 12: Global Sites Administration
  - [x] Provision Backend `SitesController.cs` Administrative HTTP commands
  - [x] Code Next.js `sites.ts` interactive Server Action components
  - [x] Construct the advanced `/admin/sites` visual React Datagrid array
  - [x] Bind new endpoints natively onto the Core layout Sidebar

