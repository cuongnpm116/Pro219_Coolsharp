namespace Application.Cqrs.Product.GetInfo;
public class ProductInfoDto
{
    public string Name { get; set; } = string.Empty;
    public IEnumerable<Guid> CategoryIds { get; set; }
}
