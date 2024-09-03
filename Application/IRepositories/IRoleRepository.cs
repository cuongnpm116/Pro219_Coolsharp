using Application.Cqrs.Role.GetAll;
using Application.Cqrs.Role.GetRoleIdsByStaffId;
using Application.Cqrs.Role.GetWithPagination;
using Application.Cqrs.Role.Update;
using Application.ValueObjects.Pagination;

namespace Application.IRepositories;
public interface IRoleRepository
{
    Task<IReadOnlyList<Guid>> GetRoleIdsByStaffId(GetRoleIdsByStaffIdQuery command);
    Task<IReadOnlyList<RoleVmForGetAll>> GetAllRoles();
    Task<PaginationResponse<RoleVm>> GetRolesWithPagination(GetRolesWithPaginationQuery query);
    Task<bool> UpdateRole(UpdateRoleCommand command);
}
