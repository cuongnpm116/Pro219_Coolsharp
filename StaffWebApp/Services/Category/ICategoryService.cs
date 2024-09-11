using StaffWebApp.Service.Category.ViewModel;
using WebAppIntegrated.ApiResponse;

namespace StaffWebApp.Service.Category;

public interface ICategoryService
{
    Task<Result<List<CategoryVm>>> Categories();
}
