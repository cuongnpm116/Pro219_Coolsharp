using Application.Cqrs.Order;
using Application.Cqrs.Order.CancelOrder;
using Application.Cqrs.Order.CreateOrder;
using Application.Cqrs.Order.Get;
using Application.Cqrs.Order.UpdateOrder;
using Application.ValueObjects.Pagination;
using Domain.Primitives;

namespace Application.IRepositories;
public interface IOrderRepository
{
    Task<Result<OrderWithPaymentVm>> AddOrder(CreateOrderCommand request);


    Task<Result<PaginationResponse<OrderVm>>> GetOrdersForStaff(GetOrdersForStaffQuery request);
    Task<Result<OrderVm>> GetOrderDetailForStaff(Guid orderId);
    Task<Result<bool>> UpdateOrderStatusForStaff(UpdateOrderForStaffCommand request);
    Task<Result<bool>> CancelOrderForStaff(CancelOrderForStaffCommand request, CancellationToken token);
    Task<Result<List<OrderVm>>> Statistical();
    Task<Result<List<OrderDetailVm>>> TopProducts();
    
}
