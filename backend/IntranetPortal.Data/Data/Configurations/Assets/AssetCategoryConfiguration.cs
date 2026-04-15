using IntranetPortal.Data.Models.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntranetPortal.Data.Data.Configurations.Assets
{
    public class AssetCategoryConfiguration : IEntityTypeConfiguration<AssetCategory>
    {
        public void Configure(EntityTypeBuilder<AssetCategory> builder)
        {
            builder.ToTable("AM_AssetCategories");

            builder.HasKey(c => c.Id);

            // Self-referencing hierarchical relationship
            builder.HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(ac => ac.DefaultApproverGroup)
                   .WithMany()
                   .HasForeignKey(ac => ac.DefaultApproverGroupId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(ac => ac.FulfillmentGroup)
                   .WithMany()
                   .HasForeignKey(ac => ac.FulfillmentGroupId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
