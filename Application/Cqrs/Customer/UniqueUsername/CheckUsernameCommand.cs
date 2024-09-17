

using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Customer.UniqueUsername;

public class CheckUsernameCommand : IRequest<Result>
{
    public string Username { get; set; }
}
