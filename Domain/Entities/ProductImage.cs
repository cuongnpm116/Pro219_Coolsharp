namespace Domain.Entities;
public class ProductImage
{
    public Guid Id { get; set; }
    public Guid ImageId { get; set; }
    public Guid ProductDetailId { get; set; }

    public virtual Image? Image { get; set; }
    public virtual ProductDetail? ProductDetail { get; set; }
}
