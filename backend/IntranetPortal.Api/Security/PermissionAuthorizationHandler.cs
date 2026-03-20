using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using IntranetPortal.Data.Data;

namespace IntranetPortal.Api.Security
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }

    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceProvider _serviceProvider;

        public PermissionAuthorizationHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null || context.User.Identity == null || !context.User.Identity.IsAuthenticated)
                return;

            var userIdString = context.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return;

            // Resolve DbContext dynamically using a local scope natively protecting the Engine loop
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Super Admin Bypass Guard
            var isMachineAdmin = await dbContext.UserAccounts
                .Where(u => u.Id == userId)
                .SelectMany(u => u.UserRoles)
                .SelectMany(ur => ur.Role.RolePermissions)
                .AnyAsync(rp => rp.Permission.Name == "System.FullAccess");

            if (isMachineAdmin)
            {
                context.Succeed(requirement);
                return;
            }

            // Strict Matrix Dependency Engine
            var hasAccess = await dbContext.UserAccounts
                .Where(u => u.Id == userId)
                .SelectMany(u => u.UserRoles)
                .SelectMany(ur => ur.Role.RolePermissions)
                .AnyAsync(rp => rp.Permission.Name == requirement.Permission);

            if (hasAccess)
            {
                context.Succeed(requirement);
            }
        }
    }
}
