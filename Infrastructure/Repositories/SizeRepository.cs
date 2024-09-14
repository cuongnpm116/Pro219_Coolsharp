using Application.Cqrs.Size;
using Application.Cqrs.Size.Create;
using Application.Cqrs.Size.Get;
using Application.Cqrs.Size.GetForSelect;
using Application.Cqrs.Size.Update;
using Application.IRepositories;
using Application.ValueObjects.Pagination;
using Domain.Entities;
using Domain.Enums;
using Domain.Primitives;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal sealed class SizeRepository : ISizeRepository
{
    private readonly AppDbContext _context;

    public SizeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> AddSize(CreateSizeCommand request)
    {

        try
        {
            var size = new Size
            {
                Id = Guid.NewGuid(),
                SizeNumber = request.SizeNumber,
            };
            await _context.Sizes.AddAsync(size);
            return Result<bool>.Success(true/*, "Thêm size thành công"*/);
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }


    public async Task<Result<SizeVm>> GetSizeById(Guid id)
    {
        try
        {
            Size? query = await _context.Sizes.FirstOrDefaultAsync(x => x.Id == id);
            if (query == null)
            {
                return Result<SizeVm>.Invalid("Size không tồn tại");
            }
            SizeVm size = new()
            {
                Id = query.Id,
                SizeNumber = query.SizeNumber,
                Status = query.Status,
            };
            return Result<SizeVm>.Success(size);
        }
        catch (Exception ex)
        {
            return Result<SizeVm>.Error(ex.Message);
        }
    }

    public async Task<Result<PaginationResponse<SizeVm>>> GetSizes(GetSizesQuery request)
    {
        var query = from a in _context.Sizes
                    select new SizeVm
                    {
                        Id = a.Id,
                        SizeNumber = a.SizeNumber,
                        Status = a.Status,
                    };

        if (!string.IsNullOrEmpty(request.SearchString))
        {
            query = query.Where(x => x.SizeNumber.ToString().Contains(request.SearchString));
        }

        var paginatedQuery = query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize + 1);

        var queryResult = await paginatedQuery.ToListAsync();

        bool hasNext = queryResult.Count > request.PageSize;

        var result = new PaginationResponse<SizeVm>()
        {
            PageNumber = request.PageNumber,
            HasNext = hasNext,
            Data = queryResult.Take(request.PageSize).ToList()
        };

        return Result<PaginationResponse<SizeVm>>.Success(result);
    }

    public async Task<Result<bool>> UpdateSize(UpdateSizeCommand request)
    {
        try
        {
            Size? query = await _context.Sizes.FirstOrDefaultAsync(x => x.Id == request.Id);
            query.SizeNumber = request.SizeNumber;
            query.Status = request.Status;

            _context.Sizes.Update(query);

            return Result<bool>.Success(true/*, "Cập nhật size thành công"*/);
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }

    public async Task<IReadOnlyList<SizeForSelectVm>> GetSizeForSelectVms()
    {
        var sizeQuery = from s in _context.Sizes
                        where s.Status == Status.Active
                        select new SizeForSelectVm(s.Id, s.SizeNumber);
        return await sizeQuery.ToListAsync();
    }
}
