using Application.IRepositories;
using MediatR;

namespace Application.Cqrs.Role.Create;
internal class CreateRoleCommandHandler
    : IRequestHandler<CreateRoleCommand, bool>
{
    private IRoleRepository _roleRepo;

    public CreateRoleCommandHandler(IRoleRepository roleRepo)
    {
        _roleRepo = roleRepo;
    }

    public async Task<bool> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _roleRepo.CreateRole(request.Code, request.Name);
            return result;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
