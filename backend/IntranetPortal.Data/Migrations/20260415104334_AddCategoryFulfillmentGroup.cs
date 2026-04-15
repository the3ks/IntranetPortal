using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntranetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryFulfillmentGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FulfillmentGroupId",
                table: "AM_AssetCategories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetCategories_FulfillmentGroupId",
                table: "AM_AssetCategories",
                column: "FulfillmentGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetCategories_AM_ApproverGroups_FulfillmentGroupId",
                table: "AM_AssetCategories",
                column: "FulfillmentGroupId",
                principalTable: "AM_ApproverGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetCategories_AM_ApproverGroups_FulfillmentGroupId",
                table: "AM_AssetCategories");

            migrationBuilder.DropIndex(
                name: "IX_AM_AssetCategories_FulfillmentGroupId",
                table: "AM_AssetCategories");

            migrationBuilder.DropColumn(
                name: "FulfillmentGroupId",
                table: "AM_AssetCategories");
        }
    }
}
