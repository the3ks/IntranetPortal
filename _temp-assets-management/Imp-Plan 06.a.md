# Assets Management Administration UI

The core foundation for Assets Management is built, and employees can submit requisitions. However, we lack the administrative surfaces for department-level personnel to formally manage their physical hardware lifecycle, accessory bulk stock, and the system dictionaries (categories/models). 

As our enterprise RBAC supports dual-axis scoping (`SiteId` and `DepartmentId`), the management interface will empower not just IT, but facilities, HR, and other departments to manage their respective tracked assets (from laptops to tables and chairs) completely independently within the same software module.

## User Review Required

> [!IMPORTANT]
> The plan proposes *four* new distinct pages under the `/assets` route for tracking and managing inventory across organizational departments. Please review the navigation structure.

## Proposed Changes

We will introduce administration pages deeply integrated into the Sidebar's `assets` module context. These pages will fetch data from their respective `AssetsController`, `AccessoriesController`, and `AssetDictionariesController` in the .NET backend.

---

### Sidebar Navigation
Update the Next.js `Sidebar.tsx` to include links for bounded-context managers (validated via `AM.Inventory.Manage` scoped permissions).

#### [MODIFY] [Sidebar.tsx](file:///d:/DEV/IntranetPortal/frontend/src/components/layout/Sidebar.tsx)
- Add "Hardware Inventory" link navigating to `/assets/inventory`
- Add "Accessory Stockpile" link navigating to `/assets/accessories`
- Add "System Dictionaries" link navigating to `/assets/dictionaries`

---

### Hardware Inventory Pane
A data-grid and management UI for serialized capitalized assets.

#### [NEW] [inventory/page.tsx](file:///d:/DEV/IntranetPortal/frontend/src/app/assets/inventory/page.tsx)
- Fetches all `AM_Assets` directly from `/api/assets`.
- Renders a table displaying Asset Tag, Serial Number, Model, Status (Available, InUse, Repair, etc.), and assigned user.
- Includes a primary action to "Register New Asset" which opens a modal calling `createAssetAction`.

---

### Accessory Stockpile Pane
A dedicated view for bulk, non-serialized consumables (Mice, Keyboards, Adapters).

#### [NEW] [accessories/page.tsx](file:///d:/DEV/IntranetPortal/frontend/src/app/assets/accessories/page.tsx)
- Fetches all `AM_Accessories` from `/api/accessories`.
- Displays Accessory Name, Category, tracking thresholds, and current `StockQuantity`.
- Provides UI to "Add Stock" and "Register New Accessory Type".

---

### System Dictionaries Pane
A lightweight administrative interface for defining the taxonomy of assets.

#### [NEW] [dictionaries/page.tsx](file:///d:/DEV/IntranetPortal/frontend/src/app/assets/dictionaries/page.tsx)
- Fetches Categories and Models from `/api/assetdictionaries/...`.
- Provides a split-view or two-column form allowing administrators to define new Categories (e.g., "Monitors") and bounded Models under those categories (e.g., "Dell UltraSharp U2720Q").

### Decentralized Fulfillment Queue
The final phase of the requisition workflow, where departmental managers explicitly hand over the requested item.

#### [NEW] [fulfillment/page.tsx](file:///d:/DEV/IntranetPortal/frontend/src/app/assets/fulfillment/page.tsx)
- Fetches requests in `PendingFulfillment` status scoped to the manager's `DepartmentId`.
- Provides an overlay to select the specific serialized asset (or bulk accessory quantity) to permanently transfer custody to the requesting user.

## Open Questions

1. **Icons:** I'll continue using raw standard Heroicons SVGs for all these new tables and sidebars to strictly match the `dev_architecture-guidelines` rule we just established. Sound good?

## Verification Plan

### Automated Tests
- Ensure `npm run dev` hot reloads without terminal sequence errors.
- Ensure the TypeScript Language Server doesn't trigger component-missing errors.

### Manual Verification
- Walkthrough mapping Next.js frontend actions directly to `.NET` backend validation (e.g. attempting to create a new Asset successfully calls the API and revalidates the UI).
