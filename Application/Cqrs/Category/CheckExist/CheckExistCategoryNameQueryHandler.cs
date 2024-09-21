using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Category.CheckExist;
internal sealed class CheckExistCategoryNameQueryHandler
    : IRequestHandler<CheckExistCategoryNameQuery, Result<bool>>
{
    private readonly ICategoryRepository _categoryRepo;

    public CheckExistCategoryNameQueryHandler(ICategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task<Result<bool>> Handle(
        CheckExistCategoryNameQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _categoryRepo.CheckExistCategoryName(request.Name);
            return result;
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }
}
