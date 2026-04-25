using IntranetPortal.Data.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntranetPortal.Data.Data.Configurations.HR
{
    public class OnboardingTemplateConfiguration : IEntityTypeConfiguration<OnboardingTemplate>
    {
        public void Configure(EntityTypeBuilder<OnboardingTemplate> builder)
        {
            builder.ToTable("HR_OnboardingTemplates");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Description).HasMaxLength(500);
        }
    }

    public class OnboardingTaskConfiguration : IEntityTypeConfiguration<OnboardingTask>
    {
        public void Configure(EntityTypeBuilder<OnboardingTask> builder)
        {
            builder.ToTable("HR_OnboardingTasks");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.AssigneeRole).IsRequired().HasMaxLength(50);

            builder.HasOne(x => x.Template)
                   .WithMany(x => x.Tasks)
                   .HasForeignKey(x => x.TemplateId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class EmployeeOnboardingTaskConfiguration : IEntityTypeConfiguration<EmployeeOnboardingTask>
    {
        public void Configure(EntityTypeBuilder<EmployeeOnboardingTask> builder)
        {
            builder.ToTable("HR_EmployeeOnboardingTasks");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Employee)
                   .WithMany(x => x.OnboardingTasks)
                   .HasForeignKey(x => x.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.OnboardingTask)
                   .WithMany()
                   .HasForeignKey(x => x.OnboardingTaskId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
