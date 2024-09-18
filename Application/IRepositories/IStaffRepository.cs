using Application.Cqrs.Staff;
using Application.Cqrs.Staff.AddStaff;
using Application.Cqrs.Staff.GetListStaff;
using Application.Cqrs.Staff.UpdateStaffInfo;
using Application.Cqrs.Staff.UpdateStaffRole;
using Application.ValueObjects.Pagination;
using Domain.Primitives;

namespace Application.IRepositories;
public interface IStaffRepository
{
    Task<Result<string>> Authenticate(string username, string password);
    Task<PaginationResponse<StaffVm>> GetListStaff(GetListStaffQuery query);
    Task<bool> AddStaffAsync(AddStaffCommand command);
    Task<bool> UpdateStaffInfo(UpdateStaffInfoCommand command, CancellationToken cancellationToken);
    Task<StaffUpdateInfoVm> GetStaffUpdateInfo(Guid staffId, CancellationToken cancellationToken);
    Task<bool> UpdateStaffRole(UpdateStaffRoleCommand command);
}
