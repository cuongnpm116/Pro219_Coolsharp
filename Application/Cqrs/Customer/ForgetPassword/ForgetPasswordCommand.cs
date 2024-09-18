

using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Customer.ForgetPassword;

public class ForgetPasswordCommand : IRequest<Result>
{
    public string UserInput { get; set; }
}
