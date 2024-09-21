using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Customer;

public interface ICustomerService
{
    Task<Result<PaginationResponse<CustomerVm>>> GetCustomersWithPagination(GetCustomerWithPaginationRequest request);
}
