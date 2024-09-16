using Application.ValueObjects.Pagination;
using Domain.Enums;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Order.GetOrderForCustomer;

public class GetOrderForCustomerQuery : PaginationRequest ,IRequest<Result>
{
    public Guid CustomerId { get; set; }
    public OrderStatus OrderStatus { get; set; }
}
