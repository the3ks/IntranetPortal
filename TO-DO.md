# Intranet Portal - Upcoming Features Roadmap

The following major architectural features have been designated for future development sprints to continue evolving the Enterprise Intranet Portal.

## 1. The Intranet Homepage & Announcements Dashboard 📰
**Objective:** Transform the main application root (`/`) into a dynamic, engaging internal news feed and company overview dashboard.
- **Core Feed:** A timeline of company announcements, critical updates, and organizational shifts.
- **Dynamic Widgets:** Modular elements natively integrating active employees, recent hires, or quick links.
- **Admin Publishing:** Dedicated administrative interfaces to draft, format (rich text), and broadcast these announcements across specific global Sites or Departments.

## 2. Intranet Wiki & Official Documentation Hub 📚
**Objective:** Build a robust, version-managed framework for official enterprise documentation, HR policies, and standard operating procedures (SOPs).
- **Staff Publishing Workflow:** Empower department heads to easily manage and publish formal guidelines natively.
- **Advanced RBAC Integrations:** Tie document visibility directly into the existing `SiteId` and `DepartmentId` mapping structures (e.g., specific IT policies perfectly siloed to the Singapore branch).
- **Interactive Formatting:** Produce beautifully formatted internal documentation dynamically rendered within the core Next.js application ecosystem mapping.

## Data validation
**Objective:** Make sure data is always consistent and not broken
- **Department/Teams:** once having employee in department/team, cannot delete department/team, nor move them to another site
- **Sites:** once having department/team in site, cannot delete site

## 3. Audit Logs in each module

## 4. JWT Security Hardening — Refresh Token & Server-Side Revocation

**Context:** Currently, `logoutAction()` only deletes the `auth_token` cookie in the browser. The JWT itself remains cryptographically valid on the backend for up to 8 hours after logout. A captured token could still be used post-logout.

**Proposed Solution: Short-lived JWT + Refresh Token (server-side revocation)**

- Shorten `auth_token` lifetime to **15 minutes**
- On login, backend issues a second **refresh token** (opaque, stored in DB) set as a separate `httpOnly` cookie (`refresh_token`, 8h)
- Add a `POST /api/auth/refresh` endpoint on the backend: validates the refresh token against DB, issues a new short-lived JWT
- Add a `POST /api/auth/logout` endpoint on the backend: marks the refresh token as revoked in DB
- Update `logoutAction()` to call the backend logout endpoint before deleting both cookies
- Add a silent background refresh in the main app root layout (client-side `setInterval` every ~10 min) to transparently renew the JWT while the user is active

**Microservice consideration:**
- Each standalone microservice frontend should also call the main app's `POST /api/auth/refresh` route (same root-domain cookie is sent automatically via `credentials: "include"`) to renew the token when the user is active in that microservice
- This prevents 401s when a user stays on a microservice tab beyond the JWT lifetime without the main app tab open

**Risk without this:** Acceptable for a corporate intranet (attacker must already be inside the network), but implementing this closes the post-logout token reuse window from 8h down to ≤15 min.

