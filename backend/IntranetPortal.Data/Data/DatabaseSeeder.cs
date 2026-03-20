using IntranetPortal.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntranetPortal.Data.Data
{
    public static class DatabaseSeeder
    {
        public static async Task InitializeAsync(ApplicationDbContext context, string adminPasswordHash)
        {
            // 1. Foundation: Always synchronize the Core Authorization Dictionary dynamically
            // Every time the API boots, it will scan this explicit list and insert any missing capability strings!
            var corePermissions = new List<Permission>
            {
                new Permission { Name = "System.FullAccess", Description = "God-mode capability across all scopes" },
                new Permission { Name = "System.ManageRoles", Description = "Create and modify Security Matrices" },
                new Permission { Name = "System.ManagePositions", Description = "Create and modify HR Job Titles" },
                new Permission { Name = "HR.Employee.Create", Description = "Onboard new enterprise personnel" },
                new Permission { Name = "HR.Employee.View", Description = "Access personnel records" },
                new Permission { Name = "HR.Employee.Edit", Description = "Modify personnel dossiers" },
                new Permission { Name = "Announcements.View", Description = "Read corporate broadcast messages" },
                new Permission { Name = "Announcements.Create", Description = "Broadcast corporate messages" }
            };
            
            foreach (var perm in corePermissions)
            {
                // Natively protects against duplicates. Missing permissions are appended safely.
                if (!await context.Permissions.AnyAsync(p => p.Name == perm.Name))
                {
                    context.Permissions.Add(perm);
                }
            }
            // Commit any new Developer Constants immediately
            await context.SaveChangesAsync();

            // Security constraint: Do not attempt to reconstruct the Root Administrator or Roles if already seeded
            if (await context.Roles.AnyAsync())
            {
                return; 
            }

            // 2. Blueprint: Seed the Admin Role mapping the Permission Matrix
            var adminRole = new Role { Name = "Admin", Description = "Global Application Administrator" };
            foreach (var perm in corePermissions)
            {
                adminRole.RolePermissions.Add(new RolePermission { Role = adminRole, Permission = perm });
            }
            context.Roles.Add(adminRole);

            // 3. Entity: Seed the initial System Administrator login
            if (!await context.UserAccounts.AnyAsync(u => u.Email == "admin@company.com"))
            {
                var mainAdmin = new UserAccount
                {
                    Email = "admin@company.com",
                    PasswordHash = adminPasswordHash, // The customer should be instructed to change this immediately upon Prod login!
                    IsActive = true,
                    UserRoles = new List<UserRole>
                    {
                        // SiteId = null perfectly grants this user Global Scope authority over the whole application.
                        new UserRole { Role = adminRole, SiteId = null } 
                    }
                };

                context.UserAccounts.Add(mainAdmin);
            }
            await context.SaveChangesAsync();
        }
    }
}
