
using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Address.GetDefaultAddress;

internal sealed class GetDefaultAddressQueryHandler : IRequestHandler<GetDefaultAddressQuery, Result>
{
    private readonly IAddressRepository _addressRepository;
    public GetDefaultAddressQueryHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<Result> Handle(GetDefaultAddressQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _addressRepository.GetDefaultAddressVm(request.CustomerId, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
