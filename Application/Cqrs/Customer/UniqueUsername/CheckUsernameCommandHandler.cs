using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Customer.UniqueUsername;

internal sealed class CheckUsernameCommandHandler : IRequestHandler<CheckUsernameCommand, Result>
{
    private readonly ICustomerRepository _customerRepository;
    public CheckUsernameCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task<Result> Handle(CheckUsernameCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _customerRepository.IsUniqueUsername(request.Username, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
