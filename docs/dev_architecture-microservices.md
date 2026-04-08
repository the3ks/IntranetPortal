---
title: "[DEV] Architecture & Guidelines: Standalone Microservices"
description: "Guidelines and structure for building standalone microservice modules for the Intranet Portal."
date: "2026-04-08"
author: "Architecture Team"
---

# Standalone Microservices: Architecture & Structure

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

- **RESTful Endpoints**: The backend will be a standalone minimal .NET Web API project.
- **CORS Configuration**: If the microservice is hosted on a distinct subdomain (`drinks.intranet.com`), its .NET API must implement strict Cross-Origin Resource Sharing (CORS) rules allowing the frontend origin to communicate securely.
