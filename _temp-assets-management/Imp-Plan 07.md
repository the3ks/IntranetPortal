Frontend & Backend Plan: Shopping Cart & Multi-Item Requisitions
Based on your feedback, we need "a single request having multiple items." This requires us to refactor the current data model (which assumes 1 Request = 1 Item) to a Parent-Child relationship before we can build the frontend cart UI.

User Review Required
IMPORTANT

The introduction of multi-item requests means we have to make structural backend changes. Please review this updated full-stack plan.

Proposed Changes
Phase 1: Backend Data Model Refactor
AssetRequest
 (Parent)
Acts as the overall "Requisition Order" submitted by the user.
Contains: RequestedByEmployeeId, RequestedForEmployeeId, AggregateStatus (e.g., Partially Approved, Fulfilled), and CreatedAt.
[NEW] 
AssetRequestLineItem
 (Child)
Represents the individual items in the cart.
Moves the following properties from the parent: RequestedCategoryId, RequestedModelId, 
Quantity
, 
Justification
, and 
Status
 (Pending Approval, Fulfilled).
Independent Routing: Moves SelectedApproverEmployeeId and AssignedApproverGroupId down to the Line Item. This is crucial: if a user orders "IT Hardware" and "Stationaries" in the same cart, the IT Hardware line item can be routed to the IT Group, while the Stationaries line item is simultaneously routed to their specific Manager.
API Controllers & Migrations
Generate a new EF Core Migration for 
AssetRequestLineItem
.
Update 
CreateRequest
 to accept an array of line items.
Update 
GetPendingApprovals
 and 
AssetRequestsView
 to return and approve items at the line-item level.
Phase 2: Catalog & Shopping Cart Flow
[NEW] app/(assets)/assets/catalog/page.tsx
Build a visual catalog browsing page displaying available AssetModels and 
Accessories
.
Items will show their manufacturer, name, and stock status (but can be requested even if out of stock).
"Add to Requisition" Button: Adds the selected item to a persistent frontend Cart State (React Context).
[NEW] Global Cart / Requisition Drawer & Checkout
A slide-out or floating cart indicator showing items ready to be requested.
Checkout Page: Users can add "Custom Items" not found in the catalog.
Line-Item Routing: For any category in the cart where AllowRequesterToSelectApprover === true, a dropdown will automatically appear next to that specific item requesting the user to select the appropriate Manager/Approver.
Phase 3: Admin Dictionaries & Group Configuration
[NEW] app/(assets)/assets/dictionaries/ApproverGroupsManager.tsx
Unifying My Assets Dashboard & Sidebar Updates
The goal is to consolidate personal asset interaction into a single dashboard and prune the sidebar of redundant or restricted links.

Proposed Changes
Backend Controllers
[MODIFY] AssetRequestsController.cs
Add a new [HttpGet("is-approver")] endpoint returning true if the current user is a member of any 
ApproverGroup
.
Frontend React Components
[MODIFY] MyAssetsView.tsx
Re-architect as a unified dashboard.
Display "Assigned Hardware".
Import and render <AssetRequestsView type="mine" /> for My Requisitions.
If the user is an Approver, also render <AssetRequestsView type="approvals" /> for quick access.
[MODIFY] Sidebar.tsx
Remove "Requisition Center" (/assets/requests) entirely.
Add an API call to /api/assetrequests/is-approver setting isApprover state.
Conditionally render "Pending Approvals" (/assets/approvals) to ONLY show if isApprover or global admin.
[DELETE] /assets/requests/page.tsx
Delete the standalone Requisition Center page since it is now embedded in My Assets.
Verification Plan
Validate as Admin: Sidebar shows Pending Approvals. Dashboard shows everything.
Validate as normal user: Dashboard shows Assigned + Mine. Requisition center disappears, no Approvals on Sidebar.
Phase 4: Granular Category-Level Management (RBAC)
To allow specific teams (like Facilities) to manage specific assets (like Stationaries) without gaining access to the entire IT Asset Inventory:

1. Data Schema Addition
AssetCategory.cs
: Introduce an int? FulfillmentGroupId (mapped to the existing AM_ApproverGroups table) representing the specific staff group authorized to view, add stock, and fulfill items under this category.
2. Dictionaries Configuration Updates
Add an "Admin Management Group" dropdown within the System Dictionaries -> Category Configuration form. Admins will optionally explicitly bind a specific AM_ApproverGroup (e.g. "Facilities Handlers").
3. Backend Controller Filtering
In 
AssetRequestsController.cs
 and 
assets.ts
: Strip away the global Perm:Assets.Manage safety check as the sole gateway. Instead, intercept the user's 
ApproverGroup
 memberships natively.
During 
GetFulfillmentQueue()
, 
GetAccessories()
, and GetAssetsList(), only retrieve items whose parent 
AssetCategory
 has a FulfillmentGroupId matching one of the user's groups (unless they hold the Perm:Assets.Manage "super-admin" claim).
4. Sidebar Dynamic Rendering
Sidebar.tsx
 will no longer render the "Bulk Inventory Stockpile" or "Global Asset Ledger" purely based on Perm:Assets.Manage.
Instead, it will proactively check an endpoint (e.g., api/modules/asset-permissions) and only render the views if the user actually holds a management boundary over at least one mapped category.
Validation Strategy
Schema Integrity: Ensure EF Core migrations successfully transfer/rebuild the tables without dropping the application.
Line-Item Routing Check: Add one "Office Stationary" (needs Manager) and one "Laptop" (No manager needed) to the same cart. Verify only the Stationary item asks for a dropdown, and verify two distinct 
AssetRequestLineItem
 rows are generated under one 
AssetRequest
.
Queue Independence: Verify the IT Manager only sees the Laptop line item in their queue, while the selected Department Manager only sees the Stationary.
Manager Scoping: Create a user belonging to a "Facilities Handlers" Group, bind that group as the Fulfillment Group for the "Stationaries" category. Verify this user only sees Stationaries in the Bulk Stockpile and doesn't see "Laptops" in Fulfillment Queue.