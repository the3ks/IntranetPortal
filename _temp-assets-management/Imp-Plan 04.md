---
Question:
there's another type of accessories, which is "office accessories": pen, board marker, paper, notebook... how to deal with them. Office Accessories might by managed by HR/Admin staff, not IT
---

# Assets Management Module - Architecture Proposal

---

## 1. The Data Ecosystem (Refined Database Schema)
All tables use the `AM_` prefix. Configuration files are targeted for `IntranetPortal.Data/Data/Configurations/Assets/`.

### 1.1 The Serialized Ledger (Hard Assets)
*Items that are serialized, returned, repaired, and tracked 1-to-1 (e.g. Laptops, Monitors).*

- **`AM_AssetCategories`**: Enforces categories hierarchies. (e.g., "Hardware" -> "Laptops", or "Peripherals" -> "Mice")
- **`AM_AssetModels`**: Abstracts Make & Model standardization.
- **`AM_Assets`**: Core ledger containing specific `SerialNumber` and `AssetTag`. Scoped by `SiteId` and `DepartmentId`.
- **`AM_AssetAssignments`**: The checkout ledger. Requires an item to be physically returned.
- **`AM_AssetMaintenance`**: Repair log for serialized items.
- **`AM_AssetAuditLogs`**: The immutable history ledger resolving "who moved this item?"

### 1.2 The Consumables Ledger (Accessories)
*Items that are bulk-managed, expended, and rarely returned (e.g., Wireless Mice, Keyboards, Notebooks).*

**The "IT vs. HR" Architecture:** Because we implemented **Dual-Axis RBAC**, we do *not* need separate tables for IT Accessories vs. HR Office Supplies. 
- An IT Mouse is inserted with `DepartmentId = IT`. 
- An HR Notebook is inserted with `DepartmentId = HR`.
- When an HR Admin logs in, the backend `.ApplyDepartmentScope()` completely hides IT's mice from them. They solely see their own pens, papers, and markers. The exact same unified UX drives both departments safely!

#### `AM_Accessories`
The stock pool management table.
- **Id** (Int, PK)
- **Name** (String, e.g., 'Logitech Wireless Mouse', 'A4 Printer Paper')
- **CategoryId** (Int, FK to `AM_AssetCategories.Id` - *e.g., "IT Accessories", "Office Supplies"*)
- **TotalQuantity** (Int, Total purchased/received)
- **AvailableQuantity** (Int, Automatically decremented on checkout)
- **MinStockThreshold** (Int, Nullable - *Trigger low-stock alerts*)
- **SiteId** & **DepartmentId** (Int, FKs - *The magic isolating IT gear from HR gear*)

#### `AM_AccessoryCheckouts`
The fulfillment log. When an employee requests an accessory, it routes securely to the matching `DepartmentId` owner.
- **Id** (Int, PK)
- **AccessoryId** (Int, FK)
- **RequestedByEmployeeId** (Int, FK - *Who asked for it*)
- **FulfilledByEmployeeId** (Int, Nullable Audit - *Which admin handed it out*)
- **Quantity** (Int, usually 1)
- **CheckoutDate** (DateTime)
- **Status** (Enum: `PendingRequest`, `Fulfilled`, `Denied`)

---

## 2. API & Frontend Map
- **`/api/assets` & `/api/accessories`**: Heavily filtered by `.ApplyDepartmentScope()`.
- **UI:** The dashboard will have distinct tabs: **"Capital Assets"** vs **"Accessories & Consumables"**.

---

## User Review Required

> [!NOTE]
> The beauty of Dual-Axis RBAC (SiteId + DepartmentId) means HR and IT can securely co-exist inside the `AM_Accessories` table without ever seeing each other's inventory!

If this architectural elegance completes the picture, hit me with **"Approve"** and we move to execution!
