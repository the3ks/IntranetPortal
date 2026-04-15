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
                new Permission { Name = "System.ManagePositions", Description = "Create and modify HR Job Titles" }
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
            if (!await context.Roles.AnyAsync())
            {
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
            }
            // 4. Modules: Seed the Core Applications
            var allSites = await context.Sites.ToListAsync();
            
            if (!await context.SystemModules.AnyAsync())
            {
                context.SystemModules.AddRange(
                    new SystemModule { Name = "The Hub", Description = "Browse the corporate directory, internal wikis, documentation, and company announcements.", Url = "/employees", IconSvg = "<svg class=\"w-8 h-8\" fill=\"none\" stroke=\"currentColor\" viewBox=\"0 0 24 24\"><path strokeLinecap=\"round\" strokeLinejoin=\"round\" strokeWidth=\"2\" d=\"M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4\" /></svg>", IsActiveGlobally = true, IsActive = true, AllowedSites = allSites },
                    new SystemModule { Name = "Assets Management", Description = "Submit requisitions and track physical equipment deployments, inventory, and accessories.", Url = "/assets", IconSvg = "<svg class=\"w-8 h-8\" fill=\"none\" stroke=\"currentColor\" viewBox=\"0 0 24 24\"><path strokeLinecap=\"round\" strokeLinejoin=\"round\" strokeWidth=\"2\" d=\"M9.75 17L9 20l-1 1h8l-1-1-.75-3M3 13h18M5 17h14a2 2 0 002-2V5a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z\" /></svg>", IsActiveGlobally = true, IsActive = true, AllowedSites = allSites },
                    new SystemModule { Name = "Administration", Description = "Manage system configuration, personnel roles, permissions, sites, and geographical footprints.", Url = "/admin/quick-setup", IconSvg = "<svg class=\"w-8 h-8\" fill=\"none\" stroke=\"currentColor\" viewBox=\"0 0 24 24\"><path strokeLinecap=\"round\" strokeLinejoin=\"round\" strokeWidth=\"2\" d=\"M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z\" /><path strokeLinecap=\"round\" strokeLinejoin=\"round\" strokeWidth=\"2\" d=\"M15 12a3 3 0 11-6 0 3 3 0 016 0z\" /></svg>", IsActiveGlobally = true, IsActive = true, AllowedSites = allSites },
                    new SystemModule { Name = "Drink Orders", Description = "Browse the pantry menu, request specialized beverages, and manage the office drink queue.", Url = "http://localhost:3001", IconSvg = "<svg class=\"w-8 h-8\" fill=\"none\" stroke=\"currentColor\" viewBox=\"0 0 24 24\"><path strokeLinecap=\"round\" strokeLinejoin=\"round\" strokeWidth=\"2\" d=\"M5 3v4M3 5h4M6 17v4m-2-2h4m5-16l2.286 6.857L21 12l-5.714 2.143L13 21l-2.286-6.857L5 12l5.714-2.143z\" /></svg>", IsActiveGlobally = true, IsActive = true, AllowedSites = allSites }
                );
            }
            else
            {
                // Sync legacy modules missing Site Authorizations
                var orphanedModules = await context.SystemModules.Include(m => m.AllowedSites).Where(m => !m.AllowedSites.Any()).ToListAsync();
                foreach(var mod in orphanedModules)
                {
                    foreach (var site in allSites) mod.AllowedSites.Add(site);
                }
            }

            // 5. Seed Asset Categories and Approver Groups
            if (!await context.AssetCategories.AnyAsync())
            {
                var itGroup = new IntranetPortal.Data.Models.Assets.ApproverGroup { Name = "Global IT Approvers" };
                var opsGroup = new IntranetPortal.Data.Models.Assets.ApproverGroup { Name = "IT & Operations Queue" };
                var hrGroup = new IntranetPortal.Data.Models.Assets.ApproverGroup { Name = "Facilities & HR" };

                context.ApproverGroups.AddRange(itGroup, opsGroup, hrGroup);
                await context.SaveChangesAsync();

                var hwCategory = new IntranetPortal.Data.Models.Assets.AssetCategory
                {
                    Name = "IT Hardware",
                    Description = "High-value, serialized equipment like laptops and servers.",
                    RequiresApproval = true,
                    AllowRequesterToSelectApprover = false,
                    DefaultApproverGroup = itGroup
                };

                var accCategory = new IntranetPortal.Data.Models.Assets.AssetCategory
                {
                    Name = "Accessories & Peripherals",
                    Description = "Low value items like mice.",
                    RequiresApproval = true,
                    AllowRequesterToSelectApprover = false,
                    DefaultApproverGroup = opsGroup
                };

                var statCategory = new IntranetPortal.Data.Models.Assets.AssetCategory
                {
                    Name = "Office Stationaries",
                    Description = "Standard office supplies.",
                    RequiresApproval = true,
                    AllowRequesterToSelectApprover = true
                };

                context.AssetCategories.AddRange(hwCategory, accCategory, statCategory);
                await context.SaveChangesAsync();
                
                context.AssetCategoryApproverGroups.AddRange(
                    new IntranetPortal.Data.Models.Assets.AssetCategoryApproverGroup { AssetCategoryId = hwCategory.Id, ApproverGroupId = itGroup.Id },
                    new IntranetPortal.Data.Models.Assets.AssetCategoryApproverGroup { AssetCategoryId = accCategory.Id, ApproverGroupId = opsGroup.Id },
                    new IntranetPortal.Data.Models.Assets.AssetCategoryApproverGroup { AssetCategoryId = statCategory.Id, ApproverGroupId = hrGroup.Id }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}
