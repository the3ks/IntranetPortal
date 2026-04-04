using IntranetPortal.Data.Models.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntranetPortal.Data.Data.Configurations.Assets
{
    public class AssetRequestConfiguration : IEntityTypeConfiguration<AssetRequest>
    {
        public void Configure(EntityTypeBuilder<AssetRequest> builder)
        {
            builder.ToTable("AM_AssetRequests");

            builder.HasKey(r => r.Id);

            builder.HasOne(r => r.RequestedByEmployee)
                .WithMany()
                .HasForeignKey(r => r.RequestedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.RequestedForEmployee)
                .WithMany()
                .HasForeignKey(r => r.RequestedForEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.RequestedCategory)
                .WithMany()
                .HasForeignKey(r => r.RequestedCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.RequestedModel)
                .WithMany()
                .HasForeignKey(r => r.RequestedModelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.RequestedAccessory)
                .WithMany()
                .HasForeignKey(r => r.RequestedAccessoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.ManagerApprovedByEmployee)
                .WithMany()
                .HasForeignKey(r => r.ManagerApprovedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.FulfilledByEmployee)
                .WithMany()
                .HasForeignKey(r => r.FulfilledByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.AssignedAsset)
                .WithMany()
                .HasForeignKey(r => r.AssignedAssetId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
