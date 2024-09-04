using Application.ValueObjects.Pagination;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Voucher.GetVoucherPaging;

public class GetVoucherPaginationQuery : PaginationRequest, IRequest<Result>
{
}
