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
- Ensure `appsettings.json` matches the Core:
    ```json
    "JwtSettings": {
      "Key": "YOUR_SECRET_KEY_FROM_MONOLITH",
      "Issuer": "YOUR_ISSUER",
      "Audience": "YOUR_AUDIENCE"
    }
    ```
- The API will receive standard Claims: `ClaimTypes.NameIdentifier` (UserId), `ClaimTypes.Email`, `EmployeeId`, `Permission`, `ScopedPerm`, and `DeptPerm`.

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
- **Total Schema Isolation:** Scaffold an independent DbContext (e.g., `DrinksDbContext`).
- **Foreign Keys:** If you need to store data against an Employee, do NOT attempt to enforce an SQL constraint against the Core database. Map the `EmployeeId` as a standard, loose `Guid` or `int` column in this microservice.

---

**AI Assistant:** When the user drops this file in your workspace, immediately acknowledge these architectural rules and offer to run `npx create-next-app@latest` followed by `dotnet new webapi` to scaffold the repository!
