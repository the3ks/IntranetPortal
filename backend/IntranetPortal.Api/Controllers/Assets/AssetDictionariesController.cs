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
    public class AssetDictionariesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AssetDictionariesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- APPROVER GROUPS ---
        [HttpGet("groups")]
        [Authorize(Policy = "Perm:Assets.Dictionaries.Manage")]
        public async Task<IActionResult> GetGroups()
        {
            var groups = await _context.ApproverGroups
                .Select(g => new
                {
                    g.Id,
                    g.Name,
                    g.IsActive
                })
                .OrderBy(g => g.Name)
                .ToListAsync();

            return Ok(groups);
        }

        [HttpPost("groups")]
        [Authorize(Policy = "Perm:Assets.Dictionaries.Manage")]
        public async Task<IActionResult> CreateGroup([FromBody] ApproverGroupDto dto)
        {
            var group = new ApproverGroup { Name = dto.Name, IsActive = true };
            _context.ApproverGroups.Add(group);
            await _context.SaveChangesAsync();
            return Ok(group);
        }

        [HttpPost("groups/{id}/members")]
        [Authorize(Policy = "Perm:Assets.Dictionaries.Manage")]
        public async Task<IActionResult> AddGroupMember(int id, [FromBody] int employeeId)
        {
            var member = new ApproverGroupMember { ApproverGroupId = id, EmployeeId = employeeId };
            _context.ApproverGroupMembers.Add(member);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("groups/{id}/members/{employeeId}")]
        [Authorize(Policy = "Perm:Assets.Dictionaries.Manage")]
        public async Task<IActionResult> RemoveGroupMember(int id, int employeeId)
        {
            var member = await _context.ApproverGroupMembers.FindAsync(id, employeeId);
            if (member != null)
            {
                _context.ApproverGroupMembers.Remove(member);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpGet("groups/{id}/members")]
        [Authorize(Policy = "Perm:Assets.Dictionaries.Manage")]
        public async Task<IActionResult> GetGroupMembers(int id)
        {
            var members = await _context.ApproverGroupMembers
                .Include(m => m.Employee)
                .Where(m => m.ApproverGroupId == id)
                .Select(m => new {
                    Id = m.EmployeeId,
                    FullName = m.Employee!.FullName,
                    Email = m.Employee.Email
                })
                .ToListAsync();
                
            return Ok(members);
        }

        [HttpGet("groups/eligible-approvers")]
        [Authorize(Policy = "Perm:Assets.Dictionaries.Manage")]
        public async Task<IActionResult> GetEligibleApprovers([FromQuery] string? search)
        {
            var query = _context.UserAccounts
                .Include(u => u.Employee)
                .Where(u => u.IsActive && u.EmployeeId != 0)
                .Where(u => u.UserRoles.Any(ur => 
                    ur.Role.Name == "Admin" || 
                    ur.Role.RolePermissions.Any(rp => 
                        rp.Permission.Name == "Perm:Assets.Manage" || 
                        rp.Permission.Name == "Perm:Assets.Approve")));

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(u => u.Employee!.FullName.ToLower().Contains(lowerSearch) || 
                                         u.Employee.Email.ToLower().Contains(lowerSearch));
            }

            var eligible = await query
                .Select(u => new
                {
                    Id = u.EmployeeId,
                    FullName = u.Employee!.FullName,
                    Email = u.Employee.Email
                })
                .Distinct()
                .OrderBy(e => e.FullName)
                .Take(50)
                .ToListAsync();

            return Ok(eligible);
        }

        // --- CATEGORIES ---

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories([FromQuery] bool activeOnly = false)
        {
            var query = _context.AssetCategories.AsQueryable();
            if (activeOnly) query = query.Where(c => c.IsActive);

            var categories = await query
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Description,
                    c.ParentCategoryId,
                    c.RequiresApproval,
                    c.AllowRequesterToSelectApprover,
                    c.DefaultApproverGroupId,
                    c.FulfillmentGroupId,
                    c.IsActive
                })
                .OrderBy(c => c.Name)
                .ToListAsync();

            return Ok(categories);
        }

        [HttpPost("categories")]
        [Authorize(Policy = "Perm:Assets.Dictionaries.Manage")]
        public async Task<IActionResult> CreateCategory([FromBody] AssetCategoryDto dto)
        {
            var category = new AssetCategory
            {
                Name = dto.Name,
                Description = dto.Description,
                ParentCategoryId = dto.ParentCategoryId,
                RequiresApproval = dto.RequiresApproval,
                AllowRequesterToSelectApprover = dto.AllowRequesterToSelectApprover,
                DefaultApproverGroupId = dto.DefaultApproverGroupId,
                FulfillmentGroupId = dto.FulfillmentGroupId,
                IsActive = true
            };

            _context.AssetCategories.Add(category);
            await _context.SaveChangesAsync();
            return Ok(category);
        }

        [HttpPut("categories/{id}")]
        [Authorize(Policy = "Perm:Assets.Dictionaries.Manage")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] AssetCategoryDto dto)
        {
            var category = await _context.AssetCategories.FindAsync(id);
            if (category == null) return NotFound("Category not found.");

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.ParentCategoryId = dto.ParentCategoryId;
            category.RequiresApproval = dto.RequiresApproval;
            category.AllowRequesterToSelectApprover = dto.AllowRequesterToSelectApprover;
            category.DefaultApproverGroupId = dto.DefaultApproverGroupId;
            category.FulfillmentGroupId = dto.FulfillmentGroupId;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("categories/{id}/toggle-active")]
        [Authorize(Policy = "Perm:Assets.Dictionaries.Manage")]
        public async Task<IActionResult> ToggleCategoryActive(int id)
        {
            var category = await _context.AssetCategories.FindAsync(id);
            if (category == null) return NotFound();

            category.IsActive = !category.IsActive;
            await _context.SaveChangesAsync();
            return Ok();
        }

        // --- MODELS ---

        [HttpGet("models")]
        public async Task<IActionResult> GetModels([FromQuery] int? categoryId, [FromQuery] bool activeOnly = false)
        {
            var query = _context.AssetModels.AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(m => m.CategoryId == categoryId.Value);

            if (activeOnly)
                query = query.Where(m => m.IsActive);

            var models = await query
                .Select(m => new
                {
                    m.Id,
                    m.Manufacturer,
                    m.Name,
                    m.CategoryId,
                    CategoryName = m.Category!.Name,
                    m.IsActive
                })
                .OrderBy(m => m.Manufacturer).ThenBy(m => m.Name)
                .ToListAsync();

            return Ok(models);
        }

        [HttpPost("models")]
        [Authorize(Policy = "Perm:Assets.Dictionaries.Manage")]
        public async Task<IActionResult> CreateModel([FromBody] AssetModelDto dto)
        {
            var model = new AssetModel
            {
                Manufacturer = dto.Manufacturer,
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                IsActive = true
            };

            _context.AssetModels.Add(model);
            await _context.SaveChangesAsync();
            return Ok(model);
        }

        [HttpPut("models/{id}")]
        [Authorize(Policy = "Perm:Assets.Dictionaries.Manage")]
        public async Task<IActionResult> UpdateModel(int id, [FromBody] AssetModelDto dto)
        {
            var model = await _context.AssetModels.FindAsync(id);
            if (model == null) return NotFound("Model not found.");

            model.Manufacturer = dto.Manufacturer;
            model.Name = dto.Name;
            model.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("models/{id}/toggle-active")]
        [Authorize(Policy = "Perm:Assets.Dictionaries.Manage")]
        public async Task<IActionResult> ToggleModelActive(int id)
        {
            var model = await _context.AssetModels.FindAsync(id);
            if (model == null) return NotFound();

            model.IsActive = !model.IsActive;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }

    public class AssetCategoryDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool RequiresApproval { get; set; } = true;
        public bool AllowRequesterToSelectApprover { get; set; } = false;
        public int? DefaultApproverGroupId { get; set; }
        public int? FulfillmentGroupId { get; set; }
    }

    public class AssetModelDto
    {
        public required string Manufacturer { get; set; }
        public required string Name { get; set; }
        public int CategoryId { get; set; }
    }

    public class ApproverGroupDto
    {
        public required string Name { get; set; }
    }
}
