using Application.Cqrs.Role;
using Application.Cqrs.Role.GetRoleIdsByStaffId;

namespace Application.IRepositories;
public interface IRoleRepository
{
    Task<IList<RoleVm>> GetRoles();
    Task<IList<Guid>> GetRoleIdsByStaffId(GetRoleIdsByStaffIdQuery command);
}
