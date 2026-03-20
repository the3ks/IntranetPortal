using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace IntranetPortal.Api.Security
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // Rapid interception block routing strict "Perm:" prefixes dynamically
            if (policyName.StartsWith("Perm:", StringComparison.OrdinalIgnoreCase))
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new PermissionRequirement(policyName.Substring(5)));
                return Task.FromResult<AuthorizationPolicy?>(policy.Build());
            }

            // Fallback for native constraints (e.g. traditional standard Roles)
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
