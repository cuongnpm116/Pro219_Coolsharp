namespace Application.Cqrs.Product.GetProductForStaff;
public class StaffAppProductVm
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IEnumerable<string> Categories { get; set; } = [];
    public int TotalStock { get; set; }
}
