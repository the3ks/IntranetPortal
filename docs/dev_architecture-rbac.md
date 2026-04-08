---
title: "[DEV] Technical Architecture: Enterprise Permission Matrix (RBAC)"
description: "A deep-dive engineering guide explaining how the ASP.NET Core backend mathematically enforces Roles, Scopes, and specific Permissions."
date: "2026-04-01"
author: "Architecture Team / Engineering Core"
---

# Technical Architecture: Enterprise Permission Matrix

Welcome to the engineering documentation for the Intranet Portal's **Role-Based Access Control (RBAC)** engine. To ensure extreme resilience and flexibility for a multinational company, the platform does not rely on simplistic "Admin" JWT claims. Instead, it operates on a highly scalable **Resource-Scoped Permission Matrix**.

## 1. The Data Model Architecture

Our security model mathematically maps access down into four database entities: **Who you are** (Employee), **What you can do** (Role), and **Where you can do it** (Site Scope).

![RBAC Permissions](media/RBAC-permissions.png)

### The Security Primitives:
1. **Positions (HR Job Titles):** The real-world organizational label. **These do not inherently grant any digital database access**. Do not write AuthZ handlers or `[Authorize(Roles = "...")]` attributes against Positions.
2. **Permissions (Capabilities):** System-defined strings representing granular actions (e.g., `HR.Employee.Create`, `System.FullAccess`). These belong to the internal enum and are seeded natively via `DatabaseSeeder.cs` during startup.
3. **Roles (Security Matrices):** A logical many-to-many grouping binding multiple Permissions together via the `RolePermissions` join table. 
4. **Scopes (Boundaries):** A physical or hierarchical limit declaring precisely *where* a user's Role applies. `SiteId` enforces geographic/functional boundaries, while `DepartmentId` enforces hierarchical boundaries.

## 2. The `ISiteScoped` & `IDepartmentScoped` Implementation Pipeline

In Enterprise Multi-Tenant systems, developers frequently make the catastrophic mistake of using **Entity Framework Core Global Query Filters** (applying `modelBuilder.Entity<x>().HasQueryFilter(...)`) to universally restrict tenant data.

### The EF Core Blindspot:
If an HR Manager is assigned `HR.Employee.View` for **London**, but `Announcements.Edit` for **Tokyo**, a global EF framework query filter will calculate that they have access to *both* locations generally. When they attempt to query London Employees, the global DB interceptor will blindly let them manipulate Tokyo Employees as well, because EF Core does **not** know which specific Controller capability (`HR` vs `Announcements`) is currently executing!

### Our Solution (The C# Extensions):
To protect granular capabilities and simultaneously prevent engineers from forgetting to apply massive security loops, the Intranet Portal heavily leverages the `ISiteScoped` and `IDepartmentScoped` entity interfaces on the data models.

Any time a new enterprise model is built (e.g., `WorkOrder.cs`, `PaymentRequest.cs`), the engineer **must** tag it with the appropriate scope interfaces.

Inside the .NET API Controller, instead of writing raw SQL `Where()` clauses, always trigger the native extension pipeline to append the `IQueryable` secure boundaries:

```csharp
[HttpGet]
public async Task<IActionResult> GetWorkOrders() {
    var orders = await _context.WorkOrders
                        // This extension parses the JWT, extracts the scopes specifically bound to this exact permission, and filters the SQL output.
                        .ApplySiteScope(_permissionService, "WorkOrders.View")
                        .ToListAsync();
    return Ok(orders);
}
```

And to completely block unauthorized Mutating data transfers (`POST`/`PUT`/`DELETE`):

```csharp
[HttpPost]
public async Task<IActionResult> CreateOrder([FromBody] OrderDto dto) {
    // Validate mathematically that the user possesses a role that contains the "WorkOrders.Create" permission explicitly scoped to the requested SiteId payload.
    if (!_permissionService.ValidateSiteScope("WorkOrders.Create", dto.SiteId)) {
        return Forbid();
    }
    
    // ... Database Execution ...
}
```

This guarantees the active HTTP context is perfectly mapped against the raw underlying capability geographically, destroying the Mixed-Boundary bleed effect completely natively, without relying on unstable Global Query Filters!

## 3. Role Delegation Engine (Temporary Substitutes)

The system supports **Delegation Overrides** (via the `RoleDelegations` table), allowing a *Source User* to temporarily lend their specific scoped role directly to a *Substitute User*.

- **Strict Constraint:** The substitute inherits *only* the specific `Permission + Scope` mapping being delegated, evaluated completely dynamically at runtime by the `PermissionService` memory cache. 
- Once the end date passes (`DateTime.UtcNow > Delegation.EndDate`), the override automatically drops out of the active claims resolution logic seamlessly.
