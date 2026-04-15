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

            builder.HasMany(r => r.LineItems)
                .WithOne(li => li.AssetRequest)
                .HasForeignKey(li => li.AssetRequestId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
