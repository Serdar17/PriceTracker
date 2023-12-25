using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PriceTracker.Infrastructure.Context.Setup;

public static class DbSeeder
{
    private static IServiceScope ServiceScope(IServiceProvider serviceProvider) => 
        serviceProvider.GetService<IServiceScopeFactory>()!.CreateScope();
    private static AppDbContext DbContext(IServiceProvider serviceProvider) => 
        ServiceScope(serviceProvider).ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();

    public static void Execute(IServiceProvider serviceProvider, bool addDemoData)
    {
        using var scope = ServiceScope(serviceProvider);
        ArgumentNullException.ThrowIfNull(scope);

        if (addDemoData)
        {
            Task.Run(async () =>
            {
                await ConfigureDemoData(serviceProvider);
            });
        }
    }

    private static async Task ConfigureDemoData(IServiceProvider serviceProvider)
    {
        // await AddBooks(serviceProvider);
    }

    // private static async Task AddBooks(IServiceProvider serviceProvider)
    // {
    //     await using var context = DbContext(serviceProvider);
    //
    //     if (context.Books.Any() || context.Authors.Any() || context.Categories.Any())
    //         return;
    //
    //     var a1 = new Entities.Author()
    //     {
    //         Name = "Mark Twen",
    //         Detail = new Entities.AuthorDetail()
    //         {
    //             Country = "USA",
    //             Family = "",
    //         }
    //     };
    //     context.Authors.Add(a1);
    //
    //     var a2 = new Entities.Author()
    //     {
    //         Name = "Lev Tolstoy",
    //         Detail = new Entities.AuthorDetail()
    //         {
    //             Country = "Russia",
    //             Family = "",
    //         }
    //     };
    //     context.Authors.Add(a2);
    //
    //     var c1 = new Entities.Category()
    //     {
    //         Title = "Classic"
    //     };
    //     context.Categories.Add(c1);
    //
    //     context.Books.Add(new Entities.Book()
    //     {
    //         Title = "Tom Soyer",
    //         description = "description description description description ",
    //         Author = a1,
    //         Categories = new List<Entities.Category>() { c1 },
    //     });
    //
    //     context.Books.Add(new Entities.Book()
    //     {
    //         Title = "War and peace",
    //         description = "description description description description ",
    //         Author = a2,
    //         Categories = new List<Entities.Category>() { c1 },
    //     });
    //
    //     context.SaveChanges();
    // }
}