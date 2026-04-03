using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntranetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCorePrefixesToTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS DropFkIfExists;
                CREATE PROCEDURE DropFkIfExists(IN tName VARCHAR(255), IN fkName VARCHAR(255))
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
                END;
            ");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Announcements\', \'FK_Announcements_Employees_AuthorId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Announcements\', \'FK_Announcements_Sites_SiteId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Departments\', \'FK_Departments_Sites_SiteId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Employees\', \'FK_Employees_Departments_DepartmentId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Employees\', \'FK_Employees_Positions_PositionId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Employees\', \'FK_Employees_Sites_SiteId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Employees\', \'FK_Employees_Teams_TeamId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'RoleDelegations\', \'FK_RoleDelegations_UserAccounts_SourceUserId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'RoleDelegations\', \'FK_RoleDelegations_UserAccounts_SubstituteUserId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'RoleDelegations\', \'FK_RoleDelegations_UserRoles_UserRoleId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'RolePermissions\', \'FK_RolePermissions_Permissions_PermissionId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'RolePermissions\', \'FK_RolePermissions_Roles_RoleId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Teams\', \'FK_Teams_Departments_DepartmentId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'UserAccounts\', \'FK_UserAccounts_Employees_EmployeeId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'UserRoles\', \'FK_UserRoles_Departments_DepartmentId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'UserRoles\', \'FK_UserRoles_Roles_RoleId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'UserRoles\', \'FK_UserRoles_Sites_SiteId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'UserRoles\', \'FK_UserRoles_UserAccounts_UserAccountId\');");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAccounts",
                table: "UserAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teams",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sites",
                table: "Sites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleDelegations",
                table: "RoleDelegations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Positions",
                table: "Positions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Departments",
                table: "Departments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Announcements",
                table: "Announcements");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "Core_UserRoles");

            migrationBuilder.RenameTable(
                name: "UserAccounts",
                newName: "Core_UserAccounts");

            migrationBuilder.RenameTable(
                name: "Teams",
                newName: "Core_Teams");

            migrationBuilder.RenameTable(
                name: "Sites",
                newName: "Core_Sites");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Core_Roles");

            migrationBuilder.RenameTable(
                name: "RolePermissions",
                newName: "Core_RolePermissions");

            migrationBuilder.RenameTable(
                name: "RoleDelegations",
                newName: "Core_RoleDelegations");

            migrationBuilder.RenameTable(
                name: "Positions",
                newName: "Core_Positions");

            migrationBuilder.RenameTable(
                name: "Permissions",
                newName: "Core_Permissions");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Core_Employees");

            migrationBuilder.RenameTable(
                name: "Departments",
                newName: "Core_Departments");

            migrationBuilder.RenameTable(
                name: "Announcements",
                newName: "Core_Announcements");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_UserAccountId",
                table: "Core_UserRoles",
                newName: "IX_Core_UserRoles_UserAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_SiteId",
                table: "Core_UserRoles",
                newName: "IX_Core_UserRoles_SiteId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_RoleId",
                table: "Core_UserRoles",
                newName: "IX_Core_UserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_DepartmentId",
                table: "Core_UserRoles",
                newName: "IX_Core_UserRoles_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccounts_EmployeeId",
                table: "Core_UserAccounts",
                newName: "IX_Core_UserAccounts_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Teams_DepartmentId",
                table: "Core_Teams",
                newName: "IX_Core_Teams_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_RoleId",
                table: "Core_RolePermissions",
                newName: "IX_Core_RolePermissions_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "Core_RolePermissions",
                newName: "IX_Core_RolePermissions_PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleDelegations_UserRoleId",
                table: "Core_RoleDelegations",
                newName: "IX_Core_RoleDelegations_UserRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleDelegations_SubstituteUserId",
                table: "Core_RoleDelegations",
                newName: "IX_Core_RoleDelegations_SubstituteUserId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleDelegations_SourceUserId",
                table: "Core_RoleDelegations",
                newName: "IX_Core_RoleDelegations_SourceUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_TeamId",
                table: "Core_Employees",
                newName: "IX_Core_Employees_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_SiteId",
                table: "Core_Employees",
                newName: "IX_Core_Employees_SiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_PositionId",
                table: "Core_Employees",
                newName: "IX_Core_Employees_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_DepartmentId",
                table: "Core_Employees",
                newName: "IX_Core_Employees_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Departments_SiteId",
                table: "Core_Departments",
                newName: "IX_Core_Departments_SiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Announcements_SiteId",
                table: "Core_Announcements",
                newName: "IX_Core_Announcements_SiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Announcements_AuthorId",
                table: "Core_Announcements",
                newName: "IX_Core_Announcements_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Core_UserRoles",
                table: "Core_UserRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Core_UserAccounts",
                table: "Core_UserAccounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Core_Teams",
                table: "Core_Teams",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Core_Sites",
                table: "Core_Sites",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Core_Roles",
                table: "Core_Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Core_RolePermissions",
                table: "Core_RolePermissions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Core_RoleDelegations",
                table: "Core_RoleDelegations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Core_Positions",
                table: "Core_Positions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Core_Permissions",
                table: "Core_Permissions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Core_Employees",
                table: "Core_Employees",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Core_Departments",
                table: "Core_Departments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Core_Announcements",
                table: "Core_Announcements",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Core_Announcements_Core_Employees_AuthorId",
                table: "Core_Announcements",
                column: "AuthorId",
                principalTable: "Core_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Core_Announcements_Core_Sites_SiteId",
                table: "Core_Announcements",
                column: "SiteId",
                principalTable: "Core_Sites",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Core_Departments_Core_Sites_SiteId",
                table: "Core_Departments",
                column: "SiteId",
                principalTable: "Core_Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Core_Employees_Core_Departments_DepartmentId",
                table: "Core_Employees",
                column: "DepartmentId",
                principalTable: "Core_Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Core_Employees_Core_Positions_PositionId",
                table: "Core_Employees",
                column: "PositionId",
                principalTable: "Core_Positions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Core_Employees_Core_Sites_SiteId",
                table: "Core_Employees",
                column: "SiteId",
                principalTable: "Core_Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Core_Employees_Core_Teams_TeamId",
                table: "Core_Employees",
                column: "TeamId",
                principalTable: "Core_Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Core_RoleDelegations_Core_UserAccounts_SourceUserId",
                table: "Core_RoleDelegations",
                column: "SourceUserId",
                principalTable: "Core_UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Core_RoleDelegations_Core_UserAccounts_SubstituteUserId",
                table: "Core_RoleDelegations",
                column: "SubstituteUserId",
                principalTable: "Core_UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Core_RoleDelegations_Core_UserRoles_UserRoleId",
                table: "Core_RoleDelegations",
                column: "UserRoleId",
                principalTable: "Core_UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Core_RolePermissions_Core_Permissions_PermissionId",
                table: "Core_RolePermissions",
                column: "PermissionId",
                principalTable: "Core_Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Core_RolePermissions_Core_Roles_RoleId",
                table: "Core_RolePermissions",
                column: "RoleId",
                principalTable: "Core_Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Core_Teams_Core_Departments_DepartmentId",
                table: "Core_Teams",
                column: "DepartmentId",
                principalTable: "Core_Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Core_UserAccounts_Core_Employees_EmployeeId",
                table: "Core_UserAccounts",
                column: "EmployeeId",
                principalTable: "Core_Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Core_UserRoles_Core_Departments_DepartmentId",
                table: "Core_UserRoles",
                column: "DepartmentId",
                principalTable: "Core_Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Core_UserRoles_Core_Roles_RoleId",
                table: "Core_UserRoles",
                column: "RoleId",
                principalTable: "Core_Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Core_UserRoles_Core_Sites_SiteId",
                table: "Core_UserRoles",
                column: "SiteId",
                principalTable: "Core_Sites",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Core_UserRoles_Core_UserAccounts_UserAccountId",
                table: "Core_UserRoles",
                column: "UserAccountId",
                principalTable: "Core_UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS DropFkIfExists;
                CREATE PROCEDURE DropFkIfExists(IN tName VARCHAR(255), IN fkName VARCHAR(255))
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
                END;
            ");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Core_Announcements\', \'FK_Core_Announcements_Core_Employees_AuthorId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Core_Announcements\', \'FK_Core_Announcements_Core_Sites_SiteId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Core_Departments\', \'FK_Core_Departments_Core_Sites_SiteId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Core_Employees\', \'FK_Core_Employees_Core_Departments_DepartmentId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Core_Employees\', \'FK_Core_Employees_Core_Positions_PositionId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Core_Employees\', \'FK_Core_Employees_Core_Sites_SiteId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Core_Employees\', \'FK_Core_Employees_Core_Teams_TeamId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Core_RoleDelegations\', \'FK_Core_RoleDelegations_Core_UserAccounts_SourceUserId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Core_RoleDelegations\', \'FK_Core_RoleDelegations_Core_UserAccounts_SubstituteUserId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Core_RoleDelegations\', \'FK_Core_RoleDelegations_Core_UserRoles_UserRoleId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Core_RolePermissions\', \'FK_Core_RolePermissions_Core_Permissions_PermissionId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Core_RolePermissions\', \'FK_Core_RolePermissions_Core_Roles_RoleId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Core_Teams\', \'FK_Core_Teams_Core_Departments_DepartmentId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Core_UserAccounts\', \'FK_Core_UserAccounts_Core_Employees_EmployeeId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Core_UserRoles\', \'FK_Core_UserRoles_Core_Departments_DepartmentId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Core_UserRoles\', \'FK_Core_UserRoles_Core_Roles_RoleId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Core_UserRoles\', \'FK_Core_UserRoles_Core_Sites_SiteId\');");

            migrationBuilder.Sql("CALL DropFkIfExists(\'Core_UserRoles\', \'FK_Core_UserRoles_Core_UserAccounts_UserAccountId\');");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Core_UserRoles",
                table: "Core_UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Core_UserAccounts",
                table: "Core_UserAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Core_Teams",
                table: "Core_Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Core_Sites",
                table: "Core_Sites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Core_Roles",
                table: "Core_Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Core_RolePermissions",
                table: "Core_RolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Core_RoleDelegations",
                table: "Core_RoleDelegations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Core_Positions",
                table: "Core_Positions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Core_Permissions",
                table: "Core_Permissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Core_Employees",
                table: "Core_Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Core_Departments",
                table: "Core_Departments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Core_Announcements",
                table: "Core_Announcements");

            migrationBuilder.RenameTable(
                name: "Core_UserRoles",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "Core_UserAccounts",
                newName: "UserAccounts");

            migrationBuilder.RenameTable(
                name: "Core_Teams",
                newName: "Teams");

            migrationBuilder.RenameTable(
                name: "Core_Sites",
                newName: "Sites");

            migrationBuilder.RenameTable(
                name: "Core_Roles",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "Core_RolePermissions",
                newName: "RolePermissions");

            migrationBuilder.RenameTable(
                name: "Core_RoleDelegations",
                newName: "RoleDelegations");

            migrationBuilder.RenameTable(
                name: "Core_Positions",
                newName: "Positions");

            migrationBuilder.RenameTable(
                name: "Core_Permissions",
                newName: "Permissions");

            migrationBuilder.RenameTable(
                name: "Core_Employees",
                newName: "Employees");

            migrationBuilder.RenameTable(
                name: "Core_Departments",
                newName: "Departments");

            migrationBuilder.RenameTable(
                name: "Core_Announcements",
                newName: "Announcements");

            migrationBuilder.RenameIndex(
                name: "IX_Core_UserRoles_UserAccountId",
                table: "UserRoles",
                newName: "IX_UserRoles_UserAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Core_UserRoles_SiteId",
                table: "UserRoles",
                newName: "IX_UserRoles_SiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Core_UserRoles_RoleId",
                table: "UserRoles",
                newName: "IX_UserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Core_UserRoles_DepartmentId",
                table: "UserRoles",
                newName: "IX_UserRoles_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Core_UserAccounts_EmployeeId",
                table: "UserAccounts",
                newName: "IX_UserAccounts_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Core_Teams_DepartmentId",
                table: "Teams",
                newName: "IX_Teams_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Core_RolePermissions_RoleId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Core_RolePermissions_PermissionId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_Core_RoleDelegations_UserRoleId",
                table: "RoleDelegations",
                newName: "IX_RoleDelegations_UserRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Core_RoleDelegations_SubstituteUserId",
                table: "RoleDelegations",
                newName: "IX_RoleDelegations_SubstituteUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Core_RoleDelegations_SourceUserId",
                table: "RoleDelegations",
                newName: "IX_RoleDelegations_SourceUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Core_Employees_TeamId",
                table: "Employees",
                newName: "IX_Employees_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Core_Employees_SiteId",
                table: "Employees",
                newName: "IX_Employees_SiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Core_Employees_PositionId",
                table: "Employees",
                newName: "IX_Employees_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_Core_Employees_DepartmentId",
                table: "Employees",
                newName: "IX_Employees_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Core_Departments_SiteId",
                table: "Departments",
                newName: "IX_Departments_SiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Core_Announcements_SiteId",
                table: "Announcements",
                newName: "IX_Announcements_SiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Core_Announcements_AuthorId",
                table: "Announcements",
                newName: "IX_Announcements_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAccounts",
                table: "UserAccounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teams",
                table: "Teams",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sites",
                table: "Sites",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleDelegations",
                table: "RoleDelegations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Positions",
                table: "Positions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Departments",
                table: "Departments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Announcements",
                table: "Announcements",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Employees_AuthorId",
                table: "Announcements",
                column: "AuthorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Sites_SiteId",
                table: "Announcements",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Sites_SiteId",
                table: "Departments",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Positions_PositionId",
                table: "Employees",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Sites_SiteId",
                table: "Employees",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Teams_TeamId",
                table: "Employees",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleDelegations_UserAccounts_SourceUserId",
                table: "RoleDelegations",
                column: "SourceUserId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleDelegations_UserAccounts_SubstituteUserId",
                table: "RoleDelegations",
                column: "SubstituteUserId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleDelegations_UserRoles_UserRoleId",
                table: "RoleDelegations",
                column: "UserRoleId",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Roles_RoleId",
                table: "RolePermissions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Departments_DepartmentId",
                table: "Teams",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Employees_EmployeeId",
                table: "UserAccounts",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Departments_DepartmentId",
                table: "UserRoles",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Sites_SiteId",
                table: "UserRoles",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_UserAccounts_UserAccountId",
                table: "UserRoles",
                column: "UserAccountId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
