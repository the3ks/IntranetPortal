using System.ComponentModel.DataAnnotations;

namespace IntranetPortal.Data.Models
{
    public class Department : ISiteScoped
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public int? SiteId { get; set; }
        public Site? Site { get; set; }

        public List<Employee> Employees { get; set; } = new();
    }
}
