

using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Customer.UniqueEmail;

internal class CheckEmailCommandHandler : IRequestHandler<CheckEmailCommand, Result>
{
    private readonly ICustomerRepository _customerRepository;
    public CheckEmailCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task<Result> Handle(CheckEmailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _customerRepository.IsUniqueEmailAddress(request.EmailAddress, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
