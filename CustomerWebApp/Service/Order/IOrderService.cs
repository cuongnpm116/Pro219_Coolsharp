using CustomerWebApp.Components.Orders.Dtos;
using CustomerWebApp.Components.Orders.ViewModel;
using WebAppIntegrated.ApiResponse;

namespace CustomerWebApp.Service.Order;

public interface IOrderService
{
    Task<Result<OrderWithPaymentVm>> CreateOrder(CreateOrderRequest request);
}
