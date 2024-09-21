using Application.Cqrs.Category;
using Application.Cqrs.Category.Create;
using Domain.Primitives;

namespace Application.IRepositories;
public interface ICategoryRepository
{
    Task<Result<bool>> AddCategory(CreateCategoryCommand request);
    Task<Result<bool>> CheckExistCategoryName(string name);
    Task<List<CategoryVm>> GetListCategory();
}
