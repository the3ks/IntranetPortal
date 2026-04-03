---
title: "[DEV] Setup Guide for Developers"
description: "How to boot and configure the Intranet Portal ecosystem locally."
date: "2026-03-19"
author: "Architecture Team"
---

# Local Development Setup

Welcome to the Intranet Portal's Developer Documentation Hub! This application runs on a fully decoupled **N-Tier Architecture** utilizing modern web capabilities.

## Codebase Structure
- **`/backend`**: Contains the ASP.NET Core 10 Web API (`IntranetPortal.Api`) and the Entity Framework Data layer (`IntranetPortal.Data`).
- **`/frontend`**: Contains the Next.js 16 Progressive Web App ecosystem. It utilizes **Turbopack** for local development and **Webpack** for reliable production builds (to support the Serwist PWA offline plugin).

## Running the API
1. Open a terminal inside `backend/IntranetPortal.Api`.
2. Ensure you have mapped your MySQL credentials in `appsettings.json`.
3. Run `dotnet ef database update` to sync the entity models.
4. Run `dotnet run` to boot the web host.

## Running the Portal UI (Local Development)
1. Open a terminal inside `/frontend`.
2. Ensure you have installed packages using `npm install`.
3. Run `npm run dev` to start the Next.js local server using Turbopack.
4. Access `http://localhost:3000` in Google Chrome. *(Note: Offline PWA features are disabled natively in development mode to allow Turbopack to compile smoothly).*

## Building for Production
To test progressive features natively or prepare for deployment:
1. Run `npm run build`. This strictly executes `next build --webpack` to generate the `public/sw.js` Service Worker file.
2. Run `npm run start` to boot the compiled Next.js Node app on port 3000.

## First-Time Setup & Database Seeding

The application features an automated, enterprise-grade database initialization sequence. 

### Production Environment
In a Production environment (`ASPNETCORE_ENVIRONMENT=Production`), the very first time the API boots up, the backend analyzes the MySQL database.
* If the `Roles` table is completely empty, the backend automatically generates the `System.FullAccess` matrix capability, the `Admin` Role, and binds them to the default `admin@company.com` login with the password `Admin123!`.
* Make sure to **change this password immediately** upon your first login!
* The database seeder will never run again as long as the system detects existing roles, protecting your live configurations from accidental overwrites.

### Local Development Environment
When running locally, the automatic startup script behaves identically. However, developers have an extra API utility available to them. 
If you are iterating on the database and need to violently reset the administration account natively to the newest models, you can invoke the seeding endpoint easily using **Swagger UI**:

1. Open your browser and navigate to your API's Swagger documentation: `http://localhost:<port>/swagger` (or whatever local port Kestrel output during boot).
2. Expand the **Auth** section and click on the **POST `/api/auth/seed-test-admin`** route.
3. Click the **"Try it out"** button, then hit **"Execute"**.

This will instantly drop the existing test user and regenerate a completely fresh account bound to the newest `System.FullAccess` matrix!

* *Security Note: This endpoint is strictly secured by environment variables. If you attempt to invoke this endpoint in a deployed Production build, the ASP.NET Core server will permanently block the request.*
