using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Staff.UpdateStaffRole;
internal sealed class UpdateStaffRoleCommandHandler
    : IRequestHandler<UpdateStaffRoleCommand, Result>
{
    private readonly IStaffRepository _staffRepository;

    public UpdateStaffRoleCommandHandler(IStaffRepository staffRepository)
    {
        _staffRepository = staffRepository;
    }

    public async Task<Result> Handle(UpdateStaffRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _staffRepository.UpdateStaffRole(request);
            return Result<bool>.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
