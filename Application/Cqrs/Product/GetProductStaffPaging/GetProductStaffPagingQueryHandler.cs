using Application.IRepositories;
using Domain.Primitives;
using MediatR;


namespace Application.Cqrs.Product.GetProductStaffPaging;

internal sealed class GetProductStaffPagingQueryHandler : IRequestHandler<GetProductStaffPagingQuery, Result>
{
    private readonly IProductRepository _productRepository;
    public GetProductStaffPagingQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(GetProductStaffPagingQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _productRepository.GetProductForStaffView(request);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
