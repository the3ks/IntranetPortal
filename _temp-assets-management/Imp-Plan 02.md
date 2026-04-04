# Assets Management Module - Implementation Plan

## Step 1: Requirements Gathering (Brain Dump)

### 1. The Goal
To provide a centralized system for tracking, managing, and auditing company-owned physical assets (e.g., laptops, monitors, mobile devices). *Note: Software licenses are excluded per user feedback.* This module aims to prevent asset loss, streamline the onboarding and offboarding processes of employee hardware, and ensure accurate tracking of equipment locations, warranties, and maintenance lifecycles.

### 2. The Data Ecosystem
*   **Asset:** The core physical item being tracked.
    *   *Fields:* Id, AssetTag (unique), Name, Category (e.g., 'Laptop', 'Phone'), SerialNumber, Status (e.g., 'Available', 'Assigned', 'In Maintenance', 'Retired', 'Lost'), PurchaseDate, WarrantyExpiration, `SiteId` (Location context).
*   **AssetAssignment**: A transactional record linking an asset to an employee.
    *   *Fields:* Id, `AssetId`, `UserId` (or `EmployeeId`), DateAssigned, ExpectedReturnDate, ActualReturnDate, ConditionOnAssign, ConditionOnReturn.
*   **AssetMaintenance** (Optional/Future scope): A log for when assets are sent out for repair.
    *   *Fields:* Id, `AssetId`, MaintenanceDate, Description, Cost.

### 3. The Security/RBAC Constraints
*   **Standard Employees:** Can only view the list of assets currently assigned to them.
*   **IT/Asset Managers:** Can perform full CRUD operations (create, edit, assign, retire) on assets, but **strictly limited to their assigned `SiteId`** to respect cross-site boundaries.
*   **Global Administrators:** Have full visibility and management capabilities over all assets globally, unrestricted by `SiteId`.

---

## Step 2: Architecture Proposal

Based on the accepted Requirements (Step 1), here is the Architecture Proposal aligning fully with `dev_architecture-guidelines.md`.

### 1. Database Architecture
All tables will use the `AM_` prefix for the "Assets Management" targeted module. 
Table configurations will be safely isolated in `IntranetPortal.Data/Data/Configurations/Assets/`.

#### [NEW] `AM_Assets`
- Stores physical hardware details.
- **Fields:** `Id` (Int), `AssetTag` (String, Unique), `Name` (String), `Category` (String), `SerialNumber` (String), `Status` (Enum), `PurchaseDate` (DateTime, Nullable), `WarrantyExpiration` (DateTime, Nullable), `SiteId` (Int, FK to mapping sites).

#### [NEW] `AM_AssetAssignments`
- Stores transactional records of asset allocations to specific users.
- **Fields:** `Id` (Int), `AssetId` (Int, FK to AM_Assets), `UserId` (Int, FK to User/Employee identity), `DateAssigned` (DateTime), `ExpectedReturnDate` (DateTime, Nullable), `ActualReturnDate` (DateTime, Nullable), `ConditionOnAssign` (String), `ConditionOnReturn` (String).

#### [NEW] `AM_AssetMaintenance`
- Logs maintenance and repair history.
- **Fields:** `Id` (Int), `AssetId` (Int, FK to AM_Assets), `MaintenanceDate` (DateTime), `Description` (String), `Cost` (Decimal).

### 2. REST API Map (Backend Layer)
To cleanly segment the API layer without bloating core endpoints, we will introduce a dedicated backend controller.

#### [NEW] `IntranetPortal.Api/Controllers/AssetsController.cs`
- `GET /api/assets` (Filterable by `SiteId`, properly guarded by standard RBAC role checks).
- `GET /api/assets/{id}` (Read detailed specific asset, including historical assignments/maintenance).
- `POST /api/assets` (Create a new physical asset).
- `PUT /api/assets/{id}` (Update physical asset basics).
- `POST /api/assets/{id}/assign` (Record a new `AM_AssetAssignments` row).
- `POST /api/assets/{id}/maintenance` (Record a new `AM_AssetMaintenance` row).

#### Data Transfer Objects (DTOs)
*(Data mapping inside `IntranetPortal.Api/DTOs/Assets/`)*
- `AssetReadDto`, `AssetCreateDto`, `AssetUpdateDto`
- `AssetAssignmentDto`, `AssetAssignRequestDto`
- `AssetMaintenanceDto`, `AssetMaintenanceRequestDto`

### 3. Frontend Application Layer (Next.js Layer)
Extending the Next.js App Router using our preferred structure.

#### [NEW] Server Actions
- **`src/app/actions/assets.ts`**: Dedicated server actions strictly handling fetch/post operations to the `/api/assets` endpoints.

#### [NEW] UI Routes
- **`src/app/assets/page.tsx`**: The primary data table grid and dashboard for monitoring assets.
- **`src/app/assets/[id]/page.tsx`**: Asset granular detail view (to see its assignment history and maintenance logs).
- **`src/app/assets/components/`**: Location for tightly coupled elements (e.g., `<AssetStatusBadge />`, `<AssignAssetModal />`).

## User Review Required

> [!NOTE]
> Please review this complete Architecture Proposal. If everything looks good, please answer the final open question below, and we will proceed to Execution.

Before green-lighting this architecture:
1. **Category Structure:** Should the `Category` field on `AM_Assets` persist as a simple string (e.g., 'Laptop', 'Monitor'), or do you want a dedicated lookup table (e.g., `AM_AssetCategories`) to strictly enforce valid options in the UI?
