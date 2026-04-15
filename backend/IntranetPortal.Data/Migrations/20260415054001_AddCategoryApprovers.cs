using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntranetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryApprovers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequests_Core_Employees_ManagerApprovedByEmployeeId",
                table: "AM_AssetRequests");

            migrationBuilder.RenameColumn(
                name: "ManagerApprovedByEmployeeId",
                table: "AM_AssetRequests",
                newName: "SelectedApproverEmployeeId");

            migrationBuilder.RenameColumn(
                name: "ManagerApprovedAt",
                table: "AM_AssetRequests",
                newName: "ApprovedAt");

            migrationBuilder.RenameIndex(
                name: "IX_AM_AssetRequests_ManagerApprovedByEmployeeId",
                table: "AM_AssetRequests",
                newName: "IX_AM_AssetRequests_SelectedApproverEmployeeId");

            migrationBuilder.AddColumn<int>(
                name: "ApprovedByEmployeeId",
                table: "AM_AssetRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedApproverGroupId",
                table: "AM_AssetRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowRequesterToSelectApprover",
                table: "AM_AssetCategories",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DefaultApproverGroupId",
                table: "AM_AssetCategories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApproverGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApproverGroups", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ApproverGroupMembers",
                columns: table => new
                {
                    ApproverGroupId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApproverGroupMembers", x => new { x.ApproverGroupId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_ApproverGroupMembers_ApproverGroups_ApproverGroupId",
                        column: x => x.ApproverGroupId,
                        principalTable: "ApproverGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApproverGroupMembers_Core_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Core_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ApproverGroupScopes",
                columns: table => new
                {
                    ApproverGroupId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApproverGroupScopes", x => new { x.ApproverGroupId, x.DepartmentId });
                    table.ForeignKey(
                        name: "FK_ApproverGroupScopes_ApproverGroups_ApproverGroupId",
                        column: x => x.ApproverGroupId,
                        principalTable: "ApproverGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApproverGroupScopes_Core_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Core_Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AssetCategoryApproverGroups",
                columns: table => new
                {
                    AssetCategoryId = table.Column<int>(type: "int", nullable: false),
                    ApproverGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetCategoryApproverGroups", x => new { x.AssetCategoryId, x.ApproverGroupId });
                    table.ForeignKey(
                        name: "FK_AssetCategoryApproverGroups_AM_AssetCategories_AssetCategory~",
                        column: x => x.AssetCategoryId,
                        principalTable: "AM_AssetCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetCategoryApproverGroups_ApproverGroups_ApproverGroupId",
                        column: x => x.ApproverGroupId,
                        principalTable: "ApproverGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequests_ApprovedByEmployeeId",
                table: "AM_AssetRequests",
                column: "ApprovedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequests_AssignedApproverGroupId",
                table: "AM_AssetRequests",
                column: "AssignedApproverGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetCategories_DefaultApproverGroupId",
                table: "AM_AssetCategories",
                column: "DefaultApproverGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ApproverGroupMembers_EmployeeId",
                table: "ApproverGroupMembers",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ApproverGroupScopes_DepartmentId",
                table: "ApproverGroupScopes",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetCategoryApproverGroups_ApproverGroupId",
                table: "AssetCategoryApproverGroups",
                column: "ApproverGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetCategories_ApproverGroups_DefaultApproverGroupId",
                table: "AM_AssetCategories",
                column: "DefaultApproverGroupId",
                principalTable: "ApproverGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequests_ApproverGroups_AssignedApproverGroupId",
                table: "AM_AssetRequests",
                column: "AssignedApproverGroupId",
                principalTable: "ApproverGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequests_Core_Employees_ApprovedByEmployeeId",
                table: "AM_AssetRequests",
                column: "ApprovedByEmployeeId",
                principalTable: "Core_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequests_Core_Employees_SelectedApproverEmployeeId",
                table: "AM_AssetRequests",
                column: "SelectedApproverEmployeeId",
                principalTable: "Core_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetCategories_ApproverGroups_DefaultApproverGroupId",
                table: "AM_AssetCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequests_ApproverGroups_AssignedApproverGroupId",
                table: "AM_AssetRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequests_Core_Employees_ApprovedByEmployeeId",
                table: "AM_AssetRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequests_Core_Employees_SelectedApproverEmployeeId",
                table: "AM_AssetRequests");

            migrationBuilder.DropTable(
                name: "ApproverGroupMembers");

            migrationBuilder.DropTable(
                name: "ApproverGroupScopes");

            migrationBuilder.DropTable(
                name: "AssetCategoryApproverGroups");

            migrationBuilder.DropTable(
                name: "ApproverGroups");

            migrationBuilder.DropIndex(
                name: "IX_AM_AssetRequests_ApprovedByEmployeeId",
                table: "AM_AssetRequests");

            migrationBuilder.DropIndex(
                name: "IX_AM_AssetRequests_AssignedApproverGroupId",
                table: "AM_AssetRequests");

            migrationBuilder.DropIndex(
                name: "IX_AM_AssetCategories_DefaultApproverGroupId",
                table: "AM_AssetCategories");

            migrationBuilder.DropColumn(
                name: "ApprovedByEmployeeId",
                table: "AM_AssetRequests");

            migrationBuilder.DropColumn(
                name: "AssignedApproverGroupId",
                table: "AM_AssetRequests");

            migrationBuilder.DropColumn(
                name: "AllowRequesterToSelectApprover",
                table: "AM_AssetCategories");

            migrationBuilder.DropColumn(
                name: "DefaultApproverGroupId",
                table: "AM_AssetCategories");

            migrationBuilder.RenameColumn(
                name: "SelectedApproverEmployeeId",
                table: "AM_AssetRequests",
                newName: "ManagerApprovedByEmployeeId");

            migrationBuilder.RenameColumn(
                name: "ApprovedAt",
                table: "AM_AssetRequests",
                newName: "ManagerApprovedAt");

            migrationBuilder.RenameIndex(
                name: "IX_AM_AssetRequests_SelectedApproverEmployeeId",
                table: "AM_AssetRequests",
                newName: "IX_AM_AssetRequests_ManagerApprovedByEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequests_Core_Employees_ManagerApprovedByEmployeeId",
                table: "AM_AssetRequests",
                column: "ManagerApprovedByEmployeeId",
                principalTable: "Core_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
