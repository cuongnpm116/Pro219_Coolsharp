using Application.Cqrs.Color;
using Application.Cqrs.Color.Create;
using Application.Cqrs.Color.Get;
using Application.Cqrs.Color.Update;
using Application.IRepositories;
using Application.ValueObjects.Pagination;
using Domain.Entities;
using Domain.Primitives;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories;
internal sealed class ColorRepository : IColorRepository
{
    private readonly AppDbContext _context;

    public ColorRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Result<bool>> AddColor(CreateColorCommand request)
    {
        try
        {
            var color = new Color
            {
                Id = Guid.NewGuid(),
                Name = request.Name,               
            };

            await _context.Colors.AddAsync(color);
            return Result<bool>.Success(true/*, "Thêm color thành công"*/);
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }

    }
    public async Task<Result<bool>> UpdateColor(UpdateColorCommand request)
    {
        try
        {
            Color? query = await _context.Colors.FirstOrDefaultAsync(x => x.Id == request.Id);
            query.Name = request.Name;
            query.Status = request.Status;

            _context.Colors.Update(query);

            return Result<bool>.Success(true/*, "Cập nhật color thành công"*/);
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }


    public async Task<Result<ColorVm>> GetColorById(Guid id)
    {
        try
        {
            Color? query = await _context.Colors.FirstOrDefaultAsync(x => x.Id == id);
            if (query == null)
            {
                return Result<ColorVm>.Invalid("color không tồn tại");
            }
            ColorVm color = new()
            {
                Id = query.Id,
                Name = query.Name,
                Status = query.Status,
            };
            return Result<ColorVm>.Success(color);
        }
        catch (Exception ex)
        {
            return Result<ColorVm>.Error(ex.Message);
        }
    }

    public async Task<Result<PaginationResponse<ColorVm>>> GetColors(GetColorsQuery request)
    {
        var query = from a in _context.Colors
                    select new ColorVm
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Status = a.Status,
                    };

        if (!string.IsNullOrEmpty(request.SearchString))
        {
            query = query.Where(x => x.Name.Contains(request.SearchString));
        }
        var paginatedQuery = query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize + 1);

        var queryResult = await paginatedQuery.ToListAsync();

        bool hasNext = queryResult.Count > request.PageSize;

        var result = new PaginationResponse<ColorVm>()
        {
            PageNumber = request.PageNumber,
            HasNext = hasNext,
            Data = queryResult.Take(request.PageSize).ToList()
        };

        return Result<PaginationResponse<ColorVm>>.Success(result);
    }
}
