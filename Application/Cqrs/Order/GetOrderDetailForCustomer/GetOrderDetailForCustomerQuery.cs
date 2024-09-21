using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Order.GetOrderDetailForCustomer;

public class GetOrderDetailForCustomerQuery : IRequest<Result>
{
    public Guid OrderId { get; set; }
}
