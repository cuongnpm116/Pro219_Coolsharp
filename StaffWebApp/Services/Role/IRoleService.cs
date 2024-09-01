using StaffWebApp.Services.Role.Vms;
using WebAppIntegrated.ApiResponse;

namespace StaffWebApp.Services.Role;
public interface IRoleService
{
    Task<Result<List<RoleVm>>> GetRoles();
    Task<Result<List<Guid>>> GetRoleIdsByStaffId(Guid staffId);
}
