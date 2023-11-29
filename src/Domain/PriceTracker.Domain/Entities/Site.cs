using PriceTracker.Domain.Base;

namespace PriceTracker.Domain.Entities;

public class Site : BaseEntity<long>
{
    public string Name { get; set; } = default!;
    public string Link { get; set; } = default!;

    public long UserId { get; set; }
    public virtual User User { get; set; }

    public ICollection<Price> Prices { get; set; } = new List<Price>();
}