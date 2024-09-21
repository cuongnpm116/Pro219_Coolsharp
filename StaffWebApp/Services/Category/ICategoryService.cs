using StaffWebApp.Services.Category.ViewModel;
using WebAppIntegrated.ApiResponse;

namespace StaffWebApp.Services.Category;

public interface ICategoryService
{
    Task<Result<List<CategoryVm>>> Categories();
    Task<bool> CheckCategoryNameExist(string name);
    Task<bool> CreateCategory(string name);
}
