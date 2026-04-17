using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IntranetPortal.Data.Models;

namespace IntranetPortal.Data.Data.Configurations.Core
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("Core_AuditLogs");
            
            builder.HasKey(a => a.Id);
            
            builder.Property(a => a.Email).HasMaxLength(255);
            builder.Property(a => a.IPAddress).HasMaxLength(45);
            builder.Property(a => a.Action).HasMaxLength(150).IsRequired();
            builder.Property(a => a.UserAgent).HasMaxLength(500);
        }
    }
}
