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
