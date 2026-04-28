---
title: "[DEV] Architecture Standalone Microservices Cheat Sheet"
description: "Universal standard for scaffolding standalone microservice modules that integrate into the IntranetPortal ecosystem — covering SSO token bridging, dynamic RBAC replication, dual-database setup, and micro-frontend shell conventions."
date: "2026-04-28"
author: "Architecture Team"
---
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
- `appsettings.json` must match the Core **exactly** — including `Issuer` and `Audience`. For local dev, default to:
    ```json
    "JwtSettings": {
      "Key": "DevelopmentSecretKeyThatIsAtLeast32Bytes",
      "Issuer": "IntranetPortalAPI",
      "Audience": "IntranetPortalFrontend"
    }
    ```
    > ⚠️ **Critical:** If `Issuer` or `Audience` do not match the Core Monolith's values, JWT validation will silently reject every request with a `401`. Always copy these values verbatim from the Core's `appsettings.json`.
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

**Logout Implementation:**
Since the microservice does NOT handle login or session management, logout is a single redirect to the Core Monolith's login page with `?reason=signed_out`. The Core middleware intercepts this, clears the `auth_token` cookie server-side, and shows the login page.

1. **Create a `logoutAction()` Server Action** in `src/app/actions/auth.ts`:
   ```tsx
   "use server";
   import { redirect } from "next/navigation";

   export async function logoutAction() {
     const coreBaseUrl = process.env.NEXT_PUBLIC_CORE_URL || "https://intranet.example.com";
     redirect(`${coreBaseUrl}/login?reason=signed_out`);
   }
   ```

2. **Call this action from your Header/Logout button** (same pattern as the Monolith):
   ```tsx
   import { logoutAction } from "@/app/actions/auth";

   <form action={logoutAction}>
     <button type="submit">Logout</button>
   </form>
   ```

**Why this works:**
- `redirect()` to `{coreBaseUrl}/login?reason=signed_out` sends the browser to the Core Monolith's Next.js middleware.
- The Core middleware detects `reason=signed_out`, clears the `auth_token` cookie in the response, and serves the login page.
- Since the Core's own server sets the `Set-Cookie` header on its own response, cross-domain cookie issues are eliminated entirely.
- All future logout enhancements (refresh token revocation, audit logging, etc.) go in one place: the Core's middleware logout-reason handler.

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
- Configure the `CoreApi` block in `appsettings.json`:
    ```json
    "CoreApi": {
      "BaseUrl": "http://localhost:5254",
      "InternalSecret": "DevelopmentInternalSecret123!"
    }
    ```
  - `CoreApi:BaseUrl` — the URL of the running Core Monolith API.
  - `CoreApi:InternalSecret` — must match the Core's `InternalApiSettings:Secret` value exactly.
- Scaffold a `BackgroundService` (`HostedServices/PermissionRegistrationService.cs`) to HTTP POST the local permissions payload to the Monolith's internal `/api/internal/permissions/register` endpoint.
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

### 7. Configuration Reference (appsettings.json)

All required and optional configuration keys for a standard microservice. Use this as a checklist when setting up `appsettings.json`, `appsettings.Development.json`, and `appsettings.Production.json`.

#### `JwtSettings` ⚠️ Required (SSO Handshake)
These keys MUST match the Core Monolith's configuration exactly. They allow the microservice to mathematically validate the `auth_token` cookie without querying the Monolith for every request.

| Key | Dev Default | Description |
|---|---|---|
| `JwtSettings:Key` | `"DevelopmentSecretKey..."` | HMAC-SHA256 signing secret (≥ 32 chars). Must be identical to the Core's key. |
| `JwtSettings:Issuer` | `"IntranetPortalAPI"` | The `iss` claim. Must match the Core Monolith exactly. |
| `JwtSettings:Audience` | `"IntranetPortalFrontend"` | The `aud` claim. Must match the Core Monolith exactly. |

#### `CoreApi` ⚠️ Required (RBAC Bridge)
The microservice uses these settings to communicate with the Core Monolith's internal APIs, primarily for auto-registering its local permissions upon startup.

| Key | Dev Default | Description |
|---|---|---|
| `CoreApi:BaseUrl` | `"http://localhost:5254"` | The URL where the Core Monolith API is reachable. |
| `CoreApi:InternalSecret` | `"DevInternalSecret123!"` | Shared secret header for internal API-to-API calls. Must match the Core's `InternalApiSettings:Secret`. |

#### `ConnectionStrings` ⚠️ Required
The microservice is isolated and must have its own dedicated database.

| Key | Dev Default | Prod Example |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | `"Data Source=module.db"` | `"Server=127.0.0.1;Database=portal_module;Uid=...;Pwd=...;"` |

> [!NOTE]
> The dual-database switch (SQLite for Dev ↔ MySQL for Prod) is driven by `IsDevelopment()` in `Program.cs`. Do not use a custom config flag for this.

#### `AllowedOrigins` (Production only)
Used to configure CORS to allow the frontend to send the `auth_token` cookie with API requests.

| Key | Description |
|---|---|
| `AllowedOrigins` | JSON array of allowed CORS origins (e.g., `["https://module.company.com"]`). |

#### `AllowedHosts`
Standard ASP.NET Core host filtering.

| Environment | Recommended Value |
|---|---|
| Development | `"*"` |
| Production | `"module-api.yourcompany.com"` |

---

**AI Assistant:** When the user drops this file in your workspace, immediately acknowledge these architectural rules and offer to run `npx create-next-app@latest` followed by `dotnet new webapi --use-controllers` to scaffold the repository!
