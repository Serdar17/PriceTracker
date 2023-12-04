using PriceTracker.Domain.Common;
using PriceTracker.Domain.Enums;

namespace PriceTracker.Domain.Entities;

public class User : BaseEntity<long>
{
    public string FirstName { get; set; } = default!;
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public long ChatId { get; set; }
    public UserStatus Status { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}