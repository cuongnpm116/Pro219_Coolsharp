namespace CustomerWebApp.Service.Order.ViewModel;

public class OrderDetailVm
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductDetailId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ImagePath { get; set; }
    public int SizeNumber { get; set; }
    public string ColorName { get; set; }
}
