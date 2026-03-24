using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntranetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteIdScopeLimits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Announcements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_SiteId",
                table: "Departments",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_SiteId",
                table: "Announcements",
                column: "SiteId");

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
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Sites_SiteId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Sites_SiteId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_SiteId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_SiteId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Announcements");
        }
    }
}
