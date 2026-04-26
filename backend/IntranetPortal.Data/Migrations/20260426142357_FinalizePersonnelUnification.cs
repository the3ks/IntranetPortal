using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntranetPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class FinalizePersonnelUnification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AM_ApproverGroups",
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
                    table.PrimaryKey("PK_AM_ApproverGroups", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Core_AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IPAddress = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Action = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Timestamp = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UserAgent = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_AuditLogs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Core_Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsObsolete = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_Permissions", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Core_Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_Roles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Core_Sites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_Sites", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Core_SystemModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IconSvg = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActiveGlobally = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_SystemModules", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                name: "HR_Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Positions", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowRequesterToSelectApprover = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DefaultApproverGroupId = table.Column<int>(type: "int", nullable: true),
                    FulfillmentGroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AM_AssetCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AM_AssetCategories_AM_ApproverGroups_DefaultApproverGroupId",
                        column: x => x.DefaultApproverGroupId,
                        principalTable: "AM_ApproverGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AM_AssetCategories_AM_ApproverGroups_FulfillmentGroupId",
                        column: x => x.FulfillmentGroupId,
                        principalTable: "AM_ApproverGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AM_AssetCategories_AM_AssetCategories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "AM_AssetCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Core_RolePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Core_RolePermissions_Core_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Core_Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Core_RolePermissions_Core_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Core_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Core_ModuleSites",
                columns: table => new
                {
                    AllowedModulesId = table.Column<int>(type: "int", nullable: false),
                    AllowedSitesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_ModuleSites", x => new { x.AllowedModulesId, x.AllowedSitesId });
                    table.ForeignKey(
                        name: "FK_Core_ModuleSites_Core_Sites_AllowedSitesId",
                        column: x => x.AllowedSitesId,
                        principalTable: "Core_Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Core_ModuleSites_Core_SystemModules_AllowedModulesId",
                        column: x => x.AllowedModulesId,
                        principalTable: "Core_SystemModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "AM_AssetCategoryApproverGroups",
                columns: table => new
                {
                    AssetCategoryId = table.Column<int>(type: "int", nullable: false),
                    ApproverGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AM_AssetCategoryApproverGroups", x => new { x.AssetCategoryId, x.ApproverGroupId });
                    table.ForeignKey(
                        name: "FK_AM_AssetCategoryApproverGroups_AM_ApproverGroups_ApproverGro~",
                        column: x => x.ApproverGroupId,
                        principalTable: "AM_ApproverGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AM_AssetCategoryApproverGroups_AM_AssetCategories_AssetCateg~",
                        column: x => x.AssetCategoryId,
                        principalTable: "AM_AssetCategories",
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
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
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
                        name: "FK_AM_Accessories_Core_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Core_Sites",
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AM_ApproverGroupMembers",
                columns: table => new
                {
                    ApproverGroupId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AM_ApproverGroupMembers", x => new { x.ApproverGroupId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_AM_ApproverGroupMembers_AM_ApproverGroups_ApproverGroupId",
                        column: x => x.ApproverGroupId,
                        principalTable: "AM_ApproverGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AM_ApproverGroupScopes",
                columns: table => new
                {
                    ApproverGroupId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AM_ApproverGroupScopes", x => new { x.ApproverGroupId, x.DepartmentId });
                    table.ForeignKey(
                        name: "FK_AM_ApproverGroupScopes_AM_ApproverGroups_ApproverGroupId",
                        column: x => x.ApproverGroupId,
                        principalTable: "AM_ApproverGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                        name: "FK_AM_AssetRequestLineItems_AM_ApproverGroups_AssignedApproverG~",
                        column: x => x.AssignedApproverGroupId,
                        principalTable: "AM_ApproverGroups",
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
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AM_AssetRequests", x => x.Id);
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
                        name: "FK_AM_Assets_Core_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Core_Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Core_Announcements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_Announcements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Core_Announcements_Core_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Core_Sites",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Core_LoginChallenges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ChallengeId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nonce = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    ConsumedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    UserAccountId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_LoginChallenges", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Core_RoleDelegations",
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
                    table.PrimaryKey("PK_Core_RoleDelegations", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Core_Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_Teams", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Core_UserAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SecurityStamp = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    FailedLoginAttempts = table.Column<int>(type: "int", nullable: false),
                    LockedUntil = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    RefreshToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefreshTokenExpiryTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_UserAccounts", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Core_UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserAccountId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Core_UserRoles_Core_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Core_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Core_UserRoles_Core_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Core_Sites",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Core_UserRoles_Core_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "Core_UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HR_AttendanceLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ClockInTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ClockOutTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_AttendanceLogs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HR_Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentDepartmentId = table.Column<int>(type: "int", nullable: true),
                    ManagerId = table.Column<int>(type: "int", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_Departments_Core_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Core_Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HR_Departments_HR_Departments_ParentDepartmentId",
                        column: x => x.ParentDepartmentId,
                        principalTable: "HR_Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HR_Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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

            migrationBuilder.CreateTable(
                name: "HR_EmployeeOnboardingTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    OnboardingTaskId = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_EmployeeOnboardingTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_EmployeeOnboardingTasks_HR_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "HR_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HR_EmployeeOnboardingTasks_HR_OnboardingTasks_OnboardingTask~",
                        column: x => x.OnboardingTaskId,
                        principalTable: "HR_OnboardingTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HR_LeaveRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_HR_LeaveRequests_HR_Employees_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "HR_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HR_LeaveRequests_HR_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "HR_Employees",
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
                name: "IX_AM_ApproverGroupMembers_EmployeeId",
                table: "AM_ApproverGroupMembers",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_ApproverGroupScopes_DepartmentId",
                table: "AM_ApproverGroupScopes",
                column: "DepartmentId");

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
                name: "IX_AM_AssetCategories_DefaultApproverGroupId",
                table: "AM_AssetCategories",
                column: "DefaultApproverGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetCategories_FulfillmentGroupId",
                table: "AM_AssetCategories",
                column: "FulfillmentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetCategories_ParentCategoryId",
                table: "AM_AssetCategories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetCategoryApproverGroups_ApproverGroupId",
                table: "AM_AssetCategoryApproverGroups",
                column: "ApproverGroupId");

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

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequests_RequestedByEmployeeId",
                table: "AM_AssetRequests",
                column: "RequestedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AM_AssetRequests_RequestedForEmployeeId",
                table: "AM_AssetRequests",
                column: "RequestedForEmployeeId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Core_Announcements_AuthorId",
                table: "Core_Announcements",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_Announcements_SiteId",
                table: "Core_Announcements",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_LoginChallenges_ChallengeId",
                table: "Core_LoginChallenges",
                column: "ChallengeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Core_LoginChallenges_ExpiresAt",
                table: "Core_LoginChallenges",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_Core_LoginChallenges_NormalizedEmail_CreatedAt",
                table: "Core_LoginChallenges",
                columns: new[] { "NormalizedEmail", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Core_LoginChallenges_UserAccountId",
                table: "Core_LoginChallenges",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_ModuleSites_AllowedSitesId",
                table: "Core_ModuleSites",
                column: "AllowedSitesId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_RoleDelegations_SourceUserId",
                table: "Core_RoleDelegations",
                column: "SourceUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_RoleDelegations_SubstituteUserId",
                table: "Core_RoleDelegations",
                column: "SubstituteUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_RoleDelegations_UserRoleId",
                table: "Core_RoleDelegations",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_RolePermissions_PermissionId",
                table: "Core_RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_RolePermissions_RoleId",
                table: "Core_RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_Teams_DepartmentId",
                table: "Core_Teams",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_UserAccounts_Email",
                table: "Core_UserAccounts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Core_UserAccounts_EmployeeId",
                table: "Core_UserAccounts",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Core_UserRoles_DepartmentId",
                table: "Core_UserRoles",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_UserRoles_RoleId",
                table: "Core_UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_UserRoles_SiteId",
                table: "Core_UserRoles",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_UserRoles_UserAccountId",
                table: "Core_UserRoles",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_AttendanceLogs_EmployeeId_Date",
                table: "HR_AttendanceLogs",
                columns: new[] { "EmployeeId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HR_Departments_ManagerId",
                table: "HR_Departments",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Departments_ParentDepartmentId",
                table: "HR_Departments",
                column: "ParentDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Departments_SiteId",
                table: "HR_Departments",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeOnboardingTasks_EmployeeId",
                table: "HR_EmployeeOnboardingTasks",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeOnboardingTasks_OnboardingTaskId",
                table: "HR_EmployeeOnboardingTasks",
                column: "OnboardingTaskId");

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
                name: "IX_HR_LeaveRequests_ApprovedById",
                table: "HR_LeaveRequests",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_HR_LeaveRequests_EmployeeId",
                table: "HR_LeaveRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_LeaveRequests_LeaveTypeId",
                table: "HR_LeaveRequests",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_OnboardingTasks_TemplateId",
                table: "HR_OnboardingTasks",
                column: "TemplateId");

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
                name: "FK_AM_AssetAssignments_AM_Assets_AssetId",
                table: "AM_AssetAssignments",
                column: "AssetId",
                principalTable: "AM_Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetAssignments_Core_Teams_AssignedToTeamId",
                table: "AM_AssetAssignments",
                column: "AssignedToTeamId",
                principalTable: "Core_Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_AM_AssetAuditLogs_AM_Assets_AssetId",
                table: "AM_AssetAuditLogs",
                column: "AssetId",
                principalTable: "AM_Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetAuditLogs_HR_Employees_PerformedByEmployeeId",
                table: "AM_AssetAuditLogs",
                column: "PerformedByEmployeeId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetMaintenance_AM_Assets_AssetId",
                table: "AM_AssetMaintenance",
                column: "AssetId",
                principalTable: "AM_Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetMaintenance_HR_Employees_LoggedByEmployeeId",
                table: "AM_AssetMaintenance",
                column: "LoggedByEmployeeId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequestLineItems_AM_AssetRequests_AssetRequestId",
                table: "AM_AssetRequestLineItems",
                column: "AssetRequestId",
                principalTable: "AM_AssetRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AM_AssetRequestLineItems_AM_Assets_AssignedAssetId",
                table: "AM_AssetRequestLineItems",
                column: "AssignedAssetId",
                principalTable: "AM_Assets",
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
                name: "FK_Core_LoginChallenges_Core_UserAccounts_UserAccountId",
                table: "Core_LoginChallenges",
                column: "UserAccountId",
                principalTable: "Core_UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Core_RoleDelegations_Core_UserAccounts_SourceUserId",
                table: "Core_RoleDelegations",
                column: "SourceUserId",
                principalTable: "Core_UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Core_RoleDelegations_Core_UserAccounts_SubstituteUserId",
                table: "Core_RoleDelegations",
                column: "SubstituteUserId",
                principalTable: "Core_UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Core_RoleDelegations_Core_UserRoles_UserRoleId",
                table: "Core_RoleDelegations",
                column: "UserRoleId",
                principalTable: "Core_UserRoles",
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
                name: "FK_Core_UserAccounts_HR_Employees_EmployeeId",
                table: "Core_UserAccounts",
                column: "EmployeeId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_HR_Departments_HR_Employees_ManagerId",
                table: "HR_Departments",
                column: "ManagerId",
                principalTable: "HR_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HR_Departments_Core_Sites_SiteId",
                table: "HR_Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_Employees_Core_Sites_SiteId",
                table: "HR_Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Core_Teams_HR_Departments_DepartmentId",
                table: "Core_Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_HR_Employees_HR_Departments_DepartmentId",
                table: "HR_Employees");

            migrationBuilder.DropTable(
                name: "AM_AccessoryCheckouts");

            migrationBuilder.DropTable(
                name: "AM_ApproverGroupMembers");

            migrationBuilder.DropTable(
                name: "AM_ApproverGroupScopes");

            migrationBuilder.DropTable(
                name: "AM_AssetAssignments");

            migrationBuilder.DropTable(
                name: "AM_AssetAuditLogs");

            migrationBuilder.DropTable(
                name: "AM_AssetCategoryApproverGroups");

            migrationBuilder.DropTable(
                name: "AM_AssetMaintenance");

            migrationBuilder.DropTable(
                name: "AM_AssetRequestLineItems");

            migrationBuilder.DropTable(
                name: "Core_Announcements");

            migrationBuilder.DropTable(
                name: "Core_AuditLogs");

            migrationBuilder.DropTable(
                name: "Core_LoginChallenges");

            migrationBuilder.DropTable(
                name: "Core_ModuleSites");

            migrationBuilder.DropTable(
                name: "Core_RoleDelegations");

            migrationBuilder.DropTable(
                name: "Core_RolePermissions");

            migrationBuilder.DropTable(
                name: "HR_AttendanceLogs");

            migrationBuilder.DropTable(
                name: "HR_EmployeeOnboardingTasks");

            migrationBuilder.DropTable(
                name: "HR_LeaveRequests");

            migrationBuilder.DropTable(
                name: "AM_Accessories");

            migrationBuilder.DropTable(
                name: "AM_AssetRequests");

            migrationBuilder.DropTable(
                name: "AM_Assets");

            migrationBuilder.DropTable(
                name: "Core_SystemModules");

            migrationBuilder.DropTable(
                name: "Core_UserRoles");

            migrationBuilder.DropTable(
                name: "Core_Permissions");

            migrationBuilder.DropTable(
                name: "HR_OnboardingTasks");

            migrationBuilder.DropTable(
                name: "HR_LeaveTypes");

            migrationBuilder.DropTable(
                name: "AM_AssetModels");

            migrationBuilder.DropTable(
                name: "Core_Roles");

            migrationBuilder.DropTable(
                name: "Core_UserAccounts");

            migrationBuilder.DropTable(
                name: "HR_OnboardingTemplates");

            migrationBuilder.DropTable(
                name: "AM_AssetCategories");

            migrationBuilder.DropTable(
                name: "AM_ApproverGroups");

            migrationBuilder.DropTable(
                name: "Core_Sites");

            migrationBuilder.DropTable(
                name: "HR_Departments");

            migrationBuilder.DropTable(
                name: "HR_Employees");

            migrationBuilder.DropTable(
                name: "Core_Teams");

            migrationBuilder.DropTable(
                name: "HR_Positions");
        }
    }
}
