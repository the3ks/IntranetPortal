---
title: "Enterprise Permission Matrix (RBAC)"
description: "A high-level guide explaining how Roles, Scopes, and specific Permissions govern access across the Intranet Portal."
date: "2026-03-20"
author: "Architecture Team"
---

# Enterprise Permission Matrix

Welcome to the internal documentation for the Intranet Portal's **Role-Based Access Control (RBAC)** engine. As an Administrator or high-level manager, you have the ability to govern exactly what employees can see and do within this platform.

To ensure extreme flexibility for a multinational or multi-site company, the platform does not rely on simplistic "Admin" or "Staff" labels. Instead, it operates on a highly scalable **Resource-Scoped Permission Matrix**.

## 1. The Core Concepts

The architecture physically separates enterprise structure into four distinct concepts:
1. **Positions (HR Job Titles):** The literal, real-world organizational label a staff member possesses on paper (e.g., *Chief Executive Officer*). Positions dictate exactly who you are on the company roster, but they **do not** inherently grant any digital database access.
2. **Permissions (Capabilities):** The exact, granular digital actions an account is allowed to take. (e.g., `HR.Employee.Create`, `Announcements.View`, `System.FullAccess`).
3. **Roles (Security Matrices):** A logical digital locker grouping multiple Permissions together. A Role grants the actual programmatic authority to execute tasks within the database. (e.g., *Asset Manager* or *HR Editor*).
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
