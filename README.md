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
- Node.js version 20+
- .NET 10 SDK
- MySQL Server

### 2. Booting the Backend API
1. Navigate to the API application layer directory:
   ```bash
   cd backend/IntranetPortal.Api
   ```
2. Update the `appsettings.json` file to match your local MySQL credentials:
   `"DefaultConnection": "Server=localhost;Database=IntranetPortal;Uid=root;Pwd=;"`
3. Generate the required MySQL schema (if not previously applied via Entity Framework migrations):
   ```bash
   dotnet ef database update
   ```
4. Start the API background server:
   ```bash
   dotnet run
   ```
The API will inherently be available on local development ports (e.g., `http://localhost:5254`).

### 3. Setting up the Administrator Account
While the API is actively running, you can rapidly seed an initial Administrative testing profile into the local system instance. Open a **new terminal** and run a simple REST request:

**Using cURL / Bash:**
```bash
curl -X POST http://localhost:5254/api/auth/seed-test-admin
```
*(Alternatively, you can trigger this endpoint via Postman or Swagger UI).*

### 4. Running the Frontend Portal
1. Navigate into the frontend ecosystem directory:
   ```bash
   cd frontend
   ```
2. Install the necessary node dependencies and PWA logic:
   ```bash
   npm install
   ```
3. Initialize the Next.js Turbopack compiler engine:
   ```bash
   npm run dev
   ```
4. Open the interface at `http://localhost:3000` via your web browser. *(Chrome is recommended to test PWA installations natively).*
5. Seamlessly log in using the Administrator credentials generated in Step 3:
   - **Email:** `admin@company.com`
   - **Password:** `Admin123!`
