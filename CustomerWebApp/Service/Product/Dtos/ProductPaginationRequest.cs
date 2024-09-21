using WebAppIntegrated.Pagination;

namespace CustomerWebApp.Service.Product.Dtos;

public class ProductPaginationRequest : PaginationRequest
{
    public List<Guid>? CategoryIds { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
