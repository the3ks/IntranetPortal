using IntranetPortal.Data.Models.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntranetPortal.Data.Data.Configurations.Assets
{
    public class AssetAssignmentConfiguration : IEntityTypeConfiguration<AssetAssignment>
    {
        public void Configure(EntityTypeBuilder<AssetAssignment> builder)
        {
            builder.ToTable("AM_AssetAssignments");

            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.Asset)
                .WithMany(asset => asset.Assignments)
                .HasForeignKey(a => a.AssetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.AssignedToEmployee)
                .WithMany()
                .HasForeignKey(a => a.AssignedToEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.AssignedToTeam)
                .WithMany()
                .HasForeignKey(a => a.AssignedToTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.AssignedByEmployee)
                .WithMany()
                .HasForeignKey(a => a.AssignedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.ReturnedByEmployee)
                .WithMany()
                .HasForeignKey(a => a.ReturnedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
