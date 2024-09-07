using StaffWebApp.Services.Product.Dtos;

namespace StaffWebApp.Services.Product.Requests;

public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public List<Guid> CategoryIds { get; set; } = [];
    public List<CreateProductDetailDto> Details { get; set; } = [];
    public List<CreateImageDto> Images { get; set; } = [];
    public List<CreateProductImageDto> ProductImages { get; set; } = [];
}
