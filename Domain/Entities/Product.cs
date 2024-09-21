using Domain.Enums;

namespace Domain.Entities;
public class Product
{
    public Product()
    {
    }

    public Product(string name)
    {
        Name = name;
    }

    public Guid Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string Name { get; set; } = string.Empty;
    public Status Status { get; set; } = Status.Active;

    public virtual ICollection<ProductCategory>? ProductCategories { get; set; }
    public virtual ICollection<ProductDetail>? ProductDetails { get; set; }
}
