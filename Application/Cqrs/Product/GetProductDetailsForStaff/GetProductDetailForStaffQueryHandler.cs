using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.GetProductDetailsForStaff;
internal class GetProductDetailForStaffQueryHandler
    : IRequestHandler<GetProductDetailsForStaffQuery, Result<IEnumerable<ProductDetailForStaffVm>>>
{
    private readonly IProductRepository _productRepository;

    public GetProductDetailForStaffQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<IEnumerable<ProductDetailForStaffVm>>> Handle(
        GetProductDetailsForStaffQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _productRepository.GetProductDetailsForStaff(request.ProductId);
            return Result<IEnumerable<ProductDetailForStaffVm>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ProductDetailForStaffVm>>.Error(ex.Message);
        }
    }
}
