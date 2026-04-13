namespace IntranetPortal.Data.Models
{
    public class Site
    {
        public int Id { get; set; }
        public required string Name { get; set; } // e.g., Headquarters, Factory 1
        public string? Address { get; set; }
        
        // Navigation properties
        public List<Employee> Employees { get; set; } = new();
        public ICollection<SystemModule> AllowedModules { get; set; } = new List<SystemModule>();
    }
}
