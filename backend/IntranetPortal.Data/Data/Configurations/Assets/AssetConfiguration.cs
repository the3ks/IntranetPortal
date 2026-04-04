using IntranetPortal.Data.Models.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntranetPortal.Data.Data.Configurations.Assets
{
    public class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.ToTable("AM_Assets");

            builder.HasKey(a => a.Id);

            builder.HasIndex(a => a.AssetTag).IsUnique();

            builder.HasOne(a => a.Model)
                .WithMany(m => m.Assets)
                .HasForeignKey(a => a.ModelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Site)
                .WithMany()
                .HasForeignKey(a => a.SiteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Department)
                .WithMany()
                .HasForeignKey(a => a.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.CreatedByEmployee)
                .WithMany()
                .HasForeignKey(a => a.CreatedByEmployeeId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(a => a.PurchasePrice).HasColumnType("decimal(18,2)");
        }
    }
}
