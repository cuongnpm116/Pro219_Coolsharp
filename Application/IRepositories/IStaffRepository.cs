using Application.Cqrs.Staff;
using Application.ValueObjects.Pagination;
using Domain.Enums;

namespace Application.IRepositories;
public interface IStaffRepository
{
    Task<PaginationResponse<StaffVm>> GetListStaff(int pageNumber, int pageSize, string searchString, Status status);
}
