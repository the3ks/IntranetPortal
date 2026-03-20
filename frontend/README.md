# Enterprise Intranet Portal - Frontend UI

This is the Next.js React user interface for the Enterprise Intranet Portal. It is natively integrated with the ASP.NET Core API backend.

## Features
- **Dynamic RBAC (Role-Based Access Control)**: Secure JWT routing mapping C# capability constants directly into conditionally rendered DOM elements.
- **Enterprise Personnel Module**: Full CRUD operations for Employee management safely masking protected fields behind specific Database scopes.
- **Infrastructure Setup Matrix**: Raw text-area massive ingestion system capable of defining Sites, Departments, and Roles on a pristine deployment.
- **Internal Knowledge Base (Wiki)**: Seamless `.md` Markdown parser rendering localized architectural documentation directly into the dashboard securely.

---

## Environment & Deployment Configuration 🌍

Next.js utilizes a built-in cascading logic for compiling configuration files depending on the current operational state:

### 1. Local Development (`npm run dev`)
During local development, Next.js natively prioritizes `.env.local`. 
- By default, it expects the C# ASP.NET Core API to be running on your local machine at `http://localhost:5254`.
- The application will automatically route requests such as `/employees` to this local origin.

### 2. Production Deployment (`npm run build` && `npm start`)
When compiling the application for a live corporate server, Next.js **halts tracking of `.env.local`** and absorbs variables directly from `.env.production`.

**How to point the Frontend to your live Server:**
1. Open `.env.production`.
2. Locate the variable: `NEXT_PUBLIC_API_URL="https://api.yourcompany.com"`.
3. Change the string to the exact IP address or Domain Name of your internally hosted ASP.NET Core API.
4. Execute `npm run build` so Next.js can physically encode the new API coordinates directly into the optimized frontend bundles.

> **Bonus Enterprise Note:** If you are securely hosting on modern container orchestration platforms (like Docker, IIS, AWS, or Vercel), you do not need to modify `.env.production` at all! You can manually type the `NEXT_PUBLIC_API_URL` secret directly into your server's native Environment Variable configuration manager, and Next.js will automatically detect and absorb it flawlessly at runtime!

---

## 🚀 Getting Started Locally

```bash
# Install dependencies
npm install

# Run the development server
npm run dev
```

Open [http://localhost:3000](http://localhost:3000) with your browser to see the Dashboard output.
