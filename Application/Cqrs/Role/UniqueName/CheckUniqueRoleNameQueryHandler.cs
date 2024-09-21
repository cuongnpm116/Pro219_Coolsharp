using Application.IRepositories;
using MediatR;

namespace Application.Cqrs.Role.UniqueName;
internal class CheckUniqueRoleNameQueryHandler
    : IRequestHandler<CheckUniqueRoleNameQuery, bool>
{
    private readonly IRoleRepository _roleRepository;

    public CheckUniqueRoleNameQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<bool> Handle(CheckUniqueRoleNameQuery request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _roleRepository.IsUniqueName(request.Name);
            return result;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
