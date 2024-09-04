
using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.GetFeaturedProduct;

internal sealed class GetFeaturedProductQueryHandler : IRequestHandler<GetFeaturedProductQuery, Result>
{
    private readonly IProductRepository _productRepository;
    public GetFeaturedProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async  Task<Result> Handle(GetFeaturedProductQuery request, CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(1, cancellationToken);
            var result =  _productRepository.GetFeaturedProducts();
            return result;
        }
        catch (Exception ex)
        {
            return Result<List<ProductCustomerAppVm>>.Error(ex.Message);
        }
    }
}
