---
Question:
- 1 more to-do item here: Audit Logs
- in term of system-wise, audit logs should be centralized in some shared component, or implemented in each module?
---
# Assets Management Module - Architecture Proposal

We've added the final missing puzzle piece for a true enterprise asset system: **Immutable Audit Logs**.

---

## 1. The Data Ecosystem (Refined Database Schema)
All tables use the `AM_` prefix. Configuration files are targeted for `IntranetPortal.Data/Data/Configurations/Assets/`.

### `AM_AssetCategories`
For enforcing categories and sub-categories (Max 2-levels).
- **Id** (Int, PK)
- **Name** (String)
- **ParentCategoryId** (Int, Nullable FK)

### `AM_AssetModels`
Abstracting Make & Model. 
- **Id** (Int, PK)
- **Manufacturer** (String, e.g., 'Apple')
- **Name** (String, e.g., 'MacBook Pro 16 M3')
- **CategoryId** (Int, FK to `AM_AssetCategories.Id`)

### `AM_Assets`
The core ledger.
- **Id** (Int, PK)
- **AssetTag** (String, Unique identifier)
- **ModelId** (Int, FK to `AM_AssetModels.Id`)
- **SerialNumber** (String)
- **Status** (Enum: `Available`, `Deployed`, `Assigned`, `In Maintenance`, `Retired`, `Lost`) 
- **PhysicalLocation** (String, Nullable - *e.g., "Meeting Room A"*)
- **SiteId** & **DepartmentId** (Int, FKs - *Dual-Axis RBAC Boundary*)
- **PurchaseDate** (DateTime, Nullable)
- **PurchasePrice** (Decimal, Nullable)
- **Vendor** (String, Nullable)
- **WarrantyExpiration** (DateTime, Nullable)

### `AM_AssetAssignments` 
Transaction log for specific people checking out hardware.
- **Id**, **AssetId**, **AssignedToEmployeeId**, **AssignedToTeamId**
- **DateAssigned** & **AssignedByEmployeeId**
- **ExpectedReturnDate**, **ActualReturnDate** & **ReturnedByEmployeeId**
- **ConditionOnAssign** & **ConditionOnReturn**

### `AM_AssetMaintenance`
- **Id**, **AssetId**, **MaintenanceDate**
- **Description**, **Cost**, **RepairVendor**, **LoggedByEmployeeId**

### [NEW] `AM_AssetAuditLogs`
The definitive immutable history timeline of an asset. This solves the tracking problem when someone moves a TV from "Meeting Room A" to "Storage Hub" or manually changes its status from `Deployed` to `Retired`. 
- **Id** (Int, PK)
- **AssetId** (Int, FK to `AM_Assets.Id`)
- **Action** (String - e.g., `Created`, `StatusChanged`, `LocationMoved`, `Assigned`, `MaintenanceLogged`)
- **OldValue** (String, Nullable - *Context of what changed*)
- **NewValue** (String, Nullable - *Context of what it changed to*)
- **Timestamp** (DateTime)
- **PerformedByEmployeeId** (Int, FK - *Exactly who triggered the change*)

---

## 2. API & Frontend Map

- **`GET /api/assets/{id}`** -> Will now also pull the `AM_AssetAuditLogs` descending.
- **UI:** The detailed View page (`src/app/assets/[id]/page.tsx`) will display a vertical "Timeline" or "History Log" showing the entire lifecycle of the asset from Purchase to Retirement.

---

## User Review Required

> [!NOTE]
> The **Audit Logs** table has been added to trap every state change over the life of the asset.

Does this Architecture Proposal covering models, assignments, maintenance, and comprehensive audit logging look 100% complete to you? If so, simply reply **"Approve"** and I will convert this directly into our executing Task list!
