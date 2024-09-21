using Application.ValueObjects.Pagination;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Customer.GetCustomerWithPagination;

public class GetCustomerWithPaginationQuery : PaginationRequest, IRequest<Result>
{
}
