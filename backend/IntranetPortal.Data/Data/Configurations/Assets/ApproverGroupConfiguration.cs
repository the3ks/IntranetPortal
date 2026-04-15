using IntranetPortal.Data.Models.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntranetPortal.Data.Data.Configurations.Assets
{
    public class ApproverGroupConfiguration : IEntityTypeConfiguration<ApproverGroup>
    {
        public void Configure(EntityTypeBuilder<ApproverGroup> builder)
        {
            builder.ToTable("AM_ApproverGroups");
        }
    }

    public class ApproverGroupMemberConfiguration : IEntityTypeConfiguration<ApproverGroupMember>
    {
        public void Configure(EntityTypeBuilder<ApproverGroupMember> builder)
        {
            builder.ToTable("AM_ApproverGroupMembers");
            builder.HasKey(agm => new { agm.ApproverGroupId, agm.EmployeeId });
        }
    }

    public class ApproverGroupScopeConfiguration : IEntityTypeConfiguration<ApproverGroupScope>
    {
        public void Configure(EntityTypeBuilder<ApproverGroupScope> builder)
        {
            builder.ToTable("AM_ApproverGroupScopes");
            builder.HasKey(ags => new { ags.ApproverGroupId, ags.DepartmentId });
        }
    }

    public class AssetCategoryApproverGroupConfiguration : IEntityTypeConfiguration<AssetCategoryApproverGroup>
    {
        public void Configure(EntityTypeBuilder<AssetCategoryApproverGroup> builder)
        {
            builder.ToTable("AM_AssetCategoryApproverGroups");
            builder.HasKey(acag => new { acag.AssetCategoryId, acag.ApproverGroupId });

            builder.HasOne(acag => acag.AssetCategory)
                   .WithMany(ac => ac.ApproverGroups)
                   .HasForeignKey(acag => acag.AssetCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(acag => acag.ApproverGroup)
                   .WithMany(ag => ag.AssetCategories)
                   .HasForeignKey(acag => acag.ApproverGroupId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
