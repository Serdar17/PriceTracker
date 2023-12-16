using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PriceTracker.Domain.Entities;
using PriceTracker.Infrastructure.Context;
using PriceTracker.Services.Parser.Factory;
using Quartz;

namespace PriceTracker.BackgroundJob;

public class ParsingBackgroundJob : IJob
{
    private readonly ILogger<ParsingBackgroundJob> _logger;
    private readonly IDbContextFactory<AppDbContext> _factory;
    private readonly IParserFactory _parserFactory;

    public ParsingBackgroundJob(ILogger<ParsingBackgroundJob> logger,
        IDbContextFactory<AppDbContext> factory,
        IServiceProvider provider)
    {
        _logger = logger;
        _factory = factory;
        _parserFactory = provider.CreateScope().ServiceProvider.GetService<IParserFactory>()!;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("background working started at {StartTime}", DateTime.UtcNow);
        await using var dbContext = await _factory.CreateDbContextAsync();
        var users = dbContext.Users
            .Include(x => x.Products)
            .ThenInclude(x => x.Prices);
        foreach (var user in users)
        {
            var products = user.Products;
            foreach (var product in products)
            {
                var parser = _parserFactory.CreateParser(product.MarketPlaceName);
                
                if (parser is null)
                    continue;
                
                var result = await parser.ParseAsync(product.Link);

                if (result.Title is not null)
                {
                    product.Prices.Add(new Price(result.Price ?? 0.0, result.CardPrice ?? 0.0));
                }

                dbContext.Products.Update(product);
            }
        }
        await dbContext.SaveChangesAsync();
        
        _logger.LogInformation("background working ended at {EndTime}", DateTime.UtcNow);
    }
}