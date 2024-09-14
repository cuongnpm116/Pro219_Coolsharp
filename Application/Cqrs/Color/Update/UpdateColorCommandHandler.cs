using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Color.Update;
internal sealed class UpdateColorCommandHandler : IRequestHandler<UpdateColorCommand, Result>
{
    private readonly IColorRepository _colorRepository;
    public UpdateColorCommandHandler(IColorRepository colorRepository)
    {
        _colorRepository = colorRepository;
    }
    public async Task<Result> Handle(UpdateColorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _colorRepository.UpdateColor(request);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);

        }
    }
}
