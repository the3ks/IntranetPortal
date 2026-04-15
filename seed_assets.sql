-- ==========================================
-- Intranet Portal: Asset Management Seeding
-- ==========================================

SET FOREIGN_KEY_CHECKS = 0;

-- 1. Create Default Approver Groups
INSERT INTO `AM_ApproverGroups` (`Id`, `Name`, `IsActive`) VALUES
(1, 'IT Support Team', 1),
(2, 'Procurement Department', 1),
(3, 'Facilities Management', 1);

-- 2. Create Categories & Subcategories
-- Level 1 Categories
INSERT INTO `AM_AssetCategories` (`Id`, `Name`, `Description`, `ParentCategoryId`, `RequiresApproval`, `IsActive`, `AllowRequesterToSelectApprover`, `DefaultApproverGroupId`) VALUES
(1, 'Hardware', 'Bounded serialized IT equipment', NULL, 1, 1, 1, 1),
(2, 'IT Accessories', 'Bulk non-serialized IT peripherals', NULL, 0, 1, 0, 1),
(3, 'Stationaries', 'Office supplies and consumables', NULL, 0, 1, 0, 3);

-- Level 2 Subcategories (Hardware)
INSERT INTO `AM_AssetCategories` (`Id`, `Name`, `Description`, `ParentCategoryId`, `RequiresApproval`, `IsActive`, `AllowRequesterToSelectApprover`, `DefaultApproverGroupId`) VALUES
(10, 'Laptops', 'Standard issue laptops', 1, 1, 1, 1, 1),
(11, 'Desktops / PCs', 'Tower and mini PCs', 1, 1, 1, 1, 1),
(12, 'Monitors', 'Display units', 1, 1, 1, 1, 1);

-- Level 2 Subcategories (IT Accessories)
INSERT INTO `AM_AssetCategories` (`Id`, `Name`, `Description`, `ParentCategoryId`, `RequiresApproval`, `IsActive`, `AllowRequesterToSelectApprover`, `DefaultApproverGroupId`) VALUES
(20, 'Input Devices', 'Keyboards and Mice', 2, 0, 1, 0, 1),
(21, 'Audio / Visual', 'Headsets and Webcams', 2, 0, 1, 0, 1),
(22, 'Cables & Adapters', 'USB, HDMI, DisplayPort adapters', 2, 0, 1, 0, 1);

-- Level 2 Subcategories (Stationaries)
INSERT INTO `AM_AssetCategories` (`Id`, `Name`, `Description`, `ParentCategoryId`, `RequiresApproval`, `IsActive`, `AllowRequesterToSelectApprover`, `DefaultApproverGroupId`) VALUES
(30, 'Writing Instruments', 'Pens, markers, highlights', 3, 0, 1, 0, 3),
(31, 'Paper Products', 'Notebooks, copy paper, sticky notes', 3, 0, 1, 0, 3),
(32, 'Desk Organizers', 'Staplers, paperclips, tape dispensers', 3, 0, 1, 0, 3);

-- 3. Hardware: Create Asset Models
INSERT INTO `AM_AssetModels` (`Id`, `Name`, `Manufacturer`, `CategoryId`, `IsActive`) VALUES
(1, 'XPS 15 9520', 'Dell', 10, 1),
(2, 'MacBook Pro 14', 'Apple', 10, 1),
(3, 'ThinkPad T14', 'Lenovo', 10, 1),
(4, 'OptiPlex 7000', 'Dell', 11, 1),
(5, 'UltraSharp 27 4K USB-C', 'Dell', 12, 1);

