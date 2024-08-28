using Application.Cqrs.Address;

namespace Application.IRepositories;
public interface IAddressRepository
{
    Task<IReadOnlyList<AddressVm>> GetAddresses(Guid userId, CancellationToken cancellationToken);
}
