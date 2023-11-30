using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceTracker.Domain.Common;

public abstract class BaseEntity<T>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public T Id { get; init; }
    public DateTime CreateDate { get; init; } = DateTime.Now;
    public DateTime UpdateDate { get; set; } = DateTime.Now;
}