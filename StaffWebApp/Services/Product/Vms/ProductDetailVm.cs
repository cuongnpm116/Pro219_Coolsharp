namespace StaffWebApp.Services.Product.Vms;

public class ProductDetailVm
{
    public Guid Id { get; set; }
    public decimal Price { set; get; }
    public decimal OriginalPrice { set; get; }
    public int Stock { set; get; }
    public string Color { set; get; }
    public int Size { get; set; }
    public List<string>? Images { get; set; } = new List<string>();
}
