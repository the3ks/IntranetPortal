# Assets Management Module - Architecture Proposal

You just caught another critical edge case: **"On Behalf Of" Requisitions**. 

If an IT item is requested by a Lead Engineer for their new Junior Developer, IT needs to know to assign the laptop to the Junior Developer, not the Lead Engineer! We have upgraded the Request Engine below to natively support this. 

---

## 1. Enterprise Reality Check (The Workflow)
1. **Top-Down Deployments (No Request Needed):** IT managers can proactively force an assignment to an employee bypassing the Request Engine entirely.
2. **Auto-Approve Consumables:** Pens bypass Manager Approval and drop straight to fulfillment.
3. **Out-of-Stock / Custom Procurement:** Supports `InProcurement` status for custom/backordered gear.
4. **"On Behalf Of" Requests [NEW]:** A manager can submit a request under their own name/authority, but explicitly designate a different target Employee who will receive the asset.

---

## 2. The Data Ecosystem (Refined Database Schema)
All tables use the `AM_` prefix. Configuration files are targeted for `IntranetPortal.Data/Data/Configurations/Assets/`.

### 2.1 The Request Engine [Upgraded]
- **`AM_AssetRequests`**
  - **Id** (Int, PK)
  - **RequestedByEmployeeId** (Int, FK - *The person who filled out the form, e.g., The Manager*)
  - **RequestedForEmployeeId** (Int, FK - *The person who actually needs the item, e.g., The Junior Dev*)
  - **RequestType** (Enum: `SerializedAsset`, `BulkAccessory`)
  - **RequestedCategoryId** & **RequestedModelId** & **RequestedAccessoryId**
  - **Quantity** (Int)
  - **Justification** (String)
  - **Status** (Enum: `PendingApproval`, `PendingFulfillment`, `InProcurement`, `Fulfilled`, `Rejected`, `Cancelled`)
  - **ManagerApprovedAt** & **ManagerApprovedByEmployeeId**
  - **FulfilledAt** & **FulfilledByEmployeeId**
  - **AssignedAssetId** (Int, FK Nullable - *The exact Hard Asset serial number that IT handed over*)
  - **CreatedAt** (DateTime) 

### 2.2 The Serialized Ledger (Hard Assets)
- **`AM_AssetCategories`**: Enforces categories hierarchies. Contains `RequiresApproval` boolean.
- **`AM_AssetModels`**: Abstracts Make & Model standardization.
- **`AM_Assets`**: Core ledger scoped securely by `SiteId` and `DepartmentId`.
- **`AM_AssetAssignments`**: The checkout ledger. Used for *both* fulfilling user requests and proactive lifecycle replacements.

### 2.3 The Consumables Ledger (Accessories)
- **`AM_Accessories`**: The stock pool management table tracking `AvailableQuantity`.
- **`AM_AccessoryCheckouts`**: The distribution log.

---

## 3. API & Frontend Map
- **`/api/asset-requests`**: Intelligent routing based on `RequiresApproval` flags.
- **UI:** Managers see a tailored queue of things they *actually* need to vet.

---

## User Review Required

> [!NOTE]
> Added `RequestedForEmployeeId`! If an employee asks for themselves, `RequestedBy` and `RequestedFor` are identical. If a manager asks for a staff member, they are different!

If we have all our bases covered with this incredible system design, give me the final **"Approve"** and let's execute!
