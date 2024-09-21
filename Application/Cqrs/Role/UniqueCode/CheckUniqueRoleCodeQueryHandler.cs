using Application.IRepositories;
using MediatR;

namespace Application.Cqrs.Role.UniqueCode;
internal class CheckUniqueRoleCodeQueryHandler
    : IRequestHandler<CheckUniqueRoleCodeQuery, bool>
{
    private readonly IRoleRepository _roleRepository;

    public CheckUniqueRoleCodeQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<bool> Handle(CheckUniqueRoleCodeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _roleRepository.IsUniqueCode(request.Code);
            return result;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
