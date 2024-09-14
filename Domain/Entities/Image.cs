namespace Domain.Entities;
public class Image
{
    public Image()
    {
    }

    public Image(Guid id, string path)
    {
        Id = id;
        ImagePath = path;
    }
    public Guid Id { get; set; }
    public string ImagePath { get; set; } = string.Empty;

    public virtual ICollection<ProductImage>? ProductImages { get; set; }
}
