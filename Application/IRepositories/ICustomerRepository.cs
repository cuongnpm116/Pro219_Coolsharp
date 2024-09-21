using Application.Cqrs.Customer;
using Application.Cqrs.Customer.Create;
using Application.Cqrs.Customer.GetCustomerWithPagination;
using Application.Cqrs.Customer.UpdateAvatar;
using Application.Cqrs.Customer.UpdateProfile;
using Application.ValueObjects.Pagination;
using Domain.Primitives;

namespace Application.IRepositories;
public interface ICustomerRepository
{
    Task<Result<bool>> AddUser(CreateUserCommand request);
    Task<Result<CustomerInfoVm>> GetCustomerById(Guid id, CancellationToken token);
    Task<Result<bool>> UpdateUserAvatar(UpdateAvatarCommand command);
    Task<Result<bool>> UpdatePersonalInformation(UpdateProfileCommand command, CancellationToken token);
    Task<Result<PaginationResponse<CustomerVm>>> GetUsersWithPagination(GetCustomerWithPaginationQuery request);

    Task<Result<string>> Authenticate(string username, string password);
    Task<Result<bool>> IsUniqueEmailAddress(string emailAddress, CancellationToken token);
    Task<Result<bool>> IsUniqueUsername(string username, CancellationToken token);
    Task<Result<string>> ForgetPassword(string request);
    Task<Result<bool>> ChangePassword(string username, string newPassword);
    Task<Result<bool>> UpdateEmailAddress(Guid userId, string emailAddress, CancellationToken token);

}

