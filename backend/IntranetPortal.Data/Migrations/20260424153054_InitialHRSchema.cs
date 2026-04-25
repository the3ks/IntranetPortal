using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntranetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialHRSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HR_LeaveTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DaysAllowed = table.Column<int>(type: "int", nullable: false),
                    RequiresApproval = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_LeaveTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HR_OnboardingTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_OnboardingTemplates", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HR_OnboardingTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AssigneeRole = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_OnboardingTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_OnboardingTasks_HR_OnboardingTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "HR_OnboardingTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HR_Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentDepartmentId = table.Column<int>(type: "int", nullable: true),
                    ManagerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_Departments_HR_Departments_ParentDepartmentId",
                        column: x => x.ParentDepartmentId,
                        principalTable: "HR_Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HR_EmployeeRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserAccountId = table.Column<int>(type: "int", nullable: false),
                    EmployeeNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HireDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EmergencyContactName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmergencyContactPhone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    PositionId = table.Column<int>(type: "int", nullable: true),
                    DirectManagerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_EmployeeRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_EmployeeRecords_Core_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "Core_UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HR_EmployeeRecords_HR_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "HR_Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_HR_EmployeeRecords_HR_EmployeeRecords_DirectManagerId",
                        column: x => x.DirectManagerId,
                        principalTable: "HR_EmployeeRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HR_EmployeeRecords_HR_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "HR_Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HR_EmployeeOnboardingTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeRecordId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AssigneeRole = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsCompleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_EmployeeOnboardingTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_EmployeeOnboardingTasks_HR_EmployeeRecords_EmployeeRecord~",
                        column: x => x.EmployeeRecordId,
                        principalTable: "HR_EmployeeRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HR_LeaveRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeRecordId = table.Column<int>(type: "int", nullable: false),
                    LeaveTypeId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Reason = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApprovedById = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_LeaveRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_LeaveRequests_HR_EmployeeRecords_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "HR_EmployeeRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HR_LeaveRequests_HR_EmployeeRecords_EmployeeRecordId",
                        column: x => x.EmployeeRecordId,
                        principalTable: "HR_EmployeeRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HR_LeaveRequests_HR_LeaveTypes_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "HR_LeaveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Departments_ManagerId",
                table: "HR_Departments",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Departments_ParentDepartmentId",
                table: "HR_Departments",
                column: "ParentDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeOnboardingTasks_EmployeeRecordId",
                table: "HR_EmployeeOnboardingTasks",
                column: "EmployeeRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeRecords_DepartmentId",
                table: "HR_EmployeeRecords",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeRecords_DirectManagerId",
                table: "HR_EmployeeRecords",
                column: "DirectManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeRecords_PositionId",
                table: "HR_EmployeeRecords",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeRecords_UserAccountId",
                table: "HR_EmployeeRecords",
                column: "UserAccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HR_LeaveRequests_ApprovedById",
                table: "HR_LeaveRequests",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_HR_LeaveRequests_EmployeeRecordId",
                table: "HR_LeaveRequests",
                column: "EmployeeRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_LeaveRequests_LeaveTypeId",
                table: "HR_LeaveRequests",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_OnboardingTasks_TemplateId",
                table: "HR_OnboardingTasks",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_HR_Departments_HR_EmployeeRecords_ManagerId",
                table: "HR_Departments",
                column: "ManagerId",
                principalTable: "HR_EmployeeRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HR_Departments_HR_EmployeeRecords_ManagerId",
                table: "HR_Departments");

            migrationBuilder.DropTable(
                name: "HR_EmployeeOnboardingTasks");

            migrationBuilder.DropTable(
                name: "HR_LeaveRequests");

            migrationBuilder.DropTable(
                name: "HR_OnboardingTasks");

            migrationBuilder.DropTable(
                name: "HR_LeaveTypes");

            migrationBuilder.DropTable(
                name: "HR_OnboardingTemplates");

            migrationBuilder.DropTable(
                name: "HR_EmployeeRecords");

            migrationBuilder.DropTable(
                name: "HR_Departments");
        }
    }
}
