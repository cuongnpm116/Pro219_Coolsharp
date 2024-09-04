using StaffWebApp.Services.Order.Requests;
using StaffWebApp.Services.Order.Vms;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Order;

public interface IOrderService
{
    Task<Result<PaginationResponse<OrderVm>>> GetOrders(OrderPaginationRequest request);
    Task<Result<OrderVm>> GetOrderDetais(Guid orderId);

    Task UpdateOrderStatus(OrderVm request);
    Task CancelOrderStatus(Guid orderId);
    Task<Result<List<OrderVm>>> Statistical();
    Task<Result<List<OrderDetailVm>>> TopProducts();
    Task<string> PrintOrder(Guid orderId);
    Task<bool> ExportOrdersToExcel(OrderPaginationRequest request);

}
