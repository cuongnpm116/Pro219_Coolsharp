using StaffWebApp.Services.Staff.Requests;
using StaffWebApp.Services.Staff.Vms;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Staff;
public interface IStaffService
{
    Task<Result<PaginationResponse<StaffVm>>> GetListStaffWithPagination(GetListStaffRequest request);
    Task<Result<bool>> AddStaff(AddStaffRequest request);
    Task<Result<UpdateStaffInfoVm>> GetStaffInfoToUpdate(Guid staffId);
    Task<Result<bool>> UpdateStaffInfo(UpdateStaffInfoVm request);
}
