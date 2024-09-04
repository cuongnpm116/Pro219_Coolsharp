using Application.Cqrs.Payment.CreatePayment;
using Application.IRepositories;
using Domain.Entities;
using Domain.Enums;
using Domain.Primitives;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;
    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> CreatePayment(CreatePaymentCommand command)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.OrderCode == command.OrderCode);
        if (order == null)
        {
            return Result<bool>.Invalid("Không tồn tại đơn hàng");
        }

        Payment payment = new Payment()
        {
            PaymentMethod = (PaymentMethod)command.PaymentMethod,
            CustomerId = command.CustomerId,
            OrderId = order.Id,
            Amount = command.Amount,
            PaymentDate = command.PaymentDate,
            
            Status = (PaymentStatus)command.PaymentStatus,
            StatusCode = command.StatusCode,
        };
        await _context.Payments.AddAsync(payment);
        return Result<bool>.Success(true);
    }
}
