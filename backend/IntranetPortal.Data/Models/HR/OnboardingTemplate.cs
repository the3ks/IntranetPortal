namespace IntranetPortal.Data.Models.HR
{
    public class OnboardingTemplate
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<OnboardingTask> Tasks { get; set; } = new List<OnboardingTask>();
    }
}
