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
        public DbSet<UserAccount> UserAccounts { get; set; }

        // Advanced Scalable Permission Matrix
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RoleDelegation> RoleDelegations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure RoleDelegation relationships to prevent cascade delete issues (multiple paths to UserAccount)
            modelBuilder.Entity<RoleDelegation>()
                .HasOne(rd => rd.SourceUser)
                .WithMany() // Assuming no explicit collection needed on UserAccount for now
                .HasForeignKey(rd => rd.SourceUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RoleDelegation>()
                .HasOne(rd => rd.SubstituteUser)
                .WithMany()
                .HasForeignKey(rd => rd.SubstituteUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RoleDelegation>()
                .HasOne(rd => rd.UserRole)
                .WithMany()
                .HasForeignKey(rd => rd.UserRoleId)
                .OnDelete(DeleteBehavior.Cascade); // If the assigned role gets deleted, delete the delegation
        }
    }
}
