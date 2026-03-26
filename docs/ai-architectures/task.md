# Intranet Portal Development Roadmap
*A comprehensive chronological ledger mapping the entire Multi-Tenant RBAC structure from inception to modern execution.*

## Phase 1. Project Initialization & Architecture
- [x] Scaffold .NET 10 Web API Backend.
- [x] Scaffold Next.js 16 App Router Frontend with Tailwind CSS.

## Phase 2. Database Foundation (EF Core & MySQL)
- [x] Integrate `Pomelo.EntityFrameworkCore.MySql` perfectly.
- [x] Establish `ApplicationDbContext` and initial connection strings to MySQL.
- [x] Deploy initial EF Core Migrations securely.

## Phase 3. Core Identity & RBAC Schema
- [x] Design explicit `UserAccount`, `Role`, `Permission`, and `RolePermission` models.
- [x] Design structural `Site` (Geographic Boundaries) and `UserRole` (Site-scoped Role binding) matrices.

## Phase 4. HR Organizational Matrix Schema
- [x] Design foundational `Employee` model bridging exact HR requirements.
- [x] Design structurally related organizational models: `Department`, `Team`, `Position`.

## Phase 5. Core Authentication & JWT Engine
- [x] Implement `AuthController.cs` executing explicit `BCrypt` password hashes natively.
- [x] Generate advanced JWTs encapsulating granular `ScopedPerm` claims mapping explicitly onto geographic sites reliably.
- [x] Engineer Next.js API Middleware seamlessly to natively proxy standard JWT headers securely throughout the App Router.

## Phase 6. Global Site Management
- [x] Build `SitesController.cs` for full geographic CRUD operations.
- [x] Engineer interactive `/sites` management UI in Next.js dynamically listing active facilities.

## Phase 7. Department & Team Infrastructure
- [x] Build `DepartmentsController.cs` structurally tracking distinct unit relationships natively over SQL.
- [x] Engineer `/departments` UI with natively nested Team cascading data matrices.

## Phase 8. HR Position & Job Title Architecture
- [x] Build `PositionsController.cs` for centralized job title schema definitions.
- [x] Formulate UI module elegantly at `/admin/positions` securely restricting viewable changes to Admins natively.

## Phase 9. Employee Lifecycle & Directory
- [x] Build `EmployeesController.cs` rendering deep EF Core relational includes organically (`Position`, `Department`, `Team`, `Site`).
- [x] Engineer `/employees` generic data grid identically tracking location dependencies gracefully via interactive UI tools.
- [x] Build `/employees/new` generic interactive Creation form directly matching dependencies securely upon Next.js bindings natively.

## Phase 10. Security Matrix & Permissions
- [x] Build `PermissionsController.cs` directly exposing global application capabilities explicitly mapped in C#.
- [x] Construct dynamic frontend grid elegantly located at `/admin/permissions` seamlessly showing distinct backend rights explicitly.

## Phase 11. Role Management Definitions
- [x] Build `RolesController.cs` executing dynamic capability maps iteratively onto `RolePermission` bridge tables stably over entity saves.
- [x] Engineer `/admin/roles` React UI seamlessly allowing explicit mappings of analytical rights directly onto formal structural models natively.

## Phase 12. Authentication UI & Layout Guardrails
- [x] Engineer `/login` screen securely persisting JWT standard cookies effectively via powerful Next.js Server Actions completely bypassing generic interceptors.
- [x] Program conditional structural Next.js Sidebar components securely parsing distinct `userRoleClaim` metrics visually configuring Admin layers exclusively.

## Phase 13. System Bulk Seeding Engine (CSV Import)
- [x] Provision Backend `SetupController.cs` safely executing multi-dimensional CSVs dynamically via lightweight `CsvHelper` interceptors mapping complex strings structurally.
- [x] Engineer robust `QuickSetupForm.tsx` cleanly transmitting raw analytical File buffers natively parsing explicitly upon POST HTTP execution dynamically mapping responses globally.

## Phase 14. Bulk Ingestion State Tracking (Phase 21)
- [x] Intercept the `SetupController.cs` bulk engine structurally encapsulating logically distinct inserted, identical, or updated models firmly tracking exact modifications onto UI array natively.
- [x] Render a gorgeous dynamically auto-scrolling analytical grid strictly parsing HTTP mapped newly uploaded workforces directly inside the Quick Setup organically smoothly tracking skips implicitly.

## Phase 15. Active User Provisioning & Search (Phase 22)
- [x] Intercept `SetupController` & `EmployeesController` natively encapsulating active `UserAccount` insertion logic identically responding smoothly upon explicit UI boolean flags directly.
- [x] Engineer the beautiful `/admin/users` Next.js screen meticulously slicing generic datasets neatly upon exact explicit parameters uniquely mapping Elevated vs Basic Logins intelligently over API cleanly.
- [x] Construct the definitive RBAC formal Geo-Role Assigner Module smartly executing real-time `UserAccount` clearances seamlessly over bounded scopes perfectly dynamically executed.
- [x] Synthesize structural EF Core `?search=` intercepts scaling dynamically mapping UI form queries natively cleanly across Employees tracking textual mappings symmetrically identical over REST bindings gracefully.
- [x] Synchronize interactive `EmployeeFormClient.tsx` actively injecting the `Allow Login` capability globally on both PUT & POST endpoints dynamically syncing visually with EF Core instantly securely.

## Phase 16. Multi-Tenant Horizontal Scoping (ISiteScoped)
- [x] Define an identical `ISiteScoped` global interface standard precisely executed securely natively mapped into Data standard structures organically.
- [x] Compute the programmatic `SiteScopeExtensions` utility explicitly clamping physical SQL bounds smoothly onto all EF Core streams identically enforcing isolation.
- [x] Structurally attach interfaces safely bridging `Employee`, `Department`, and `Announcement` entities into geographic mapping standards elegantly.
- [x] Audit REST Controllers natively invoking internal programmatic logic safely forcefully resolving explicit 403 Forbidden intercept boundaries against cross-tenant payload intrusions gracefully securely.

## Phase 17. Corporate Announcements Feed
- [x] Design architectural `DbSet<Announcement>` exactly bridging generic text data structurally towards `Site` locations organically within DB dynamically correctly tracking context explicitly safely over MySQL perfectly mapped dynamically structurally explicitly natively smoothly.
- [x] Deploy explicit generic `AnnouncementsController.cs` accurately tracking standard JWT scoped permissions directly computing text arrays structurally resolving REST protocols gracefully stably reliably globally.
- [x] Synthesize the native generic `/announcements/page.tsx` structural News Hub seamlessly resolving complex mapping metrics exactly gracefully smoothly rendering dynamic updates explicitly mapping cleanly across UI forms seamlessly properly explicitly.
