---
title: "Managing User Access & Employee Roles"
description: "A high-level guide explaining how Roles, Scopes, and specific Permissions govern access across the Intranet Portal."
date: "2026-04-01"
author: "System Administration"
---

# Managing User Access & Employee Roles

Welcome to the Administrator guide for the Intranet Portal's **Role-Based Access Control (RBAC)** system. As a Site Administrator or HR Manager, you have the ability to govern exactly what employees can see and do within this platform.

To ensure flexibility for our multinational organization, the platform does not rely on simplistic "Admin" or "Staff" labels. Instead, it operates on a highly scalable **Resource-Scoped Permission Matrix**.

## 1. The Core Concepts

Our security model breaks access down into four simple layers: **Who you are** (Employee), **What you can do** (Role), and **Where you can do it** (Scope).

![Enterprise Permissions Diagram](/rbac-diagram.png)

### The 4 Pillars of Access:
1. **Positions (HR Job Titles):** The literal real-world organizational label (e.g., *Chief Executive Officer*). Positions dictate an employee's title on the roster, but **they do not inherently grant any digital access to the portal**.
2. **Permissions (Capabilities):** The exact, granular digital actions an account is allowed to take (e.g., `Create Employees`, `System Configuration`).
3. **Roles (Security Matrices):** A logical grouping of multiple Permissions. A Role grants the actual authority to execute tasks in the system (e.g., *Asset Manager* or *HR Editor*). You will assign Roles to employees.
4. **Scopes (Boundaries):** A physical or hierarchical limit declaring precisely *where* a user's Role applies. Scopes can be **Functional** (Site-Wide, like the London Office) or **Hierarchical** (restricted to a specific Department, like IT).
5. **Delegations (Temporary Overrides):** The system's ability to temporarily transfer a specific scoped role from one user to a substitute.

## 2. Assigning Access: How The Matrix Works

Instead of vaguely granting someone "Admin Access" automatically due to their HR Position, you will assign an employee a **Role**, and explicitly bind that Role to a **Scope**.

### Example 1: The Local Manager
Alice is the HR Manager exclusively for the **New York Office**.
- You assign Alice the `HR Manager` Role in the portal.
- You bind her `HR Manager` Role strictly to the `New York` Site Scope.
- **Result:** Alice is securely granted the ability to edit employees, but the system filters her view so she can only load and edit employees who *also* belong to the New York office. The system actively hides data regarding the London Office from her view.

### Example 2: The Global Director
Bob is the Global HR Director overseeing all locations.
- You assign Bob the `HR Manager` Role.
- You bind his Role to the **Global Scope**.
- **Result:** Because Bob's scope is Global, his ability to edit employees unlocks his authority identically across every single Site in the database.

### Example 3: The Department Head (Hierarchical Scope)
Charlie is the IT Manager across the entire organization, but he only has authority over his own department.
- You assign Charlie the `Department Manager` Role.
- You bind his Role to the **IT Department Scope**.
- **Result:** Charlie is granted the `Approve Payments` permission, but he can only approve requests where the requester's Department matches his own. He cannot view or approve marketing or sales requests.

## 3. Role Delegation (Temporary Substitutes)

The system supports **Delegation**, allowing an employee to temporarily "lend" their specific scoped role to another worker (the *Substitute User*) while they are on vacation or leave.

- **Strict Constraint:** The substitute inherits *only* the specific Role and Scope being delegated (e.g., *IT Department Approval for New York*). They do not inherit the original user's email access or other private profile capabilities. 
- **Automatic Expiration:** Once the end date passes, the override automatically expires and the substitute loses the temporary access.

## 4. Why This Matters

This Matrix prevents our system from breaking as the company scales. 

If the organization decides to build a brand new *Assets Management* module tomorrow, you do not need to call IT to restructure the entire company layout. You simply create a new custom Role (like `Asset Manager`) in the Admin Dashboard, assign the new `Delete Assets` permission to it, and bind that Role to whichever physical Sites your warehouse workers belong to!
