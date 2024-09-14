using Domain.Enums;

namespace Domain.Entities;
public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Status Status { get; set; } = Status.Active;

    public virtual ICollection<ProductCategory>? ProductCategories { get; set; }
}
