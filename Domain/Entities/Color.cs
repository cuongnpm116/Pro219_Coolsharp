using Domain.Enums;

namespace Domain.Entities;
public class Color
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Status Status { get; set; } = Status.Active;

    public virtual ICollection<ProductDetail>? ProductDetails { get; set; }
}
