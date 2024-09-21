namespace StaffWebApp.Services.Product.Requests.Update;

public class UpdateProductInfoRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IEnumerable<Guid> CategoryIds { get; set; } = [];

    public UpdateProductInfoRequest()
    {
    }

    public UpdateProductInfoRequest(Guid id, string name, IEnumerable<Guid> categoryIds)
    {
        Id = id;
        Name = name;
        CategoryIds = categoryIds;
    }
}
