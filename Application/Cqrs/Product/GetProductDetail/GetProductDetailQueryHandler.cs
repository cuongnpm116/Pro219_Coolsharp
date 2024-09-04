
using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.GetProductDetail;

internal sealed class GetProductDetailQueryHandler : IRequestHandler<GetProductDetailQuery, Result>
{
    private IProductRepository _productRepository;
    public GetProductDetailQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<Result> Handle(GetProductDetailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _productRepository.GetProductDetailForShowOnCustomerApp(request.ProductId);    
            return result;
        }
        catch (Exception ex)
        {
                return Result<ProductDetailVm>.Error(ex.Message);
        }
    }
}
