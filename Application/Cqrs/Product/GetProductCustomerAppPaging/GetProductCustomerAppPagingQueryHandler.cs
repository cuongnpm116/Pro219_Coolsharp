

using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.GetProductCustomerAppPaging;

internal sealed class GetProductCustomerAppPagingQueryHandler : IRequestHandler<GetProductCustomerAppPagingQuery, Result>
{
    private readonly IProductRepository _productRepository;
    public GetProductCustomerAppPagingQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(GetProductCustomerAppPagingQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _productRepository.GetProductForShowOnCustomerApp(request);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
