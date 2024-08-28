using Domain.Enums;

namespace Domain.Entities;
public class Size
{
    public Guid Id { get; set; }
    public int SizeNumber { get; set; }
    public Status Status { get; set; } = Status.Active;

    public virtual ICollection<ProductDetail>? ProductDetails { get; set; }
}
