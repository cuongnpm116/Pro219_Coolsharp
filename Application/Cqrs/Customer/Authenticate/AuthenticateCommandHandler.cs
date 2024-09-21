

using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Customer.Authenticate;

internal sealed class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, Result>
{
    private readonly ICustomerRepository _customerRepository;
    public AuthenticateCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task<Result> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _customerRepository.Authenticate(request.Username, request.Password);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
