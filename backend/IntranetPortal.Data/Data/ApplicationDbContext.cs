using IntranetPortal.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace IntranetPortal.Data.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<SystemModule> SystemModules { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }

        // Advanced Scalable Permission Matrix
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RoleDelegation> RoleDelegations { get; set; }

        // Assets Management Module
        public DbSet<IntranetPortal.Data.Models.Assets.AssetCategory> AssetCategories { get; set; }
        public DbSet<IntranetPortal.Data.Models.Assets.AssetModel> AssetModels { get; set; }
        public DbSet<IntranetPortal.Data.Models.Assets.Asset> Assets { get; set; }
        public DbSet<IntranetPortal.Data.Models.Assets.AssetAssignment> AssetAssignments { get; set; }
        public DbSet<IntranetPortal.Data.Models.Assets.AssetMaintenance> AssetMaintenanceRecords { get; set; }
        public DbSet<IntranetPortal.Data.Models.Assets.AssetAuditLog> AssetAuditLogs { get; set; }
        public DbSet<IntranetPortal.Data.Models.Assets.Accessory> Accessories { get; set; }
        public DbSet<IntranetPortal.Data.Models.Assets.AccessoryCheckout> AccessoryCheckouts { get; set; }
        public DbSet<IntranetPortal.Data.Models.Assets.AssetRequest> AssetRequests { get; set; }
        public DbSet<IntranetPortal.Data.Models.Assets.AssetRequestLineItem> AssetRequestLineItems { get; set; }
        public DbSet<IntranetPortal.Data.Models.Assets.ApproverGroup> ApproverGroups { get; set; }
        public DbSet<IntranetPortal.Data.Models.Assets.ApproverGroupMember> ApproverGroupMembers { get; set; }
        public DbSet<IntranetPortal.Data.Models.Assets.ApproverGroupScope> ApproverGroupScopes { get; set; }
        public DbSet<IntranetPortal.Data.Models.Assets.AssetCategoryApproverGroup> AssetCategoryApproverGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Automatically apply all entity type configurations (e.g., from Configurations/Core folder)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
