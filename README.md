# Intranet Portal

A modern, fast, and secure Intranet Portal application meticulously designed with a scalable, decoupled N-Tier enterprise architecture.

---

## 🏗️ Architecture Design

This project is separated into a Next.js Progressive Web App (PWA) client and an ASP.NET Core C# Backend API. They communicate entirely over REST API protocols, secured via HTTP-only JSON Web Tokens (JWT) and dynamic Role-Based Access Control (RBAC).

### Frontend (Next.js 16)
- **App Router**: Leverages modern React 19 Server Actions for fully secure, server-side data fetching and cookie mapping.
- **Turbopack & Serwist PWA**: Built for speed natively using the Turbopack compiler. The system employs `@serwist/next` to seamlessly compile the offline Service Workers required for Mobile PWA installation.
- **UI/UX**: Custom-designed, premium glassmorphic dashboard built natively on Tailwind CSS v4, supporting rich micro-animations and responsive flex layouts.

### Backend (ASP.NET Core 10)
- **N-Tier Clean Architecture**: Isolated logic inside `IntranetPortal.Api` (Presentation & Routing) and `IntranetPortal.Data` (Entity Framework Models & Object Mapping).
- **Database Architecture**: Standard relational structure driven by `MySQL` and `Pomelo.EntityFrameworkCore.MySql`. Features distinct domain models for `Employees`, `Departments`, `Sites`, and `Announcements`.
- **Security Hub**: An independent authentication model (`UserAccount`) securely wraps credentials with robust `BCrypt` password hashing. The `Employee` data models remain fully decoupled from sensitive authentication metrics.

---

## 🚀 Getting Started

### 1. Requirements
Ensure your development machine is prepared:
- Node.js 20+
- .NET 10 SDK
- MySQL / MariaDB Server

### 2. Booting the Backend API
```bash
cd backend/IntranetPortal.Api
dotnet ef database update
dotnet run
```
> For full configuration details (JWT keys, connection strings, security settings), see [`backend/IntranetPortal.Api/README.md`](backend/IntranetPortal.Api/README.md).

### 3. Setting up the Administrator Account
While the API is running, seed the initial admin account (Development only):
```bash
curl -X POST http://localhost:5254/api/auth/seed-test-admin
```
Credentials: `admin@company.com` / `Admin123!`

### 4. Running the Frontend Portal
```bash
cd frontend
npm install
npm run dev
```
Open `http://localhost:3000`. Log in with the credentials from Step 3.
> For environment variables, PWA config, and deployment notes, see [`frontend/README.md`](frontend/README.md).

### 5. AI-Assisted Development & Documentation
1. **Living Architecture Docs**: The `docs/ai-architectures/` directory contains the Master Artifacts (`implementation_plan.md`, `task.md`, `walkthrough.md`) serving as the permanent historical documentation hub for the project.
2. **Synchronizing Updates**: Whenever you finish building a major feature alongside an AI coding assistant, always append **"Sync the AI Docs"** to your final prompt. This ensures the agent explicitly exports its internal structural logs back into this repository so other developers can review the architectural decisions.