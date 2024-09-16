using StaffWebApp.Services.Product.Dtos;
using StaffWebApp.Services.Product.Requests;
using StaffWebApp.Services.Product.Vms;
using StaffWebApp.Services.Product.Vms.Create;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Product;

public interface IProductService
{
    Task<bool> CreateProductAsync(
        ProductInfoVm info,
        List<ProductDetailVm> details,
        Dictionary<Guid, List<ImageVm>> imagesByColor);
    Task<PaginationResponse<ProductVm>> GetProducts(GetProductPaginationRequest request);
    Task<ProductInfoDto> GetProductInfo(Guid productId);
    Task<bool> UpdateProductInfoAsync(Guid productId, ProductInfoVm info);
    Task<IEnumerable<DetailVm>> GetDetails(Guid productId);
    Task<bool> UpdateDetailAsync(DetailVm detail);
}
