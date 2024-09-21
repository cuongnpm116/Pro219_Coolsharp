using CustomerWebApp.Service.Category.ViewModel;
using WebAppIntegrated.ApiResponse;

namespace CustomerWebApp.Service.Category;

public interface ICategoryService
{
    Task<Result<List<CategoryVm>>> Categories();
}
