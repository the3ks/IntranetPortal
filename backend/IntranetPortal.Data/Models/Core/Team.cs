using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntranetPortal.Data.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        // Relational Mapping: A Team intrinsically belongs strictly to a specific Department
        [Required]
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        
        public Department? Department { get; set; }

        // Inverse Navigation: The employees strictly assigned to this team
        public ICollection<Employee>? Employees { get; set; }
    }
}
