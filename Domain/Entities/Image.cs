namespace Domain.Entities;
public class Image
{
    public Guid Id { get; set; }
    public string ImagePath { get; set; } = string.Empty;

    public virtual ICollection<ProductImage>? ProductImages { get; set; }
}
