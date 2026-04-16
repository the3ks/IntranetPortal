using IntranetPortal.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntranetPortal.Data.Data.Configurations.Core
{
    public class LoginChallengeConfiguration : IEntityTypeConfiguration<LoginChallenge>
    {
        public void Configure(EntityTypeBuilder<LoginChallenge> builder)
        {
            builder.ToTable("Core_LoginChallenges");

            builder.Property(c => c.ChallengeId).HasMaxLength(64).IsRequired();
            builder.Property(c => c.NormalizedEmail).HasMaxLength(256).IsRequired();
            builder.Property(c => c.Nonce).HasMaxLength(128).IsRequired();

            builder.HasIndex(c => c.ChallengeId).IsUnique();
            builder.HasIndex(c => c.ExpiresAt);
            builder.HasIndex(c => new { c.NormalizedEmail, c.CreatedAt });

            builder.HasOne(c => c.UserAccount)
                .WithMany()
                .HasForeignKey(c => c.UserAccountId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}