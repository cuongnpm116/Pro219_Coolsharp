using Application.ValueObjects.Pagination;
using Domain.Enums;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Customer.GetCustomerWithPagination;

public class GetCustomerWithPaginationQuery : PaginationRequest, IRequest<Result>
{
    public Status Status { get; set; } = Status.None;
}
