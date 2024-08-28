using Domain.Enums;

namespace Domain.Entities;
public class Product
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public string Name { get; set; } = string.Empty;
    public Status Status { get; set; }

    public virtual ICollection<ProductCategory>? ProductCategories { get; set; }
    public virtual ICollection<ProductDetail>? ProductDetails { get; set; }
}
