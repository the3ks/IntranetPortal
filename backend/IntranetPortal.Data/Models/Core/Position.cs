namespace IntranetPortal.Data.Models
{
    public class Position
    {
        public int Id { get; set; }
        public required string Name { get; set; } // e.g., Chief Executive Officer, Senior Developer
        public string? Description { get; set; }
        
        // Relational Navigation mapping
        public List<Employee> Employees { get; set; } = new();
    }
}
