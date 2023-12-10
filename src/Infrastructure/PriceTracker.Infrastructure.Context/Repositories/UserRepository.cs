using Microsoft.EntityFrameworkCore;
using PriceTracker.Domain.Entities;
using PriceTracker.Domain.Repositories;

namespace PriceTracker.Infrastructure.Context.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    
    public UserRepository(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public Task CreateUserAsync(User user)
    {
        throw new NotImplementedException();
    }
}