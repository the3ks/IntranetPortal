using IntranetPortal.Data.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntranetPortal.Data.Data.Configurations.HR
{
    public class AttendanceLogConfiguration : IEntityTypeConfiguration<AttendanceLog>
    {
        public void Configure(EntityTypeBuilder<AttendanceLog> builder)
        {
            builder.ToTable("HR_AttendanceLogs");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.ClockInTime);
            builder.Property(x => x.ClockOutTime);

            builder.HasOne(x => x.Employee)
                   .WithMany(x => x.AttendanceLogs)
                   .HasForeignKey(x => x.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade);

            // One log per employee per day
            builder.HasIndex(x => new { x.EmployeeId, x.Date }).IsUnique();
        }
    }
}
