namespace StaffWebApp.Services.Product.Dtos;
public class ProductInfoDto
{
    public string Name { get; set; }
    public IEnumerable<Guid> CategoryIds { get; set; }
}
