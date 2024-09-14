using Application.ValueObjects.Pagination;
using Domain.Enums;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Order.Get;

public class GetOrdersForStaffQuery : PaginationRequest, IRequest<Result<PaginationResponse<OrderVm>>>
{
    public OrderStatus? OrderStatus { get; set; }
}