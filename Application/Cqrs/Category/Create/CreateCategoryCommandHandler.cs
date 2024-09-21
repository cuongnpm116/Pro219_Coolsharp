using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Category.Create;
internal sealed class CreateCategoryCommandHandler
    : IRequestHandler<CreateCategoryCommand, Result<bool>>
{
    private readonly ICategoryRepository _categoryRepo;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task<Result<bool>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _categoryRepo.AddCategory(request);
            return result;
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }
}
