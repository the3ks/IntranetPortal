using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IntranetPortal.Data.Data;

namespace IntranetPortal.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TeamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTeams([FromQuery] int? departmentId)
        {
            var query = _context.Teams.AsQueryable();

            if (departmentId.HasValue)
            {
                query = query.Where(t => t.DepartmentId == departmentId.Value);
            }

            var teams = await query
                .Select(t => new { t.Id, t.Name, t.DepartmentId })
                .OrderBy(t => t.Name)
                .ToListAsync();

            return Ok(teams);
        }

        [HttpPost]
        [Authorize(Policy = "Perm:Admin")]
        public async Task<IActionResult> CreateTeam([FromBody] TeamDto dto)
        {
            var team = new IntranetPortal.Data.Models.Team 
            { 
                Name = dto.Name,
                DepartmentId = dto.DepartmentId
            };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return Ok(new { team.Id, team.Name, team.DepartmentId });
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Perm:Admin")]
        public async Task<IActionResult> UpdateTeam(int id, [FromBody] TeamDto dto)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null) return NotFound();

            team.Name = dto.Name;
            team.DepartmentId = dto.DepartmentId;
            await _context.SaveChangesAsync();
            return Ok(new { team.Id, team.Name, team.DepartmentId });
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Perm:Admin")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null) return NotFound();

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class TeamDto
    {
        public required string Name { get; set; }
        public int DepartmentId { get; set; }
    }
}
