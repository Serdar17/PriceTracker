using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PriceTracker.Domain.Entities;
using PriceTracker.Domain.Telegram;
using PriceTracker.Infrastructure.Context;
using PriceTracker.Services.Parser.Factory;
using PriceTracker.Services.Parser.Models;
using Quartz;

namespace PriceTracker.BackgroundJob;

public class ParsingBackgroundJob : IJob
{
    private readonly ILogger<ParsingBackgroundJob> _logger;
    private readonly IDbContextFactory<AppDbContext> _factory;
    private readonly IParserFactory _parserFactory;
    private readonly ITelegramClient _client;
    private const double Eps = 1e-10;

    public ParsingBackgroundJob(ILogger<ParsingBackgroundJob> logger,
        IDbContextFactory<AppDbContext> factory,
        IServiceProvider provider, 
        ITelegramClient client)
    {
        _logger = logger;
        _factory = factory;
        _client = client;
        _parserFactory = provider.CreateScope().ServiceProvider.GetService<IParserFactory>()!;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("background working started at {StartTime}", DateTime.Now);
        
        await using var dbContext = await _factory.CreateDbContextAsync();
        var users = dbContext.Users
            .Include(x => x.Products)
            .ThenInclude(x => x.Prices.OrderByDescending(p => p.CreateDate).Take(1));
        
        foreach (var user in users)
        {
            var products = user.Products.ToList();
            foreach (var product in products)
            {
                var parser = _parserFactory.CreateParser(product.MarketPlaceName);
                
                if (parser is null)
                    continue;
    
                var price = product.Prices.Last();
                
                _logger.LogInformation("The last price creted at: {CreatedAt} and has current price = {Price}, " +
                                       "discounted price = {DiscountedPrice}", price.CreateDate, price.CurrentPrice, price.DiscountedPrice);
                
                var result = await parser.ParseAsync(product.Link);
    
                if (result is not null && (Math.Abs(price.CurrentPrice - (double)result?.Price!) > Eps
                                                 || Math.Abs(price.DiscountedPrice - (double)result?.CardPrice!) > Eps))
                {
                    _logger.LogInformation("The product name is {Title} has price: {Price} and discounted price: {DiscountedPrice}",
                        result.Title, result.Price, result.CardPrice);
                    
                    product.Prices.Add(new Price(result.Price ?? 0.0, result.CardPrice ?? 0.0));
                    var message = GetNotificationMessage(product, price, result);
                    await _client.SendPriceChangingNotification(user.ChatId, message);
                }
                
                if (result.Title is null)
                {
                    var message = "\u274c Проверьте наличие товара на маркетплейсе. Возможно его удалили или он " +
                                   "просто закончился.\n" +
                                   $"\ud83d\udd17 [Ссылка на товар]({product.Link})";
                    await _client.SendPriceChangingNotification(user.ChatId, message);
                }
    
                dbContext.Products.Update(product);
            }
        }
        await dbContext.SaveChangesAsync();
        
        _logger.LogInformation("background working ended at {EndTime}", DateTime.Now);
    }

    private string GetNotificationMessage(Product product, Price price, ParseResult result)
    {
        var currency = result.Currency ?? "\u20bd";
        var message = $"\ud83d\udd14 Уведомление об изменении цены!\n" +
                      $"\ud83d\udcb0 Прошлая цена без скидки: {price.CurrentPrice} {currency} \n" +
                      $"\ud83d\udcb3 Прошлая цена по скидке/карте: {price.DiscountedPrice} {currency} \n" +
                      $"\n" +
                      $"\ud83d\udcbc Новая цена без скидки: *{result.Price}* {currency} \n" +
                      $"\ud83d\udcb3 Новая цена по скидке/карте: *{result.CardPrice}* {currency} \n" +
                      $"\n" +
                      $"\ud83d\udd17 [Ссылка на товар]({product.Link})";
        return message;
    }

