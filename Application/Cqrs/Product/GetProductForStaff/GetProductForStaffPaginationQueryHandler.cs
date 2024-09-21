using Application.IRepositories;
using Application.ValueObjects.Pagination;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.GetProductForStaff;
internal class GetProductForStaffPaginationQueryHandler
    : IRequestHandler<GetProductForStaffPaginationQuery, Result<PaginationResponse<StaffAppProductVm>>>
{
    private readonly IProductRepository _productRepository;

    public GetProductForStaffPaginationQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<PaginationResponse<StaffAppProductVm>>> Handle(GetProductForStaffPaginationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _productRepository.GetStaffAppProducts(request);
            return Result<PaginationResponse<StaffAppProductVm>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<PaginationResponse<StaffAppProductVm>>.Error(ex.Message);
        }
    }
}
