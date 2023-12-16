using PriceTracker.Domain.Common;

namespace PriceTracker.Domain.Entities;

public class Price : BaseEntity<long>
{
    public double CurrentPrice { get; set; }
    public double DiscountedPrice { get; set; }

    public long ProductId { get; set; }
    public virtual Product Product { get; set; }

    public Price(double currentPrice, double discountedPrice)
    {
        CurrentPrice = currentPrice;
        DiscountedPrice = discountedPrice;
    }
}