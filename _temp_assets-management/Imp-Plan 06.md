# Assets Management Module - Final Implementation Plan

## 1. Goal & Description

The **Assets Management Module** delivers an enterprise-grade platform for tracking company resources. It natively handles both Serialized Capital Assets (Laptops, standing desks) and Bulk Consumables (Mice, pens).

### Key Architectural Pillars:
- **Dual-Axis Security:** All resources enforce `SiteId` and `DepartmentId` boundaries. IT only manages IT assets; Facilities only manages Facilities assets—even though they share the exact same UI and tables.
- **Top-Down & Self-Service Workflow:** Allows administrators to proactively push hardware to users or seamlessly handle "On-Behalf-Of" multi-tier user requisitions.
- **Strict Ledger Mutability:** Ensures full traceability via a localized `AM_AssetAuditLogs` table rather than relying on generic system tables.

---

## 2. Final Data Ecosystem (Database Models)
All tables operate under the `AM_` module prefix. Database configurations (`IEntityTypeConfiguration<T>`) will be isolated inside `IntranetPortal.Data/Data/Configurations/Assets/`.

### 2.1 The Master Data (Categories & Models)
- **`AM_AssetCategories`**: Hierarchical category tree. 
  - *Fields:* `Id`, `Name`, `ParentCategoryId`, `RequiresApproval` (Bool - allows cheap consumables to skip manager approval).
- **`AM_AssetModels`**: Normalizes Make/Model to prevent data entry spelling errors.
  - *Fields:* `Id`, `Manufacturer`, `Name`, `CategoryId`.

### 2.2 Ledger 1: Capital Assets (Serialized items tracked 1-to-1)
- **`AM_Assets`**: The core repository for hard tracking.
  - *Fields:* `Id`, `AssetTag`, `ModelId`, `SerialNumber`, `Status` (Enum: `Available`, `Deployed`, `Assigned`, `Maintenance`, `Retired`), `PhysicalLocation`, `SiteId`, `DepartmentId`, `PurchaseDate`, `PurchasePrice`, `Vendor`, `WarrantyExpiration`, `CreatedAt`, `CreatedByEmployeeId`.
- **`AM_AssetAssignments`**: The Checkout Log (for items destined to be returned).
  - *Fields:* `Id`, `AssetId`, `AssignedToEmployeeId`, `AssignedToTeamId`, `DateAssigned`, `ExpectedReturnDate`, `ActualReturnDate`, `Condition` logs, Audit Employee FKs.
- **`AM_AssetMaintenance`**: The Repair Log.
  - *Fields:* `Id`, `AssetId`, `MaintenanceDate`, `Description`, `Cost`, `RepairVendor`, `LoggedByEmployeeId`.
- **`AM_AssetAuditLogs`**: The strict history timeline (e.g., Status changed to XYZ).
  - *Fields:* `Id`, `AssetId`, `Action`, `OldValue`, `NewValue`, `Timestamp`, `PerformedByEmployeeId`.

### 2.3 Ledger 2: Consumables (Accessories bulk-tracked)
- **`AM_Accessories`**: The pool inventory for items like pens and cables.
  - *Fields:* `Id`, `Name`, `CategoryId`, `TotalQuantity`, `AvailableQuantity`, `MinStockThreshold`, `SiteId`, `DepartmentId`.
- **`AM_AccessoryCheckouts`**: The distribution ledger (items not strictly needing returns).
  - *Fields:* `Id`, `AccessoryId`, `RequestedByEmployeeId`, `FulfilledByEmployeeId`, `Quantity`, `CheckoutDate`, `Status`.

### 2.4 The Requisition Engine (Workflow)
- **`AM_AssetRequests`**: Drives the Two-Step flow (Manager Approval -> Department Fulfillment). Supports "On-Behalf-Of" requests.
  - *Fields:* `Id`, `RequestedByEmployeeId` (Form Submitter), `RequestedForEmployeeId` (Actual Recipient), `RequestType`, `RequestedCategoryId` (For custom specs), `RequestedModelId`, `RequestedAccessoryId`, `Justification`, `Status` (PendingApproval, PendingFulfillment, InProcurement, Fulfilled, Rejected), Audit timestamps/employee IDs, `AssignedAssetId` (FK mapping to the exact assigned hard asset).

---

## 3. Backend & Frontend Maps
- **API Controllers:** `AssetsController.cs`, `AccessoriesController.cs`, `AssetDictionariesController.cs` (Categories/Models), and `AssetRequestsController.cs`. All protected natively by `.ApplyDepartmentScope()` where applicable.
- **Next.js:** A unified UI under `src/app/assets/` segregated internally into "My Assets", "Team Approvals" (for managers), and "Administration Dashboard" (for IT/Facilities).

---

## User Review Required

> [!NOTE]
> Final sign-off required!

The logic, workflows, and strict enterprise rules have been fully documented into this master blueprint. Are you ready to **approve** this plan so we can transition into generating the C# code?
