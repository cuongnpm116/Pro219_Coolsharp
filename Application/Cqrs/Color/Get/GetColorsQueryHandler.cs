using Application.IRepositories;
using Application.ValueObjects.Pagination;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Color.Get;
internal sealed class GetColorsQueryHandler
    : IRequestHandler<GetColorsQuery, Result<PaginationResponse<ColorVm>>>
{
    private readonly IColorRepository _colorRepository;
    public GetColorsQueryHandler(IColorRepository colorRepository)
    {
        _colorRepository = colorRepository;
    }

    public async Task<Result<PaginationResponse<ColorVm>>> Handle(GetColorsQuery request, CancellationToken cancellationToken)
    {
        Result<PaginationResponse<ColorVm>> result = await _colorRepository.GetColors(request);
        return result;
    }


}




