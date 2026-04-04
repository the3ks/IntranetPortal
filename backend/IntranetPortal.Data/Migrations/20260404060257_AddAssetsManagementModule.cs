using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntranetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAssetsManagementModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AM_AssetCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    RequiresApproval = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AM_AssetCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AM_AssetCategories_AM_AssetCategories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "AM_AssetCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AM_Accessories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    TotalQuantity = table.Column<int>(type: "int", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    MinStockThreshold = table.Column<int>(type: "int", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AM_Accessories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AM_Accessories_AM_AssetCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "AM_AssetCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_Accessories_Core_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Core_Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_Accessories_Core_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Core_Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AM_AssetModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Manufacturer = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AM_AssetModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AM_AssetModels_AM_AssetCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "AM_AssetCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AM_AccessoryCheckouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccessoryId = table.Column<int>(type: "int", nullable: false),
                    RequestedByEmployeeId = table.Column<int>(type: "int", nullable: false),
                    FulfilledByEmployeeId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CheckoutDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AM_AccessoryCheckouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AM_AccessoryCheckouts_AM_Accessories_AccessoryId",
                        column: x => x.AccessoryId,
                        principalTable: "AM_Accessories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AM_AccessoryCheckouts_Core_Employees_FulfilledByEmployeeId",
                        column: x => x.FulfilledByEmployeeId,
                        principalTable: "Core_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_AccessoryCheckouts_Core_Employees_RequestedByEmployeeId",
                        column: x => x.RequestedByEmployeeId,
                        principalTable: "Core_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AM_Assets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssetTag = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    SerialNumber = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PhysicalLocation = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Vendor = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WarrantyExpiration = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByEmployeeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AM_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AM_Assets_AM_AssetModels_ModelId",
                        column: x => x.ModelId,
                        principalTable: "AM_AssetModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_Assets_Core_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Core_Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_Assets_Core_Employees_CreatedByEmployeeId",
                        column: x => x.CreatedByEmployeeId,
                        principalTable: "Core_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AM_Assets_Core_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Core_Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AM_AssetAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    AssignedToEmployeeId = table.Column<int>(type: "int", nullable: true),
                    AssignedToTeamId = table.Column<int>(type: "int", nullable: true),
                    DateAssigned = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AssignedByEmployeeId = table.Column<int>(type: "int", nullable: false),
                    ExpectedReturnDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ActualReturnDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ReturnedByEmployeeId = table.Column<int>(type: "int", nullable: true),
                    ConditionOnAssign = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConditionOnReturn = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AM_AssetAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AM_AssetAssignments_AM_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "AM_Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AM_AssetAssignments_Core_Employees_AssignedByEmployeeId",
                        column: x => x.AssignedByEmployeeId,
                        principalTable: "Core_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_AssetAssignments_Core_Employees_AssignedToEmployeeId",
                        column: x => x.AssignedToEmployeeId,
                        principalTable: "Core_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_AssetAssignments_Core_Employees_ReturnedByEmployeeId",
                        column: x => x.ReturnedByEmployeeId,
                        principalTable: "Core_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_AssetAssignments_Core_Teams_AssignedToTeamId",
                        column: x => x.AssignedToTeamId,
                        principalTable: "Core_Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AM_AssetAuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OldValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NewValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PerformedByEmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AM_AssetAuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AM_AssetAuditLogs_AM_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "AM_Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AM_AssetAuditLogs_Core_Employees_PerformedByEmployeeId",
                        column: x => x.PerformedByEmployeeId,
                        principalTable: "Core_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AM_AssetMaintenance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    MaintenanceDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RepairVendor = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LoggedByEmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AM_AssetMaintenance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AM_AssetMaintenance_AM_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "AM_Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AM_AssetMaintenance_Core_Employees_LoggedByEmployeeId",
                        column: x => x.LoggedByEmployeeId,
                        principalTable: "Core_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AM_AssetRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RequestedByEmployeeId = table.Column<int>(type: "int", nullable: false),
                    RequestedForEmployeeId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    RequestedCategoryId = table.Column<int>(type: "int", nullable: true),
                    RequestedModelId = table.Column<int>(type: "int", nullable: true),
                    RequestedAccessoryId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Justification = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ManagerApprovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ManagerApprovedByEmployeeId = table.Column<int>(type: "int", nullable: true),
                    FulfilledAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FulfilledByEmployeeId = table.Column<int>(type: "int", nullable: true),
                    AssignedAssetId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AM_AssetRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AM_AssetRequests_AM_Accessories_RequestedAccessoryId",
                        column: x => x.RequestedAccessoryId,
                        principalTable: "AM_Accessories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_AssetRequests_AM_AssetCategories_RequestedCategoryId",
                        column: x => x.RequestedCategoryId,
                        principalTable: "AM_AssetCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_AssetRequests_AM_AssetModels_RequestedModelId",
                        column: x => x.RequestedModelId,
                        principalTable: "AM_AssetModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_AssetRequests_AM_Assets_AssignedAssetId",
                        column: x => x.AssignedAssetId,
                        principalTable: "AM_Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_AssetRequests_Core_Employees_FulfilledByEmployeeId",
                        column: x => x.FulfilledByEmployeeId,
                        principalTable: "Core_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_AssetRequests_Core_Employees_ManagerApprovedByEmployeeId",
                        column: x => x.ManagerApprovedByEmployeeId,
                        principalTable: "Core_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_AssetRequests_Core_Employees_RequestedByEmployeeId",
                        column: x => x.RequestedByEmployeeId,
                        principalTable: "Core_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_AssetRequests_Core_Employees_RequestedForEmployeeId",
                        column: x => x.RequestedForEmployeeId,
                        principalTable: "Core_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AM_Accessories_CategoryId",
                table: "AM_Accessories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_Accessories_DepartmentId",
                table: "AM_Accessories",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_Accessories_SiteId",
                table: "AM_Accessories",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AccessoryCheckouts_AccessoryId",
                table: "AM_AccessoryCheckouts",
                column: "AccessoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AccessoryCheckouts_FulfilledByEmployeeId",
                table: "AM_AccessoryCheckouts",
                column: "FulfilledByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AccessoryCheckouts_RequestedByEmployeeId",
                table: "AM_AccessoryCheckouts",
                column: "RequestedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetAssignments_AssetId",
                table: "AM_AssetAssignments",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetAssignments_AssignedByEmployeeId",
                table: "AM_AssetAssignments",
                column: "AssignedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetAssignments_AssignedToEmployeeId",
                table: "AM_AssetAssignments",
                column: "AssignedToEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetAssignments_AssignedToTeamId",
                table: "AM_AssetAssignments",
                column: "AssignedToTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetAssignments_ReturnedByEmployeeId",
                table: "AM_AssetAssignments",
                column: "ReturnedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetAuditLogs_AssetId",
                table: "AM_AssetAuditLogs",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetAuditLogs_PerformedByEmployeeId",
                table: "AM_AssetAuditLogs",
                column: "PerformedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetCategories_ParentCategoryId",
                table: "AM_AssetCategories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetMaintenance_AssetId",
                table: "AM_AssetMaintenance",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetMaintenance_LoggedByEmployeeId",
                table: "AM_AssetMaintenance",
                column: "LoggedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetModels_CategoryId",
                table: "AM_AssetModels",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequests_AssignedAssetId",
                table: "AM_AssetRequests",
                column: "AssignedAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequests_FulfilledByEmployeeId",
                table: "AM_AssetRequests",
                column: "FulfilledByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequests_ManagerApprovedByEmployeeId",
                table: "AM_AssetRequests",
                column: "ManagerApprovedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequests_RequestedAccessoryId",
                table: "AM_AssetRequests",
                column: "RequestedAccessoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequests_RequestedByEmployeeId",
                table: "AM_AssetRequests",
                column: "RequestedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequests_RequestedCategoryId",
                table: "AM_AssetRequests",
                column: "RequestedCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequests_RequestedForEmployeeId",
                table: "AM_AssetRequests",
                column: "RequestedForEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequests_RequestedModelId",
                table: "AM_AssetRequests",
                column: "RequestedModelId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_Assets_AssetTag",
                table: "AM_Assets",
                column: "AssetTag",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AM_Assets_CreatedByEmployeeId",
                table: "AM_Assets",
                column: "CreatedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_Assets_DepartmentId",
                table: "AM_Assets",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_Assets_ModelId",
                table: "AM_Assets",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_Assets_SiteId",
                table: "AM_Assets",
                column: "SiteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AM_AccessoryCheckouts");

            migrationBuilder.DropTable(
                name: "AM_AssetAssignments");

            migrationBuilder.DropTable(
                name: "AM_AssetAuditLogs");

            migrationBuilder.DropTable(
                name: "AM_AssetMaintenance");

            migrationBuilder.DropTable(
                name: "AM_AssetRequests");

            migrationBuilder.DropTable(
                name: "AM_Accessories");

            migrationBuilder.DropTable(
                name: "AM_Assets");

            migrationBuilder.DropTable(
                name: "AM_AssetModels");

            migrationBuilder.DropTable(
                name: "AM_AssetCategories");
        }
    }
}
