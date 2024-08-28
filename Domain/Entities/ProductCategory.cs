using Domain.Enums;

namespace Domain.Entities;
public class ProductCategory
{
    public Guid Id { get; set; }

    public Guid CategoryId { get; set; }
    public Guid ProductId { get; set; }
    public Status Status { get; set; } = Status.Active;

    public virtual Category? Category { get; set; }
    public virtual Product? Product { get; set; }
}
