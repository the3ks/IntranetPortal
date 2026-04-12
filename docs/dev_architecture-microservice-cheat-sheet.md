# Microservice Extractor: Context Cheat Sheet

*Drag and drop this file into your brand new empty Workspace before you prompt the AI so it instantly remembers the overarching architecture!*

---

## The Goal
We are scaffolding a **Standalone Microservice Module** (e.g., Drink Ordering) that integrates cleanly into the parent `IntranetPortal` ecosystem. 
- It requires its own Next.js App Router frontend (e.g. port `3001`).
- It requires its own .NET Minimal API backend (e.g. port `8001`).
- It requires its own completely isolated SQL database schema.

## 1. Authentication Bridge (The "Magic" Token)
The Core Monolith issues a JWT upon login and saves it natively as a cookie named `auth_token`. **This microservice does NOT handle logins.** It simply piggybacks off the Monolith's token!

### Backend (.NET API) Setup
The new .NET API must authenticate users mathematically using the exact same `JwtSettings` from the Monolith.
**Requirements:**
- Include `Microsoft.AspNetCore.Authentication.JwtBearer`.
- Ensure `appsettings.json` matches the Core. **For local development, default to using these mock values unconditionally so no manual generation is needed:**
    ```json
    "JwtSettings": {
      "Key": "DevelopmentSecretKeyThatIsAtLeast32Bytes",
      "Issuer": "IntranetPortalLocal",
      "Audience": "IntranetPortalMicroservices"
    }
    ```
- The API will inherently receive the Monolith's standard Claims framework. 
  - **CRITICAL DATA RULE:** To map relationships to a physical person, you MUST explicitly extract the custom `"EmployeeId"` claim (`user.FindFirst("EmployeeId")?.Value`). 
  - **WARNING:** Do *not* extract `ClaimTypes.NameIdentifier`, as this will incorrectly bind your module to the arbitrary system `UserAccountId` and corrupt your relational integrity!

### Global Authorization / "God-Mode"
Microservice RBAC configuration MUST mathematically account for the `Admin` role and `System.FullAccess` god-mode. Do not use `.RequireClaim` stringently without incorporating these fallbacks, to ensure core administrators are not locked out. Example:
```csharp
options.AddPolicy("QueueAccess", p => p.RequireAssertion(context => 
    context.User.HasClaim("Permission", "Module.Queue.View") || 
    context.User.HasClaim("Permission", "System.FullAccess") ||
    context.User.IsInRole("Admin")));
```

### Frontend (Next.js) Setup
The new Next.js micro-frontend must blindly extract the `auth_token` cookie and decode it to know who the user is.
**Requirements:**
- A simple Server Action (or Middleware) that executes:
  ```typescript
  import { cookies } from "next/headers";
  import { jwtDecode } from "jwt-decode";
  
  export async function getUserSession() {
    const cookieStore = await cookies();
    const token = cookieStore.get("auth_token")?.value;
    if (!token) return null; // Unauthenticated -> redirect to http://localhost:3000/login
    try { return jwtDecode(token); } catch { return null; }
  }
  ```

## 2. Micro-Frontend Shell Consistency
To ensure users do not feel like they have traversed repositories, the Next.js visual shell must perfectly mimic the Core Monolith.

**UI Guidelines:**
- **Tailwind CSS:** Ensure Tailwind is scaffolding identical root variables/colors.
- **Sidebar & Header:** We must manually construct a clone of the `Sidebar.tsx` navigation block. 
  - Standard styling class for the Sidebar is typically `<aside className="bg-gray-900 text-gray-300 w-72...">`.
  - Include a "Back to Intranet Hub" external `<a>` link pointing back to the core monolith (`http://localhost:3000`) so the user can escape.
- **Root Wrapper:** Obey the golden structural rule: all pages MUST be wrapped natively in `<div className="max-w-7xl mx-auto space-y-8">`.

## 3. Database Rules
- **Database Provider (Dual Setup):**
  - Use **SQLite** for local development speed and **MySQL/Pomelo** for production.
  - Install both packages in the `.csproj`, but isolate SQLite using an MSBuild condition so it doesn't ship to production:
    ```xml
    <ItemGroup>
      <PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="8.0.2" />
    </ItemGroup>
    <ItemGroup Condition="'$(Configuration)' == 'Debug'">
      <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.2" />
    </ItemGroup>
    ```
  - In `Program.cs`, branch based on environment: `if (builder.Environment.IsDevelopment()) { options.UseSqlite(...); } else { options.UseMySql(...); }`
  - **EF Migrations:** Do *not* use migrations locally for SQLite to avoid cross-provider conflicts. Simply use `dbContext.Database.EnsureCreated()` during local startup!
- **Total Schema Isolation:** Scaffold an independent DbContext (e.g., `DrinksDbContext`).
- **Foreign Keys:** If you need to store data against an Employee, do NOT attempt to enforce an SQL constraint against the Core database. Map the `EmployeeId` as a standard, loose `Guid` or `int` column in this microservice.

## 4. RBAC Permission Auto-Registration
To seamlessly integrate security natively, this microservice should automatically broadcast any custom internal permissions it needs to the Core Monolith when it boots so they are saved into the global Permissions DB table.

**Implementation Rules:**
- Configure `appsettings.json` with an `InternalSecret` matching the Core Monolith's internal secret configuration.
- Scaffold a `BackgroundService` (`IHostedService`) designed to hit the Monolith's internal `/api/internal/permissions/register` endpoint on boot.
- Wrap this HTTP call in a `try/catch` and `while (!stoppingToken.IsCancellationRequested)` loop with a `Task.Delay(10000)`. It must **continuously retry** if the Monolith is offline, guaranteeing successful eventual syncing without blocking microservice startup!

## 5. Next.js CORS Integration Constraints
Because Next.js `fetch` commands structurally require `{ credentials: "include" }` to push your `auth_token` cross-port, your API endpoints **CANNOT** use `.AllowAnyOrigin()`.
- **Local Rules:** Explicitly use `policy.SetIsOriginAllowed(_ => true).AllowCredentials()...` inside the `IsDevelopment()` pipeline block.
- **Production Rules:** Explicitly enforce `.WithOrigins("https://your-production-subdomain.com").AllowCredentials()...` to block unauthorized CSRF data harvesting.

---

**AI Assistant:** When the user drops this file in your workspace, immediately acknowledge these architectural rules and offer to run `npx create-next-app@latest` followed by `dotnet new webapi` to scaffold the repository!
