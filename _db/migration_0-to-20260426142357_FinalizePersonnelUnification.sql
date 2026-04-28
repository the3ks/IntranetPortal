-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               11.4.2-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win64
-- HeidiSQL Version:             12.7.0.6850
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

-- Dumping structure for table intranetportal.am_accessories
CREATE TABLE IF NOT EXISTS `am_accessories` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) NOT NULL,
  `CategoryId` int(11) NOT NULL,
  `TotalQuantity` int(11) NOT NULL,
  `AvailableQuantity` int(11) NOT NULL,
  `MinStockThreshold` int(11) DEFAULT NULL,
  `SiteId` int(11) DEFAULT NULL,
  `DepartmentId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AM_Accessories_CategoryId` (`CategoryId`),
  KEY `IX_AM_Accessories_DepartmentId` (`DepartmentId`),
  KEY `IX_AM_Accessories_SiteId` (`SiteId`),
  CONSTRAINT `FK_AM_Accessories_AM_AssetCategories_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `am_assetcategories` (`Id`),
  CONSTRAINT `FK_AM_Accessories_Core_Sites_SiteId` FOREIGN KEY (`SiteId`) REFERENCES `core_sites` (`Id`),
  CONSTRAINT `FK_AM_Accessories_HR_Departments_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `hr_departments` (`Id`) ON DELETE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.am_accessories: ~2 rows (approximately)
DELETE FROM `am_accessories`;
INSERT INTO `am_accessories` (`Id`, `Name`, `CategoryId`, `TotalQuantity`, `AvailableQuantity`, `MinStockThreshold`, `SiteId`, `DepartmentId`) VALUES
	(1, 'Logitech MX Master 3', 6, 50, 45, 10, 1, 1),
	(2, 'Dell Universal Dock (D6000)', 6, 20, 2, 5, 1, 1);

-- Dumping structure for table intranetportal.am_accessorycheckouts
CREATE TABLE IF NOT EXISTS `am_accessorycheckouts` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AccessoryId` int(11) NOT NULL,
  `RequestedByEmployeeId` int(11) NOT NULL,
  `FulfilledByEmployeeId` int(11) DEFAULT NULL,
  `Quantity` int(11) NOT NULL,
  `CheckoutDate` datetime(6) NOT NULL,
  `Status` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AM_AccessoryCheckouts_AccessoryId` (`AccessoryId`),
  KEY `IX_AM_AccessoryCheckouts_FulfilledByEmployeeId` (`FulfilledByEmployeeId`),
  KEY `IX_AM_AccessoryCheckouts_RequestedByEmployeeId` (`RequestedByEmployeeId`),
  CONSTRAINT `FK_AM_AccessoryCheckouts_AM_Accessories_AccessoryId` FOREIGN KEY (`AccessoryId`) REFERENCES `am_accessories` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AM_AccessoryCheckouts_HR_Employees_FulfilledByEmployeeId` FOREIGN KEY (`FulfilledByEmployeeId`) REFERENCES `hr_employees` (`Id`) ON DELETE NO ACTION,
  CONSTRAINT `FK_AM_AccessoryCheckouts_HR_Employees_RequestedByEmployeeId` FOREIGN KEY (`RequestedByEmployeeId`) REFERENCES `hr_employees` (`Id`) ON DELETE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.am_accessorycheckouts: ~0 rows (approximately)
DELETE FROM `am_accessorycheckouts`;

-- Dumping structure for table intranetportal.am_approvergroupmembers
CREATE TABLE IF NOT EXISTS `am_approvergroupmembers` (
  `ApproverGroupId` int(11) NOT NULL,
  `EmployeeId` int(11) NOT NULL,
  PRIMARY KEY (`ApproverGroupId`,`EmployeeId`),
  KEY `IX_AM_ApproverGroupMembers_EmployeeId` (`EmployeeId`),
  CONSTRAINT `FK_AM_ApproverGroupMembers_AM_ApproverGroups_ApproverGroupId` FOREIGN KEY (`ApproverGroupId`) REFERENCES `am_approvergroups` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AM_ApproverGroupMembers_HR_Employees_EmployeeId` FOREIGN KEY (`EmployeeId`) REFERENCES `hr_employees` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.am_approvergroupmembers: ~0 rows (approximately)
DELETE FROM `am_approvergroupmembers`;

-- Dumping structure for table intranetportal.am_approvergroups
CREATE TABLE IF NOT EXISTS `am_approvergroups` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.am_approvergroups: ~3 rows (approximately)
DELETE FROM `am_approvergroups`;
INSERT INTO `am_approvergroups` (`Id`, `Name`, `IsActive`) VALUES
	(1, 'Global IT Approvers', 1),
	(2, 'IT & Operations Queue', 1),
	(3, 'Facilities & HR', 1);