    // private async Task TryParseAsync(Product product, User user)
    // {
    //     var parser = _parserFactory.CreateParser(product.MarketPlaceName); 
    //     
    //     var price = product.Prices.Last();
    //             
    //     _logger.LogInformation("The last price creted at: {CreatedAt} and has current price = {Price}, " +
    //                            "discounted price = {DiscountedPrice}", price.CreateDate, price.CurrentPrice, price.DiscountedPrice);
    //
    //
    //     if (parser is null)
    //         return;
    //     
    //     var result = await parser.ParseAsync(product.Link);
    //
    //     if (result.Title is not null && (Math.Abs(price.CurrentPrice - (double)result?.Price!) > Eps
    //                                      || Math.Abs(price.DiscountedPrice - (double)result?.CardPrice!) > Eps))
    //     {
    //         _logger.LogInformation("The product name is {Title} has price: {Price} and discounted price: {DiscountedPrice}",
    //             result.Title, result.Price, result.CardPrice);
    //         product.Prices.Add(new Price(result.Price ?? 0.0, result.CardPrice ?? 0.0));
    //         var message = $"\ud83d\udd14 Уведомление об изменении цены!\n" +
    //                       $"\ud83d\udcb0 Прошлая цена без скидки: {price.CurrentPrice} \u20bd \n" +
    //                       $"\ud83d\udcb3 Прошлая цена по скидке/карте: {price.DiscountedPrice} \u20bd \n" +
    //                       $"\n" +
    //                       $"\ud83d\udcbc Новая цена без скидки: *{result.Price}* \u20bd \n" +
    //                       $"\ud83d\udcb3 Новая цена по скидке/карте: *{result.CardPrice}* \u20bd \n" +
    //                       $"\n" +
    //                       $"\ud83d\udd17 [Ссылка на товар]({product.Link})";
    //         await _client.SendPriceChangingNotification(user.ChatId, message);
    //     }
    //             
    //     if (result.Title is null)
    //     {
    //         var message = "\u274c Проверьте наличие товара на маркетплейсе. Возможно его удалили или он " +
    //                       "просто закончился.\n" +
    //                       $"\ud83d\udd17 [Ссылка на товар]({product.Link})";
    //         await _client.SendPriceChangingNotification(user.ChatId, message);
    //     }
    // }
    
    

    
    
    // public async Task Execute(IJobExecutionContext context)
    // {
    //     _logger.LogInformation("background working started at {StartTime}", DateTime.Now);
    //     
    //     await using var dbContext = await _factory.CreateDbContextAsync();
    //     var users = dbContext.Users
    //         .Include(x => x.Products)
    //         .ThenInclude(x => x.Prices.OrderByDescending(p => p.CreateDate).Take(1));
    //     
    //     foreach (var user in users)
    //     {
    //         var products = user.Products.ToList();
    //         var tasks = new Task[products.Count];
    //         for (var i = 0; i < products.Count; i++)
    //         {
    //             var i1 = i;
    //             tasks[i] = Task.Run(() => TryParseAsync(products[i1], user));
    //         }
    //         
    //         Task.WaitAll(tasks);
    //         
    //         foreach (var product in products)
    //         {
    //             dbContext.Products.Update(product);
    //
    //             // var parser = _parserFactory.CreateParser(product.MarketPlaceName);
    //             //
    //             // if (parser is null)
    //             //     continue;
    //             //
    //             // var price = product.Prices.Last();
    //             //
    //             // _logger.LogInformation("The last price creted at: {CreatedAt} and has current price = {Price}, " +
    //             //                        "discounted price = {DiscountedPrice}", price.CreateDate, price.CurrentPrice, price.DiscountedPrice);
    //             //
    //             // var result = await parser.ParseAsync(product.Link);
    //             //
    //             // if (result.Title is not null && (Math.Abs(price.CurrentPrice - (double)result?.Price!) > _eps
    //             //                                  || Math.Abs(price.DiscountedPrice - (double)result?.CardPrice!) > _eps))
    //             // {
    //             //     _logger.LogInformation("The product name is {Title} has price: {Price} and discounted price: {DiscountedPrice}",
    //             //         result.Title, result.Price, result.CardPrice);
    //             //     product.Prices.Add(new Price(result.Price ?? 0.0, result.CardPrice ?? 0.0));
    //             //     var message = $"\ud83d\udd14 Уведомление об изменении цены!\n" +
    //             //                   $"\ud83d\udcb0 Прошлая цена без скидки: {price.CurrentPrice} \u20bd \n" +
    //             //                   $"\ud83d\udcb3 Прошлая цена по скидке/карте: {price.DiscountedPrice} \u20bd \n" +
    //             //                   $"\n" +
    //             //                   $"\ud83d\udcbc Новая цена без скидки: *{result.Price}* \u20bd \n" +
    //             //                   $"\ud83d\udcb3 Новая цена по скидке/карте: *{result.CardPrice}* \u20bd \n" +
    //             //                   $"\n" +
    //             //                   $"\ud83d\udd17 [Ссылка на товар]({product.Link})";
    //             //     await _client.SendPriceChangingNotification(user.ChatId, message);
    //             // }
    //             //
    //             // if (result.Title is null)
    //             // {
    //             //     var message = "\u274c Проверьте наличие товара на маркетплейсе. Возможно его удалили или он " +
    //             //                    "просто закончился.\n" +
    //             //                    $"\ud83d\udd17 [Ссылка на товар]({product.Link})";
    //             //     await _client.SendPriceChangingNotification(user.ChatId, message);
    //             // }
    //             //
    //             // dbContext.Products.Update(product);
    //         }
    //
    //     }
    //     await dbContext.SaveChangesAsync();
    //     
    //     _logger.LogInformation("background working ended at {EndTime}", DateTime.Now);
    // }
}