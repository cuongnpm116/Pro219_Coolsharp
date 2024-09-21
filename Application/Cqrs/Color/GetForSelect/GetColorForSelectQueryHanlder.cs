using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Color.GetForSelect;
internal sealed class GetColorForSelectQueryHanlder
    : IRequestHandler<GetColorForSelectQuery, Result<IReadOnlyList<ColorForSelectVm>>>
{
    private readonly IColorRepository _colorRepo;

    public GetColorForSelectQueryHanlder(IColorRepository colorRepo)
    {
        _colorRepo = colorRepo;
    }

    public async Task<Result<IReadOnlyList<ColorForSelectVm>>> Handle(
        GetColorForSelectQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _colorRepo.GetColorForSelectVms();
            return Result<IReadOnlyList<ColorForSelectVm>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<IReadOnlyList<ColorForSelectVm>>.Error(ex.Message);
        }
    }
}
