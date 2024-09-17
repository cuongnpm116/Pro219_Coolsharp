

using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Customer.Create;

public class CreateUserCommand : IRequest<Result>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Gender { get; set; }
    public string EmailAddress { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

}
