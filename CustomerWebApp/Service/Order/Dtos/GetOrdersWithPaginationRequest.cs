using WebAppIntegrated.Enum;
using WebAppIntegrated.Pagination;

namespace CustomerWebApp.Service.Order.Dtos;

public class GetOrdersWithPaginationRequest : PaginationRequest
{
    public Guid CustomerId { get; set; }
    public OrderStatus OrderStatus { get; set; }
}
