using Application.Cqrs.Order;
using Application.Cqrs.Order.CreateOrder;
using Domain.Primitives;

namespace Application.IRepositories;
public interface IOrderRepository
{
    Task<Result<OrderWithPaymentVm>> AddOrder(CreateOrderCommand request);
}
