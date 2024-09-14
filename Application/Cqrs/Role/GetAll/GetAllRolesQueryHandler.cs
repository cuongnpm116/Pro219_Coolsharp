using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Role.GetAll;
internal sealed class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, Result>
{
    private readonly IRoleRepository _roleRepository;

    public GetAllRolesQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var roles = await _roleRepository.GetAllRoles();
            return Result<IReadOnlyList<RoleVmForGetAll>>.Success(roles);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
