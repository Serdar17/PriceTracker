using PriceTracker.Domain.Common;

namespace PriceTracker.Domain.Entities;

public class Product : BaseEntity<long>
{
    public string MarketPlaceName { get; set; }
    public string Title { get; set; }
    public string Link { get; set; }
    
    public string? Currency { get; set; }

    public long UserId { get; set; }
    public virtual User User { get; set; }

    public virtual ICollection<Price> Prices { get; set; } = new List<Price>();

    public Product(string marketPlaceName, string title, string link, string? currency = null)
    {
        MarketPlaceName = marketPlaceName;
        Title = title;
        Link = link;
        Currency = currency;
    }
}