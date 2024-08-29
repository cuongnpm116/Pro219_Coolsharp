using WebAppIntegrated.Enum;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Staff.Requests;
public class GetListStaffRequest : PaginationRequest
{
    public Status Status { get; set; } = Status.None;
}
