using Microsoft.AspNetCore.Http;
using System.Linq;

namespace IntranetPortal.Api.Security
{
    public interface IPermissionService
    {
        bool HasSitePermission(string permission, int targetSiteId);
        List<int> GetAllowedSites(string permission);
        bool IsGlobal(string permission);
    }

    public class PermissionService : IPermissionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool HasSitePermission(string permission, int targetSiteId)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null) return false;

            // System Admin override natively protects core machine owners
            if (user.HasClaim("Permission", "System.FullAccess")) return true;

            // Check if they possess Global access spanning all instances
            if (user.HasClaim("ScopedPerm", $"{permission}:Global")) return true;

            // Check if they possess specific localized Site constraint bounds
            return user.HasClaim("ScopedPerm", $"{permission}:{targetSiteId}");
        }

        public bool IsGlobal(string permission)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null) return false;

            if (user.HasClaim("Permission", "System.FullAccess")) return true;
            return user.HasClaim("ScopedPerm", $"{permission}:Global");
        }

        public List<int> GetAllowedSites(string permission)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null) return new List<int>();

            var claims = user.Claims.Where(c => c.Type == "ScopedPerm" && c.Value.StartsWith($"{permission}:"));
            var sites = new List<int>();

            foreach (var claim in claims)
            {
                var parts = claim.Value.Split(':');
                if (parts.Length == 2 && int.TryParse(parts[1], out int sId))
                {
                    sites.Add(sId);
                }
            }
            return sites;
        }
    }
}
