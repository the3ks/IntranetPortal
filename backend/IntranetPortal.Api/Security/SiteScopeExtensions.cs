using System.Security.Claims;
using IntranetPortal.Data.Models;

namespace IntranetPortal.Api.Security
{
    public static class SiteScopeExtensions
    {
        /// <summary>
        /// Natively injects physical geographical bounds into raw SQL pipelines mathematically blocking out-of-scope horizontal arrays.
        /// Automatically checks the native token execution context and constructs the explicit SQL parameters implicitly.
        /// </summary>
        public static IQueryable<T> ApplySiteScope<T>(
            this IQueryable<T> query, 
            IPermissionService permissionService, 
            string permissionName,
            int? implicitUserSiteId = null) where T : ISiteScoped
        {
            if (permissionService.IsGlobal(permissionName)) return query;

            var allowedSites = permissionService.GetAllowedSites(permissionName);
            
            return query.Where(x => x.SiteId == null || 
                                    allowedSites.Contains(x.SiteId.Value) || 
                                    (implicitUserSiteId.HasValue && x.SiteId == implicitUserSiteId.Value));
        }

        /// <summary>
        /// Rigidly tests explicit capabilities returning a boolean to physically prevent 403 Forbidden cross-tenant bleeding.
        /// </summary>
        public static bool ValidateSiteScope(this IPermissionService service, string permissionName, int? targetSiteId)
        {
            return targetSiteId.HasValue 
                ? service.HasSitePermission(permissionName, targetSiteId.Value)
                : service.IsGlobal(permissionName);
        }
    }
}
