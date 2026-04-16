# Assets Management Module

Following **Step 1: Requirements Gathering** of the `dev_module-development-lifecycle.md`, here is the first draft blueprint for the new Assets Management module. 

## 1. The Goal
**What business problem does this solve?**

To provide a centralized system for tracking, managing, and auditing company-owned physical and digital assets (e.g., laptops, monitors, mobile devices, software licenses). This module aims to prevent asset loss, streamline the onboarding and offboarding processes of employee hardware, and ensure accurate tracking of equipment locations, warranties, and maintenance lifecycles.

## 2. The Data Ecosystem
**What specific entities and fields need to be tracked?**

*   **Asset:** The core item being tracked in the system.
    *   *Proposed Fields:* Id, AssetTag (unique), Name, Category (e.g., 'Laptop', 'Phone', 'License'), SerialNumber, Status (e.g., 'Available', 'Assigned', 'In Maintenance', 'Retired', 'Lost'), PurchaseDate, WarrantyExpiration, `SiteId` (Location context).
*   **AssetAssignment**: A transactional record linking an asset to an employee.
    *   *Proposed Fields:* Id, `AssetId`, `UserId` (or `EmployeeId`), DateAssigned, ExpectedReturnDate, ActualReturnDate, ConditionOnAssign, ConditionOnReturn.
*   **AssetMaintenance** (Optional/Future scope): A log for when assets are sent out for repair.
    *   *Proposed Fields:* Id, `AssetId`, MaintenanceDate, Description, Cost.

## 3. The Security/RBAC Constraints
**Who executes tasks?**

*   **Standard Employees:** Can only view the list of assets currently assigned to them. (Optionally, they can acknowledge receipt of a new assignment).
*   **IT/Asset Managers:** Can perform full CRUD operations (create, edit, assign, retire) on assets, but **strictly limited to their assigned `SiteId`** to respect cross-site boundaries.
*   **Global Administrators:** Have full visibility and management capabilities over all assets globally, unrestricted by `SiteId`.

## User Review Required

> [!NOTE]
> Please review the "Brain Dump" above and let me know your thoughts before we proceed to Step 2: Architecture Proposal.

1. Do we need to track *Software Licenses* under this module, or just physical hardware?
2. Does the proposed Data Ecosystem capture enough detail for your reporting needs (e.g., tracking costs, vendors)?
3. Are the `SiteId` constraints accurate for your current organizational structure?
