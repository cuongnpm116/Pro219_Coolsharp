namespace CustomerWebApp.Components.Carts.ViewModel;

public class CartItemVm
{
    public Guid CartId { get; set; }
    public Guid CartItemId { get; set; }
    public Guid ProductDetailId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public string ImagePath { get; set; } = "";
    public int ProductQuantity { get; set; }
    public int SizeNumber { get; set; }
    public string ColorName { get; set; } = "";
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal AmountOfMoney => Quantity * UnitPrice;
    public bool IsChecked { get; set; } = false;
}
