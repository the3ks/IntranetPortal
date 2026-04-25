namespace IntranetPortal.Data.Models.HR
{
    public class OnboardingTask
    {
        public int Id { get; set; }
        
        public int TemplateId { get; set; }
        public OnboardingTemplate? Template { get; set; }

        public required string Title { get; set; }
        public string? Description { get; set; }
        
        // E.g. IT, HR, Manager, or Employee
        public string AssigneeRole { get; set; } = "Employee"; 
        
        public int Order { get; set; } = 0;
    }
}
