using IntranetPortal.Data.Models.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntranetPortal.Data.Data.Configurations.Assets
{
    public class AssetMaintenanceConfiguration : IEntityTypeConfiguration<AssetMaintenance>
    {
        public void Configure(EntityTypeBuilder<AssetMaintenance> builder)
        {
            builder.ToTable("AM_AssetMaintenance");

            builder.HasKey(m => m.Id);

            builder.HasOne(m => m.Asset)
                .WithMany(a => a.MaintenanceRecords)
                .HasForeignKey(m => m.AssetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.LoggedByEmployee)
                .WithMany()
                .HasForeignKey(m => m.LoggedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(m => m.Cost).HasColumnType("decimal(18,2)");
        }
    }
}
