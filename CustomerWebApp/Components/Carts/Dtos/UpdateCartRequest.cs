namespace CustomerWebApp.Components.Carts.Dtos;

public class UpdateCartRequest
{
    public Guid CartId { get; set; }
    public Guid ProductDetailId { get; set; }
    public int Quantity { get; set; }
}
