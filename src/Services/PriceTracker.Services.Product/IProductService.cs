namespace PriceTracker.Services.Product;

public interface IProductService
{
    Task RemoveProductById(long productId, CancellationToken cancellationToken = default);
}