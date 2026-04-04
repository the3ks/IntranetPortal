using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IntranetPortal.Data.Data;
using IntranetPortal.Data.Models.Assets;
using IntranetPortal.Api.Security;

namespace IntranetPortal.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPermissionService _permissionService;

        public AssetsController(ApplicationDbContext context, IPermissionService permissionService)
        {
            _context = context;
            _permissionService = permissionService;
        }

        [HttpGet]
        [Authorize(Policy = "Perm:Assets.View")]
        public async Task<IActionResult> GetAssets()
        {
            var query = _context.Assets
                .Include(a => a.Model)
                .Include(a => a.Assignments.Where(asg => asg.ActualReturnDate == null))
                    .ThenInclude(asg => asg.AssignedToEmployee)
                .Include(a => a.Site)
                .Include(a => a.Department)
                .AsQueryable();

            // Apply Dual-Axis RBAC scoping natively to SQL Provider
            query = query.ApplySiteScope(_permissionService, "Perm:Assets.View");
            query = query.ApplyDepartmentScope(_permissionService, "Perm:Assets.View");

            var assets = await query.OrderByDescending(a => a.CreatedAt).ToListAsync();

            var result = assets.Select(a => new AssetDto
            {
                Id = a.Id,
                AssetTag = a.AssetTag,
                SerialNumber = a.SerialNumber,
                Status = a.Status.ToString(),
                PhysicalLocation = a.PhysicalLocation,
                SiteName = a.Site?.Name,
                DepartmentName = a.Department?.Name,
                ModelName = a.Model?.Name,
                Manufacturer = a.Model?.Manufacturer,
                AssignedToName = a.Assignments.FirstOrDefault()?.AssignedToEmployee?.FullName
            });

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "Perm:Assets.Manage")]
        public async Task<IActionResult> CreateAsset([FromBody] AssetCreateDto dto)
        {
            // RBAC Insert Verification
            if (!_permissionService.ValidateSiteScope("Perm:Assets.Manage", dto.SiteId)) return Forbid();
            if (!_permissionService.ValidateDepartmentScope("Perm:Assets.Manage", dto.DepartmentId)) return Forbid();

            var asset = new Asset
            {
                AssetTag = dto.AssetTag,
                ModelId = dto.ModelId,
                SerialNumber = dto.SerialNumber,
                Status = dto.Status ?? AssetStatus.Available,
                PhysicalLocation = dto.PhysicalLocation,
                SiteId = dto.SiteId,
                DepartmentId = dto.DepartmentId,
                PurchaseDate = dto.PurchaseDate,
                PurchasePrice = dto.PurchasePrice,
                Vendor = dto.Vendor,
                WarrantyExpiration = dto.WarrantyExpiration,
                CreatedByEmployeeId = int.TryParse(User.FindFirst("EmployeeId")?.Value, out var empId) ? empId : null
            };

            _context.Assets.Add(asset);

            // Create initial Audit Log
            _context.AssetAuditLogs.Add(new AssetAuditLog
            {
                Asset = asset,
                Action = "Created",
                NewValue = $"Status: {asset.Status}",
                PerformedByEmployeeId = asset.CreatedByEmployeeId ?? 0 // Note: in real-world, throw if 0
            });

            await _context.SaveChangesAsync();
            return Ok(new { asset.Id });
        }

        [HttpPost("{id}/assign")]
        [Authorize(Policy = "Perm:Assets.Manage")]
        public async Task<IActionResult> AssignAsset(int id, [FromBody] AssetAssignmentCreateDto dto)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null) return NotFound();

            if (!_permissionService.ValidateSiteScope("Perm:Assets.Manage", asset.SiteId)) return Forbid();
            if (!_permissionService.ValidateDepartmentScope("Perm:Assets.Manage", asset.DepartmentId)) return Forbid();

            if (asset.Status == AssetStatus.Assigned || asset.Status == AssetStatus.Deployed)
            {
                return BadRequest("Asset is already deployed or assigned.");
            }

            var adminId = int.Parse(User.FindFirst("EmployeeId")?.Value ?? "0");

            var assignment = new AssetAssignment
            {
                AssetId = asset.Id,
                AssignedToEmployeeId = dto.AssignedToEmployeeId,
                AssignedToTeamId = dto.AssignedToTeamId,
                DateAssigned = DateTime.UtcNow,
                ExpectedReturnDate = dto.ExpectedReturnDate,
                ConditionOnAssign = dto.ConditionOnAssign,
                AssignedByEmployeeId = adminId
            };

            asset.Status = AssetStatus.Assigned;
            _context.AssetAssignments.Add(assignment);

            _context.AssetAuditLogs.Add(new AssetAuditLog
            {
                AssetId = asset.Id,
                Action = "Assigned",
                OldValue = "Available",
                NewValue = $"Assigned to EmployeeId {dto.AssignedToEmployeeId}",
                PerformedByEmployeeId = adminId
            });

            await _context.SaveChangesAsync();
            return Ok(new { assignment.Id });
        }

        [HttpPost("{id}/return")]
        [Authorize(Policy = "Perm:Assets.Manage")]
        public async Task<IActionResult> ReturnAsset(int id, [FromBody] AssetReturnDto dto)
        {
            var asset = await _context.Assets
                .Include(a => a.Assignments.Where(asg => asg.ActualReturnDate == null))
                .FirstOrDefaultAsync(a => a.Id == id);

            if (asset == null) return NotFound();

            if (!_permissionService.ValidateSiteScope("Perm:Assets.Manage", asset.SiteId)) return Forbid();
            if (!_permissionService.ValidateDepartmentScope("Perm:Assets.Manage", asset.DepartmentId)) return Forbid();

            var assignment = asset.Assignments.FirstOrDefault();
            if (assignment == null)
            {
                return BadRequest("Asset is not currently assigned.");
            }

            var adminId = int.Parse(User.FindFirst("EmployeeId")?.Value ?? "0");

            assignment.ActualReturnDate = DateTime.UtcNow;
            assignment.ReturnedByEmployeeId = adminId;
            assignment.ConditionOnReturn = dto.ConditionOnReturn;

            var oldStatus = asset.Status.ToString();
            asset.Status = dto.NewStatus ?? AssetStatus.Available;

            _context.AssetAuditLogs.Add(new AssetAuditLog
            {
                AssetId = asset.Id,
                Action = "Returned",
                OldValue = oldStatus,
                NewValue = $"Status: {asset.Status}. Cond: {dto.ConditionOnReturn}",
                PerformedByEmployeeId = adminId
            });

            await _context.SaveChangesAsync();
            return Ok();
        }
    }

    public class AssetDto
    {
        public int Id { get; set; }
        public string? AssetTag { get; set; }
        public string? SerialNumber { get; set; }
        public string? Status { get; set; }
        public string? PhysicalLocation { get; set; }
        public string? SiteName { get; set; }
        public string? DepartmentName { get; set; }
        public string? ModelName { get; set; }
        public string? Manufacturer { get; set; }
        public string? AssignedToName { get; set; }
    }

    public class AssetCreateDto
    {
        public required string AssetTag { get; set; }
        public int ModelId { get; set; }
        public string? SerialNumber { get; set; }
        public AssetStatus? Status { get; set; }
        public string? PhysicalLocation { get; set; }
        public int? SiteId { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public string? Vendor { get; set; }
        public DateTime? WarrantyExpiration { get; set; }
    }

    public class AssetAssignmentCreateDto
    {
        public int? AssignedToEmployeeId { get; set; }
        public int? AssignedToTeamId { get; set; }
        public DateTime? ExpectedReturnDate { get; set; }
        public string? ConditionOnAssign { get; set; }
    }

    public class AssetReturnDto
    {
        public string? ConditionOnReturn { get; set; }
        public AssetStatus? NewStatus { get; set; }
    }
}
