using Application.Cqrs.Product;
using Application.Cqrs.Product.AddDetail;
using Application.Cqrs.Product.AddDetailWithNewImages;
using Application.Cqrs.Product.CheckUpdateDetail;
using Application.Cqrs.Product.Create;
using Application.Cqrs.Product.GetInfo;
using Application.Cqrs.Product.GetProductCustomerAppPaging;
using Application.Cqrs.Product.GetProductDetailsForStaff;
using Application.Cqrs.Product.GetProductForStaff;
using Application.Cqrs.Product.UpdateDetailWithNewImages;
using Application.Cqrs.Product.UpdateProductDetail;
using Application.Cqrs.Product.UpdateProductInfo;
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
    Task<bool> CreateProductAsync(CreateProductCommand request);
    Task<ProductInfoDto> GetProductInfo(Guid productId);
    Task<PaginationResponse<StaffAppProductVm>> GetStaffAppProducts(GetProductForStaffPaginationQuery request);
    Task<bool> UpdateProductInfoAsync(UpdateProductInfoCommand request);
    Task<IEnumerable<ProductDetailForStaffVm>> GetProductDetailsForStaff(Guid productId);
    Task<bool> UpdateProductDetail(UpdateProductDetailCommand request);
    Task<Guid> CheckUpdateDetailExist(CheckUpdateDetailQuery request);
    Task<bool> CheckColorExistedInProduct(Guid productId, Guid colorId);
    Task<bool> UpdateDetailWithNewImage(UpdateDetailWithNewImageCommand request);
    Task<bool> AddDetailWithNewImages(AddDetailWithNewImagesCommand request);
    Task<bool> AddDetailAsync(AddDetailCommand request);
}
