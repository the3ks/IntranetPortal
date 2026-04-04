using IntranetPortal.Data.Models.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntranetPortal.Data.Data.Configurations.Assets
{
    public class AssetModelConfiguration : IEntityTypeConfiguration<AssetModel>
    {
        public void Configure(EntityTypeBuilder<AssetModel> builder)
        {
            builder.ToTable("AM_AssetModels");

            builder.HasKey(m => m.Id);

            builder.HasOne(m => m.Category)
                .WithMany(c => c.Models)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
