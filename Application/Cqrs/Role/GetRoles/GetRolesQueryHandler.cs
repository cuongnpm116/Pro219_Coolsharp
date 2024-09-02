using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Role.GetRoles;
internal sealed class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, Result>
{
    private readonly IRoleRepository _roleRepository;

    public GetRolesQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var roles = await _roleRepository.GetRoles();
            return Result<IList<RoleVm>>.Success(roles);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
