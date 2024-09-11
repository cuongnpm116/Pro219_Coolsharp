

using Application.ValueObjects.Pagination;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Product.GetProductStaffPaging;

public class GetProductStaffPagingQuery : PaginationRequest, IRequest<Result>
{
    public string? CategoryName { get; set; }
}
