using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Staff.UpdateStaffInfo;
internal sealed class UpdateStaffInfoCommandHandler : IRequestHandler<UpdateStaffInfoCommand, Result>
{
    private readonly IStaffRepository _staffRepository;

    public UpdateStaffInfoCommandHandler(IStaffRepository staffRepository)
    {
        _staffRepository = staffRepository;
    }

    public async Task<Result> Handle(UpdateStaffInfoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _staffRepository.UpdateStaffInfo(request, cancellationToken);
            return Result<bool>.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
