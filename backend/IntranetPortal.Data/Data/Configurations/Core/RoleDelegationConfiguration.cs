using IntranetPortal.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntranetPortal.Data.Data.Configurations.Core
{
    public class RoleDelegationConfiguration : IEntityTypeConfiguration<RoleDelegation>
    {
        public void Configure(EntityTypeBuilder<RoleDelegation> builder)
        {
            builder.ToTable("Core_RoleDelegations");

            builder.HasOne(rd => rd.SourceUser)
                .WithMany()
                .HasForeignKey(rd => rd.SourceUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(rd => rd.SubstituteUser)
                .WithMany()
                .HasForeignKey(rd => rd.SubstituteUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(rd => rd.UserRole)
                .WithMany()
                .HasForeignKey(rd => rd.UserRoleId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
