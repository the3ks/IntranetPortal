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

            var lineItems = await _context.AssetRequestLineItems
                .Include(li => li.AssetRequest)
                .Include(li => li.AssetRequest!.RequestedForEmployee)
                .Include(li => li.RequestedCategory)
                .Include(li => li.RequestedModel)
                .Include(li => li.RequestedAccessory)
                .Where(li => li.AssetRequest!.RequestedByEmployeeId == empId || li.AssetRequest.RequestedForEmployeeId == empId)
                .OrderByDescending(li => li.AssetRequest!.CreatedAt)
                .ToListAsync();

            return Ok(lineItems.Select(MapToDto));
        }

        [HttpGet("approvers")]
        public async Task<IActionResult> GetApprovers([FromQuery] int categoryId, [FromQuery] int departmentId)
        {
            var category = await _context.AssetCategories
                .Include(c => c.ApproverGroups)
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null || !category.AllowRequesterToSelectApprover)
                return Ok(new List<object>());

            var groupIds = category.ApproverGroups.Select(ag => ag.ApproverGroupId).ToList();

            var validGroups = await _context.ApproverGroupScopes
                .Where(s => s.DepartmentId == departmentId && groupIds.Contains(s.ApproverGroupId))
                .Select(s => s.ApproverGroupId)
                .ToListAsync();

            if (!validGroups.Any())
            {
                var scopedGroups = await _context.ApproverGroupScopes.Select(s => s.ApproverGroupId).Distinct().ToListAsync();
                validGroups = groupIds.Except(scopedGroups).ToList();
            }

            var approvers = await _context.ApproverGroupMembers
                .Include(m => m.Employee)
                .Where(m => validGroups.Contains(m.ApproverGroupId))
                .Select(m => new {
                    m.EmployeeId,
                    m.Employee!.FullName,
                    m.Employee.Email
                })
                .Distinct()
                .ToListAsync();

            return Ok(approvers);
        }

        [HttpGet("pending-approvals")]
        public async Task<IActionResult> GetPendingApprovals()
        {
            int.TryParse(User.FindFirst("EmployeeId")?.Value, out var empId);
            var isGlobal = _permissionService.IsGlobal("Perm:Assets.Approve");

            var userGroupIds = await _context.ApproverGroupMembers
                .Where(m => m.EmployeeId == empId)
                .Select(m => m.ApproverGroupId)
                .ToListAsync();

            var query = _context.AssetRequestLineItems
                .Include(li => li.AssetRequest)
                .Include(li => li.AssetRequest!.RequestedForEmployee)
                .Include(li => li.RequestedCategory)
                .Include(li => li.RequestedModel)
                .Include(li => li.RequestedAccessory)
                .Where(li => li.Status == RequestStatus.PendingApproval);

            if (!isGlobal)
            {
                query = query.Where(li => li.SelectedApproverEmployeeId == empId || 
                                        (li.AssignedApproverGroupId.HasValue && userGroupIds.Contains(li.AssignedApproverGroupId.Value)));
            }

            var lineItems = await query.OrderBy(li => li.AssetRequest!.CreatedAt).ToListAsync();
            return Ok(lineItems.Select(MapToDto));
        }

        [HttpGet("is-manager")]
        public async Task<IActionResult> IsManager()
        {
            var isGlobal = _permissionService.IsGlobal("Perm:Assets.Manage");
            if (isGlobal) return Ok(true);

            int.TryParse(User.FindFirst("EmployeeId")?.Value, out var empId);
            var userGroupIds = await _context.ApproverGroupMembers.Where(m => m.EmployeeId == empId).Select(m => m.ApproverGroupId).ToListAsync();
            
            var hasMappedCategory = await _context.AssetCategories
                .AnyAsync(c => (c.FulfillmentGroupId.HasValue && userGroupIds.Contains(c.FulfillmentGroupId.Value)) ||
                               (c.ParentCategory != null && c.ParentCategory.FulfillmentGroupId.HasValue && userGroupIds.Contains(c.ParentCategory.FulfillmentGroupId.Value)));
            
            return Ok(hasMappedCategory);
        }

        [HttpGet("is-approver")]
        public async Task<IActionResult> IsApprover()
        {
            var isGlobal = _permissionService.IsGlobal("Perm:Assets.Manage");
            if (isGlobal) return Ok(true);

            int.TryParse(User.FindFirst("EmployeeId")?.Value, out var empId);
            var isApprover = await _context.ApproverGroupMembers.AnyAsync(m => m.EmployeeId == empId);
            
            return Ok(isApprover);
        }

        [HttpGet("fulfillment-queue")]
        public async Task<IActionResult> GetFulfillmentQueue()
        {
            int.TryParse(User.FindFirst("EmployeeId")?.Value, out var empId);
            var isGlobal = _permissionService.IsGlobal("Perm:Assets.Manage");

            var userGroupIds = await _context.ApproverGroupMembers
                .Where(m => m.EmployeeId == empId)
                .Select(m => m.ApproverGroupId)
                .ToListAsync();

            var query = _context.AssetRequestLineItems
                .Include(li => li.AssetRequest)
                .Include(li => li.AssetRequest!.RequestedForEmployee)
                .Include(li => li.RequestedCategory)
                .Include(li => li.RequestedModel)
                .Include(li => li.RequestedAccessory)
                .Where(li => li.Status == RequestStatus.PendingFulfillment || li.Status == RequestStatus.InProcurement);

            if (!isGlobal)
            {
                query = query.Where(li => li.RequestedCategory != null && 
                                          ((li.RequestedCategory.FulfillmentGroupId.HasValue && userGroupIds.Contains(li.RequestedCategory.FulfillmentGroupId.Value)) ||
                                           (li.RequestedCategory.ParentCategory != null && li.RequestedCategory.ParentCategory.FulfillmentGroupId.HasValue && userGroupIds.Contains(li.RequestedCategory.ParentCategory.FulfillmentGroupId.Value))));
            }

            var lineItems = await query.OrderBy(li => li.AssetRequest!.CreatedAt).ToListAsync();
            return Ok(lineItems.Select(MapToDto));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest([FromBody] AssetRequestCreateDto dto)
        {
            int.TryParse(User.FindFirst("EmployeeId")?.Value, out var empId);

            var request = new AssetRequest
            {
                RequestedByEmployeeId = empId,
                RequestedForEmployeeId = dto.RequestedForEmployeeId ?? empId,
                Status = RequestStatus.PendingApproval,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var item in dto.Items)
            {
                var lineItem = new AssetRequestLineItem
                {
                    Type = item.Type,
                    RequestedCategoryId = item.RequestedCategoryId,
                    RequestedModelId = item.RequestedModelId,
                    RequestedAccessoryId = item.RequestedAccessoryId,
                    Quantity = item.Quantity,
                    Justification = item.Justification,
                    Status = RequestStatus.PendingApproval
                };

                if (item.RequestedCategoryId.HasValue)
                {
                    var cat = await _context.AssetCategories
                        .Include(c => c.ApproverGroups)
                        .FirstOrDefaultAsync(c => c.Id == item.RequestedCategoryId.Value);
                    if (cat != null)
                    {
                        if (!cat.RequiresApproval)
                        {
                            lineItem.Status = RequestStatus.PendingFulfillment;
                        }
                        else
                        {
                            if (cat.AllowRequesterToSelectApprover && item.SelectedApproverEmployeeId.HasValue)
                            {
                                lineItem.SelectedApproverEmployeeId = item.SelectedApproverEmployeeId;
                            }
                            else
                            {
                                var requester = await _context.Employees.FindAsync(request.RequestedForEmployeeId);
                                if (requester != null)
                                {
                                    var scopedGroup = await _context.ApproverGroupScopes
                                        .Where(s => s.DepartmentId == requester.DepartmentId && 
                                                    cat.ApproverGroups.Select(ag => ag.ApproverGroupId).Contains(s.ApproverGroupId))
                                        .Select(s => s.ApproverGroupId)
                                        .FirstOrDefaultAsync();

                                    if (scopedGroup > 0)
                                        lineItem.AssignedApproverGroupId = scopedGroup;
                                    else
                                        lineItem.AssignedApproverGroupId = cat.DefaultApproverGroupId;
                                }
                            }
                        }
                    }
                }
                request.LineItems.Add(lineItem);
            }

            _context.AssetRequests.Add(request);
            await _context.SaveChangesAsync();
            return Ok(new { request.Id });
        }

        [HttpPost("line-items/{id}/approve")]
        public async Task<IActionResult> ApproveLineItem(int id)
        {
            var lineItem = await _context.AssetRequestLineItems.FindAsync(id);
            if (lineItem == null) return NotFound();

            int.TryParse(User.FindFirst("EmployeeId")?.Value, out var empId);

            lineItem.Status = RequestStatus.PendingFulfillment;
            lineItem.ApprovedByEmployeeId = empId;
            lineItem.ApprovedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("line-items/{id}/fulfill")]
        public async Task<IActionResult> FulfillLineItem(int id, [FromBody] AssetRequestFulfillDto dto)
        {
            var lineItem = await _context.AssetRequestLineItems
                .Include(li => li.AssetRequest)
                .Include(li => li.RequestedCategory).ThenInclude(c => c.ParentCategory)
                .FirstOrDefaultAsync(li => li.Id == id);
            
            if (lineItem == null) return NotFound();

            int.TryParse(User.FindFirst("EmployeeId")?.Value, out var empId);
            var isGlobal = _permissionService.IsGlobal("Perm:Assets.Manage");

            if (!isGlobal)
            {
                var userGroupIds = await _context.ApproverGroupMembers.Where(m => m.EmployeeId == empId).Select(m => m.ApproverGroupId).ToListAsync();
                if (lineItem.RequestedCategory == null) return Forbid();
                
                bool hasAccess = (lineItem.RequestedCategory.FulfillmentGroupId.HasValue && userGroupIds.Contains(lineItem.RequestedCategory.FulfillmentGroupId.Value)) ||
                                 (lineItem.RequestedCategory.ParentCategory != null && lineItem.RequestedCategory.ParentCategory.FulfillmentGroupId.HasValue && userGroupIds.Contains(lineItem.RequestedCategory.ParentCategory.FulfillmentGroupId.Value));
                
                if (!hasAccess) return Forbid();
            }

            lineItem.Status = RequestStatus.Fulfilled;
            lineItem.FulfilledByEmployeeId = empId;
            lineItem.FulfilledAt = DateTime.UtcNow;

            if (dto.AssignedAssetId.HasValue)
            {
                lineItem.AssignedAssetId = dto.AssignedAssetId;
                
                var asset = await _context.Assets.FindAsync(dto.AssignedAssetId.Value);
                if (asset != null)
                {
                    asset.Status = AssetStatus.Assigned;
                    _context.AssetAssignments.Add(new AssetAssignment
                    {
                        AssetId = asset.Id,
                        AssignedToEmployeeId = lineItem.AssetRequest!.RequestedForEmployeeId,
                        DateAssigned = DateTime.UtcNow,
                        AssignedByEmployeeId = empId
                    });
                }
            }

            if (lineItem.Type == RequestType.BulkAccessory && lineItem.RequestedAccessoryId.HasValue)
            {
                var accessory = await _context.Accessories.FindAsync(lineItem.RequestedAccessoryId.Value);
                if (accessory != null && accessory.AvailableQuantity >= lineItem.Quantity)
                {
                    accessory.AvailableQuantity -= lineItem.Quantity;
                    _context.AccessoryCheckouts.Add(new AccessoryCheckout
                    {
                        AccessoryId = accessory.Id,
                        RequestedByEmployeeId = lineItem.AssetRequest!.RequestedForEmployeeId,
                        FulfilledByEmployeeId = empId,
                        Quantity = lineItem.Quantity
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        private static AssetRequestDto MapToDto(AssetRequestLineItem li)
        {
            return new AssetRequestDto
            {
                Id = li.Id,
                RequestedForName = li.AssetRequest?.RequestedForEmployee?.FullName,
                Type = li.Type.ToString(),
                CategoryName = li.RequestedCategory?.Name,
                ModelName = li.RequestedModel?.Name,
                AccessoryName = li.RequestedAccessory?.Name,
                Quantity = li.Quantity,
                Justification = li.Justification,
                Status = li.Status.ToString(),
                CreatedAt = li.AssetRequest?.CreatedAt ?? DateTime.UtcNow
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
        public List<AssetRequestLineItemCreateDto> Items { get; set; } = new();
    }

    public class AssetRequestLineItemCreateDto
    {
        public RequestType Type { get; set; }
        public int? RequestedCategoryId { get; set; }
        public int? RequestedModelId { get; set; }
        public int? RequestedAccessoryId { get; set; }
        public int Quantity { get; set; } = 1;
        public required string Justification { get; set; }
        public int? SelectedApproverEmployeeId { get; set; }
    }

    public class AssetRequestFulfillDto
    {
        public int? AssignedAssetId { get; set; }
    }
}
