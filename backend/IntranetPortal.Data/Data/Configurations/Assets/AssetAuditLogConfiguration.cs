using IntranetPortal.Data.Models.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntranetPortal.Data.Data.Configurations.Assets
{
    public class AssetAuditLogConfiguration : IEntityTypeConfiguration<AssetAuditLog>
    {
        public void Configure(EntityTypeBuilder<AssetAuditLog> builder)
        {
            builder.ToTable("AM_AssetAuditLogs");

            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.Asset)
                .WithMany(asset => asset.AuditLogs)
                .HasForeignKey(a => a.AssetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.PerformedByEmployee)
                .WithMany()
                .HasForeignKey(a => a.PerformedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
