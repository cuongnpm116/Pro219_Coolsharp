

using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Address.AddAddress;

internal sealed class AddCustomerAddressCommandHandler : IRequestHandler<AddCustomerAddressCommand, Result>
{
    private readonly IAddressRepository _addressRepository;
    public AddCustomerAddressCommandHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }
    public async Task<Result> Handle(AddCustomerAddressCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _addressRepository.AddUserAddress(request, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
