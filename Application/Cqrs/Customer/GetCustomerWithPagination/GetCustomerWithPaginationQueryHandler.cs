using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Customer.GetCustomerWithPagination;

internal sealed class GetCustomerWithPaginationQueryHandler : IRequestHandler<GetCustomerWithPaginationQuery, Result>
{
    private readonly ICustomerRepository _customerRepository;
    public GetCustomerWithPaginationQueryHandler(ICustomerRepository customerRepository)
    {
         _customerRepository = customerRepository; 
    }
    public async Task<Result> Handle(GetCustomerWithPaginationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _customerRepository.GetUsersWithPagination(request);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
