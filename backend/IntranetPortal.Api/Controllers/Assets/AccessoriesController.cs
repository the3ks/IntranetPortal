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
    public class AccessoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPermissionService _permissionService;

        public AccessoriesController(ApplicationDbContext context, IPermissionService permissionService)
        {
            _context = context;
            _permissionService = permissionService;
        }

        [HttpGet]
        [Authorize(Policy = "Perm:Assets.View")]
        public async Task<IActionResult> GetAccessories()
        {
            var query = _context.Accessories
                .Include(a => a.Category)
                .Include(a => a.Site)
                .Include(a => a.Department)
                .AsQueryable();

            query = query.ApplySiteScope(_permissionService, "Perm:Assets.View");
            query = query.ApplyDepartmentScope(_permissionService, "Perm:Assets.View");

            var accessories = await query.OrderBy(a => a.Name).ToListAsync();

            var result = accessories.Select(a => new AccessoryDto
            {
                Id = a.Id,
                Name = a.Name,
                CategoryName = a.Category?.Name,
                TotalQuantity = a.TotalQuantity,
                AvailableQuantity = a.AvailableQuantity,
                MinStockThreshold = a.MinStockThreshold,
                SiteName = a.Site?.Name,
                DepartmentName = a.Department?.Name
            });

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "Perm:Assets.Manage")]
        public async Task<IActionResult> CreateAccessory([FromBody] AccessoryCreateDto dto)
        {
            if (!_permissionService.ValidateSiteScope("Perm:Assets.Manage", dto.SiteId)) return Forbid();
            if (!_permissionService.ValidateDepartmentScope("Perm:Assets.Manage", dto.DepartmentId)) return Forbid();

            var accessory = new Accessory
            {
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                TotalQuantity = dto.TotalQuantity,
                AvailableQuantity = dto.TotalQuantity, // Starts equal
                MinStockThreshold = dto.MinStockThreshold,
                SiteId = dto.SiteId,
                DepartmentId = dto.DepartmentId
            };

            _context.Accessories.Add(accessory);
            await _context.SaveChangesAsync();
            return Ok(new { accessory.Id });
        }

        [HttpPost("{id}/add-stock")]
        [Authorize(Policy = "Perm:Assets.Manage")]
        public async Task<IActionResult> AddStock(int id, [FromBody] int quantity)
        {
            var accessory = await _context.Accessories.FindAsync(id);
            if (accessory == null) return NotFound();

            if (!_permissionService.ValidateSiteScope("Perm:Assets.Manage", accessory.SiteId)) return Forbid();
            if (!_permissionService.ValidateDepartmentScope("Perm:Assets.Manage", accessory.DepartmentId)) return Forbid();

            accessory.TotalQuantity += quantity;
            accessory.AvailableQuantity += quantity;
            
            await _context.SaveChangesAsync();
            return Ok(new { accessory.TotalQuantity, accessory.AvailableQuantity });
        }
    }

    public class AccessoryDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? CategoryName { get; set; }
        public int TotalQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public int? MinStockThreshold { get; set; }
        public string? SiteName { get; set; }
        public string? DepartmentName { get; set; }
    }

    public class AccessoryCreateDto
    {
        public required string Name { get; set; }
        public int CategoryId { get; set; }
        public int TotalQuantity { get; set; }
        public int? MinStockThreshold { get; set; }
        public int? SiteId { get; set; }
        public int? DepartmentId { get; set; }
    }
}
