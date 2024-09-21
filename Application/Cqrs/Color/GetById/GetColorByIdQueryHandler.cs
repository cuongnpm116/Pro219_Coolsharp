using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Color.GetById;
internal sealed class GetColorByIdQueryHandler : IRequestHandler<GetColorByIdQuery, Result>
{
    private readonly IColorRepository _colorRepository;
    public GetColorByIdQueryHandler(IColorRepository colorRepository)
    {
        _colorRepository = colorRepository;
    }


    public async Task<Result> Handle(GetColorByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Result<ColorVm> result = await _colorRepository.GetColorById(request.coloId);

            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }


}