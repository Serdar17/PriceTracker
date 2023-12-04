using PriceTracker.Domain.Common;

namespace PriceTracker.Domain.Entities;

public class Price : BaseEntity<long>
{
    public decimal CurrentPrice { get; set; }
    public decimal DiscountedPrice { get; set; }

    public long ProductId { get; set; }
    public virtual Product Product { get; set; }
}