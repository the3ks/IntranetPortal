using IntranetPortal.Data.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntranetPortal.Data.Data.Configurations.HR
{
    public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> builder)
        {
            builder.ToTable("HR_LeaveRequests");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Reason).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");

            builder.HasOne(x => x.Employee)
                   .WithMany(x => x.LeaveRequests)
                   .HasForeignKey(x => x.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.LeaveType)
                   .WithMany()
                   .HasForeignKey(x => x.LeaveTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ApprovedBy)
                   .WithMany()
                   .HasForeignKey(x => x.ApprovedById)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
