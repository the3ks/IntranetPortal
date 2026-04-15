using IntranetPortal.Data.Models.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntranetPortal.Data.Data.Configurations.Assets
{
    public class AssetRequestLineItemConfiguration : IEntityTypeConfiguration<AssetRequestLineItem>
    {
        public void Configure(EntityTypeBuilder<AssetRequestLineItem> builder)
        {
            builder.ToTable("AM_AssetRequestLineItems");

            builder.HasKey(li => li.Id);

            builder.HasOne(li => li.RequestedCategory)
                .WithMany()
                .HasForeignKey(li => li.RequestedCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(li => li.RequestedModel)
                .WithMany()
                .HasForeignKey(li => li.RequestedModelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(li => li.RequestedAccessory)
                .WithMany()
                .HasForeignKey(li => li.RequestedAccessoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(li => li.ApprovedByEmployee)
                   .WithMany()
                   .HasForeignKey(li => li.ApprovedByEmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(li => li.FulfilledByEmployee)
                .WithMany()
                .HasForeignKey(li => li.FulfilledByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(li => li.AssignedAsset)
                .WithMany()
                .HasForeignKey(li => li.AssignedAssetId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(li => li.SelectedApproverEmployee)
                   .WithMany()
                   .HasForeignKey(li => li.SelectedApproverEmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(li => li.AssignedApproverGroup)
                   .WithMany()
                   .HasForeignKey(li => li.AssignedApproverGroupId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
