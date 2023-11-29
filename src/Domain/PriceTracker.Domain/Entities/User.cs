using PriceTracker.Domain.Base;
using PriceTracker.Domain.Enums;

namespace PriceTracker.Domain.Entities;

public class User : BaseEntity<long>
{
    public string FirstName { get; set; } = default!;
    public string? LastName { get; set; }
    public string? Username { get; set; }

    public ICollection<Site> Sites { get; set; } = new List<Site>();
}