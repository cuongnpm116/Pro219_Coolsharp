using StaffWebApp.Services.Product.Requests;
using StaffWebApp.Services.Product.Vms;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Product;

public interface IProductService
{
    Task<Result<PaginationResponse<ProductVm>>> ShowProduct(ProductPaginationRequest request);
}
