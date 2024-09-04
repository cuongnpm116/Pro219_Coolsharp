using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Size.GetById;
internal sealed class GetSizeByIdQueryHandler : IRequestHandler<GetSizeByIdQuery, Result>
{
    private readonly ISizeRepository _sizeRepository;
    public GetSizeByIdQueryHandler(ISizeRepository sizeRepository)
    {
        _sizeRepository = sizeRepository;
    }


    public async Task<Result> Handle(GetSizeByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Result<SizeVm> result = await _sizeRepository.GetSizeById(request.sizeId);

            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }


}