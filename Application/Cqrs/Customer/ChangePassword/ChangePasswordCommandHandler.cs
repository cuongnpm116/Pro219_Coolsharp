

using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Customer.ChangePassword;

internal sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
{
    private readonly ICustomerRepository _customerRepository;
    public ChangePasswordCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _customerRepository.ChangePassword(request.Username, request.NewPassword);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
