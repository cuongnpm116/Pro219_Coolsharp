using Application.Cqrs.Customer;
using Application.Cqrs.Customer.UpdateAvatar;
using Application.Cqrs.Customer.UpdateProfile;
using Domain.Primitives;

namespace Application.IRepositories;
public interface ICustomerRepository
{
    Task<Result<CustomerInfoVm>> GetCustomerById(Guid id, CancellationToken token);
    Task<Result<bool>> UpdateUserAvatar(UpdateAvatarCommand command);
    Task<Result<bool>> UpdatePersonalInformation(UpdateProfileCommand command, CancellationToken token);

}

