---
title: "[DEV] Architecture & Guidelines: Standalone Microservices"
description: "Guidelines and structure for building standalone microservice modules for the Intranet Portal."
date: "2026-04-08"
author: "Architecture Team"
---

# Standalone Microservices: Architecture & Structure

## The Big Picture: The "Independent Satellites"
The Intranet Portal utilizes a **Hybrid Architecture.** While the Core Monolith manages central global truths (Authentication, System Admin, Core HR mapping), we extract distinct functional domain boundaries into Standalone Microservices. 

These microservices act as completely decoupled "Satellites." They orchestrate their own physical databases and isolated Git repositories, yet they securely bridge back into the enterprise ecosystem by blindly verifying the Monolith's JWT `auth_token` cookies, and recursively auto-registering their local RBAC custom permissions securely to the Monolith via network jobs.

**When to Build a Microservice:**
You MUST extract your domain out to a Standalone Microservice if:
1. It is functionally distinct and highly isolated (e.g., *Drink Ordering*, *Vehicle Booking*, *Helpdesk*).
2. It can natively tolerate loose SQL relationships, storing pure `EmployeeId` generic keys without demanding hard SQL `INNER JOINS` against core tables.
3. The domain has independent SLA, traffic, or deployment lifecycles separate from the core company intranet platform stability.

*(If your feature does NOT meet these criteria—for instance, if it directly mutates foundational enterprise hierarchy—you must build it natively inside the central `IntranetPortal`. Refer to `dev_architecture-modular-monolith.md` for guidance).*

---

As the Intranet Portal ecosystem expands, significantly isolated domain boundaries (e.g., *Drink Ordering*, *HR Leaves*, *IT Helpdesk*) will be extracted into **Standalone Microservice Modules**. 

This document defines the structural requirements, integrations, and operational boundaries for building any new Microservice that connects to the Core Monolith.

---

## 1. Repository & Deployment Boundary

A Standalone Microservice is defined by its complete physical extraction from the `IntranetPortal` monolith codebase.

- **Independent Repositories**: Every new microservice must reside in its own Git repository.
- **Independent CI/CD**: Deployments for the microservice must not trigger or rely upon deployments of the Core Monolith. They should build and ship safely in isolation.
- **Subdomain Routing**: The frontend and API will ideally be exposed via subdomains (e.g., `drinks.intranet.com` and `api-drinks.intranet.com`) or seamlessly routed via an API Gateway/Reverse Proxy (e.g., `intranet.com/drinks`).

## 2. Infrastructure & Data Layer

Microservices must never reach directly into the Core Monolith's database.

- **Isolated Database**: The microservice MUST possess its own distinct database schema/server.
- **No Shared Tables**: If the microservice needs references to core entities (like `EmployeeId` or `DepartmentId`), it must store those keys as loose Foreign Keys (Integers/Guids) without an enforced SQL constraint back to the Core database.
- **Data Synchronization**: If the microservice requires rich data about an Employee (beyond just an ID), it must either fetch it via HTTP from the Core API or react to event-driven webhooks when Core data changes.

## 3. The Frontend (Next.js)

The frontend of a microservice acts as a **Micro-Frontend**.

- **Technology Stack**: It must be a standalone Next.js App Router application.
- **UI Shell Consistency**: The module must construct its own navigation Shell (Sidebar and Header) that visually mimics the Core Monolith to ensure users do not feel like they have left the ecosystem.
- **Global Sidebar Registration**: For users to discover the microservice naturally, a hardcoded external link (or reverse proxy path) must be registered inside the unified `Sidebar.tsx` file *within the Core Monolith repository*. 

## 4. Authentication & Security (JWT)

Because the microservice is completely detached, it cannot share the local authentication cookie of the Monolith by default.

- **Identity Provider (IdP)**: The Core Monolith API serves as the central Identity Provider. 
- **Token Validation**: The microservice must NOT issue its own login tokens. Instead, users will authenticate against the Core Monolith, receive a standardized JWT, and that JWT will be cleanly passed in the `Authorization: Bearer <token>` header to the microservice API.
- **Shared Secret / Verification**: The microservice API must be configured mathematically to validate the signature of the JWT issued by the Core.

## 5. API Layer

- **RESTful Endpoints**: The backend will be a standalone .NET Web API project utilizing **Controllers with direct DbContext injection** (the `Fat Controller` pattern) to perfectly mirror the Core Monolith's architecture. No Minimal APIs.
- **CORS Configuration**: If the microservice is hosted on a distinct subdomain (`drinks.intranet.com`), its .NET API must implement strict Cross-Origin Resource Sharing (CORS) rules allowing the frontend origin to communicate securely.

## 6. Cross-Boundary Data Scoping (Hierarchies)

Standalone Microservices do not directly access the `Sites`, `Departments`, or `Teams` lookup tables in the Core database. To enforce identical hierarchical boundaries securely:

- **JWT The Central Carrier:** The Core Monolith inherently injects `SiteId`, `DepartmentId`, and `TeamId` string claims directly into the `auth_token` JWT payload upon user login.
- **Reporting & Data Isolation:** Microservices process scoping locally based on two standard options:
  1. **The Frozen Snapshot (Data Denormalization - Preferred):** The Microservice permanently stores `SiteId`, `DepartmentId`, and `TeamId` as generic integers natively on its local transactional tables (e.g., `DrinkOrders`). This allows blazingly fast querying natively within the microservice controller (`db.Orders.Where(o => o.DepartmentId == claimsDeptId)`) inherently freezing the financial/eventual consistency of the hierarchy permanently at the time of the transaction.
  2. **Microservice Federation (API Aggregation):** If dynamic hierarchy evaluation is strictly required (e.g., querying real-time members of a department), the Microservice uses `IHttpClientFactory` to seamlessly request the current array of `EmployeeIds` from a Core backend Internal API, and subsequently filters its local SQL table dynamically.
