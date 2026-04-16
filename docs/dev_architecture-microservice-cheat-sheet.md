# Universal Microservice Architecture: Context Cheat Sheet

*Drag and drop this file into your brand new empty Workspace before you prompt the AI so it instantly remembers the overarching architecture!*

---

## The Goal
This document defines the **Universal Standard** for scaffolding *any* Standalone Microservice Module (from a simple 2-endpoint "Drink Ordering" tool to a massive "Ticketing" system) so it integrates cleanly into the parent `IntranetPortal` ecosystem. 

By standardizing on one architecture, we completely eliminate context-switching. The developer experience in a Microservice will perfectly mirror the Core Monolith.

- It requires its own Next.js App Router frontend (e.g. port `3001`).
- It requires its own .NET backend using **Controllers** with direct `DbContext` injection (Mirroring the Monolith).
- It requires its own completely isolated SQL database schema.

---

## 1. Backend Architecture (Controllers + DbContext)
To mirror the `IntranetPortal.Api` Core Monolith, all microservices must use the "Fat Controller" pattern instead of Minimal APIs.

**Implementation Rules:**
- Configure `Program.cs` for Controllers:
  ```csharp
  builder.Services.AddControllers();
  builder.Services.AddEndpointsApiExplorer();
  // ...
  app.MapControllers();
  ```
- **Direct DbContext Injection:** Do not create abstract `Service` layers unless strictly necessary. Inject the specific Microservice DbContext directly into the Controller and handle business logic, mapping, and database saves there.
  ```csharp
  [ApiController]
  [Route("api/[controller]")]
  public class OrdersController : ControllerBase
  {
      private readonly DrinkOrdersDbContext _context;
      public OrdersController(DrinkOrdersDbContext context) { _context = context; }
  }
  ```

---

## 2. Authentication Bridge (The "Magic" Token)
The Core Monolith issues a JWT upon login and saves it natively as a cookie named `auth_token`. **This microservice does NOT handle logins.** It simply piggybacks off the Monolith's token!

### Backend (.NET API) Setup
- The new .NET API must authenticate users mathematically using the exact same `JwtSettings` from the Monolith.
- `appsettings.json` must match the Core. For local dev, default to mock values:
    ```json
    "JwtSettings": {
      "Key": "DevelopmentSecretKeyThatIsAtLeast32Bytes",
      "Issuer": "IntranetPortalLocal",
      "Audience": "IntranetPortalMicroservices"
    }
    ```
- Use Native Controller Authorization: Secure endpoints using attributes like `[Authorize(Policy = "Module.Action")]`.
- **CRITICAL DATA RULE (Identity & Scopes):** The Core Monolith inherently packs five exact identity claims inside the `auth_token`: `"EmployeeId"`, `"FullName"`, `"SiteId"`, `"DepartmentId"`, and `"TeamId"`.
  - **Identity:** To associate a physical record with a user, extract loosely: `user.FindFirst("EmployeeId")?.Value`. Do not use `NameIdentifier`.
  - **Scoping (The Frozen Snapshot):** If the microservice enforces geographical or departmental data boundaries (e.g., users only seeing local orders), DO NOT query the Core API. Extract the scope directly from the token (e.g., `int.Parse(user.FindFirst("SiteId").Value)`) and enforce it dynamically inside your native EF Core `.Where()` queries or save it directly alongside transaction data.

### Global Authorization / Dynamic "Perm:" Prefix
To ensure "Zero Context-Switching" for developers working across the platform, all microservices MUST intercept the `Perm:` prefix natively on Controllers. DO NOT manually create hardcoded `AddPolicy` assertions in `Program.cs`.

**Implementation Rules:**
1. Replicate the core `PermissionPolicyProvider.cs` and `PermissionAuthorizationHandler.cs` engine into the microservice's `Security/` namespace.
2. Register the dynamic interceptor in `Program.cs`:
   ```csharp
   builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
   builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
   ```
3. Use the exact Monolith syntax seamlessly on Microservice Controllers, which will organically evaluate the required local permission, the `Admin` role, and the `System.FullAccess` God-mode bypass:
   ```csharp
   [Authorize(Policy = "Perm:Module.Queue.View")]
   ```

### Frontend (Next.js) Setup
The new Next.js micro-frontend must blindly extract the `auth_token` cookie and decode it to know who the user is using Server Actions.

---

## 3. Database Rules & EF Configurations
A microservice must manage its own table configurations cleanly, scaling for both small and large schemas.

- **Dual Database Provider:**
  - Use **SQLite** for local development speed and **MySQL/Pomelo** for production using MSBuild `<ItemGroup Condition="'$(Configuration)' == 'Debug'">` boundaries.
- **EF Configurations:** Do not clutter `OnModelCreating` inline. Group EF Model Mappings in dedicated classes implementing `IEntityTypeConfiguration<T>` just like the Monolith, and apply them using `ApplyConfigurationsFromAssembly`.
- **Foreign Keys:** If you need to store data against an Employee, map the `EmployeeId` as a standard, loose `int` column without enforcing a rigid SQL Foreign Key constraint to the external Core database.

---

## 4. RBAC Permission Auto-Registration
Microservices automatically broadcast their required internal permissions to the Core Monolith when booting.

**Implementation Rules:**
- Configure an `InternalSecret` in `appsettings.json`.
- Scaffold a `BackgroundService` to HTTP POST the local permissions payload to the Monolith's internal `/api/internal/permissions/register` endpoint.
- Wrap this in a `try/catch` loop with `Task.Delay(10000)` to continuously retry if the monolith is offline.

---

## 5. Micro-Frontend Shell & CORS Constraints
Ensure users do not feel like they have traversed repositories. The Next.js visual shell must perfectly mimic the Core Monolith.

- **UI Structure:** Obey the golden structural rule: all pages MUST be wrapped natively in `<div className="max-w-7xl mx-auto space-y-8">`.
- **Next.js CORS Constraints:**
  - Because `fetch` forces `{ credentials: "include" }` cross-port, the API endpoints **CANNOT** use `.AllowAnyOrigin()`.
  - Local Dev MUST explicitly use `policy.SetIsOriginAllowed(_ => true).AllowCredentials()`.
  - Production MUST enforce `.WithOrigins("https://your-production-subdomain...")`.

---

## 6. Workspace AI Constraints (.geminirules)
Always ensure that the microservice repository contains its own `.geminirules` file. You can copy the `microservice-template.geminirules` from the main (`docs/`) repository to the root of the microservice directory. This file dictates everyday coding rules (like SVG constraints, DTO enforcement, and EF grouping) that the Assistant uses continuously.

---

**AI Assistant:** When the user drops this file in your workspace, immediately acknowledge these architectural rules and offer to run `npx create-next-app@latest` followed by `dotnet new webapi -use-controllers` to scaffold the repository!
