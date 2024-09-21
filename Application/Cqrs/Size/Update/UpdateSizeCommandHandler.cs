using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Size.Update;
internal sealed class UpdateSizeCommandHandler : IRequestHandler<UpdateSizeCommand, Result>
{
    private readonly ISizeRepository _sizeRepository;
    public UpdateSizeCommandHandler(ISizeRepository sizeRepository)
    {
        _sizeRepository = sizeRepository;
    }
    public async Task<Result> Handle(UpdateSizeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _sizeRepository.UpdateSize(request);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);

        }
    }
}