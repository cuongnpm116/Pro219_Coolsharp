using StaffWebApp.Services.Product.Vms.Create;
using StaffWebApp.Services.Product.Requests;
using StaffWebApp.Services.Product.Vms;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Product;

public interface IProductService
{
    Task<bool> CreateProductAsync(
        ProductInfoVm info,
        List<ProductDetailVm> details,
        Dictionary<Guid, List<ImageVm>> imagesByColor);
    Task<Result<PaginationResponse<ProductVm>>> ShowProduct(ProductPaginationRequest request);
}
