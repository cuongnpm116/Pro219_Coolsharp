using CustomerWebApp.Service.Order.Dtos;
using CustomerWebApp.Service.Order.ViewModel;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Pagination;

namespace CustomerWebApp.Service.Order;

public interface IOrderService
{
    Task<Result<OrderWithPaymentVm>> CreateOrder(CreateOrderRequest request);
    Task<Result<PaginationResponse<OrderVm>>> GetOrdersForCustomerWithPagination(GetOrdersWithPaginationRequest request);
    Task<Result<IReadOnlyList<OrderDetailVm>>> GetOrderDetail(Guid orderId);
    Task<Result<bool>> CancelOrder(CancelOrderRequest request);
}
