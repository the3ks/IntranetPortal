namespace IntranetPortal.Data.Models.HR
{
    public class EmployeeOnboardingTask
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public int OnboardingTaskId { get; set; }
        public OnboardingTask? OnboardingTask { get; set; }

        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedAt { get; set; }
    }
}
