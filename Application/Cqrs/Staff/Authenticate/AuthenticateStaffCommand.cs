
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Staff.Authenticate;

public class AuthenticateStaffCommand : IRequest<Result>
{
    public string Username { get; set; }
    public string Password { get; set; }

}
