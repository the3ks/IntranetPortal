-- ==========================================
-- Intranet Portal: Asset Management Cleanup
-- ==========================================
-- Disables foreign keys to ensure clean truncation
SET FOREIGN_KEY_CHECKS = 0;

TRUNCATE TABLE `AM_AssetRequestLineItems`;
TRUNCATE TABLE `AM_AssetRequests`;
TRUNCATE TABLE `AM_AccessoryCheckouts`;
TRUNCATE TABLE `AM_AssetAuditLogs`;
TRUNCATE TABLE `AM_AssetAssignments`;
TRUNCATE TABLE `AM_AssetMaintenance`;
TRUNCATE TABLE `AM_Assets`;
TRUNCATE TABLE `AM_AssetModels`;
TRUNCATE TABLE `AM_Accessories`;
TRUNCATE TABLE `AM_AssetCategoryApproverGroups`;
TRUNCATE TABLE `AM_AssetCategories`;
TRUNCATE TABLE `AM_ApproverGroupMembers`;
TRUNCATE TABLE `AM_ApproverGroupScopes`;
TRUNCATE TABLE `AM_ApproverGroups`;

SET FOREIGN_KEY_CHECKS = 1;
