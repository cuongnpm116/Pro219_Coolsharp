using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Size.GetForSelect;
internal sealed class GetSizeForSelectQueryHanlder
    : IRequestHandler<GetSizeForSelectQuery, Result<IReadOnlyList<SizeForSelectVm>>>
{
    private readonly ISizeRepository _sizeRepo;

    public GetSizeForSelectQueryHanlder(ISizeRepository sizeRepo)
    {
        _sizeRepo = sizeRepo;
    }

    public async Task<Result<IReadOnlyList<SizeForSelectVm>>> Handle(
        GetSizeForSelectQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _sizeRepo.GetSizeForSelectVms();
            return Result<IReadOnlyList<SizeForSelectVm>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<IReadOnlyList<SizeForSelectVm>>.Error(ex.Message);
        }
    }
}
