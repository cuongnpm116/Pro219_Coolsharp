using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Size.Create;
internal sealed class CreateSizeCommandHandler : IRequestHandler<CreateSizeCommand, Result>
{

    private readonly ISizeRepository _sizeRepository;
    public CreateSizeCommandHandler(ISizeRepository sizeRepository)
    {
        _sizeRepository = sizeRepository;
    }


    public async Task<Result> Handle(CreateSizeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _sizeRepository.AddSize(request);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);

        }
    }
}
