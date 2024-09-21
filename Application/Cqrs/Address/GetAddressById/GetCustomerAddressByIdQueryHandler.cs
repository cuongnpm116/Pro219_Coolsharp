
using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Address.GetAddressById;

internal sealed class GetCustomerAddressByIdQueryHandler : IRequestHandler<GetCustomerAddressByIdQuery, Result>
{
    private readonly IAddressRepository _addressRepository;
    public GetCustomerAddressByIdQueryHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }
    public async Task<Result> Handle(GetCustomerAddressByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _addressRepository.GetAddressVmById(request.AddressId, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {

            return Result.Error(ex.Message);
        }
    }
}
