using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceTracker.Domain.Entities;

namespace PriceTracker.Infrastructure.Context.EntityTypeConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.Property(x => x.FirstName).HasMaxLength(250);
        builder.Property(x => x.LastName).HasMaxLength(250);
        builder.Property(x => x.Username).HasMaxLength(250);

        builder.HasIndex(x => x.Username);

        builder.HasMany(x => x.Sites)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}