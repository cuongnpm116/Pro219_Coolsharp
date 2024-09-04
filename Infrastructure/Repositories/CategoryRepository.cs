using Application.Cqrs.Category;
using Application.IRepositories;
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
}
