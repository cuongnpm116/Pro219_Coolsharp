using Application.Cqrs.Category;
using Application.Cqrs.Category.Create;
using Application.IRepositories;
using Domain.Entities;
using Domain.Primitives;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal sealed class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryVm>> GetListCategory()
    {
        var categories = await _context.Categories
            .Select(x => new CategoryVm
            {
                CategoryId = x.Id,
                CategoryName = x.Name
            }).ToListAsync();
        return categories;
    }

    public async Task<Result<bool>> AddCategory(CreateCategoryCommand request)
    {
        await _context.Categories.AddAsync(new Category { Name = request.Name });
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> CheckExistCategoryName(string name)
    {
        bool isExist = await _context.Categories.AnyAsync(x => x.Name == name);
        return Result<bool>.Success(isExist);
    }
}