-- 4. Hardware: 10 Bounded Assets
INSERT INTO `AM_Assets` (`Id`, `ModelId`, `AssetTag`, `SerialNumber`, `Status`, `PurchaseDate`, `WarrantyExpiration`, `PurchasePrice`, `PhysicalLocation`, `CreatedAt`) VALUES
(1, 1, 'LT-XPS-001', 'DX99P1', 0, '2025-01-10', '2028-01-10', 1899.99, 'HQ IT Vault', NOW()),
(2, 1, 'LT-XPS-002', 'DX99P2', 0, '2025-01-10', '2028-01-10', 1899.99, 'HQ IT Vault', NOW()),
(3, 2, 'LT-MBP-001', 'C02F99XQ1', 0, '2025-02-15', '2026-02-15', 1999.00, 'HQ IT Vault', NOW()),
(4, 2, 'LT-MBP-002', 'C02F99XQ2', 0, '2025-02-15', '2026-02-15', 1999.00, 'HQ IT Vault', NOW()),
(5, 3, 'LT-TP-001', 'PF99XZ1', 0, '2025-03-01', '2028-03-01', 1450.00, 'Branch Office Storage', NOW()),
(6, 3, 'LT-TP-002', 'PF99XZ2', 0, '2025-03-01', '2028-03-01', 1450.00, 'Branch Office Storage', NOW()),
(7, 4, 'PC-OPT-001', 'OPT99X1', 0, '2024-11-20', '2027-11-20', 899.99, 'HQ IT Vault', NOW()),
(8, 4, 'PC-OPT-002', 'OPT99X2', 0, '2024-11-20', '2027-11-20', 899.99, 'HQ IT Vault', NOW()),
(9, 5, 'MN-U27-001', 'CN099XZ1', 0, '2025-01-05', '2028-01-05', 650.00, 'HQ IT Vault', NOW()),
(10, 5, 'MN-U27-002', 'CN099XZ2', 0, '2025-01-05', '2028-01-05', 650.00, 'HQ IT Vault', NOW());

-- 5. IT Accessories: 10 Bulk Consumables 
INSERT INTO `AM_Accessories` (`Id`, `Name`, `CategoryId`, `TotalQuantity`, `AvailableQuantity`, `MinStockThreshold`) VALUES
(1, 'Logitech MX Master 3S Wireless Mouse', 20, 50, 50, 10),
(2, 'Logitech MX Keys Wireless Keyboard', 20, 40, 40, 5),
(3, 'Apple Magic Mouse', 20, 20, 20, 5),
(4, 'Logitech Zone Wireless Headset', 21, 30, 30, 8),
(5, 'Poly Voyager Focus 2', 21, 25, 25, 5),
(6, 'Logitech Brio 4K Webcam', 21, 15, 15, 3),
(7, 'Anker USB-C to HDMI Adapter', 22, 100, 100, 20),
(8, 'Anker USB-C Multiport Hub', 22, 60, 60, 10),
(9, 'Amazon Basics 2m HDMI Cable', 22, 200, 200, 50),
(10, 'Kensington Ergonomic Mouse Pad', 20, 45, 45, 10);

-- 6. Stationaries: 10 Bulk Consumables
INSERT INTO `AM_Accessories` (`Id`, `Name`, `CategoryId`, `TotalQuantity`, `AvailableQuantity`, `MinStockThreshold`) VALUES
(11, 'Bic Blue Ballpoint Pens (Box of 50)', 30, 200, 200, 20),
(12, 'Pilot Black Gel Pens (Box of 12)', 30, 150, 150, 15),
(13, 'Sharpie Yellow Highlighters (Pack of 6)', 30, 100, 100, 10),
(14, 'Expo Dry Erase Markers (Pack of 4)', 30, 80, 80, 10),
(15, 'Hammermill A4 Copy Paper (Ream - 500 Sheets)', 31, 500, 500, 50),
(16, 'Amazon Basics Legal Pads (Pack of 12)', 31, 60, 60, 5),
(17, 'Post-it Sticky Notes 3x3 (Pack of 24)', 31, 120, 120, 15),
(18, 'Swingline Standard Staples (Box of 5000)', 32, 40, 40, 5),
(19, 'Swingline Desktop Stapler', 32, 50, 50, 5),
(20, 'Scotch Transparent Tape (6 Pack)', 32, 80, 80, 10);

SET FOREIGN_KEY_CHECKS = 1;
