using PriceTracker.Domain.Base;

namespace PriceTracker.Domain.Entities;

public class Price : BaseEntity<long>
{
    public decimal CurrentPrice { get; set; }
    public decimal DiscountedPrice { get; set; }

    public long SiteId { get; set; }
    public virtual Site Site { get; set; }
}