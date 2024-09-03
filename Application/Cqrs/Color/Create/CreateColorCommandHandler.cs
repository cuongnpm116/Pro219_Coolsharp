using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Color.Create;
internal sealed class CreateColorCommandHandler : IRequestHandler<CreateColorCommand, Result>
{

    private readonly IColorRepository _colorRepository;
    public CreateColorCommandHandler(IColorRepository colorRepository)
    {
        _colorRepository = colorRepository;
    }


    public async Task<Result> Handle(CreateColorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _colorRepository.AddColor(request);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);

        }
    }
}
