using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Product.Requests
{
    public class ProductPaginationRequest : PaginationRequest
    {
        public string? CategoryName { get; set; }
    }
}
