using Microsoft.EntityFrameworkCore;

namespace PriceTracker.Infrastructure.Context.Factories;

public class DbContextFactory
{
    private readonly DbContextOptions<AppDbContext> _options;

    public DbContextFactory(DbContextOptions<AppDbContext> options)
    {
        _options = options;
    }

    public AppDbContext Create()
    {
        return new AppDbContext(_options);
    }
}