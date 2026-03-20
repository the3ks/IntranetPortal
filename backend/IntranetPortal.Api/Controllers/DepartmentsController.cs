using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IntranetPortal.Data.Data;

namespace IntranetPortal.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _context.Departments
                .Select(d => new { d.Id, d.Name })
                .OrderBy(d => d.Name)
                .ToListAsync();

            return Ok(departments);
        }

        [HttpPost]
        [Authorize(Policy = "Perm:Admin")]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDto dto)
        {
            var dept = new IntranetPortal.Data.Models.Department { Name = dto.Name };
            _context.Departments.Add(dept);
            await _context.SaveChangesAsync();
            return Ok(new { dept.Id, dept.Name });
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Perm:Admin")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentDto dto)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null) return NotFound();

            dept.Name = dto.Name;
            await _context.SaveChangesAsync();
            return Ok(new { dept.Id, dept.Name });
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Perm:Admin")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null) return NotFound();

            _context.Departments.Remove(dept);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class DepartmentDto
    {
        public required string Name { get; set; }
    }
}
