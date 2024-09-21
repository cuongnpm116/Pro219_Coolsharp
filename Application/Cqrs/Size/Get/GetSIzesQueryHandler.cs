using Application.IRepositories;
using Application.ValueObjects.Pagination;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Size.Get;
internal sealed class GetSIzesQueryHandler
    : IRequestHandler<GetSizesQuery, Result<PaginationResponse<SizeVm>>>
{
    private readonly ISizeRepository _sizeRepository;
    public GetSIzesQueryHandler(ISizeRepository sizeRepository)
    {
        _sizeRepository = sizeRepository;
    }


    public async Task<Result<PaginationResponse<SizeVm>>> Handle(GetSizesQuery request, CancellationToken cancellationToken)
    {
        var result = await _sizeRepository.GetSizes(request);
        return result;
    }


}
