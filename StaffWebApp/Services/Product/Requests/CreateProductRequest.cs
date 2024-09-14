namespace StaffWebApp.Services.Product.Requests;
public class CreateProductRequest
{
    public string Name { get; set; }
    public IEnumerable<Guid> CategoryIds { get; set; }
    public List<CreateImageRequest> Images { get; set; }
    public List<CreateProductDetailRequest> Details { get; set; }
    public List<CreateProductImageRequest> DetailImages { get; set; }

    public CreateProductRequest(
        string name,
        IEnumerable<Guid> categoryIds,
        List<CreateImageRequest> images,
        List<CreateProductDetailRequest> details,
        List<CreateProductImageRequest> detailImages)
    {
        Name = name;
        CategoryIds = categoryIds;
        Images = images;
        Details = details;
        DetailImages = detailImages;
    }
}
