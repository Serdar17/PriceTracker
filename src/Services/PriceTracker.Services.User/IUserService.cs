using PriceTracker.Domain.Entities;

namespace PriceTracker.Services.User;

public interface IUserService
{
    Task CreateUserAsync(Domain.Entities.User user, CancellationToken cancellationToken = default);
    Task AddProductToUserAsync(long userId, Product product, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetProductsByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Domain.Entities.User>> GetUsersAsync(CancellationToken cancellationToken = default);

}