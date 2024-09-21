namespace StaffWebApp.Services.Product.Vms;

public class ProductVm
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IEnumerable<string> Categories { get; set; } = [];
    public int TotalStock { get; set; }
}
