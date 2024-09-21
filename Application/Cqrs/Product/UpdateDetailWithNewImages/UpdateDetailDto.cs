namespace Application.Cqrs.Product.UpdateDetailWithNewImages;
public class UpdateDetailDto
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public decimal OriginalPrice { get; set; }
    public int Stock { get; set; }
    public Guid ColorId { get; set; }
    public Guid SizeId { get; set; }
}
