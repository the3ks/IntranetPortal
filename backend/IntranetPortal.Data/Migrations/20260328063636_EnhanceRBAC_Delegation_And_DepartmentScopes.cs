using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntranetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceRBAC_Delegation_And_DepartmentScopes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "UserRoles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RoleDelegations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SourceUserId = table.Column<int>(type: "int", nullable: false),
                    SubstituteUserId = table.Column<int>(type: "int", nullable: false),
                    UserRoleId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleDelegations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleDelegations_UserAccounts_SourceUserId",
                        column: x => x.SourceUserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleDelegations_UserAccounts_SubstituteUserId",
                        column: x => x.SubstituteUserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleDelegations_UserRoles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_DepartmentId",
                table: "UserRoles",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleDelegations_SourceUserId",
                table: "RoleDelegations",
                column: "SourceUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleDelegations_SubstituteUserId",
                table: "RoleDelegations",
                column: "SubstituteUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleDelegations_UserRoleId",
                table: "RoleDelegations",
                column: "UserRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Departments_DepartmentId",
                table: "UserRoles",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Departments_DepartmentId",
                table: "UserRoles");

            migrationBuilder.DropTable(
                name: "RoleDelegations");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_DepartmentId",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "UserRoles");
        }
    }
}
