using Application.IRepositories;
using Domain.Primitives;
using MediatR;

namespace Application.Cqrs.Address.DeleteAddress;

internal sealed class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, Result>
{
    private readonly IAddressRepository _addressRepository;
    public DeleteAddressCommandHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }
    public async Task<Result> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _addressRepository.DeleteAddress(request, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
