using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntranetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class PartitionPositionToHRModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Core_Employees_Core_Positions_PositionId",
                table: "Core_Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Core_Positions",
                table: "Core_Positions");

            migrationBuilder.RenameTable(
                name: "Core_Positions",
                newName: "HR_Positions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HR_Positions",
                table: "HR_Positions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Core_Employees_HR_Positions_PositionId",
                table: "Core_Employees",
                column: "PositionId",
                principalTable: "HR_Positions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Core_Employees_HR_Positions_PositionId",
                table: "Core_Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HR_Positions",
                table: "HR_Positions");

            migrationBuilder.RenameTable(
                name: "HR_Positions",
                newName: "Core_Positions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Core_Positions",
                table: "Core_Positions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Core_Employees_Core_Positions_PositionId",
                table: "Core_Employees",
                column: "PositionId",
                principalTable: "Core_Positions",
                principalColumn: "Id");
        }
    }
}
