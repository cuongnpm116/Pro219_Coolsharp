using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Address.MakeDefaultAddress;

internal sealed class MakeDefaultAddressCommandHandler : IRequestHandler<MakeDefaultAddressCommand, Result>
{
    private readonly IAddressRepository _addressRepository;
    public MakeDefaultAddressCommandHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }
    public async Task<Result> Handle(MakeDefaultAddressCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _addressRepository.MakeDefaultAddress(request, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
