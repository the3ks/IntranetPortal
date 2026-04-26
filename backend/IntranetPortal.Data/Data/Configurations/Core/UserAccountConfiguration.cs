using IntranetPortal.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntranetPortal.Data.Data.Configurations.Core
{
    public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder.ToTable("Core_UserAccounts");

            builder.Property(u => u.Email).HasMaxLength(256).IsRequired();
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.SecurityStamp).HasDefaultValue(1);

            // Strict 1-to-1 link to HR Employee record (Master Link)
            builder.HasOne(u => u.Employee)
                   .WithOne(e => e.UserAccount)
                   .HasForeignKey<UserAccount>(u => u.EmployeeId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
