using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IntranetPortal.Data.Data;
using IntranetPortal.Data.Models;

namespace IntranetPortal.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PositionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PositionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPositions()
        {
            var positions = await _context.Positions
                .Select(p => new { p.Id, p.Name, p.Description })
                .OrderBy(p => p.Name)
                .ToListAsync();

            return Ok(positions);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePosition([FromBody] PositionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var position = new Position { Name = dto.Name, Description = dto.Description };
            _context.Positions.Add(position);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPositions), new { id = position.Id }, position);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position == null) return NotFound();

            // Prevent deletion if employees are explicitly heavily mapped to this Job Title primitive
            var hasEmployees = await _context.Employees.AnyAsync(e => e.PositionId == id);
            if (hasEmployees) 
            {
                return BadRequest(new { Message = "Cannot delete a position actively assigned to an employee within the organizational matrix." });
            }

            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class PositionDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
