using IntranetPortal.Data.Data;
using IntranetPortal.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IntranetPortal.Api.Security
{
    public static class PermissionScanner
    {
        public static async Task SyncPoliciesAsync(ApplicationDbContext context, Assembly assembly)
        {
            var controllers = assembly.GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type));
                
            var discoveredPolicies = new HashSet<string>();

            foreach (var controller in controllers)
            {
                var attrs = new List<AuthorizeAttribute>();
                
                // 1. Scrape Class-Level Authorize policies
                attrs.AddRange(controller.GetCustomAttributes(typeof(AuthorizeAttribute), true).Cast<AuthorizeAttribute>());

                // 2. Scrape Method-Level Authorize policies
                foreach (var method in controller.GetMethods())
                {
                    attrs.AddRange(method.GetCustomAttributes(typeof(AuthorizeAttribute), true).Cast<AuthorizeAttribute>());
                }

                foreach (var attr in attrs)
                {
                    if (!string.IsNullOrEmpty(attr.Policy) && attr.Policy.StartsWith("Perm:"))
                    {
                        // Strip the 'Perm:' prefix so the DB strictly stores 'Announcements.Create', 'Structure.Site.Delete', etc.
                        discoveredPolicies.Add(attr.Policy.Substring(5));
                    }
                }
            }

            // 1. Snapshot the actual database tables
            var existingPermissions = await context.Permissions.ToListAsync();

            // 2. Safelist the foundation matrices that Reflection cannot see natively
            var systemProtected = new HashSet<string> { "System.FullAccess", "System.ManageRoles", "System.ManagePositions" };

            // 3. Sweep the database to flag missing strings as obsolete (Soft-Delete)
            foreach (var p in existingPermissions)
            {
                if (systemProtected.Contains(p.Name))
                    continue;

                if (!discoveredPolicies.Contains(p.Name))
                {
                    if (!p.IsObsolete)
                    {
                        p.IsObsolete = true;
                        p.Description = "(OBSOLETE) " + (p.Description ?? "");
                    }
                }
                else
                {
                    if (p.IsObsolete)
                    {
                        p.IsObsolete = false;
                        p.Description = p.Description?.Replace("(OBSOLETE) ", "");
                    }
                }
            }

            // 4. Safely initialize completely brand-new Reflection hits
            foreach (var policy in discoveredPolicies)
            {
                if (!existingPermissions.Any(p => p.Name == policy))
                {
                    context.Permissions.Add(new Permission 
                    { 
                        Name = policy, 
                        Description = $"Auto-discovered dynamic capability map: {policy}",
                        IsObsolete = false
                    });
                }
            }
            
            await context.SaveChangesAsync();
        }
    }
}
