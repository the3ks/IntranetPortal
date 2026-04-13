using IntranetPortal.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntranetPortal.Data.Data.Configurations.Core
{
    public class SystemModuleConfiguration : IEntityTypeConfiguration<SystemModule>
    {
        public void Configure(EntityTypeBuilder<SystemModule> builder)
        {
            builder.ToTable("Core_SystemModules");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Name).IsRequired().HasMaxLength(150);
            builder.Property(m => m.Description).IsRequired().HasMaxLength(500);
            builder.Property(m => m.IconSvg).IsRequired(); // SVG can be large
            builder.Property(m => m.Url).IsRequired().HasMaxLength(300);

            // Configure implicit many-to-many relationship securely
            builder.HasMany(m => m.AllowedSites)
                   .WithMany(s => s.AllowedModules)
                   .UsingEntity(j => j.ToTable("Core_ModuleSites"));
        }
    }
}
