using StaffWebApp.Services.Size.Requests;
using StaffWebApp.Services.Size.Vms;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Size;

public interface ISizeService
{
    Task<Result<PaginationResponse<SizeVm>>> GetSizesWithPagination(SizePaginationRequest request);
    Task<Result<SizeVm>> GetSizeById(Guid id);
    Task<Result<bool>> CreateSize(CreateSizeRequest request);
    Task<Result<bool>> UpdateSize(UpdateSizeRequest request);
    Task<IEnumerable<SizeForSelectVm>> GetSizeForSelectVms();
}
