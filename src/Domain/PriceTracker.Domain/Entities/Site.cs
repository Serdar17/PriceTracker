using PriceTracker.Domain.Base;

namespace PriceTracker.Domain.Entities;

public class Site : BaseEntity<long>
{
    public string Name { get; set; }
    public string Link { get; set; }

    public long UserId { get; set; }
    public virtual User User { get; set; }

    public ICollection<Price> Prices { get; set; } = new List<Price>();


    public Site(string name, string link)
    {
        Name = name;
        Link = link;
    }
}