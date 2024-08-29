using StaffWebApp.Components.Staff.Vms;
using StaffWebApp.Services.Staff.Requests;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Staff;
public interface IStaffService
{
    Task<Result<PaginationResponse<StaffVm>>> GetListStaffWithPagination(GetListStaffRequest request);
}
