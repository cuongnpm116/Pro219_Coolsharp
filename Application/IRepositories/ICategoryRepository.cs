using Application.Cqrs.Category;

namespace Application.IRepositories;
public interface ICategoryRepository
{
    Task<List<CategoryVm>> GetListCategory();
}
