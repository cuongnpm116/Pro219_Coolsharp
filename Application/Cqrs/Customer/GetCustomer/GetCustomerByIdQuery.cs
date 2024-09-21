
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Customer.GetCustomer;

public class GetCustomerByIdQuery : IRequest<Result>
{
    public Guid CustomerId { get; set; }
}
