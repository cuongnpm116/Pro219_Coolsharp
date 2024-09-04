using Application.ValueObjects.Pagination;
using Domain.Primitives;
using MediatR;


namespace Application.Cqrs.Product.GetProductCustomerAppPaging;

public class GetProductCustomerAppPagingQuery : PaginationRequest,IRequest<Result>
{
    public List<Guid>? CategoryIds { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
