using PriceTracker.Domain.Entities;

namespace PriceTracker.Domain.Repositories;

public interface IUserRepository
{
    Task CreateUserAsync(User user);
}