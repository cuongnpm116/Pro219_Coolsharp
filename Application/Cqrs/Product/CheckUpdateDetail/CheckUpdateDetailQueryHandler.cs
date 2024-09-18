using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.CheckUpdateDetail;
internal class CheckUpdateDetailQueryHandler
    : IRequestHandler<CheckUpdateDetailQuery, Result<Guid>>
{
    private readonly IProductRepository _productRepository;

    public CheckUpdateDetailQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<Guid>> Handle(CheckUpdateDetailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Guid existedDetail = await _productRepository.CheckUpdateDetailExist(request);
            return Result<Guid>.Success(existedDetail);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Error(ex.Message);
        }
    }
}
