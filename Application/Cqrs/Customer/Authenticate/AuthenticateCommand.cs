
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Customer.Authenticate;

public class AuthenticateCommand : IRequest<Result>
{
    public string Username { get; set; }
    public string Password { get; set; }

}
