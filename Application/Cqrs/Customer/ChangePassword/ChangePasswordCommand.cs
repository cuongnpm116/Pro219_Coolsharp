using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Customer.ChangePassword;

public class ChangePasswordCommand : IRequest<Result>
{
    public string Username { get; set; }
    public string NewPassword { get; set; }
}
