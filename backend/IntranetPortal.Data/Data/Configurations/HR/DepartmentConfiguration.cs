using IntranetPortal.Data.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntranetPortal.Data.Data.Configurations.HR
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("HR_Departments");

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(500);

            builder.HasOne(x => x.ParentDepartment)
                   .WithMany(x => x.ChildDepartments)
                   .HasForeignKey(x => x.ParentDepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Manager)
                   .WithMany(x => x.ManagedDepartments)
                   .HasForeignKey(x => x.ManagerId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Site)
                   .WithMany()
                   .HasForeignKey(x => x.SiteId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
