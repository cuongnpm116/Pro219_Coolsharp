

using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Customer.UniqueEmail;

public class CheckEmailCommand : IRequest<Result>
{
    public string EmailAddress { get; set; }
}
