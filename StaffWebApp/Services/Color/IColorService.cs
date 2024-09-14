using StaffWebApp.Services.Color.Requests;
using StaffWebApp.Services.Color.Vms;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Color;

public interface IColorService
{
    Task<Result<PaginationResponse<ColorVm>>> GetColorsWithPagination(ColorPaginationRequest request);
    Task<Result<ColorVm>> GetColorById(Guid id);
    Task<Result<bool>> CreateColor(CreateColorRequest request);
    Task<Result<bool>> UpdateColor(UpdateColorRequest request);
    Task<IEnumerable<ColorForSelectVm>> GetColorForSelectVms();
}
