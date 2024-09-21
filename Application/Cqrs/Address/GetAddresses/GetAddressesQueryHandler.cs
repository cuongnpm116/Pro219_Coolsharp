using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Address.GetAddresses;
internal sealed class GetAddressesQueryHandler : IRequestHandler<GetAddressesQuery, Result>
{
    private readonly IAddressRepository _addressRepository;

    public GetAddressesQueryHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<Result> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _addressRepository.GetAddresses(request.CustomerId, cancellationToken);
            return Result<IReadOnlyList<AddressVm>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
