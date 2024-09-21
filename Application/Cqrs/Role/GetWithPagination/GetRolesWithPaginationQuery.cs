using Application.ValueObjects.Pagination;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Role.GetWithPagination;
public class GetRolesWithPaginationQuery : PaginationRequest, IRequest<Result>
{
}
