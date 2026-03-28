using System.Security.Claims;
using IntranetPortal.Data.Models;

namespace IntranetPortal.Api.Security
{
    public static class DepartmentScopeExtensions
    {
        /// <summary>
        /// Natively injects departmental hierarchical bounds into raw SQL pipelines mathematically blocking out-of-scope queries.
        /// </summary>
        public static IQueryable<T> ApplyDepartmentScope<T>(
            this IQueryable<T> query, 
            IPermissionService permissionService, 
            string permissionName,
            int? implicitUserDepartmentId = null) where T : IDepartmentScoped
        {
            if (permissionService.IsGlobal(permissionName)) return query;

            var allowedDepartments = permissionService.GetAllowedDepartments(permissionName);
            
            return query.Where(x => x.DepartmentId == null || 
                                    allowedDepartments.Contains(x.DepartmentId.Value) || 
                                    (implicitUserDepartmentId.HasValue && x.DepartmentId == implicitUserDepartmentId.Value));
        }

        /// <summary>
        /// Rigidly tests explicit departmental capabilities to prevent 403 Forbidden cross-tenant bleeding.
        /// </summary>
        public static bool ValidateDepartmentScope(this IPermissionService service, string permissionName, int? targetDepartmentId)
        {
            return targetDepartmentId.HasValue 
                ? service.HasDepartmentPermission(permissionName, targetDepartmentId.Value)
                : service.IsGlobal(permissionName);
        }
    }
}
