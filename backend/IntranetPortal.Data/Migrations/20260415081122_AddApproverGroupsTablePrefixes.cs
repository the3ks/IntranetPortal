using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntranetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddApproverGroupsTablePrefixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetCategories_ApproverGroups_DefaultApproverGroupId",
                table: "AM_AssetCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequestLineItems_ApproverGroups_AssignedApproverGrou~",
                table: "AM_AssetRequestLineItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ApproverGroupMembers_ApproverGroups_ApproverGroupId",
                table: "ApproverGroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ApproverGroupMembers_Core_Employees_EmployeeId",
                table: "ApproverGroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ApproverGroupScopes_ApproverGroups_ApproverGroupId",
                table: "ApproverGroupScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_ApproverGroupScopes_Core_Departments_DepartmentId",
                table: "ApproverGroupScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetCategoryApproverGroups_AM_AssetCategories_AssetCategory~",
                table: "AssetCategoryApproverGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetCategoryApproverGroups_ApproverGroups_ApproverGroupId",
                table: "AssetCategoryApproverGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssetCategoryApproverGroups",
                table: "AssetCategoryApproverGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApproverGroupScopes",
                table: "ApproverGroupScopes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApproverGroups",
                table: "ApproverGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApproverGroupMembers",
                table: "ApproverGroupMembers");

            migrationBuilder.RenameTable(
                name: "AssetCategoryApproverGroups",
                newName: "AM_AssetCategoryApproverGroups");

            migrationBuilder.RenameTable(
                name: "ApproverGroupScopes",
                newName: "AM_ApproverGroupScopes");

            migrationBuilder.RenameTable(
                name: "ApproverGroups",
                newName: "AM_ApproverGroups");

            migrationBuilder.RenameTable(
                name: "ApproverGroupMembers",
                newName: "AM_ApproverGroupMembers");

            migrationBuilder.RenameIndex(
                name: "IX_AssetCategoryApproverGroups_ApproverGroupId",
                table: "AM_AssetCategoryApproverGroups",
                newName: "IX_AM_AssetCategoryApproverGroups_ApproverGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_ApproverGroupScopes_DepartmentId",
                table: "AM_ApproverGroupScopes",
                newName: "IX_AM_ApproverGroupScopes_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_ApproverGroupMembers_EmployeeId",
                table: "AM_ApproverGroupMembers",
                newName: "IX_AM_ApproverGroupMembers_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AM_AssetCategoryApproverGroups",
                table: "AM_AssetCategoryApproverGroups",
                columns: new[] { "AssetCategoryId", "ApproverGroupId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AM_ApproverGroupScopes",
                table: "AM_ApproverGroupScopes",
                columns: new[] { "ApproverGroupId", "DepartmentId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AM_ApproverGroups",
                table: "AM_ApproverGroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AM_ApproverGroupMembers",
                table: "AM_ApproverGroupMembers",
                columns: new[] { "ApproverGroupId", "EmployeeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AM_ApproverGroupMembers_AM_ApproverGroups_ApproverGroupId",
                table: "AM_ApproverGroupMembers",
                column: "ApproverGroupId",
                principalTable: "AM_ApproverGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_ApproverGroupMembers_Core_Employees_EmployeeId",
                table: "AM_ApproverGroupMembers",
                column: "EmployeeId",
                principalTable: "Core_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_ApproverGroupScopes_AM_ApproverGroups_ApproverGroupId",
                table: "AM_ApproverGroupScopes",
                column: "ApproverGroupId",
                principalTable: "AM_ApproverGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_ApproverGroupScopes_Core_Departments_DepartmentId",
                table: "AM_ApproverGroupScopes",
                column: "DepartmentId",
                principalTable: "Core_Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetCategories_AM_ApproverGroups_DefaultApproverGroupId",
                table: "AM_AssetCategories",
                column: "DefaultApproverGroupId",
                principalTable: "AM_ApproverGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetCategoryApproverGroups_AM_ApproverGroups_ApproverGro~",
                table: "AM_AssetCategoryApproverGroups",
                column: "ApproverGroupId",
                principalTable: "AM_ApproverGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetCategoryApproverGroups_AM_AssetCategories_AssetCateg~",
                table: "AM_AssetCategoryApproverGroups",
                column: "AssetCategoryId",
                principalTable: "AM_AssetCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequestLineItems_AM_ApproverGroups_AssignedApproverG~",
                table: "AM_AssetRequestLineItems",
                column: "AssignedApproverGroupId",
                principalTable: "AM_ApproverGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AM_ApproverGroupMembers_AM_ApproverGroups_ApproverGroupId",
                table: "AM_ApproverGroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_ApproverGroupMembers_Core_Employees_EmployeeId",
                table: "AM_ApproverGroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_ApproverGroupScopes_AM_ApproverGroups_ApproverGroupId",
                table: "AM_ApproverGroupScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_ApproverGroupScopes_Core_Departments_DepartmentId",
                table: "AM_ApproverGroupScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetCategories_AM_ApproverGroups_DefaultApproverGroupId",
                table: "AM_AssetCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetCategoryApproverGroups_AM_ApproverGroups_ApproverGro~",
                table: "AM_AssetCategoryApproverGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetCategoryApproverGroups_AM_AssetCategories_AssetCateg~",
                table: "AM_AssetCategoryApproverGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequestLineItems_AM_ApproverGroups_AssignedApproverG~",
                table: "AM_AssetRequestLineItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AM_AssetCategoryApproverGroups",
                table: "AM_AssetCategoryApproverGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AM_ApproverGroupScopes",
                table: "AM_ApproverGroupScopes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AM_ApproverGroups",
                table: "AM_ApproverGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AM_ApproverGroupMembers",
                table: "AM_ApproverGroupMembers");

            migrationBuilder.RenameTable(
                name: "AM_AssetCategoryApproverGroups",
                newName: "AssetCategoryApproverGroups");

            migrationBuilder.RenameTable(
                name: "AM_ApproverGroupScopes",
                newName: "ApproverGroupScopes");

            migrationBuilder.RenameTable(
                name: "AM_ApproverGroups",
                newName: "ApproverGroups");

            migrationBuilder.RenameTable(
                name: "AM_ApproverGroupMembers",
                newName: "ApproverGroupMembers");

            migrationBuilder.RenameIndex(
                name: "IX_AM_AssetCategoryApproverGroups_ApproverGroupId",
                table: "AssetCategoryApproverGroups",
                newName: "IX_AssetCategoryApproverGroups_ApproverGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_AM_ApproverGroupScopes_DepartmentId",
                table: "ApproverGroupScopes",
                newName: "IX_ApproverGroupScopes_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_AM_ApproverGroupMembers_EmployeeId",
                table: "ApproverGroupMembers",
                newName: "IX_ApproverGroupMembers_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssetCategoryApproverGroups",
                table: "AssetCategoryApproverGroups",
                columns: new[] { "AssetCategoryId", "ApproverGroupId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApproverGroupScopes",
                table: "ApproverGroupScopes",
                columns: new[] { "ApproverGroupId", "DepartmentId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApproverGroups",
                table: "ApproverGroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApproverGroupMembers",
                table: "ApproverGroupMembers",
                columns: new[] { "ApproverGroupId", "EmployeeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetCategories_ApproverGroups_DefaultApproverGroupId",
                table: "AM_AssetCategories",
                column: "DefaultApproverGroupId",
                principalTable: "ApproverGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequestLineItems_ApproverGroups_AssignedApproverGrou~",
                table: "AM_AssetRequestLineItems",
                column: "AssignedApproverGroupId",
                principalTable: "ApproverGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApproverGroupMembers_ApproverGroups_ApproverGroupId",
                table: "ApproverGroupMembers",
                column: "ApproverGroupId",
                principalTable: "ApproverGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApproverGroupMembers_Core_Employees_EmployeeId",
                table: "ApproverGroupMembers",
                column: "EmployeeId",
                principalTable: "Core_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApproverGroupScopes_ApproverGroups_ApproverGroupId",
                table: "ApproverGroupScopes",
                column: "ApproverGroupId",
                principalTable: "ApproverGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApproverGroupScopes_Core_Departments_DepartmentId",
                table: "ApproverGroupScopes",
                column: "DepartmentId",
                principalTable: "Core_Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetCategoryApproverGroups_AM_AssetCategories_AssetCategory~",
                table: "AssetCategoryApproverGroups",
                column: "AssetCategoryId",
                principalTable: "AM_AssetCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetCategoryApproverGroups_ApproverGroups_ApproverGroupId",
                table: "AssetCategoryApproverGroups",
                column: "ApproverGroupId",
                principalTable: "ApproverGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
