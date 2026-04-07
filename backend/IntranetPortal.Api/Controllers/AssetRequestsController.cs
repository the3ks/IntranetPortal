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
    public class AssetRequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPermissionService _permissionService;

        public AssetRequestsController(ApplicationDbContext context, IPermissionService permissionService)
        {
            _context = context;
            _permissionService = permissionService;
        }

        [HttpGet("my-requests")]
        public async Task<IActionResult> GetMyRequests()
        {
            int.TryParse(User.FindFirst("EmployeeId")?.Value, out var empId);

            var requests = await _context.AssetRequests
                .Include(r => r.RequestedCategory)
                .Include(r => r.RequestedModel)
                .Include(r => r.RequestedAccessory)
                .Include(r => r.RequestedForEmployee)
                .Where(r => r.RequestedByEmployeeId == empId || r.RequestedForEmployeeId == empId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(requests.Select(MapToDto));
        }

        [HttpGet("pending-approvals")]
        [Authorize(Policy = "Perm:Assets.Approve")]
        public async Task<IActionResult> GetPendingApprovals()
        {
            // A manager can only see requests for employees inside departments they manage
            var allowedDepts = _permissionService.GetAllowedDepartments("Perm:Assets.Approve");
            var isGlobal = _permissionService.IsGlobal("Perm:Assets.Approve");

            var query = _context.AssetRequests
                .Include(r => r.RequestedCategory)
                .Include(r => r.RequestedModel)
                .Include(r => r.RequestedAccessory)
                .Include(r => r.RequestedForEmployee)
                .Where(r => r.Status == RequestStatus.PendingApproval);

            if (!isGlobal)
            {
                query = query.Where(r => r.RequestedForEmployee != null && 
                                         allowedDepts.Contains(r.RequestedForEmployee.DepartmentId));
            }

            var requests = await query.OrderBy(r => r.CreatedAt).ToListAsync();
            return Ok(requests.Select(MapToDto));
        }

        [HttpGet("fulfillment-queue")]
        [Authorize(Policy = "Perm:Assets.Manage")]
        public async Task<IActionResult> GetFulfillmentQueue()
        {
            // IT/Facilities only see items pending fulfillment
            var requests = await _context.AssetRequests
                .Include(r => r.RequestedCategory)
                .Include(r => r.RequestedModel)
                .Include(r => r.RequestedAccessory)
                .Include(r => r.RequestedForEmployee)
                .Where(r => r.Status == RequestStatus.PendingFulfillment || r.Status == RequestStatus.InProcurement)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();

            return Ok(requests.Select(MapToDto));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest([FromBody] AssetRequestCreateDto dto)
        {
            int.TryParse(User.FindFirst("EmployeeId")?.Value, out var empId);

            var request = new AssetRequest
            {
                RequestedByEmployeeId = empId,
                RequestedForEmployeeId = dto.RequestedForEmployeeId ?? empId,
                Type = dto.Type,
                RequestedCategoryId = dto.RequestedCategoryId,
                RequestedModelId = dto.RequestedModelId,
                RequestedAccessoryId = dto.RequestedAccessoryId,
                Quantity = dto.Quantity,
                Justification = dto.Justification,
                Status = RequestStatus.PendingApproval,
                CreatedAt = DateTime.UtcNow
            };

            // Auto-Approval Logic for Consumables
            if (dto.RequestedCategoryId.HasValue)
            {
                var cat = await _context.AssetCategories.FindAsync(dto.RequestedCategoryId.Value);
                if (cat != null && !cat.RequiresApproval)
                {
                    request.Status = RequestStatus.PendingFulfillment;
                }
            }

            _context.AssetRequests.Add(request);
            await _context.SaveChangesAsync();
            return Ok(new { request.Id });
        }

        [HttpPost("{id}/approve")]
        [Authorize(Policy = "Perm:Assets.Approve")]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            var request = await _context.AssetRequests.FindAsync(id);
            if (request == null) return NotFound();

            int.TryParse(User.FindFirst("EmployeeId")?.Value, out var empId);

            request.Status = RequestStatus.PendingFulfillment;
            request.ManagerApprovedByEmployeeId = empId;
            request.ManagerApprovedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("{id}/fulfill")]
        [Authorize(Policy = "Perm:Assets.Manage")]
        public async Task<IActionResult> FulfillRequest(int id, [FromBody] AssetRequestFulfillDto dto)
        {
            var request = await _context.AssetRequests.FindAsync(id);
            if (request == null) return NotFound();

            int.TryParse(User.FindFirst("EmployeeId")?.Value, out var empId);

            request.Status = RequestStatus.Fulfilled;
            request.FulfilledByEmployeeId = empId;
            request.FulfilledAt = DateTime.UtcNow;

            if (dto.AssignedAssetId.HasValue)
            {
                request.AssignedAssetId = dto.AssignedAssetId;
                
                // Automatically assign it in the hard ledger too
                var asset = await _context.Assets.FindAsync(dto.AssignedAssetId.Value);
                if (asset != null)
                {
                    asset.Status = AssetStatus.Assigned;
                    _context.AssetAssignments.Add(new AssetAssignment
                    {
                        AssetId = asset.Id,
                        AssignedToEmployeeId = request.RequestedForEmployeeId,
                        DateAssigned = DateTime.UtcNow,
                        AssignedByEmployeeId = empId
                    });
                }
            }

            if (request.Type == RequestType.BulkAccessory && request.RequestedAccessoryId.HasValue)
            {
                var accessory = await _context.Accessories.FindAsync(request.RequestedAccessoryId.Value);
                if (accessory != null && accessory.AvailableQuantity >= request.Quantity)
                {
                    accessory.AvailableQuantity -= request.Quantity;
                    _context.AccessoryCheckouts.Add(new AccessoryCheckout
                    {
                        AccessoryId = accessory.Id,
                        RequestedByEmployeeId = request.RequestedForEmployeeId,
                        FulfilledByEmployeeId = empId,
                        Quantity = request.Quantity
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        private static AssetRequestDto MapToDto(AssetRequest r)
        {
            return new AssetRequestDto
            {
                Id = r.Id,
                RequestedForName = r.RequestedForEmployee?.FullName,
                Type = r.Type.ToString(),
                CategoryName = r.RequestedCategory?.Name,
                ModelName = r.RequestedModel?.Name,
                AccessoryName = r.RequestedAccessory?.Name,
                Quantity = r.Quantity,
                Justification = r.Justification,
                Status = r.Status.ToString(),
                CreatedAt = r.CreatedAt
            };
        }
    }

    public class AssetRequestDto
    {
        public int Id { get; set; }
        public string? RequestedForName { get; set; }
        public string? Type { get; set; }
        public string? CategoryName { get; set; }
        public string? ModelName { get; set; }
        public string? AccessoryName { get; set; }
        public int Quantity { get; set; }
        public required string Justification { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AssetRequestCreateDto
    {
        public int? RequestedForEmployeeId { get; set; }
        public RequestType Type { get; set; }
        public int? RequestedCategoryId { get; set; }
        public int? RequestedModelId { get; set; }
        public int? RequestedAccessoryId { get; set; }
        public int Quantity { get; set; } = 1;
        public required string Justification { get; set; }
    }

    public class AssetRequestFulfillDto
    {
        public int? AssignedAssetId { get; set; }
    }
}
