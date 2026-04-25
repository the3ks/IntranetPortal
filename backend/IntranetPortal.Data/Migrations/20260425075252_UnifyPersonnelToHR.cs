using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntranetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class UnifyPersonnelToHR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SET FOREIGN_KEY_CHECKS = 0;");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_Accessories_Core_Departments_DepartmentId",
                table: "AM_Accessories");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AccessoryCheckouts_Core_Employees_FulfilledByEmployeeId",
                table: "AM_AccessoryCheckouts");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AccessoryCheckouts_Core_Employees_RequestedByEmployeeId",
                table: "AM_AccessoryCheckouts");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_ApproverGroupMembers_Core_Employees_EmployeeId",
                table: "AM_ApproverGroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_ApproverGroupScopes_Core_Departments_DepartmentId",
                table: "AM_ApproverGroupScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetAssignments_Core_Employees_AssignedByEmployeeId",
                table: "AM_AssetAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetAssignments_Core_Employees_AssignedToEmployeeId",
                table: "AM_AssetAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetAssignments_Core_Employees_ReturnedByEmployeeId",
                table: "AM_AssetAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetAuditLogs_Core_Employees_PerformedByEmployeeId",
                table: "AM_AssetAuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetMaintenance_Core_Employees_LoggedByEmployeeId",
                table: "AM_AssetMaintenance");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequestLineItems_Core_Employees_ApprovedByEmployeeId",
                table: "AM_AssetRequestLineItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequestLineItems_Core_Employees_FulfilledByEmployeeId",
                table: "AM_AssetRequestLineItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequestLineItems_Core_Employees_SelectedApproverEmpl~",
                table: "AM_AssetRequestLineItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequests_Core_Employees_RequestedByEmployeeId",
                table: "AM_AssetRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequests_Core_Employees_RequestedForEmployeeId",
                table: "AM_AssetRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_Assets_Core_Departments_DepartmentId",
                table: "AM_Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_Assets_Core_Employees_CreatedByEmployeeId",
                table: "AM_Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Core_Announcements_Core_Employees_AuthorId",
                table: "Core_Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Core_Teams_Core_Departments_DepartmentId",
                table: "Core_Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Core_UserAccounts_Core_Employees_EmployeeId",
                table: "Core_UserAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Core_UserRoles_Core_Departments_DepartmentId",
                table: "Core_UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_AttendanceLogs_HR_EmployeeRecords_EmployeeRecordId",
                table: "HR_AttendanceLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_Departments_HR_EmployeeRecords_ManagerId",
                table: "HR_Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_EmployeeOnboardingTasks_HR_EmployeeRecords_EmployeeRecord~",
                table: "HR_EmployeeOnboardingTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_LeaveRequests_HR_EmployeeRecords_ApprovedById",
                table: "HR_LeaveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_LeaveRequests_HR_EmployeeRecords_EmployeeRecordId",
                table: "HR_LeaveRequests");

            migrationBuilder.DropTable(
                name: "Core_Employees");

            migrationBuilder.DropTable(
                name: "HR_EmployeeRecords");

            migrationBuilder.DropTable(
                name: "Core_Departments");

            migrationBuilder.DropIndex(
                name: "IX_Core_UserAccounts_EmployeeId",
                table: "Core_UserAccounts");

            migrationBuilder.DropColumn(
                name: "AssigneeRole",
                table: "HR_EmployeeOnboardingTasks");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "HR_EmployeeOnboardingTasks");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "HR_EmployeeOnboardingTasks");

            migrationBuilder.RenameColumn(
                name: "EmployeeRecordId",
                table: "HR_LeaveRequests",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_HR_LeaveRequests_EmployeeRecordId",
                table: "HR_LeaveRequests",
                newName: "IX_HR_LeaveRequests_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "HR_EmployeeOnboardingTasks",
                newName: "OnboardingTaskId");

            migrationBuilder.RenameColumn(
                name: "EmployeeRecordId",
                table: "HR_EmployeeOnboardingTasks",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_HR_EmployeeOnboardingTasks_EmployeeRecordId",
                table: "HR_EmployeeOnboardingTasks",
                newName: "IX_HR_EmployeeOnboardingTasks_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "EmployeeRecordId",
                table: "HR_AttendanceLogs",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_HR_AttendanceLogs_EmployeeRecordId_Date",
                table: "HR_AttendanceLogs",
                newName: "IX_HR_AttendanceLogs_EmployeeId_Date");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "HR_Departments",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldMaxLength: 150)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "HR_Departments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "HR_Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserAccountId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmployeeNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HireDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EmergencyContactName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmergencyContactPhone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    PositionId = table.Column<int>(type: "int", nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    DirectManagerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_Employees_Core_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Core_Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HR_Employees_Core_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Core_Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HR_Employees_Core_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "Core_UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HR_Employees_HR_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "HR_Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_HR_Employees_HR_Employees_DirectManagerId",
                        column: x => x.DirectManagerId,
                        principalTable: "HR_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HR_Employees_HR_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "HR_Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeOnboardingTasks_OnboardingTaskId",
                table: "HR_EmployeeOnboardingTasks",
                column: "OnboardingTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Departments_SiteId",
                table: "HR_Departments",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Employees_DepartmentId",
                table: "HR_Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Employees_DirectManagerId",
                table: "HR_Employees",
                column: "DirectManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Employees_PositionId",
                table: "HR_Employees",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Employees_SiteId",
                table: "HR_Employees",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Employees_TeamId",
                table: "HR_Employees",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Employees_UserAccountId",
                table: "HR_Employees",
                column: "UserAccountId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_Accessories_HR_Departments_DepartmentId",
                table: "AM_Accessories",
                column: "DepartmentId",
                principalTable: "HR_Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AccessoryCheckouts_HR_Employees_FulfilledByEmployeeId",
                table: "AM_AccessoryCheckouts",
                column: "FulfilledByEmployeeId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AccessoryCheckouts_HR_Employees_RequestedByEmployeeId",
                table: "AM_AccessoryCheckouts",
                column: "RequestedByEmployeeId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_ApproverGroupMembers_HR_Employees_EmployeeId",
                table: "AM_ApproverGroupMembers",
                column: "EmployeeId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_ApproverGroupScopes_HR_Departments_DepartmentId",
                table: "AM_ApproverGroupScopes",
                column: "DepartmentId",
                principalTable: "HR_Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetAssignments_HR_Employees_AssignedByEmployeeId",
                table: "AM_AssetAssignments",
                column: "AssignedByEmployeeId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetAssignments_HR_Employees_AssignedToEmployeeId",
                table: "AM_AssetAssignments",
                column: "AssignedToEmployeeId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetAssignments_HR_Employees_ReturnedByEmployeeId",
                table: "AM_AssetAssignments",
                column: "ReturnedByEmployeeId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetAuditLogs_HR_Employees_PerformedByEmployeeId",
                table: "AM_AssetAuditLogs",
                column: "PerformedByEmployeeId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetMaintenance_HR_Employees_LoggedByEmployeeId",
                table: "AM_AssetMaintenance",
                column: "LoggedByEmployeeId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequestLineItems_HR_Employees_ApprovedByEmployeeId",
                table: "AM_AssetRequestLineItems",
                column: "ApprovedByEmployeeId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequestLineItems_HR_Employees_FulfilledByEmployeeId",
                table: "AM_AssetRequestLineItems",
                column: "FulfilledByEmployeeId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequestLineItems_HR_Employees_SelectedApproverEmploy~",
                table: "AM_AssetRequestLineItems",
                column: "SelectedApproverEmployeeId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequests_HR_Employees_RequestedByEmployeeId",
                table: "AM_AssetRequests",
                column: "RequestedByEmployeeId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequests_HR_Employees_RequestedForEmployeeId",
                table: "AM_AssetRequests",
                column: "RequestedForEmployeeId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_Assets_HR_Departments_DepartmentId",
                table: "AM_Assets",
                column: "DepartmentId",
                principalTable: "HR_Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_Assets_HR_Employees_CreatedByEmployeeId",
                table: "AM_Assets",
                column: "CreatedByEmployeeId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Core_Announcements_HR_Employees_AuthorId",
                table: "Core_Announcements",
                column: "AuthorId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Core_Teams_HR_Departments_DepartmentId",
                table: "Core_Teams",
                column: "DepartmentId",
                principalTable: "HR_Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Core_UserRoles_HR_Departments_DepartmentId",
                table: "Core_UserRoles",
                column: "DepartmentId",
                principalTable: "HR_Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HR_AttendanceLogs_HR_Employees_EmployeeId",
                table: "HR_AttendanceLogs",
                column: "EmployeeId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HR_Departments_Core_Sites_SiteId",
                table: "HR_Departments",
                column: "SiteId",
                principalTable: "Core_Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HR_Departments_HR_Employees_ManagerId",
                table: "HR_Departments",
                column: "ManagerId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_HR_EmployeeOnboardingTasks_HR_Employees_EmployeeId",
                table: "HR_EmployeeOnboardingTasks",
                column: "EmployeeId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HR_EmployeeOnboardingTasks_HR_OnboardingTasks_OnboardingTask~",
                table: "HR_EmployeeOnboardingTasks",
                column: "OnboardingTaskId",
                principalTable: "HR_OnboardingTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HR_LeaveRequests_HR_Employees_ApprovedById",
                table: "HR_LeaveRequests",
                column: "ApprovedById",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HR_LeaveRequests_HR_Employees_EmployeeId",
                table: "HR_LeaveRequests",
                column: "EmployeeId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql("SET FOREIGN_KEY_CHECKS = 1;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AM_Accessories_HR_Departments_DepartmentId",
                table: "AM_Accessories");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AccessoryCheckouts_HR_Employees_FulfilledByEmployeeId",
                table: "AM_AccessoryCheckouts");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AccessoryCheckouts_HR_Employees_RequestedByEmployeeId",
                table: "AM_AccessoryCheckouts");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_ApproverGroupMembers_HR_Employees_EmployeeId",
                table: "AM_ApproverGroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_ApproverGroupScopes_HR_Departments_DepartmentId",
                table: "AM_ApproverGroupScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetAssignments_HR_Employees_AssignedByEmployeeId",
                table: "AM_AssetAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetAssignments_HR_Employees_AssignedToEmployeeId",
                table: "AM_AssetAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetAssignments_HR_Employees_ReturnedByEmployeeId",
                table: "AM_AssetAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetAuditLogs_HR_Employees_PerformedByEmployeeId",
                table: "AM_AssetAuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetMaintenance_HR_Employees_LoggedByEmployeeId",
                table: "AM_AssetMaintenance");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequestLineItems_HR_Employees_ApprovedByEmployeeId",
                table: "AM_AssetRequestLineItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequestLineItems_HR_Employees_FulfilledByEmployeeId",
                table: "AM_AssetRequestLineItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequestLineItems_HR_Employees_SelectedApproverEmploy~",
                table: "AM_AssetRequestLineItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequests_HR_Employees_RequestedByEmployeeId",
                table: "AM_AssetRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_AssetRequests_HR_Employees_RequestedForEmployeeId",
                table: "AM_AssetRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_Assets_HR_Departments_DepartmentId",
                table: "AM_Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_AM_Assets_HR_Employees_CreatedByEmployeeId",
                table: "AM_Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Core_Announcements_HR_Employees_AuthorId",
                table: "Core_Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Core_Teams_HR_Departments_DepartmentId",
                table: "Core_Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Core_UserRoles_HR_Departments_DepartmentId",
                table: "Core_UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_AttendanceLogs_HR_Employees_EmployeeId",
                table: "HR_AttendanceLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_Departments_Core_Sites_SiteId",
                table: "HR_Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_Departments_HR_Employees_ManagerId",
                table: "HR_Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_EmployeeOnboardingTasks_HR_Employees_EmployeeId",
                table: "HR_EmployeeOnboardingTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_EmployeeOnboardingTasks_HR_OnboardingTasks_OnboardingTask~",
                table: "HR_EmployeeOnboardingTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_LeaveRequests_HR_Employees_ApprovedById",
                table: "HR_LeaveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_LeaveRequests_HR_Employees_EmployeeId",
                table: "HR_LeaveRequests");

            migrationBuilder.DropTable(
                name: "HR_Employees");

            migrationBuilder.DropIndex(
                name: "IX_HR_EmployeeOnboardingTasks_OnboardingTaskId",
                table: "HR_EmployeeOnboardingTasks");

            migrationBuilder.DropIndex(
                name: "IX_HR_Departments_SiteId",
                table: "HR_Departments");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "HR_Departments");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "HR_LeaveRequests",
                newName: "EmployeeRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_HR_LeaveRequests_EmployeeId",
                table: "HR_LeaveRequests",
                newName: "IX_HR_LeaveRequests_EmployeeRecordId");

            migrationBuilder.RenameColumn(
                name: "OnboardingTaskId",
                table: "HR_EmployeeOnboardingTasks",
                newName: "Order");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "HR_EmployeeOnboardingTasks",
                newName: "EmployeeRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_HR_EmployeeOnboardingTasks_EmployeeId",
                table: "HR_EmployeeOnboardingTasks",
                newName: "IX_HR_EmployeeOnboardingTasks_EmployeeRecordId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "HR_AttendanceLogs",
                newName: "EmployeeRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_HR_AttendanceLogs_EmployeeId_Date",
                table: "HR_AttendanceLogs",
                newName: "IX_HR_AttendanceLogs_EmployeeRecordId_Date");

            migrationBuilder.AddColumn<string>(
                name: "AssigneeRole",
                table: "HR_EmployeeOnboardingTasks",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "HR_EmployeeOnboardingTasks",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "HR_EmployeeOnboardingTasks",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "HR_Departments",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Core_Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Core_Departments_Core_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Core_Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HR_EmployeeRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    DirectManagerId = table.Column<int>(type: "int", nullable: true),
                    PositionId = table.Column<int>(type: "int", nullable: true),
                    UserAccountId = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EmergencyContactName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmergencyContactPhone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmployeeNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HireDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
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
                name: "Core_Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FullName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Core_Employees_Core_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Core_Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Core_Employees_Core_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Core_Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Core_Employees_Core_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Core_Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Core_Employees_HR_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "HR_Positions",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Core_UserAccounts_EmployeeId",
                table: "Core_UserAccounts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_Departments_SiteId",
                table: "Core_Departments",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_Employees_DepartmentId",
                table: "Core_Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_Employees_PositionId",
                table: "Core_Employees",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_Employees_SiteId",
                table: "Core_Employees",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_Employees_TeamId",
                table: "Core_Employees",
                column: "TeamId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AM_Accessories_Core_Departments_DepartmentId",
                table: "AM_Accessories",
                column: "DepartmentId",
                principalTable: "Core_Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AccessoryCheckouts_Core_Employees_FulfilledByEmployeeId",
                table: "AM_AccessoryCheckouts",
                column: "FulfilledByEmployeeId",
                principalTable: "Core_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AccessoryCheckouts_Core_Employees_RequestedByEmployeeId",
                table: "AM_AccessoryCheckouts",
                column: "RequestedByEmployeeId",
                principalTable: "Core_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_ApproverGroupMembers_Core_Employees_EmployeeId",
                table: "AM_ApproverGroupMembers",
                column: "EmployeeId",
                principalTable: "Core_Employees",
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
                name: "FK_AM_AssetAssignments_Core_Employees_AssignedByEmployeeId",
                table: "AM_AssetAssignments",
                column: "AssignedByEmployeeId",
                principalTable: "Core_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetAssignments_Core_Employees_AssignedToEmployeeId",
                table: "AM_AssetAssignments",
                column: "AssignedToEmployeeId",
                principalTable: "Core_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetAssignments_Core_Employees_ReturnedByEmployeeId",
                table: "AM_AssetAssignments",
                column: "ReturnedByEmployeeId",
                principalTable: "Core_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetAuditLogs_Core_Employees_PerformedByEmployeeId",
                table: "AM_AssetAuditLogs",
                column: "PerformedByEmployeeId",
                principalTable: "Core_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetMaintenance_Core_Employees_LoggedByEmployeeId",
                table: "AM_AssetMaintenance",
                column: "LoggedByEmployeeId",
                principalTable: "Core_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequestLineItems_Core_Employees_ApprovedByEmployeeId",
                table: "AM_AssetRequestLineItems",
                column: "ApprovedByEmployeeId",
                principalTable: "Core_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequestLineItems_Core_Employees_FulfilledByEmployeeId",
                table: "AM_AssetRequestLineItems",
                column: "FulfilledByEmployeeId",
                principalTable: "Core_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequestLineItems_Core_Employees_SelectedApproverEmpl~",
                table: "AM_AssetRequestLineItems",
                column: "SelectedApproverEmployeeId",
                principalTable: "Core_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequests_Core_Employees_RequestedByEmployeeId",
                table: "AM_AssetRequests",
                column: "RequestedByEmployeeId",
                principalTable: "Core_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequests_Core_Employees_RequestedForEmployeeId",
                table: "AM_AssetRequests",
                column: "RequestedForEmployeeId",
                principalTable: "Core_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_Assets_Core_Departments_DepartmentId",
                table: "AM_Assets",
                column: "DepartmentId",
                principalTable: "Core_Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_Assets_Core_Employees_CreatedByEmployeeId",
                table: "AM_Assets",
                column: "CreatedByEmployeeId",
                principalTable: "Core_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Core_Announcements_Core_Employees_AuthorId",
                table: "Core_Announcements",
                column: "AuthorId",
                principalTable: "Core_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Core_Teams_Core_Departments_DepartmentId",
                table: "Core_Teams",
                column: "DepartmentId",
                principalTable: "Core_Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Core_UserAccounts_Core_Employees_EmployeeId",
                table: "Core_UserAccounts",
                column: "EmployeeId",
                principalTable: "Core_Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Core_UserRoles_Core_Departments_DepartmentId",
                table: "Core_UserRoles",
                column: "DepartmentId",
                principalTable: "Core_Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HR_AttendanceLogs_HR_EmployeeRecords_EmployeeRecordId",
                table: "HR_AttendanceLogs",
                column: "EmployeeRecordId",
                principalTable: "HR_EmployeeRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HR_Departments_HR_EmployeeRecords_ManagerId",
                table: "HR_Departments",
                column: "ManagerId",
                principalTable: "HR_EmployeeRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_HR_EmployeeOnboardingTasks_HR_EmployeeRecords_EmployeeRecord~",
                table: "HR_EmployeeOnboardingTasks",
                column: "EmployeeRecordId",
                principalTable: "HR_EmployeeRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HR_LeaveRequests_HR_EmployeeRecords_ApprovedById",
                table: "HR_LeaveRequests",
                column: "ApprovedById",
                principalTable: "HR_EmployeeRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HR_LeaveRequests_HR_EmployeeRecords_EmployeeRecordId",
                table: "HR_LeaveRequests",
                column: "EmployeeRecordId",
                principalTable: "HR_EmployeeRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
