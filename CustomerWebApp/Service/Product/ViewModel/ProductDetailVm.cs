namespace CustomerWebApp.Service.Product.ViewModel;

public class ProductDetailVm
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public IDictionary<Guid, string>? ColorsDictionary { get; set; }
    public IDictionary<Guid, int>? SizesDictionary { get; set; }
}
