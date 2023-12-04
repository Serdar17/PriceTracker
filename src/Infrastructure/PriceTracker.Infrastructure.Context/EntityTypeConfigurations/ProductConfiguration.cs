using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceTracker.Domain.Entities;

namespace PriceTracker.Infrastructure.Context.EntityTypeConfigurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.Property(x => x.Title).HasMaxLength(250);

        builder.HasMany(x => x.Prices)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}