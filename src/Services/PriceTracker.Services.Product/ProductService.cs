using Microsoft.EntityFrameworkCore;
using PriceTracker.Infrastructure.Context;

namespace PriceTracker.Services.Product;

public class ProductService : IProductService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public ProductService(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task RemoveProductById(long productId, CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var product = await context.Products
            .FirstOrDefaultAsync(x => x.Id.Equals(productId), cancellationToken);

        if (product is null)
            return;

        context.Products.Remove(product);
        await context.SaveChangesAsync(cancellationToken);
    }
}