-- Dumping structure for table intranetportal.am_approvergroupscopes
CREATE TABLE IF NOT EXISTS `am_approvergroupscopes` (
  `ApproverGroupId` int(11) NOT NULL,
  `DepartmentId` int(11) NOT NULL,
  PRIMARY KEY (`ApproverGroupId`,`DepartmentId`),
  KEY `IX_AM_ApproverGroupScopes_DepartmentId` (`DepartmentId`),
  CONSTRAINT `FK_AM_ApproverGroupScopes_AM_ApproverGroups_ApproverGroupId` FOREIGN KEY (`ApproverGroupId`) REFERENCES `am_approvergroups` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AM_ApproverGroupScopes_HR_Departments_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `hr_departments` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.am_approvergroupscopes: ~0 rows (approximately)
DELETE FROM `am_approvergroupscopes`;

-- Dumping structure for table intranetportal.am_assetassignments
CREATE TABLE IF NOT EXISTS `am_assetassignments` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AssetId` int(11) NOT NULL,
  `AssignedToEmployeeId` int(11) DEFAULT NULL,
  `AssignedToTeamId` int(11) DEFAULT NULL,
  `DateAssigned` datetime(6) NOT NULL,
  `AssignedByEmployeeId` int(11) NOT NULL,
  `ExpectedReturnDate` datetime(6) DEFAULT NULL,
  `ActualReturnDate` datetime(6) DEFAULT NULL,
  `ReturnedByEmployeeId` int(11) DEFAULT NULL,
  `ConditionOnAssign` varchar(500) DEFAULT NULL,
  `ConditionOnReturn` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AM_AssetAssignments_AssetId` (`AssetId`),
  KEY `IX_AM_AssetAssignments_AssignedByEmployeeId` (`AssignedByEmployeeId`),
  KEY `IX_AM_AssetAssignments_AssignedToEmployeeId` (`AssignedToEmployeeId`),
  KEY `IX_AM_AssetAssignments_AssignedToTeamId` (`AssignedToTeamId`),
  KEY `IX_AM_AssetAssignments_ReturnedByEmployeeId` (`ReturnedByEmployeeId`),
  CONSTRAINT `FK_AM_AssetAssignments_AM_Assets_AssetId` FOREIGN KEY (`AssetId`) REFERENCES `am_assets` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AM_AssetAssignments_Core_Teams_AssignedToTeamId` FOREIGN KEY (`AssignedToTeamId`) REFERENCES `core_teams` (`Id`),
  CONSTRAINT `FK_AM_AssetAssignments_HR_Employees_AssignedByEmployeeId` FOREIGN KEY (`AssignedByEmployeeId`) REFERENCES `hr_employees` (`Id`) ON DELETE NO ACTION,
  CONSTRAINT `FK_AM_AssetAssignments_HR_Employees_AssignedToEmployeeId` FOREIGN KEY (`AssignedToEmployeeId`) REFERENCES `hr_employees` (`Id`) ON DELETE NO ACTION,
  CONSTRAINT `FK_AM_AssetAssignments_HR_Employees_ReturnedByEmployeeId` FOREIGN KEY (`ReturnedByEmployeeId`) REFERENCES `hr_employees` (`Id`) ON DELETE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.am_assetassignments: ~1 rows (approximately)
DELETE FROM `am_assetassignments`;
INSERT INTO `am_assetassignments` (`Id`, `AssetId`, `AssignedToEmployeeId`, `AssignedToTeamId`, `DateAssigned`, `AssignedByEmployeeId`, `ExpectedReturnDate`, `ActualReturnDate`, `ReturnedByEmployeeId`, `ConditionOnAssign`, `ConditionOnReturn`) VALUES
	(1, 3, 1, NULL, '2026-04-28 09:58:07.043108', 1, NULL, NULL, NULL, NULL, NULL);

-- Dumping structure for table intranetportal.am_assetauditlogs
CREATE TABLE IF NOT EXISTS `am_assetauditlogs` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AssetId` int(11) NOT NULL,
  `Action` varchar(100) NOT NULL,
  `OldValue` longtext DEFAULT NULL,
  `NewValue` longtext DEFAULT NULL,
  `Timestamp` datetime(6) NOT NULL,
  `PerformedByEmployeeId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AM_AssetAuditLogs_AssetId` (`AssetId`),
  KEY `IX_AM_AssetAuditLogs_PerformedByEmployeeId` (`PerformedByEmployeeId`),
  CONSTRAINT `FK_AM_AssetAuditLogs_AM_Assets_AssetId` FOREIGN KEY (`AssetId`) REFERENCES `am_assets` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AM_AssetAuditLogs_HR_Employees_PerformedByEmployeeId` FOREIGN KEY (`PerformedByEmployeeId`) REFERENCES `hr_employees` (`Id`) ON DELETE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.am_assetauditlogs: ~0 rows (approximately)
DELETE FROM `am_assetauditlogs`;

-- Dumping structure for table intranetportal.am_assetcategories
CREATE TABLE IF NOT EXISTS `am_assetcategories` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `Description` varchar(500) DEFAULT NULL,
  `ParentCategoryId` int(11) DEFAULT NULL,
  `RequiresApproval` tinyint(1) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `AllowRequesterToSelectApprover` tinyint(1) NOT NULL DEFAULT 0,
  `DefaultApproverGroupId` int(11) DEFAULT NULL,
  `FulfillmentGroupId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AM_AssetCategories_ParentCategoryId` (`ParentCategoryId`),
  KEY `IX_AM_AssetCategories_DefaultApproverGroupId` (`DefaultApproverGroupId`),
  KEY `IX_AM_AssetCategories_FulfillmentGroupId` (`FulfillmentGroupId`),
  CONSTRAINT `FK_AM_AssetCategories_AM_ApproverGroups_DefaultApproverGroupId` FOREIGN KEY (`DefaultApproverGroupId`) REFERENCES `am_approvergroups` (`Id`) ON DELETE SET NULL,
  CONSTRAINT `FK_AM_AssetCategories_AM_ApproverGroups_FulfillmentGroupId` FOREIGN KEY (`FulfillmentGroupId`) REFERENCES `am_approvergroups` (`Id`) ON DELETE SET NULL,
  CONSTRAINT `FK_AM_AssetCategories_AM_AssetCategories_ParentCategoryId` FOREIGN KEY (`ParentCategoryId`) REFERENCES `am_assetcategories` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.am_assetcategories: ~6 rows (approximately)
DELETE FROM `am_assetcategories`;
INSERT INTO `am_assetcategories` (`Id`, `Name`, `Description`, `ParentCategoryId`, `RequiresApproval`, `IsActive`, `AllowRequesterToSelectApprover`, `DefaultApproverGroupId`, `FulfillmentGroupId`) VALUES
	(1, 'IT Hardware', 'High-value, serialized equipment like laptops and servers.', NULL, 1, 1, 0, 1, NULL),
	(2, 'Accessories & Peripherals', 'Low value items like mice.', NULL, 1, 1, 0, 2, NULL),
	(3, 'Office Stationaries', 'Standard office supplies.', NULL, 1, 1, 1, NULL, NULL),
	(4, 'Laptops', '', 1, 1, 1, 0, NULL, NULL),
	(5, 'Monitors', '', 1, 1, 1, 0, NULL, NULL),
	(6, 'Peripherals', '', 2, 1, 1, 0, NULL, NULL);

-- Dumping structure for table intranetportal.am_assetcategoryapprovergroups
CREATE TABLE IF NOT EXISTS `am_assetcategoryapprovergroups` (
  `AssetCategoryId` int(11) NOT NULL,
  `ApproverGroupId` int(11) NOT NULL,
  PRIMARY KEY (`AssetCategoryId`,`ApproverGroupId`),
  KEY `IX_AM_AssetCategoryApproverGroups_ApproverGroupId` (`ApproverGroupId`),
  CONSTRAINT `FK_AM_AssetCategoryApproverGroups_AM_ApproverGroups_ApproverGro~` FOREIGN KEY (`ApproverGroupId`) REFERENCES `am_approvergroups` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AM_AssetCategoryApproverGroups_AM_AssetCategories_AssetCateg~` FOREIGN KEY (`AssetCategoryId`) REFERENCES `am_assetcategories` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.am_assetcategoryapprovergroups: ~3 rows (approximately)
DELETE FROM `am_assetcategoryapprovergroups`;
INSERT INTO `am_assetcategoryapprovergroups` (`AssetCategoryId`, `ApproverGroupId`) VALUES
	(1, 1),
	(2, 2),
	(3, 3);

-- Dumping structure for table intranetportal.am_assetmaintenance
CREATE TABLE IF NOT EXISTS `am_assetmaintenance` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AssetId` int(11) NOT NULL,
  `MaintenanceDate` datetime(6) NOT NULL,
  `Description` varchar(1000) NOT NULL,
  `Cost` decimal(18,2) DEFAULT NULL,
  `RepairVendor` varchar(200) DEFAULT NULL,
  `LoggedByEmployeeId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AM_AssetMaintenance_AssetId` (`AssetId`),
  KEY `IX_AM_AssetMaintenance_LoggedByEmployeeId` (`LoggedByEmployeeId`),
  CONSTRAINT `FK_AM_AssetMaintenance_AM_Assets_AssetId` FOREIGN KEY (`AssetId`) REFERENCES `am_assets` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AM_AssetMaintenance_HR_Employees_LoggedByEmployeeId` FOREIGN KEY (`LoggedByEmployeeId`) REFERENCES `hr_employees` (`Id`) ON DELETE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.am_assetmaintenance: ~0 rows (approximately)
DELETE FROM `am_assetmaintenance`;

-- Dumping structure for table intranetportal.am_assetmodels
CREATE TABLE IF NOT EXISTS `am_assetmodels` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Manufacturer` varchar(100) NOT NULL,
  `Name` varchar(200) NOT NULL,
  `CategoryId` int(11) NOT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
  KEY `IX_AM_AssetModels_CategoryId` (`CategoryId`),
  CONSTRAINT `FK_AM_AssetModels_AM_AssetCategories_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `am_assetcategories` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.am_assetmodels: ~3 rows (approximately)
DELETE FROM `am_assetmodels`;
INSERT INTO `am_assetmodels` (`Id`, `Manufacturer`, `Name`, `CategoryId`, `IsActive`) VALUES
	(1, 'Dell', 'XPS 15', 4, 1),
	(2, 'Lenovo', 'ThinkPad T14', 4, 1),
	(3, 'Dell', 'UltraSharp U2720Q', 5, 1);

-- Dumping structure for table intranetportal.am_assetrequestlineitems
CREATE TABLE IF NOT EXISTS `am_assetrequestlineitems` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AssetRequestId` int(11) NOT NULL,
  `Type` int(11) NOT NULL,
  `RequestedCategoryId` int(11) DEFAULT NULL,
  `RequestedModelId` int(11) DEFAULT NULL,
  `RequestedAccessoryId` int(11) DEFAULT NULL,
  `Quantity` int(11) NOT NULL,
  `Justification` varchar(2000) NOT NULL,
  `Status` int(11) NOT NULL,
  `SelectedApproverEmployeeId` int(11) DEFAULT NULL,
  `AssignedApproverGroupId` int(11) DEFAULT NULL,
  `ApprovedAt` datetime(6) DEFAULT NULL,
  `ApprovedByEmployeeId` int(11) DEFAULT NULL,
  `FulfilledAt` datetime(6) DEFAULT NULL,
  `FulfilledByEmployeeId` int(11) DEFAULT NULL,
  `AssignedAssetId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AM_AssetRequestLineItems_ApprovedByEmployeeId` (`ApprovedByEmployeeId`),
  KEY `IX_AM_AssetRequestLineItems_AssetRequestId` (`AssetRequestId`),
  KEY `IX_AM_AssetRequestLineItems_AssignedApproverGroupId` (`AssignedApproverGroupId`),
  KEY `IX_AM_AssetRequestLineItems_AssignedAssetId` (`AssignedAssetId`),
  KEY `IX_AM_AssetRequestLineItems_FulfilledByEmployeeId` (`FulfilledByEmployeeId`),
  KEY `IX_AM_AssetRequestLineItems_RequestedAccessoryId` (`RequestedAccessoryId`),
  KEY `IX_AM_AssetRequestLineItems_RequestedCategoryId` (`RequestedCategoryId`),
  KEY `IX_AM_AssetRequestLineItems_RequestedModelId` (`RequestedModelId`),
  KEY `IX_AM_AssetRequestLineItems_SelectedApproverEmployeeId` (`SelectedApproverEmployeeId`),
  CONSTRAINT `FK_AM_AssetRequestLineItems_AM_Accessories_RequestedAccessoryId` FOREIGN KEY (`RequestedAccessoryId`) REFERENCES `am_accessories` (`Id`),
  CONSTRAINT `FK_AM_AssetRequestLineItems_AM_ApproverGroups_AssignedApproverG~` FOREIGN KEY (`AssignedApproverGroupId`) REFERENCES `am_approvergroups` (`Id`),
  CONSTRAINT `FK_AM_AssetRequestLineItems_AM_AssetCategories_RequestedCategor~` FOREIGN KEY (`RequestedCategoryId`) REFERENCES `am_assetcategories` (`Id`),
  CONSTRAINT `FK_AM_AssetRequestLineItems_AM_AssetModels_RequestedModelId` FOREIGN KEY (`RequestedModelId`) REFERENCES `am_assetmodels` (`Id`),
  CONSTRAINT `FK_AM_AssetRequestLineItems_AM_AssetRequests_AssetRequestId` FOREIGN KEY (`AssetRequestId`) REFERENCES `am_assetrequests` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AM_AssetRequestLineItems_AM_Assets_AssignedAssetId` FOREIGN KEY (`AssignedAssetId`) REFERENCES `am_assets` (`Id`),
  CONSTRAINT `FK_AM_AssetRequestLineItems_HR_Employees_ApprovedByEmployeeId` FOREIGN KEY (`ApprovedByEmployeeId`) REFERENCES `hr_employees` (`Id`) ON DELETE NO ACTION,
  CONSTRAINT `FK_AM_AssetRequestLineItems_HR_Employees_FulfilledByEmployeeId` FOREIGN KEY (`FulfilledByEmployeeId`) REFERENCES `hr_employees` (`Id`) ON DELETE NO ACTION,
  CONSTRAINT `FK_AM_AssetRequestLineItems_HR_Employees_SelectedApproverEmploy~` FOREIGN KEY (`SelectedApproverEmployeeId`) REFERENCES `hr_employees` (`Id`) ON DELETE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.am_assetrequestlineitems: ~0 rows (approximately)
DELETE FROM `am_assetrequestlineitems`;

-- Dumping structure for table intranetportal.am_assetrequests
CREATE TABLE IF NOT EXISTS `am_assetrequests` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RequestedByEmployeeId` int(11) NOT NULL,
  `RequestedForEmployeeId` int(11) NOT NULL,
  `Status` int(11) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AM_AssetRequests_RequestedByEmployeeId` (`RequestedByEmployeeId`),
  KEY `IX_AM_AssetRequests_RequestedForEmployeeId` (`RequestedForEmployeeId`),
  CONSTRAINT `FK_AM_AssetRequests_HR_Employees_RequestedByEmployeeId` FOREIGN KEY (`RequestedByEmployeeId`) REFERENCES `hr_employees` (`Id`) ON DELETE NO ACTION,
  CONSTRAINT `FK_AM_AssetRequests_HR_Employees_RequestedForEmployeeId` FOREIGN KEY (`RequestedForEmployeeId`) REFERENCES `hr_employees` (`Id`) ON DELETE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.am_assetrequests: ~0 rows (approximately)
DELETE FROM `am_assetrequests`;

-- Dumping structure for table intranetportal.am_assets
CREATE TABLE IF NOT EXISTS `am_assets` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AssetTag` varchar(100) NOT NULL,
  `ModelId` int(11) NOT NULL,
  `SerialNumber` varchar(100) DEFAULT NULL,
  `Status` int(11) NOT NULL,
  `PhysicalLocation` varchar(200) DEFAULT NULL,
  `SiteId` int(11) DEFAULT NULL,
  `DepartmentId` int(11) DEFAULT NULL,
  `PurchaseDate` datetime(6) DEFAULT NULL,
  `PurchasePrice` decimal(18,2) DEFAULT NULL,
  `Vendor` varchar(200) DEFAULT NULL,
  `WarrantyExpiration` datetime(6) DEFAULT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `CreatedByEmployeeId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_AM_Assets_AssetTag` (`AssetTag`),
  KEY `IX_AM_Assets_CreatedByEmployeeId` (`CreatedByEmployeeId`),
  KEY `IX_AM_Assets_DepartmentId` (`DepartmentId`),
  KEY `IX_AM_Assets_ModelId` (`ModelId`),
  KEY `IX_AM_Assets_SiteId` (`SiteId`),
  CONSTRAINT `FK_AM_Assets_AM_AssetModels_ModelId` FOREIGN KEY (`ModelId`) REFERENCES `am_assetmodels` (`Id`),
  CONSTRAINT `FK_AM_Assets_Core_Sites_SiteId` FOREIGN KEY (`SiteId`) REFERENCES `core_sites` (`Id`),
  CONSTRAINT `FK_AM_Assets_HR_Departments_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `hr_departments` (`Id`) ON DELETE NO ACTION,
  CONSTRAINT `FK_AM_Assets_HR_Employees_CreatedByEmployeeId` FOREIGN KEY (`CreatedByEmployeeId`) REFERENCES `hr_employees` (`Id`) ON DELETE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.am_assets: ~5 rows (approximately)
DELETE FROM `am_assets`;
INSERT INTO `am_assets` (`Id`, `AssetTag`, `ModelId`, `SerialNumber`, `Status`, `PhysicalLocation`, `SiteId`, `DepartmentId`, `PurchaseDate`, `PurchasePrice`, `Vendor`, `WarrantyExpiration`, `CreatedAt`, `CreatedByEmployeeId`) VALUES
	(1, 'IT-LPT-001', 1, 'XPS15-A1', 0, NULL, 1, 1, '2025-10-28 09:58:06.868299', NULL, NULL, NULL, '2026-04-28 09:58:06.866857', NULL),
	(2, 'IT-LPT-002', 1, 'XPS15-A2', 0, NULL, 1, 1, '2025-11-28 09:58:06.868447', NULL, NULL, NULL, '2026-04-28 09:58:06.868445', NULL),
	(3, 'IT-LPT-003', 2, 'TPT14-B1', 2, NULL, 1, 1, '2026-02-28 09:58:06.868449', NULL, NULL, NULL, '2026-04-28 09:58:06.868448', NULL),
	(4, 'IT-LPT-004', 2, 'TPT14-B2', 3, NULL, 1, 1, '2026-02-28 09:58:06.868450', NULL, NULL, NULL, '2026-04-28 09:58:06.868449', NULL),
	(5, 'IT-MON-001', 3, 'U2720Q-001', 0, NULL, 1, 1, NULL, NULL, NULL, NULL, '2026-04-28 09:58:06.868450', NULL);

-- Dumping structure for table intranetportal.core_announcements
CREATE TABLE IF NOT EXISTS `core_announcements` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` longtext NOT NULL,
  `Content` longtext NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `AuthorId` int(11) NOT NULL,
  `SiteId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Core_Announcements_AuthorId` (`AuthorId`),
  KEY `IX_Core_Announcements_SiteId` (`SiteId`),
  CONSTRAINT `FK_Core_Announcements_Core_Sites_SiteId` FOREIGN KEY (`SiteId`) REFERENCES `core_sites` (`Id`),
  CONSTRAINT `FK_Core_Announcements_HR_Employees_AuthorId` FOREIGN KEY (`AuthorId`) REFERENCES `hr_employees` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.core_announcements: ~0 rows (approximately)
DELETE FROM `core_announcements`;

-- Dumping structure for table intranetportal.core_auditlogs
CREATE TABLE IF NOT EXISTS `core_auditlogs` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `IPAddress` varchar(45) DEFAULT NULL,
  `Action` varchar(150) NOT NULL,
  `Timestamp` datetime(6) NOT NULL,
  `UserAgent` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.core_auditlogs: ~30 rows (approximately)
DELETE FROM `core_auditlogs`;
INSERT INTO `core_auditlogs` (`Id`, `UserId`, `Email`, `IPAddress`, `Action`, `Timestamp`, `UserAgent`) VALUES
	(1, 1, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-25 08:00:12.859460', 'node'),
	(2, 1, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-26 02:34:22.491830', 'node'),
	(3, 1, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-26 02:49:08.236674', 'node'),
	(4, 1, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-26 03:11:17.815693', 'node'),
	(5, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-26 09:35:22.469389', 'node'),
	(6, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-26 09:35:42.064337', 'node'),
	(7, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-27 15:09:14.083658', 'node'),
	(8, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 04:35:56.630450', 'node'),
	(9, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 04:41:56.005086', 'node'),
	(10, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 09:01:46.585293', 'node'),
	(11, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 09:04:15.484612', 'node'),
	(12, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 09:33:28.915575', 'node'),
	(13, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 09:57:22.583414', 'node'),
	(14, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 12:37:32.515075', 'node'),
	(15, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 12:37:41.384643', 'node'),
	(16, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 12:52:58.080203', 'node'),
	(17, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 12:53:11.937759', 'node'),
	(18, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 12:58:23.143897', 'node'),
	(19, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 12:58:29.461219', 'node'),
	(20, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 13:16:10.925941', 'node'),
	(21, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 13:25:56.798003', 'node'),
	(22, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 13:56:59.936512', 'node'),
	(23, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 13:57:04.927187', 'node'),
	(24, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 13:57:21.267081', 'node'),
	(25, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 13:57:26.732909', 'node'),
	(26, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 13:59:56.440782', 'node'),
	(27, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 14:00:03.424961', 'node'),
	(28, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 14:00:44.357008', 'node'),
	(29, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 14:08:49.316110', 'node'),
	(30, 22, 'admin@company.com', '::1', 'Login Success (Challenge)', '2026-04-28 14:10:12.285341', 'node');

-- Dumping structure for table intranetportal.core_loginchallenges
CREATE TABLE IF NOT EXISTS `core_loginchallenges` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ChallengeId` varchar(64) NOT NULL,
  `NormalizedEmail` varchar(256) NOT NULL,
  `Nonce` varchar(128) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `ExpiresAt` datetime(6) NOT NULL,
  `ConsumedAt` datetime(6) DEFAULT NULL,
  `UserAccountId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Core_LoginChallenges_ChallengeId` (`ChallengeId`),
  KEY `IX_Core_LoginChallenges_ExpiresAt` (`ExpiresAt`),
  KEY `IX_Core_LoginChallenges_NormalizedEmail_CreatedAt` (`NormalizedEmail`,`CreatedAt`),
  KEY `IX_Core_LoginChallenges_UserAccountId` (`UserAccountId`),
  CONSTRAINT `FK_Core_LoginChallenges_Core_UserAccounts_UserAccountId` FOREIGN KEY (`UserAccountId`) REFERENCES `core_useraccounts` (`Id`) ON DELETE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.core_loginchallenges: ~30 rows (approximately)
DELETE FROM `core_loginchallenges`;
INSERT INTO `core_loginchallenges` (`Id`, `ChallengeId`, `NormalizedEmail`, `Nonce`, `CreatedAt`, `ExpiresAt`, `ConsumedAt`, `UserAccountId`) VALUES
	(1, '67dfd625b2834d14b0a09980bd09510d', 'admin@company.com', 'ZqQaAVLZQH5PtwceRCnWIHlXxmtunDxg', '2026-04-25 08:00:09.568239', '2026-04-25 08:01:09.574892', '2026-04-25 08:00:11.680642', NULL),
	(2, 'b58c16734541412980557c66ddd0168f', 'admin@company.com', '3c7Fq3O1pk3lhYmCWNUpCl0eYUaFRCNq', '2026-04-26 02:34:21.498817', '2026-04-26 02:35:21.499651', '2026-04-26 02:34:22.003391', NULL),
	(3, 'c892be4fd0904d2e8150b22dc58ebddb', 'admin@company.com', 'DeiyMMPk2b9fjWk5YJTHaFZS5L0lGLmr', '2026-04-26 02:49:07.506535', '2026-04-26 02:50:07.507085', '2026-04-26 02:49:07.778373', NULL),
	(4, '6e30819b1906465e89154049a67cb99b', 'admin@company.com', '6AJRbKBuw1tMtdYHWl90b4c3m7hRQwPc', '2026-04-26 03:11:16.957708', '2026-04-26 03:12:16.958558', '2026-04-26 03:11:17.467831', NULL),
	(5, 'e7211847e7ee4e4ea67f73bedbafecfb', 'admin@company.com', '7nleJv4VI+aKHIPzGoIjnNPHNFR/znvS', '2026-04-26 09:35:21.937077', '2026-04-26 09:36:21.937478', '2026-04-26 09:35:22.213520', 22),
	(6, '3e5e4f72b526463aa5529713ee6dd74c', 'admin@company.com', '7PUbvj0Z9uYWe9oe33Rk0TM8Bj55IgRM', '2026-04-26 09:35:41.798260', '2026-04-26 09:36:41.798277', '2026-04-26 09:35:41.858023', 22),
	(7, '38807e6024804550afab19b023c3695d', 'admin@company.com', 'FQFNqmxQeml3uWQePtWviAlQ+npPOIMk', '2026-04-27 15:09:12.880911', '2026-04-27 15:10:12.881832', '2026-04-27 15:09:13.512366', 22),
	(8, 'd72b2d90d96544c6aca1bcf4dd8bdb3f', 'admin@company.com', 'AIxF9cG+MESLc1zK/bczcuFRwj84E+pp', '2026-04-28 04:35:55.515273', '2026-04-28 04:36:55.516327', '2026-04-28 04:35:56.199438', 22),
	(9, '96d8a1fa61464382a82755e46ff92fb9', 'admin@company.com', 'xizWrB0c6hR+cm9q9yoAIya25Pze0x74', '2026-04-28 04:41:55.614107', '2026-04-28 04:42:55.614139', '2026-04-28 04:41:55.752458', 22),
	(10, '6f4191ef72b749d1ab1c9244ba9d9516', 'admin@company.com', 'uMdu6YhRzLlM20FJFavG+walleahSkpJ', '2026-04-28 09:01:45.290532', '2026-04-28 09:02:45.292953', '2026-04-28 09:01:45.933731', 22),
	(11, 'a60a823011864a81bef099222f0e0dec', 'admin@company.com', 'G/XG7+grYkGRySxRUHG1La6vZt2s5Tno', '2026-04-28 09:04:15.232755', '2026-04-28 09:05:15.232792', '2026-04-28 09:04:15.277783', 22),
	(12, '0c0349fdfa06411e93bff8895ef8b327', 'admin@company.com', '5f/hHuSQLS7NNtYlr+ZsR2H8s2cXhJs/', '2026-04-28 09:33:28.663721', '2026-04-28 09:34:28.663744', '2026-04-28 09:33:28.715867', 22),
	(13, '759e5291f49b40ac83d4ba03db6849ec', 'admin@company.com', '8+vY0hJGa+r7tOnQeKS68lvdoiMNHuEz', '2026-04-28 09:57:21.939555', '2026-04-28 09:58:21.939581', '2026-04-28 09:57:22.130455', 22),
	(14, 'd43b95c152dc40bfbb38af669d57ad6d', 'admin@company.com', 'oTWBK7mIiPNFP+Aw3UMXTLghv63uhSUX', '2026-04-28 12:37:31.113632', '2026-04-28 12:38:31.115412', '2026-04-28 12:37:31.869638', 22),
	(15, '2bdbc7a1656a4a4596ea6f8a49ac2cf0', 'admin@company.com', '1HxcClX0IEic9uonnr9JAD9BCtHIyqpp', '2026-04-28 12:37:41.053542', '2026-04-28 12:38:41.053567', '2026-04-28 12:37:41.123137', 22),
	(16, '2978fa6af3e1461bb1986675d093fe72', 'admin@company.com', 'IjOEOndC5NXFszd+0mhAAbNebJK/3AD9', '2026-04-28 12:52:57.765287', '2026-04-28 12:53:57.765302', '2026-04-28 12:52:57.843092', 22),
	(17, 'b76413a393be4ac9902e3ed7ae71be60', 'admin@company.com', '7UFJSNHKd/pRf3Hbwr2LSWCEVG7Cwllz', '2026-04-28 12:53:11.626135', '2026-04-28 12:54:11.626157', '2026-04-28 12:53:11.692888', 22),
	(18, '113e389a03b24053a647b0ff1320325d', 'admin@company.com', 'hLGU/3rDjpu2Et6iwST6bmf9jYc1ZkHu', '2026-04-28 12:58:22.831964', '2026-04-28 12:59:22.832007', '2026-04-28 12:58:22.910342', 22),
	(19, '31cc85ca544145bfaacc7cb73d4fd241', 'admin@company.com', 'O6SH5MR6PGpuH9Ow7b+zr6ZA2FSRFfTq', '2026-04-28 12:58:29.175667', '2026-04-28 12:59:29.175688', '2026-04-28 12:58:29.233789', 22),
	(20, '563f7b5efc54458f9b5eea12237d14bf', 'admin@company.com', '8c2e0bPrOYzPtzPH0vdz7Z+yu2UfEok+', '2026-04-28 13:16:10.549526', '2026-04-28 13:17:10.549545', '2026-04-28 13:16:10.668784', 22),
	(21, 'fb9b8f3c32af4e3b9e6ca153a5eacbee', 'admin@company.com', 'hkrgFd1zGrD9VJFMYPTZgvM46KWlSvLV', '2026-04-28 13:25:56.491478', '2026-04-28 13:26:56.491494', '2026-04-28 13:25:56.548255', 22),
	(22, '19d1c1d4c3414139a906899b0ded165e', 'admin@company.com', '9s3XA/zZOjOyAuSrOR9kG84nWK1AwpDn', '2026-04-28 13:56:59.558354', '2026-04-28 13:57:59.558377', '2026-04-28 13:56:59.703258', 22),
	(23, '330b76c5ca69475ea2c2ad99b08687db', 'admin@company.com', 'hw75Hy6+443vfMDSkzt8NBUctG+55oFD', '2026-04-28 13:57:04.613591', '2026-04-28 13:58:04.613607', '2026-04-28 13:57:04.691191', 22),
	(24, '307218ba73ac43f7a7254bcf84f8a83f', 'admin@company.com', 'f8qVlBI6/zne8XST55NvdG2q5pITWUi+', '2026-04-28 13:57:20.968894', '2026-04-28 13:58:20.968917', '2026-04-28 13:57:21.018624', 22),
	(25, '0cbfa02486094d2dabd24166478a2981', 'admin@company.com', 'ej+rWsIoBILfaCUNAZn3vSP1Pobki4Pp', '2026-04-28 13:57:26.418254', '2026-04-28 13:58:26.418270', '2026-04-28 13:57:26.477573', 22),
	(26, '08bb513c7e4d4e8bb4dde5505da10784', 'admin@company.com', 'kRabPHWbbtWT7JKUVJwS6+iUUddQSyhP', '2026-04-28 13:59:56.089694', '2026-04-28 14:00:56.089707', '2026-04-28 13:59:56.195056', 22),
	(27, 'c200640657f242b39d12ce429dd2fc9b', 'admin@company.com', '7nDRx/jsXzq6zlB/rHTQVn1urR38rsON', '2026-04-28 14:00:03.120640', '2026-04-28 14:01:03.120654', '2026-04-28 14:00:03.173887', 22),
	(28, '2b4268d8b2f243848405ab6410266856', 'admin@company.com', '644zWx1JxB+L6KvQN2y6WhMR6sxjiQFB', '2026-04-28 14:00:44.059949', '2026-04-28 14:01:44.059968', '2026-04-28 14:00:44.121292', 22),
	(29, 'c817a3c544bb4bc69bf8358eba542403', 'admin@company.com', 'kFNUOZAsfqAu2b/6ymtuABBtENKo5gHM', '2026-04-28 14:08:48.981053', '2026-04-28 14:09:48.981073', '2026-04-28 14:08:49.042229', 22),
	(30, 'a046110c1a164fcc9406f5d75d248d8d', 'admin@company.com', 'TAomQiWl6GOuiOCJtaeElt2R0iHayPZC', '2026-04-28 14:10:11.984660', '2026-04-28 14:11:11.984766', '2026-04-28 14:10:12.045228', 22);

-- Dumping structure for table intranetportal.core_modulesites
CREATE TABLE IF NOT EXISTS `core_modulesites` (
  `AllowedModulesId` int(11) NOT NULL,
  `AllowedSitesId` int(11) NOT NULL,
  PRIMARY KEY (`AllowedModulesId`,`AllowedSitesId`),
  KEY `IX_Core_ModuleSites_AllowedSitesId` (`AllowedSitesId`),
  CONSTRAINT `FK_Core_ModuleSites_Core_Sites_AllowedSitesId` FOREIGN KEY (`AllowedSitesId`) REFERENCES `core_sites` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Core_ModuleSites_Core_SystemModules_AllowedModulesId` FOREIGN KEY (`AllowedModulesId`) REFERENCES `core_systemmodules` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.core_modulesites: ~6 rows (approximately)
DELETE FROM `core_modulesites`;
INSERT INTO `core_modulesites` (`AllowedModulesId`, `AllowedSitesId`) VALUES
	(1, 1),
	(2, 1),
	(3, 1),
	(4, 1),
	(5, 1),
	(6, 1);

-- Dumping structure for table intranetportal.core_permissions
CREATE TABLE IF NOT EXISTS `core_permissions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` longtext NOT NULL,
  `Description` longtext DEFAULT NULL,
  `IsObsolete` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.core_permissions: ~17 rows (approximately)
DELETE FROM `core_permissions`;
INSERT INTO `core_permissions` (`Id`, `Name`, `Description`, `IsObsolete`) VALUES
	(1, 'System.FullAccess', 'God-mode capability across all scopes', 0),
	(2, 'System.ManageRoles', 'Create and modify Security Matrices', 0),
	(3, 'System.ManagePositions', 'Create and modify HR Job Titles', 0),
	(4, 'Assets.Dictionaries.Manage', 'Auto-discovered dynamic capability map: Assets.Dictionaries.Manage', 0),
	(5, 'Announcements.Create', 'Auto-discovered dynamic capability map: Announcements.Create', 0),
	(6, 'Announcements.Delete', 'Auto-discovered dynamic capability map: Announcements.Delete', 0),
	(7, 'Admin.System.Access', 'Auto-discovered dynamic capability map: Admin.System.Access', 0),
	(8, 'Structure.Site.Create', 'Auto-discovered dynamic capability map: Structure.Site.Create', 0),
	(9, 'Structure.Site.Edit', 'Auto-discovered dynamic capability map: Structure.Site.Edit', 0),
	(10, 'Structure.Site.Delete', 'Auto-discovered dynamic capability map: Structure.Site.Delete', 0),
	(11, 'Admin', 'Auto-discovered dynamic capability map: Admin', 0),
	(12, 'HR.Employee.Create', 'Auto-discovered dynamic capability map: HR.Employee.Create', 0),
	(13, 'HR.Employee.Edit', 'Auto-discovered dynamic capability map: HR.Employee.Edit', 0),
	(14, 'System.Modules.Manage', 'Auto-discovered dynamic capability map: System.Modules.Manage', 0),
	(15, 'Drinks.Queue.View', '(OBSOLETE) Can view the drink order queue', 1),
	(16, 'Drinks.Orders.Approve', '(OBSOLETE) Can approve or reject drink orders', 1),
	(17, 'Drinks.Menu.Manage', '(OBSOLETE) Can manage the drinks menu catalog', 1);

-- Dumping structure for table intranetportal.core_roledelegations
CREATE TABLE IF NOT EXISTS `core_roledelegations` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SourceUserId` int(11) NOT NULL,
  `SubstituteUserId` int(11) NOT NULL,
  `UserRoleId` int(11) NOT NULL,
  `StartDate` datetime(6) NOT NULL,
  `EndDate` datetime(6) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Core_RoleDelegations_SourceUserId` (`SourceUserId`),
  KEY `IX_Core_RoleDelegations_SubstituteUserId` (`SubstituteUserId`),
  KEY `IX_Core_RoleDelegations_UserRoleId` (`UserRoleId`),
  CONSTRAINT `FK_Core_RoleDelegations_Core_UserAccounts_SourceUserId` FOREIGN KEY (`SourceUserId`) REFERENCES `core_useraccounts` (`Id`),
  CONSTRAINT `FK_Core_RoleDelegations_Core_UserAccounts_SubstituteUserId` FOREIGN KEY (`SubstituteUserId`) REFERENCES `core_useraccounts` (`Id`),
  CONSTRAINT `FK_Core_RoleDelegations_Core_UserRoles_UserRoleId` FOREIGN KEY (`UserRoleId`) REFERENCES `core_userroles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.core_roledelegations: ~0 rows (approximately)
DELETE FROM `core_roledelegations`;

-- Dumping structure for table intranetportal.core_rolepermissions
CREATE TABLE IF NOT EXISTS `core_rolepermissions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RoleId` int(11) NOT NULL,
  `PermissionId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Core_RolePermissions_PermissionId` (`PermissionId`),
  KEY `IX_Core_RolePermissions_RoleId` (`RoleId`),
  CONSTRAINT `FK_Core_RolePermissions_Core_Permissions_PermissionId` FOREIGN KEY (`PermissionId`) REFERENCES `core_permissions` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Core_RolePermissions_Core_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `core_roles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.core_rolepermissions: ~3 rows (approximately)
DELETE FROM `core_rolepermissions`;
INSERT INTO `core_rolepermissions` (`Id`, `RoleId`, `PermissionId`) VALUES
	(1, 1, 1),
	(2, 1, 2),
	(3, 1, 3);

-- Dumping structure for table intranetportal.core_roles
CREATE TABLE IF NOT EXISTS `core_roles` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` longtext NOT NULL,
  `Description` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.core_roles: ~14 rows (approximately)
DELETE FROM `core_roles`;
INSERT INTO `core_roles` (`Id`, `Name`, `Description`) VALUES
	(1, 'Admin', 'Global Application Administrator'),
	(2, 'Content Editor', NULL),
	(3, 'Inventory Manager', NULL),
	(4, 'Customer Support', NULL),
	(5, 'HR Generalist', NULL),
	(6, 'IT Support Specialist', NULL),
	(7, 'Financial Analyst', NULL),
	(8, 'Sales Representative', NULL),
	(9, 'Marketing Coordinator', NULL),
	(10, 'Procurement Officer', NULL),
	(11, 'Logistics Coordinator', NULL),
	(12, 'Legal Counsel', NULL),
	(13, 'Business Development Manager', NULL),
	(14, 'Operations Supervisor', NULL);

-- Dumping structure for table intranetportal.core_sites
CREATE TABLE IF NOT EXISTS `core_sites` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` longtext NOT NULL,
  `Address` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.core_sites: ~2 rows (approximately)
DELETE FROM `core_sites`;
INSERT INTO `core_sites` (`Id`, `Name`, `Address`) VALUES
	(1, 'Global Headquarters', 'Corporate Plaza, Level 42'),
	(2, 'Head Office', NULL);

-- Dumping structure for table intranetportal.core_systemmodules
CREATE TABLE IF NOT EXISTS `core_systemmodules` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(150) NOT NULL,
  `Description` varchar(500) NOT NULL,
  `IconSvg` longtext NOT NULL,
  `Url` varchar(300) NOT NULL,
  `IsActiveGlobally` tinyint(1) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `Order` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.core_systemmodules: ~6 rows (approximately)
DELETE FROM `core_systemmodules`;
INSERT INTO `core_systemmodules` (`Id`, `Name`, `Description`, `IconSvg`, `Url`, `IsActiveGlobally`, `IsActive`, `Order`) VALUES
	(1, 'The Hub', 'Browse the corporate directory, internal wikis, documentation, and company announcements.', '<svg class="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4" /></svg>', '/employees', 1, 1, 10),
	(2, 'Assets Management', 'Submit requisitions and track physical equipment deployments, inventory, and accessories.', '<svg class="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9.75 17L9 20l-1 1h8l-1-1-.75-3M3 13h18M5 17h14a2 2 0 002-2V5a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" /></svg>', '/assets', 1, 1, 21),
	(3, 'Administration', 'Manage system configuration, personnel roles, permissions, sites, and geographical footprints.', '<svg class="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z" /><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" /></svg>', '/admin/quick-setup', 1, 1, 30),
	(4, 'Drink Orders', 'Browse the pantry menu, request specialized beverages, and manage the office drink queue.', '<svg class="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M5 3v4M3 5h4M6 17v4m-2-2h4m5-16l2.286 6.857L21 12l-5.714 2.143L13 21l-2.286-6.857L5 12l5.714-2.143z" /></svg>', 'http://localhost:3001', 1, 1, 21),
	(5, 'Human Resources', 'Manage organizational structure, job mapping, and internal personnel records.', '<svg class="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" /></svg>', '/hr', 1, 1, 20),
	(6, 'Another App', 'Just a placeholder for another modules', '<svg class="w-8 h-8" fill="none" stroke="#D1D5DB" viewBox="0 0 24 24" style="background-color: #4B5563; border-radius: 4px;">\r\n  <rect width="18" height="18" x="3" y="3" rx="2" ry="2" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />\r\n  <circle cx="9" cy="9" r="2" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />\r\n  <path d="M14 18h4v-3h-4v3z" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />\r\n  <path d="M6 18h4v-5H6v5z" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />\r\n</svg>', '/', 1, 1, 100);

-- Dumping structure for table intranetportal.core_teams
CREATE TABLE IF NOT EXISTS `core_teams` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `DepartmentId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Core_Teams_DepartmentId` (`DepartmentId`),
  CONSTRAINT `FK_Core_Teams_HR_Departments_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `hr_departments` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.core_teams: ~0 rows (approximately)
DELETE FROM `core_teams`;

-- Dumping structure for table intranetportal.core_useraccounts
CREATE TABLE IF NOT EXISTS `core_useraccounts` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Email` varchar(256) NOT NULL,
  `PasswordHash` longtext NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `EmployeeId` int(11) NOT NULL DEFAULT 0,
  `FailedLoginAttempts` int(11) NOT NULL DEFAULT 0,
  `LockedUntil` datetime(6) DEFAULT NULL,
  `SecurityStamp` int(11) NOT NULL DEFAULT 1,
  `RefreshToken` longtext DEFAULT NULL,
  `RefreshTokenExpiryTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Core_UserAccounts_Email` (`Email`),
  UNIQUE KEY `IX_Core_UserAccounts_EmployeeId` (`EmployeeId`),
  CONSTRAINT `FK_Core_UserAccounts_HR_Employees_EmployeeId` FOREIGN KEY (`EmployeeId`) REFERENCES `hr_employees` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.core_useraccounts: ~21 rows (approximately)
DELETE FROM `core_useraccounts`;
INSERT INTO `core_useraccounts` (`Id`, `Email`, `PasswordHash`, `IsActive`, `EmployeeId`, `FailedLoginAttempts`, `LockedUntil`, `SecurityStamp`, `RefreshToken`, `RefreshTokenExpiryTime`) VALUES
	(2, 'user1@company.com', '$2a$11$KvGkrmt6tQcUMqd5OYxrXu5wNayZ/i0ACoOqAW7uGroDBZtRUxdta', 1, 1, 0, NULL, 1, NULL, NULL),
	(3, 'user2@company.com', '$2a$11$Px46aBlXu2/LIngIa7vqE.NuqugedyewftLfKqI6oay20vdrGqsA6', 1, 2, 0, NULL, 1, NULL, NULL),
	(4, 'user3@company.com', '$2a$11$LpTjwu6jkdDRnys.RVSQpOOriV0zt6PYGvyZz.mpUViPu.ZUW8.GS', 1, 3, 0, NULL, 1, NULL, NULL),
	(5, 'user4@company.com', '$2a$11$wNd85TMRPeU3gLEhszblm.ChZQ9bQaiSGLnt2mRvgpqc9vUaLMmDa', 1, 4, 0, NULL, 1, NULL, NULL),
	(6, 'user5@company.com', '$2a$11$G1iWKhEJGqkUfsjdt9sljeEZJ2Forw3mZvVskuIXWw4BbdRKxvu9u', 1, 5, 0, NULL, 1, NULL, NULL),
	(7, 'user6@company.com', '$2a$11$0AiGgqeTE2psGuMy8qhS7uHZC/COVVYOVrHrSWgsjXhKMOWpfmq0y', 1, 6, 0, NULL, 1, NULL, NULL),
	(8, 'user7@company.com', '$2a$11$vmVjbYm494VyDLRtZCCFzeSuyj3e51REnFUJSN8nT5YVSJdhiHwpW', 1, 7, 0, NULL, 1, NULL, NULL),
	(9, 'user8@company.com', '$2a$11$VGu1w9eAN1L85.sJTQUWLuU9.jghNmeJBjSOZcirZdlOLxVhSxzPO', 1, 8, 0, NULL, 1, NULL, NULL),
	(10, 'user9@company.com', '$2a$11$ONPkB1jJP3IeYsgYjaPWce1wOurpdHqmVQukou.H7bQW9TRfyxceW', 1, 9, 0, NULL, 1, NULL, NULL),
	(11, 'user10@company.com', '$2a$11$RscBhZV08yVPd38aSbiVuesGBZqeJQpHAeq3I3JscuDySuELeNPDm', 1, 10, 0, NULL, 1, NULL, NULL),
	(12, 'user11@company.com', '$2a$11$9XyxyZguvBBXRFthkq6mK.fi4XWDjILH2.PJXJ01M671ha0mXOkdS', 1, 11, 0, NULL, 1, NULL, NULL),
	(13, 'user12@company.com', '$2a$11$eyC1nDhNYYE1Haat3vP5WOUTbrTF5IwHhJsQYewBo28uKR5n5hZwG', 1, 12, 0, NULL, 1, NULL, NULL),
	(14, 'user13@company.com', '$2a$11$l6bSeV/.vwuxJN9E7hicuOxzZ/vKzUakulGCDB9ndL0/4Lc3UESzW', 1, 13, 0, NULL, 1, NULL, NULL),
	(15, 'user14@company.com', '$2a$11$uWd3SznPkCyg1RhVG2me5OGZWCMaJw1sKtR/6HvX9onaPoy4vuZ0u', 1, 14, 0, NULL, 1, NULL, NULL),
	(16, 'user15@company.com', '$2a$11$VF7giXBiWASv69HUrVOsA.P7ro5dDSs8XxZEXvOC8CgBUsICGM5ry', 1, 15, 0, NULL, 1, NULL, NULL),
	(17, 'user16@company.com', '$2a$11$lCONRMvmT.ed9Rt7Y5JIFuFcCCaJyQFUJt/qFMfN5Xf9K5qc26Jfy', 1, 16, 0, NULL, 1, NULL, NULL),
	(18, 'user17@company.com', '$2a$11$vVr2GAu33PT2QWu.FyD9Kevt8dxmx4iy2eG08Xn/bRBN2oLNMqJFq', 1, 17, 0, NULL, 1, NULL, NULL),
	(19, 'user18@company.com', '$2a$11$LlYxr9sA8vewWviCklWn1OrtaF/YWFwbq1Q/kMbhP2sClgojTKOZy', 1, 18, 0, NULL, 1, NULL, NULL),
	(20, 'user19@company.com', '$2a$11$ZwL9ViqiwQP89zK67LCkqOUxQYJErI1f9nb9vpvJh8MgaiBQzAvqi', 1, 19, 0, NULL, 1, NULL, NULL),
	(21, 'user20@company.com', '$2a$11$aKQ84DME9n819uNWrumWK.8u1UwBQ5U92xGZDtpUzQJ/BYAcvwOaq', 1, 20, 0, NULL, 1, NULL, NULL),
	(22, 'admin@company.com', '$2a$11$TPmJq6BlXIZeMeOQbzqLieyWAT2kK7WFoK9dMLx5iIgTlpTqWPHzm', 1, 21, 0, NULL, 1, NULL, NULL);

-- Dumping structure for table intranetportal.core_userroles
CREATE TABLE IF NOT EXISTS `core_userroles` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserAccountId` int(11) NOT NULL,
  `RoleId` int(11) NOT NULL,
  `SiteId` int(11) DEFAULT NULL,
  `DepartmentId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Core_UserRoles_RoleId` (`RoleId`),
  KEY `IX_Core_UserRoles_SiteId` (`SiteId`),
  KEY `IX_Core_UserRoles_UserAccountId` (`UserAccountId`),
  KEY `IX_Core_UserRoles_DepartmentId` (`DepartmentId`),
  CONSTRAINT `FK_Core_UserRoles_Core_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `core_roles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Core_UserRoles_Core_Sites_SiteId` FOREIGN KEY (`SiteId`) REFERENCES `core_sites` (`Id`),
  CONSTRAINT `FK_Core_UserRoles_Core_UserAccounts_UserAccountId` FOREIGN KEY (`UserAccountId`) REFERENCES `core_useraccounts` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Core_UserRoles_HR_Departments_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `hr_departments` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.core_userroles: ~1 rows (approximately)
DELETE FROM `core_userroles`;
INSERT INTO `core_userroles` (`Id`, `UserAccountId`, `RoleId`, `SiteId`, `DepartmentId`) VALUES
	(2, 22, 1, NULL, NULL);

-- Dumping structure for procedure intranetportal.DropFkIfExists
DELIMITER //
CREATE PROCEDURE `DropFkIfExists`(IN tName VARCHAR(255), IN fkName VARCHAR(255))
BEGIN
                    IF EXISTS (
                        SELECT * FROM information_schema.table_constraints 
                        WHERE constraint_schema = DATABASE() AND table_name = tName AND constraint_name = fkName AND constraint_type = 'FOREIGN KEY'
                    ) THEN
                        SET @s = CONCAT('ALTER TABLE `', tName, '` DROP FOREIGN KEY `', fkName, '`');
                        PREPARE stmt FROM @s;
                        EXECUTE stmt;
                        DEALLOCATE PREPARE stmt;
                    END IF;
                END//
DELIMITER ;

-- Dumping structure for table intranetportal.hr_attendancelogs
CREATE TABLE IF NOT EXISTS `hr_attendancelogs` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `EmployeeId` int(11) NOT NULL,
  `Date` datetime(6) NOT NULL,
  `ClockInTime` datetime(6) DEFAULT NULL,
  `ClockOutTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_HR_AttendanceLogs_EmployeeId_Date` (`EmployeeId`,`Date`),
  CONSTRAINT `FK_HR_AttendanceLogs_HR_Employees_EmployeeId` FOREIGN KEY (`EmployeeId`) REFERENCES `hr_employees` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.hr_attendancelogs: ~0 rows (approximately)
DELETE FROM `hr_attendancelogs`;

-- Dumping structure for table intranetportal.hr_departments
CREATE TABLE IF NOT EXISTS `hr_departments` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `Description` varchar(500) DEFAULT NULL,
  `ParentDepartmentId` int(11) DEFAULT NULL,
  `ManagerId` int(11) DEFAULT NULL,
  `SiteId` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
  KEY `IX_HR_Departments_ManagerId` (`ManagerId`),
  KEY `IX_HR_Departments_ParentDepartmentId` (`ParentDepartmentId`),
  KEY `IX_HR_Departments_SiteId` (`SiteId`),
  CONSTRAINT `FK_HR_Departments_Core_Sites_SiteId` FOREIGN KEY (`SiteId`) REFERENCES `core_sites` (`Id`) ON DELETE NO ACTION,
  CONSTRAINT `FK_HR_Departments_HR_Departments_ParentDepartmentId` FOREIGN KEY (`ParentDepartmentId`) REFERENCES `hr_departments` (`Id`),
  CONSTRAINT `FK_HR_Departments_HR_Employees_ManagerId` FOREIGN KEY (`ManagerId`) REFERENCES `hr_employees` (`Id`) ON DELETE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.hr_departments: ~11 rows (approximately)
DELETE FROM `hr_departments`;
INSERT INTO `hr_departments` (`Id`, `Name`, `Description`, `ParentDepartmentId`, `ManagerId`, `SiteId`) VALUES
	(1, 'Human Resources', NULL, NULL, NULL, 2),
	(2, 'Information Technology', NULL, NULL, NULL, 2),
	(3, 'Finance', NULL, NULL, NULL, 2),
	(4, 'Sales', NULL, NULL, NULL, 2),
	(5, 'Customer Service', NULL, NULL, NULL, 2),
	(6, 'Marketing', NULL, NULL, NULL, 2),
	(7, 'Procurement & Supply Chain', NULL, NULL, NULL, 2),
	(8, 'Logistics & Warehousing', NULL, NULL, NULL, 2),
	(9, 'Legal & Compliance', NULL, NULL, NULL, 2),
	(10, 'Business Development', NULL, NULL, NULL, 2),
	(11, 'Operations', NULL, NULL, NULL, 2);

-- Dumping structure for table intranetportal.hr_employeeonboardingtasks
CREATE TABLE IF NOT EXISTS `hr_employeeonboardingtasks` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `EmployeeId` int(11) NOT NULL,
  `IsCompleted` tinyint(1) NOT NULL,
  `CompletedAt` datetime(6) DEFAULT NULL,
  `OnboardingTaskId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_HR_EmployeeOnboardingTasks_EmployeeId` (`EmployeeId`),
  KEY `IX_HR_EmployeeOnboardingTasks_OnboardingTaskId` (`OnboardingTaskId`),
  CONSTRAINT `FK_HR_EmployeeOnboardingTasks_HR_Employees_EmployeeId` FOREIGN KEY (`EmployeeId`) REFERENCES `hr_employees` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_HR_EmployeeOnboardingTasks_HR_OnboardingTasks_OnboardingTask~` FOREIGN KEY (`OnboardingTaskId`) REFERENCES `hr_onboardingtasks` (`Id`) ON DELETE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.hr_employeeonboardingtasks: ~0 rows (approximately)
DELETE FROM `hr_employeeonboardingtasks`;

-- Dumping structure for table intranetportal.hr_employees
CREATE TABLE IF NOT EXISTS `hr_employees` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FullName` varchar(200) NOT NULL,
  `Email` varchar(200) NOT NULL,
  `EmployeeNumber` varchar(50) NOT NULL,
  `HireDate` datetime(6) DEFAULT NULL,
  `DateOfBirth` datetime(6) DEFAULT NULL,
  `EmergencyContactName` longtext DEFAULT NULL,
  `EmergencyContactPhone` longtext DEFAULT NULL,
  `DepartmentId` int(11) DEFAULT NULL,
  `PositionId` int(11) DEFAULT NULL,
  `TeamId` int(11) DEFAULT NULL,
  `SiteId` int(11) NOT NULL,
  `DirectManagerId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_HR_Employees_DepartmentId` (`DepartmentId`),
  KEY `IX_HR_Employees_DirectManagerId` (`DirectManagerId`),
  KEY `IX_HR_Employees_PositionId` (`PositionId`),
  KEY `IX_HR_Employees_SiteId` (`SiteId`),
  KEY `IX_HR_Employees_TeamId` (`TeamId`),
  CONSTRAINT `FK_HR_Employees_Core_Sites_SiteId` FOREIGN KEY (`SiteId`) REFERENCES `core_sites` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_HR_Employees_Core_Teams_TeamId` FOREIGN KEY (`TeamId`) REFERENCES `core_teams` (`Id`),
  CONSTRAINT `FK_HR_Employees_HR_Departments_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `hr_departments` (`Id`) ON DELETE SET NULL,
  CONSTRAINT `FK_HR_Employees_HR_Employees_DirectManagerId` FOREIGN KEY (`DirectManagerId`) REFERENCES `hr_employees` (`Id`),
  CONSTRAINT `FK_HR_Employees_HR_Positions_PositionId` FOREIGN KEY (`PositionId`) REFERENCES `hr_positions` (`Id`) ON DELETE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.hr_employees: ~21 rows (approximately)
DELETE FROM `hr_employees`;
INSERT INTO `hr_employees` (`Id`, `FullName`, `Email`, `EmployeeNumber`, `HireDate`, `DateOfBirth`, `EmergencyContactName`, `EmergencyContactPhone`, `DepartmentId`, `PositionId`, `TeamId`, `SiteId`, `DirectManagerId`) VALUES
	(1, 'James Smith', 'user1@company.com', 'EMP-100', '2026-04-25 15:37:41.150985', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL),
	(2, 'Mary Johnson', 'user2@company.com', 'EMP-101', '2026-03-25 15:37:41.732720', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL),
	(3, 'Robert Williams', 'user3@company.com', 'EMP-102', '2026-02-25 15:37:42.104024', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL),
	(4, 'Patricia Brown', 'user4@company.com', 'EMP-103', '2026-01-25 15:37:42.445140', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL),
	(5, 'John Jones', 'user5@company.com', 'EMP-104', '2025-12-25 15:37:42.789824', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL),
	(6, 'Jennifer Garcia', 'user6@company.com', 'EMP-105', '2025-11-25 15:37:43.084765', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL),
	(7, 'Michael Miller', 'user7@company.com', 'EMP-106', '2025-10-25 15:37:43.291390', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL),
	(8, 'Linda Davis', 'user8@company.com', 'EMP-107', '2025-09-25 15:37:43.490075', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL),
	(9, 'David Rodriguez', 'user9@company.com', 'EMP-108', '2025-08-25 15:37:43.674292', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL),
	(10, 'Elizabeth Martinez', 'user10@company.com', 'EMP-109', '2025-07-25 15:37:43.855279', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL),
	(11, 'William Hernandez', 'user11@company.com', 'EMP-110', '2025-06-25 15:37:44.035987', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL),
	(12, 'Barbara Lopez', 'user12@company.com', 'EMP-111', '2025-05-25 15:37:44.253067', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL),
	(13, 'Richard Gonzalez', 'user13@company.com', 'EMP-112', '2025-04-25 15:37:44.440958', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL),
	(14, 'Susan Wilson', 'user14@company.com', 'EMP-113', '2025-03-25 15:37:44.625567', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL),
	(15, 'Joseph Anderson', 'user15@company.com', 'EMP-114', '2025-02-25 15:37:44.821078', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL),
	(16, 'Jessica Thomas', 'user16@company.com', 'EMP-115', '2025-01-25 15:37:45.005537', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL),
	(17, 'Thomas Taylor', 'user17@company.com', 'EMP-116', '2024-12-25 15:37:45.197000', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL),
	(18, 'Sarah Moore', 'user18@company.com', 'EMP-117', '2024-11-25 15:37:45.389006', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL),
	(19, 'Christopher Jackson', 'user19@company.com', 'EMP-118', '2024-10-25 15:37:45.590256', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL),
	(20, 'Karen Martin', 'user20@company.com', 'EMP-119', '2024-09-25 15:37:45.780416', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL),
	(21, 'System Administrator', 'admin@company.com', 'EMP-000', '2026-04-26 09:34:42.596576', NULL, NULL, NULL, 1, NULL, NULL, 1, NULL);

-- Dumping structure for table intranetportal.hr_leaverequests
CREATE TABLE IF NOT EXISTS `hr_leaverequests` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `EmployeeId` int(11) NOT NULL,
  `LeaveTypeId` int(11) NOT NULL,
  `StartDate` datetime(6) NOT NULL,
  `EndDate` datetime(6) NOT NULL,
  `Reason` varchar(500) NOT NULL,
  `Status` varchar(50) NOT NULL DEFAULT 'Pending',
  `ApprovedById` int(11) DEFAULT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_HR_LeaveRequests_ApprovedById` (`ApprovedById`),
  KEY `IX_HR_LeaveRequests_EmployeeId` (`EmployeeId`),
  KEY `IX_HR_LeaveRequests_LeaveTypeId` (`LeaveTypeId`),
  CONSTRAINT `FK_HR_LeaveRequests_HR_Employees_ApprovedById` FOREIGN KEY (`ApprovedById`) REFERENCES `hr_employees` (`Id`) ON DELETE NO ACTION,
  CONSTRAINT `FK_HR_LeaveRequests_HR_Employees_EmployeeId` FOREIGN KEY (`EmployeeId`) REFERENCES `hr_employees` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_HR_LeaveRequests_HR_LeaveTypes_LeaveTypeId` FOREIGN KEY (`LeaveTypeId`) REFERENCES `hr_leavetypes` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.hr_leaverequests: ~0 rows (approximately)
DELETE FROM `hr_leaverequests`;

-- Dumping structure for table intranetportal.hr_leavetypes
CREATE TABLE IF NOT EXISTS `hr_leavetypes` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `Description` varchar(250) DEFAULT NULL,
  `DaysAllowed` int(11) NOT NULL,
  `RequiresApproval` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.hr_leavetypes: ~0 rows (approximately)
DELETE FROM `hr_leavetypes`;

-- Dumping structure for table intranetportal.hr_onboardingtasks
CREATE TABLE IF NOT EXISTS `hr_onboardingtasks` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TemplateId` int(11) NOT NULL,
  `Title` varchar(200) NOT NULL,
  `Description` varchar(1000) DEFAULT NULL,
  `AssigneeRole` varchar(50) NOT NULL,
  `Order` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_HR_OnboardingTasks_TemplateId` (`TemplateId`),
  CONSTRAINT `FK_HR_OnboardingTasks_HR_OnboardingTemplates_TemplateId` FOREIGN KEY (`TemplateId`) REFERENCES `hr_onboardingtemplates` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.hr_onboardingtasks: ~0 rows (approximately)
DELETE FROM `hr_onboardingtasks`;

-- Dumping structure for table intranetportal.hr_onboardingtemplates
CREATE TABLE IF NOT EXISTS `hr_onboardingtemplates` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(150) NOT NULL,
  `Description` varchar(500) DEFAULT NULL,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.hr_onboardingtemplates: ~0 rows (approximately)
DELETE FROM `hr_onboardingtemplates`;

-- Dumping structure for table intranetportal.hr_positions
CREATE TABLE IF NOT EXISTS `hr_positions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` longtext NOT NULL,
  `Description` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.hr_positions: ~14 rows (approximately)
DELETE FROM `hr_positions`;
INSERT INTO `hr_positions` (`Id`, `Name`, `Description`) VALUES
	(1, 'Chief Executive Officer', NULL),
	(2, 'Lead Engineer', NULL),
	(3, 'Junior Developer', NULL),
	(4, 'Accountant', NULL),
	(5, 'HR Generalist', NULL),
	(6, 'IT Support Specialist', NULL),
	(7, 'Financial Analyst', NULL),
	(8, 'Sales Representative', NULL),
	(9, 'Marketing Coordinator', NULL),
	(10, 'Procurement Officer', NULL),
	(11, 'Logistics Coordinator', NULL),
	(12, 'Legal Counsel', NULL),
	(13, 'Business Development Manager', NULL),
	(14, 'Operations Supervisor', NULL);

-- Dumping structure for table intranetportal.__efmigrationshistory
CREATE TABLE IF NOT EXISTS `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table intranetportal.__efmigrationshistory: ~29 rows (approximately)
DELETE FROM `__efmigrationshistory`;
INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
	('20260318151408_InitialCreate', '9.0.2'),
	('20260318152031_RenameLocationToSite', '9.0.2'),
	('20260319073253_FinalAuthSync', '9.0.2'),
	('20260319113723_EnterpriseRBAC', '9.0.2'),
	('20260320083429_AddPositionModel', '9.0.2'),
	('20260320153700_AddTeamsHierarchy', '9.0.2'),
	('20260323151934_AddIsObsoleteFlagToPermissions', '9.0.2'),
	('20260323155206_AddSiteIdScopeLimits', '9.0.2'),
	('20260324071019_MakeEmployeeSiteIdNullable', '9.0.2'),
	('20260324073042_EnforceStrictSiteIdBindings', '9.0.2'),
	('20260328063636_EnhanceRBAC_Delegation_And_DepartmentScopes', '9.0.2'),
	('20260403064210_AddCorePrefixesToTables', '9.0.2'),
	('20260404060257_AddAssetsManagementModule', '9.0.2'),
	('20260404082656_AddAssetModelIsActive', '9.0.2'),
	('20260413105558_AddSystemModules', '9.0.2'),
	('20260415054001_AddCategoryApprovers', '9.0.2'),
	('20260415072533_AddAssetRequestLineItems', '9.0.2'),
	('20260415081122_AddApproverGroupsTablePrefixes', '9.0.2'),
	('20260415104334_AddCategoryFulfillmentGroup', '9.0.2'),
	('20260416125408_AddSecurityStampAndLockout', '9.0.2'),
	('20260416134358_AddCoreLoginChallenges', '9.0.2'),
	('20260417053936_Phase1_5_SecurityHardening', '9.0.2'),
	('20260424094632_PartitionPositionToHRModule', '9.0.2'),
	('20260424150625_AddOrderToSystemModules', '9.0.2'),
	('20260424153054_InitialHRSchema', '9.0.2'),
	('20260424160358_AddAttendanceLogs', '9.0.2'),
	('20260425075252_UnifyPersonnelToHR', '9.0.2'),
	('20260426093249_UpdatePersonnelUnification', '9.0.2'),
	('20260426142357_FinalizePersonnelUnification', '9.0.2');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
