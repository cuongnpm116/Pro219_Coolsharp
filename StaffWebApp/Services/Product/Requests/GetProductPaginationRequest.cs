using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Product.Requests;
public class GetProductPaginationRequest : PaginationRequest
{
    public string? SearchString { get; set; }
    public string? CategoryId { get; set; }
}
