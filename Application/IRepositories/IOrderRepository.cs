using Application.Cqrs.Order;
using Application.Cqrs.Order.CancelOrder;
using Application.Cqrs.Order.CreateOrder;
using Application.Cqrs.Order.Get;
using Application.Cqrs.Order.GetOrderForCustomer;
using Application.Cqrs.Order.Statisticals;
using Application.Cqrs.Order.UpdateOrder;
using Application.ValueObjects.Pagination;
using Domain.Primitives;

namespace Application.IRepositories;
public interface IOrderRepository
{
    Task<Result<OrderWithPaymentVm>> AddOrder(CreateOrderCommand request);

    Task<Result<List<OrderDetailVm>>> GetOrderDetails(Guid orderId);

    Task<Result<PaginationResponse<OrderVm>>> GetOrdersForCustomer(GetOrderForCustomerQuery request);

    Task<Result<PaginationResponse<OrderVm>>> GetOrdersForStaff(GetOrdersForStaffQuery request);

    Task<Result<OrderVm>> GetOrderDetailForStaff(Guid orderId);

    Task<Result<bool>> UpdateOrderStatusForStaff(UpdateOrderForStaffCommand request);

    Task<Result<bool>> CancelOrder(CancelOrderCommand request, CancellationToken token);

    Task<Result<List<OrderVm>>> Statistical();

    Task<Result<List<OrderDetailVm>>> TopProducts(TopProductQuery request);

    Task<Result<List<ProductDetailForStaffVm>>> LowStockProducts();
}
