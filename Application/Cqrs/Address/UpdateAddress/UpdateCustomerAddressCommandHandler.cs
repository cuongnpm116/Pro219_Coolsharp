using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Address.UpdateAddress;

internal sealed class UpdateCustomerAddressCommandHandler : IRequestHandler<UpdateCustomerAddressCommand, Result>
{
    private readonly IAddressRepository _addressRepository;
    public UpdateCustomerAddressCommandHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }
    public async Task<Result> Handle(UpdateCustomerAddressCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _addressRepository.UpdateUserAddress(request, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
