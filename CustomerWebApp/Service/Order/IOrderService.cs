using CustomerWebApp.Service.Order.Dtos;
using CustomerWebApp.Service.Order.ViewModel;
using WebAppIntegrated.ApiResponse;

namespace CustomerWebApp.Service.Order;

public interface IOrderService
{
    Task<Result<OrderWithPaymentVm>> CreateOrder(CreateOrderRequest request);
}
