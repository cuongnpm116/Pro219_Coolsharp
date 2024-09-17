using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Customer.ForgetPassword;

internal sealed class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, Result>
{
    private readonly ICustomerRepository _customerRepository;
    public ForgetPasswordCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task<Result> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _customerRepository.ForgetPassword(request.UserInput);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
