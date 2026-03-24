using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntranetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnforceStrictSiteIdBindings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Employees SET SiteId = 1 WHERE SiteId IS NULL;");
            migrationBuilder.Sql("UPDATE Departments SET SiteId = 1 WHERE SiteId IS NULL;");

            migrationBuilder.AlterColumn<int>(
                name: "SiteId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SiteId",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Sites_SiteId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Sites_SiteId",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "SiteId",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SiteId",
                table: "Departments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Sites_SiteId",
                table: "Departments",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Sites_SiteId",
                table: "Employees",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id");
        }
    }
}
