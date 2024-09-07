using Application.Cqrs.Voucher;
using Application.Cqrs.Voucher.AddVoucher;
using Application.Cqrs.Voucher.GetVoucherPaging;
using Application.Cqrs.Voucher.UpdateVoucher;
using Application.IRepositories;
using Application.ValueObjects.Pagination;
using Common.Utilities;
using Domain.Entities;
using Domain.Enums;
using Domain.Primitives;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class VoucherRepository : IVoucherRepository
{
    private readonly AppDbContext _context;
    public VoucherRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Result<bool>> AddVoucher(AddVoucherCommand command)
    {
        Voucher voucher = new()
        {
            Name = command.Name,
            DiscountAmount = command.DiscountAmount,
            DiscountPercent = command.DiscountPercent,
            StartedDate = command.StartedDate,
            FinishedDate = command.FinishedDate,
            Status = (Status)command.Status,
            DiscountCondition = command.DiscountCondition,
            VoucherCode = StringUtility.GenerateVoucherCode(8),
            Stock = command.Stock,
        };

        await _context.Vouchers.AddAsync(voucher);
        return Result<bool>.Success(true);

    }


    public async Task<Result<List<VoucherVm>>> GetListVoucher()
    {
        var vouchers = await _context.Vouchers.Where(x => x.Status == Status.Active && x.StartedDate <= DateTime.Now && x.FinishedDate >= DateTime.Now)
            .Select(x => new VoucherVm
            {
                Id = x.Id,
                DiscountAmount = x.DiscountAmount,
                DiscountPercent = x.DiscountPercent,
                StartedDate = x.StartedDate,
                FinishedDate = x.FinishedDate,
                Stock = x.Stock,
                Name = x.Name,
                VoucherCode = x.VoucherCode,
                DiscountCondition = x.DiscountCondition,
                Status = (int)x.Status
            })
            .ToListAsync();
        if (vouchers == null)
        {
            return Result<List<VoucherVm>>.Invalid("");
        }
        return Result<List<VoucherVm>>.Success(vouchers);
    }

    public async Task<Result<VoucherDetailVm>> GetVoucherById(Guid Id)
    {
        var voucher = await _context.Vouchers
            .Select(x => new VoucherDetailVm
            {
                Id = x.Id,
                DiscountAmount = x.DiscountAmount,
                DiscountPercent = x.DiscountPercent,
                Stock = x.Stock,
                Name = x.Name,
                VoucherCode = x.VoucherCode,
                StartedDate = x.StartedDate,
                FinishedDate = x.FinishedDate,
                Status = (int)x.Status,
                DiscountCondition = x.DiscountCondition,
            })
            .FirstOrDefaultAsync(x => x.Id == Id && x.Status != (int)Status.Deleted);
        if (voucher == null)
        {
            return Result<VoucherDetailVm>.Invalid("Không tồn tại khuyến mãi");
        }
        return Result<VoucherDetailVm>.Success(voucher);
    }

    public async Task<Result<PaginationResponse<VoucherVm>>> GetVoucherPaging(GetVoucherPaginationQuery query)
    {
        try
        {
            var voucherQuery = from d in _context.Vouchers
                               where d.Status != Status.Deleted
                               select new VoucherVm
                               {
                                   Id = d.Id,
                                   Name = d.Name,
                                   StartedDate = d.StartedDate,
                                   Stock = d.Stock,
                                   FinishedDate = d.FinishedDate,
                                   DiscountAmount = d.DiscountAmount,
                                   DiscountPercent = d.DiscountPercent,
                                   VoucherCode = d.VoucherCode,
                                   DiscountCondition = d.DiscountCondition,
                                   Status = (int)d.Status
                               };
            if (!string.IsNullOrEmpty(query.SearchString))
            {
                voucherQuery = voucherQuery.Where(p => p.Name.Contains(query.SearchString)
                                     || p.VoucherCode.Contains(query.SearchString));
            }
            var queryResult = await voucherQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize + 1)
            .ToListAsync();

            bool hasNext = queryResult.Count > query.PageSize;
            var result = new PaginationResponse<VoucherVm>
            {
                PageNumber = query.PageNumber,
                HasNext = hasNext,
                Data = queryResult.Take(query.PageSize).ToList()
            };

            return Result<PaginationResponse<VoucherVm>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<PaginationResponse<VoucherVm>>.Error(ex.Message);
        }
    }

    public async Task<Result<bool>> UpdateVoucher(UpdateVoucherCommand command)
    {
        var voucher = await _context.Vouchers.FirstOrDefaultAsync(x => x.Id == command.Id);
        if (voucher == null)
        {
            return Result<bool>.Invalid("");
        }
        voucher.StartedDate = command.StartedDate;
        voucher.FinishedDate = command.FinishedDate;
        voucher.DiscountPercent = command.DiscountPercent;
        voucher.DiscountAmount = command.DiscountAmount;
        voucher.DiscountCondition = command.DiscountCondition;
        voucher.Status = (Status)command.Status;
        voucher.ModifiedOn = DateTime.Now;
        voucher.Name = command.Name;

        _context.Vouchers.Update(voucher);
        return Result<bool>.Success(true);
    }
}
