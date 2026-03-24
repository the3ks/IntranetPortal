---
title: "Enterprise Permission Matrix (RBAC)"
description: "A high-level guide explaining how Roles, Scopes, and specific Permissions govern access across the Intranet Portal."
date: "2026-03-20"
author: "Architecture Team"
---

# Enterprise Permission Matrix

Welcome to the internal documentation for the Intranet Portal's **Role-Based Access Control (RBAC)** engine. As an Administrator or high-level manager, you have the ability to govern exactly what employees can see and do within this platform.

To ensure extreme flexibility for a multinational or multi-site company, the platform does not rely on simplistic "Admin" or "Staff" labels. Instead, it operates on a highly scalable **Resource-Scoped Permission Matrix**.

## 1. The Core Concepts & Architecture Diagram

Our security model breaks access down into four simple layers: **Who you are** (Employee), **What you can do** (Role), and **Where you can do it** (Site Scope).

![Enterprise Permissions Diagram](/rbac-diagram.png)

### The 4 Pillars of Access:
1. **Positions (HR Job Titles):** The literal real-world organizational label (e.g., *Chief Executive Officer*). Positions dictate who you are on the roster, but **they do not inherently grant any digital database access**.
2. **Permissions (Capabilities):** The exact, granular digital actions an account is allowed to take (e.g., `HR.Employee.Create`, `System.FullAccess`).
3. **Roles (Security Matrices):** A logical "keycard" grouping multiple Permissions together. A Role grants the actual authority to execute tasks (e.g., *Asset Manager* or *HR Editor*).
4. **Scopes (Site Boundaries):** An optional physical limit declaring precisely *where* a user's Role applies.

## 2. How The Matrix Works

Instead of vaguely granting someone "Admin Access" automatically due to their HR Position, the system strictly relies on assigning an employee a **Role**, and explicitly binding that Role to a **Site Scope**.

### Example 1: The Local Manager
Alice is the HR Manager exclusively for the **New York Office** (`SiteId = 1`).
- The system assigns Alice the `HR Manager` Role.
- The system bounds her `HR Manager` Role strictly to `SiteId = 1`.
- **Result:** Alice is securely granted the `HR.Employee.Edit` permission, but the backend API mathematically filters her queries so she can only load and edit employees who *also* belong to the New York office. The server actively refuses to send her data regarding the London Office.

### Example 2: The Global Director
Bob is the Global HR Director overseeing all locations.
- The system assigns Bob the `HR Manager` Role.
- The system binds his Role to a **Global Scope** (`SiteId = null` or Global).
- **Result:** Because Bob's Site boundary is null (Global), his `HR.Employee.Edit` permission unlocks his authority identically across every single Site in the database.

## 3. Why This Matters

This multi-tenant matrix prevents the system from breaking as the company scales. 

If the organization decides to build a brand new *Assets Management* module tomorrow, engineers do not have to rewrite the security ecosystem. The platform simply creates new granular capabilities (like `Assets.Delete`), attaches them to a new custom Role (like `Asset Manager`), and binds that Role to whichever physical Sites your warehouse workers belong to!

---

## 4. The `ISiteScoped` Implementation Matrix

In Enterprise Multi-Tenant systems, developers frequently make the mistake of using **Entity Framework Core Global Query Filters** (applying `modelBuilder.Entity<x>().HasQueryFilter(...)`). 

### The EF Core Blindspot:
If an HR Manager is assigned `HR.Employee.View` for **London**, but `Announcements.Edit` for **Tokyo**, a global EF framework will see they have access to *both* locations in general. When they attempt to query London Employees, the global DB interceptor will accidentally let them manipulate Tokyo Employees as well, because EF Core does **not** know which specific Controller capability (HR vs Announcements) is currently executing!

### Our Solution (The C# Extensions):
To protect granular capabilities and simultaneously prevent developers from forgetting to apply security loops, the Intranet Portal heavily leverages the `ISiteScoped` Database Model Interface.

Any time a new enterprise model is built (`WorkOrder.cs`, `Project.cs`), the engineer tags it with `ISiteScoped`.

Inside the API Controller, instead of writing raw SQL `Where()` clauses, they trigger our native extension pipeline:
```csharp
[HttpGet]
public async Task<IActionResult> GetWorkOrders() {
    var orders = await _context.WorkOrders
                        .ApplySiteScope(_permissionService, "WorkOrders.View")
                        .ToListAsync();
    return Ok(orders);
}
```
And to completely block unauthorized Mutating data transfers:
```csharp
[HttpPost]
public async Task<IActionResult> CreateOrder([FromBody] OrderDto dto) {
    if (!_permissionService.ValidateSiteScope("WorkOrders.Create", dto.SiteId)) return Forbid();
    // ... Database Execution ...
}
```
This guarantees the active HTTP context is perfectly mapped against the raw underlying capability mathematically, destroying the Mixed-Boundary bleed effect completely natively!
