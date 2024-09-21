using Application.Cqrs.Address;
using Application.Cqrs.Address.AddAddress;
using Application.Cqrs.Address.DeleteAddress;
using Application.Cqrs.Address.MakeDefaultAddress;
using Application.Cqrs.Address.UpdateAddress;
using Domain.Primitives;

namespace Application.IRepositories;
public interface IAddressRepository
{
    Task<IReadOnlyList<AddressVm>> GetAddresses(Guid userId, CancellationToken cancellationToken);
    Task<Result<AddressVm>> GetAddressVmById(Guid addressId, CancellationToken cancellationToken);

    Task<Result<AddressVm>> GetDefaultAddressVm(Guid userId, CancellationToken cancellationToken);

    Task<Result<bool>> AddUserAddress(AddCustomerAddressCommand request, CancellationToken cancellationToken);

    Task<Result<bool>> UpdateUserAddress(UpdateCustomerAddressCommand request, CancellationToken cancellationToken);

    Task<Result<bool>> MakeDefaultAddress(MakeDefaultAddressCommand request, CancellationToken cancellationToken);

    Task<Result<bool>> DeleteAddress(DeleteAddressCommand request, CancellationToken cancellationToken);
}
