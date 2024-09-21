using StaffWebApp.Services.Role.Requests;
using StaffWebApp.Services.Role.Vms;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Role;
public interface IRoleService
{
    Task<Result<List<RoleVmForGetAll>>> GetRoles();
    Task<Result<List<Guid>>> GetRoleIdsByStaffId(Guid staffId);
    Task<Result<PaginationResponse<RoleVm>>> GetRolesWithPagination(GetRolesWithPaginationRequest request);
    Task<Result<bool>> UpdateRole(RoleVm role);
    Task<bool> CreateRole(CreateRoleRequest request);
    Task<bool> CheckUniqueRoleCode(string code);
    Task<bool> CheckUniqueRoleName(string name);
}
