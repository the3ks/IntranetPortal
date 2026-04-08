using System.ComponentModel.DataAnnotations;

namespace IntranetPortal.Data.Models
{
    public class Employee : ISiteScoped
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public int? PositionId { get; set; }
        public Position? Position { get; set; }

        // Foreign Keys
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public int? TeamId { get; set; }
        public Team? Team { get; set; }

        [Required]
        public int? SiteId { get; set; }
        public Site? Site { get; set; }
    }
}
