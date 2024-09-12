using StaffWebApp.Services.Product.Vms.Create;

namespace StaffWebApp.Services.Product;

public interface IProductService
{
    Task<bool> CreateProductAsync(
        ProductInfoVm info,
        List<ProductDetailVm> details,
        Dictionary<Guid, List<ImageVm>> imagesByColor);
}
