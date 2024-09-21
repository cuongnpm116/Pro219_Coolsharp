using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Product.Requests;
public class GetProductPaginationRequest : PaginationRequest
{
    public Guid CategoryId { get; set; } = Guid.Empty;
}
