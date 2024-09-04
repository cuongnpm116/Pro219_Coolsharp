using CustomerWebApp.Service.Product.Dtos;
using CustomerWebApp.Service.Product.ViewModel;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Pagination;

namespace CustomerWebApp.Service.Product;

public interface IProductService
{
    Task<Result<PaginationResponse<ProductVm>>> ShowProductOnCustomerAppVm(ProductPaginationRequest request);
    Task<Result<ProductDetailVm>> ShowProductDetail(Guid productId);
    Task<Result<ProductPriceVm>> GetProductDetailPrice(Guid productId, Guid colorId, Guid sizeId);
    Task<Result<int>> GetProductDetailStock(Guid productId, Guid colorId, Guid sizeId);
    Task<Result<Guid>> GetProductDetailId(Guid productId, Guid colorId, Guid sizeId);
    Task<Result<List<ProductVm>>> GetFeaturedProducts();
    Task<Result<Dictionary<Guid, List<string>>>> GetDetailImage(Guid productId);
}

