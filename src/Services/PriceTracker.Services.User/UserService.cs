using Microsoft.EntityFrameworkCore;
using PriceTracker.Domain.Entities;
using PriceTracker.Infrastructure.Context;

namespace PriceTracker.Services.User;

public class UserService : IUserService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public UserService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task CreateUserAsync(Domain.Entities.User user, CancellationToken cancellationToken = default)
    {
        await using var context = await _factory.CreateDbContextAsync(cancellationToken);
        var existUser = context.Users.FirstOrDefault(x => x.Id.Equals(user!.Id));
        
        if (existUser is null)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task AddProductToUserAsync(long userId, Product product, CancellationToken cancellationToken = default)
    {
        await using var context = await _factory.CreateDbContextAsync(cancellationToken);
        var user = context.Users.FirstOrDefault(x => x.Id.Equals(userId));
        user!.Products.Add(product);
        context.Users.Update(user);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetProductsByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        await using var context = await _factory.CreateDbContextAsync(cancellationToken);
        var user = context.Users
            .Include(x => x.Products)
            .FirstOrDefault(x => x.Id.Equals(userId));

        if (user is null)
            return Enumerable.Empty<Product>();
        
        return user.Products;
    }
}