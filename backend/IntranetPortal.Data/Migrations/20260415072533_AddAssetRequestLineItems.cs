using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntranetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAssetRequestLineItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequests_AM_Accessories_RequestedAccessoryId",
                table: "AM_AssetRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequests_AM_AssetCategories_RequestedCategoryId",
                table: "AM_AssetRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequests_AM_AssetModels_RequestedModelId",
                table: "AM_AssetRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequests_AM_Assets_AssignedAssetId",
                table: "AM_AssetRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequests_ApproverGroups_AssignedApproverGroupId",
                table: "AM_AssetRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequests_Core_Employees_ApprovedByEmployeeId",
                table: "AM_AssetRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequests_Core_Employees_FulfilledByEmployeeId",
                table: "AM_AssetRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequests_Core_Employees_SelectedApproverEmployeeId",
                table: "AM_AssetRequests");

            migrationBuilder.DropIndex(
                name: "IX_AM_AssetRequests_ApprovedByEmployeeId",
                table: "AM_AssetRequests");

            migrationBuilder.DropIndex(
                name: "IX_AM_AssetRequests_AssignedApproverGroupId",
                table: "AM_AssetRequests");

            migrationBuilder.DropIndex(
                name: "IX_AM_AssetRequests_AssignedAssetId",
                table: "AM_AssetRequests");

            migrationBuilder.DropIndex(
                name: "IX_AM_AssetRequests_FulfilledByEmployeeId",
                table: "AM_AssetRequests");

            migrationBuilder.DropIndex(
                name: "IX_AM_AssetRequests_RequestedAccessoryId",
                table: "AM_AssetRequests");

            migrationBuilder.DropIndex(
                name: "IX_AM_AssetRequests_RequestedCategoryId",
                table: "AM_AssetRequests");

            migrationBuilder.DropIndex(
                name: "IX_AM_AssetRequests_RequestedModelId",
                table: "AM_AssetRequests");

            migrationBuilder.DropIndex(
                name: "IX_AM_AssetRequests_SelectedApproverEmployeeId",
                table: "AM_AssetRequests");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "AM_AssetRequests");

            migrationBuilder.DropColumn(
                name: "ApprovedByEmployeeId",
                table: "AM_AssetRequests");

            migrationBuilder.DropColumn(
                name: "AssignedApproverGroupId",
                table: "AM_AssetRequests");

            migrationBuilder.DropColumn(
                name: "AssignedAssetId",
                table: "AM_AssetRequests");

            migrationBuilder.DropColumn(
                name: "FulfilledAt",
                table: "AM_AssetRequests");

            migrationBuilder.DropColumn(
                name: "FulfilledByEmployeeId",
                table: "AM_AssetRequests");

            migrationBuilder.DropColumn(
                name: "Justification",
                table: "AM_AssetRequests");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "AM_AssetRequests");

            migrationBuilder.DropColumn(
                name: "RequestedAccessoryId",
                table: "AM_AssetRequests");

            migrationBuilder.DropColumn(
                name: "RequestedCategoryId",
                table: "AM_AssetRequests");

            migrationBuilder.DropColumn(
                name: "RequestedModelId",
                table: "AM_AssetRequests");

            migrationBuilder.DropColumn(
                name: "SelectedApproverEmployeeId",
                table: "AM_AssetRequests");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "AM_AssetRequests");

            migrationBuilder.CreateTable(
                name: "AM_AssetRequestLineItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssetRequestId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    RequestedCategoryId = table.Column<int>(type: "int", nullable: true),
                    RequestedModelId = table.Column<int>(type: "int", nullable: true),
                    RequestedAccessoryId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Justification = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SelectedApproverEmployeeId = table.Column<int>(type: "int", nullable: true),
                    AssignedApproverGroupId = table.Column<int>(type: "int", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ApprovedByEmployeeId = table.Column<int>(type: "int", nullable: true),
                    FulfilledAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FulfilledByEmployeeId = table.Column<int>(type: "int", nullable: true),
                    AssignedAssetId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AM_AssetRequestLineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AM_AssetRequestLineItems_AM_Accessories_RequestedAccessoryId",
                        column: x => x.RequestedAccessoryId,
                        principalTable: "AM_Accessories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_AssetRequestLineItems_AM_AssetCategories_RequestedCategor~",
                        column: x => x.RequestedCategoryId,
                        principalTable: "AM_AssetCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_AssetRequestLineItems_AM_AssetModels_RequestedModelId",
                        column: x => x.RequestedModelId,
                        principalTable: "AM_AssetModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_AssetRequestLineItems_AM_AssetRequests_AssetRequestId",
                        column: x => x.AssetRequestId,
                        principalTable: "AM_AssetRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AM_AssetRequestLineItems_AM_Assets_AssignedAssetId",
                        column: x => x.AssignedAssetId,
                        principalTable: "AM_Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_AssetRequestLineItems_ApproverGroups_AssignedApproverGrou~",
                        column: x => x.AssignedApproverGroupId,
                        principalTable: "ApproverGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_AssetRequestLineItems_Core_Employees_ApprovedByEmployeeId",
                        column: x => x.ApprovedByEmployeeId,
                        principalTable: "Core_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_AssetRequestLineItems_Core_Employees_FulfilledByEmployeeId",
                        column: x => x.FulfilledByEmployeeId,
                        principalTable: "Core_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AM_AssetRequestLineItems_Core_Employees_SelectedApproverEmpl~",
                        column: x => x.SelectedApproverEmployeeId,
                        principalTable: "Core_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequestLineItems_ApprovedByEmployeeId",
                table: "AM_AssetRequestLineItems",
                column: "ApprovedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequestLineItems_AssetRequestId",
                table: "AM_AssetRequestLineItems",
                column: "AssetRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequestLineItems_AssignedApproverGroupId",
                table: "AM_AssetRequestLineItems",
                column: "AssignedApproverGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequestLineItems_AssignedAssetId",
                table: "AM_AssetRequestLineItems",
                column: "AssignedAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequestLineItems_FulfilledByEmployeeId",
                table: "AM_AssetRequestLineItems",
                column: "FulfilledByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequestLineItems_RequestedAccessoryId",
                table: "AM_AssetRequestLineItems",
                column: "RequestedAccessoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequestLineItems_RequestedCategoryId",
                table: "AM_AssetRequestLineItems",
                column: "RequestedCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequestLineItems_RequestedModelId",
                table: "AM_AssetRequestLineItems",
                column: "RequestedModelId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequestLineItems_SelectedApproverEmployeeId",
                table: "AM_AssetRequestLineItems",
                column: "SelectedApproverEmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AM_AssetRequestLineItems");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "AM_AssetRequests",
                type: "datetime(6)",
                nullable: true);

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

            migrationBuilder.AddColumn<int>(
                name: "AssignedAssetId",
                table: "AM_AssetRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FulfilledAt",
                table: "AM_AssetRequests",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FulfilledByEmployeeId",
                table: "AM_AssetRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Justification",
                table: "AM_AssetRequests",
                type: "varchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "AM_AssetRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequestedAccessoryId",
                table: "AM_AssetRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestedCategoryId",
                table: "AM_AssetRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestedModelId",
                table: "AM_AssetRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SelectedApproverEmployeeId",
                table: "AM_AssetRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "AM_AssetRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequests_ApprovedByEmployeeId",
                table: "AM_AssetRequests",
                column: "ApprovedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequests_AssignedApproverGroupId",
                table: "AM_AssetRequests",
                column: "AssignedApproverGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequests_AssignedAssetId",
                table: "AM_AssetRequests",
                column: "AssignedAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequests_FulfilledByEmployeeId",
                table: "AM_AssetRequests",
                column: "FulfilledByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequests_RequestedAccessoryId",
                table: "AM_AssetRequests",
                column: "RequestedAccessoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequests_RequestedCategoryId",
                table: "AM_AssetRequests",
                column: "RequestedCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequests_RequestedModelId",
                table: "AM_AssetRequests",
                column: "RequestedModelId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequests_SelectedApproverEmployeeId",
                table: "AM_AssetRequests",
                column: "SelectedApproverEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequests_AM_Accessories_RequestedAccessoryId",
                table: "AM_AssetRequests",
                column: "RequestedAccessoryId",
                principalTable: "AM_Accessories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequests_AM_AssetCategories_RequestedCategoryId",
                table: "AM_AssetRequests",
                column: "RequestedCategoryId",
                principalTable: "AM_AssetCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequests_AM_AssetModels_RequestedModelId",
                table: "AM_AssetRequests",
                column: "RequestedModelId",
                principalTable: "AM_AssetModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequests_AM_Assets_AssignedAssetId",
                table: "AM_AssetRequests",
                column: "AssignedAssetId",
                principalTable: "AM_Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_AM_AssetRequests_Core_Employees_FulfilledByEmployeeId",
                table: "AM_AssetRequests",
                column: "FulfilledByEmployeeId",
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
    }
}
