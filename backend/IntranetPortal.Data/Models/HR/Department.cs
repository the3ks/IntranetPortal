using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntranetPortal.Data.Models.HR
{
    public class Department : ISiteScoped
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        // Hierarchy
        public int? ParentDepartmentId { get; set; }
        public Department? ParentDepartment { get; set; }
        public ICollection<Department> ChildDepartments { get; set; } = new List<Department>();

        // Authority
        public int? ManagerId { get; set; }
        public Employee? Manager { get; set; }

        // Scoping (Unified)
        [Required]
        public int? SiteId { get; set; }
        public Site? Site { get; set; }

        // Personnel
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
