using IntranetPortal.Data.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntranetPortal.Data.Data.Configurations.HR
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("HR_Employees");

            builder.Property(x => x.FullName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(200);
            builder.Property(x => x.EmployeeNumber).IsRequired().HasMaxLength(50);

            // Relational mapping to Identity
            builder.HasOne(x => x.UserAccount)
                   .WithOne(x => x.Employee)
                   .HasForeignKey<Employee>(x => x.UserAccountId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Internal Reporting Hierarchy
            builder.HasOne(x => x.DirectManager)
                   .WithMany(x => x.DirectReports)
                   .HasForeignKey(x => x.DirectManagerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Department)
                   .WithMany(x => x.Employees)
                   .HasForeignKey(x => x.DepartmentId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Position)
                   .WithMany(x => x.Employees)
                   .HasForeignKey(x => x.PositionId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
