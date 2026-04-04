using IntranetPortal.Data.Models.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntranetPortal.Data.Data.Configurations.Assets
{
    public class AccessoryCheckoutConfiguration : IEntityTypeConfiguration<AccessoryCheckout>
    {
        public void Configure(EntityTypeBuilder<AccessoryCheckout> builder)
        {
            builder.ToTable("AM_AccessoryCheckouts");

            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Accessory)
                .WithMany(a => a.Checkouts)
                .HasForeignKey(c => c.AccessoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.RequestedByEmployee)
                .WithMany()
                .HasForeignKey(c => c.RequestedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.FulfilledByEmployee)
                .WithMany()
                .HasForeignKey(c => c.FulfilledByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
