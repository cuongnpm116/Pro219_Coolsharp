using WebAppIntegrated.Enum;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Order.Requests;

public class OrderPaginationRequest : PaginationRequest
{
    public int Stock { get; set; } = 5;
    public DateTime? Begin { get; set; }
    public DateTime? End { get; set; }
    public OrderStatus OrderStatus { get; set; }
}
