using StaffWebApp.Services.Product.Dtos;

namespace StaffWebApp.Services.Product.Requests.Create;
public class CreateProductRequest
{
    public string Name { get; private set; }
    public IEnumerable<Guid> CategoryIds { get; private set; }
    public List<ImageDto> Images { get; private set; }
    public List<ProductDetailDto> Details { get; private set; }
    public List<CreateProductImageRequest> DetailImages { get; private set; }

    public CreateProductRequest(
        string name,
        IEnumerable<Guid> categoryIds,
        List<ImageDto> images,
        List<ProductDetailDto> details,
        List<CreateProductImageRequest> detailImages)
    {
        Name = name;
        CategoryIds = categoryIds;
        Images = images;
        Details = details;
        DetailImages = detailImages;
    }
}
