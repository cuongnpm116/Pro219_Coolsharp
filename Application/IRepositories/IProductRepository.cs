using Application.Cqrs.Product;
using Application.Cqrs.Product.Create;
using Application.Cqrs.Product.GetProductCustomerAppPaging;
using Application.Cqrs.Product.GetProductStaffPaging;
using Application.ValueObjects.Pagination;
using Domain.Primitives;

namespace Application.IRepositories;
public interface IProductRepository
{
    Task<Result<PaginationResponse<ProductCustomerAppVm>>> GetProductForShowOnCustomerApp(GetProductCustomerAppPagingQuery request);
    Task<Result<ProductDetailVm>> GetProductDetailForShowOnCustomerApp(Guid productId);
    Task<Result<ProductPriceVm>> GetProductDetailPrice(Guid productId, Guid colorId, Guid sizeId);
    Task<Result<int>> GetProductDetailStock(Guid productId, Guid colorId, Guid sizeId);
    Task<Result<Guid>> GetProductDetailId(Guid productId, Guid colorId, Guid sizeId);
    Result<List<ProductCustomerAppVm>> GetFeaturedProducts();
    Result<Dictionary<Guid, List<string>>> GetDetailImage(Guid productId);
    //staff
    Task<Result<PaginationResponse<ProductStaffVm>>> GetProductForStaffView(GetProductStaffPagingQuery request);
    Task<bool> CreateProductAsync(CreateProductCommand request);
}
