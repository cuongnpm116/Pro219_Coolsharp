using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Role.Update;
internal sealed class UpdateRoleCommandHandler
    : IRequestHandler<UpdateRoleCommand, Result>
{
    private readonly IRoleRepository _roleRepository;

    public UpdateRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _roleRepository.UpdateRole(request);
            if (result is true)
            {
                return Result<bool>.Success(result);
            }
            return Result<bool>.Error("Không tìm thấy vai trò");
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
