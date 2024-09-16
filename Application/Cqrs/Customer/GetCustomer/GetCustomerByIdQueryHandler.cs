using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Customer.GetCustomer;

internal sealed class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Result>
{
    private readonly ICustomerRepository _customerRepository;
    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task<Result> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _customerRepository.GetCustomerById(request.CustomerId, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
