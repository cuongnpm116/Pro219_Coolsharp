namespace Application.Cqrs.Product.Create;

public class CreateProductImageRequest
{
    public Guid ProductDetailId { get; set; }
    public Guid ImageId { get; set; }
}