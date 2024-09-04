

using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Category.GetCategories;

internal class GetListCategoryQueryHandler : IRequestHandler<GetListCategoryQuery, Result>
{
    private readonly ICategoryRepository _categoryRepository;
    public GetListCategoryQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    public async Task<Result> Handle(GetListCategoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var categories = await _categoryRepository.GetListCategory();

            if (categories == null || !categories.Any())
            {
                return Result<List<CategoryVm>>.Invalid("Không có danh mục");
            }

            return Result<List<CategoryVm>>.Success(categories.ToList());

            
        }
        catch (Exception ex)
        {
            return Result<List<CategoryVm>>.Error(ex.Message);
        }
    }
}
