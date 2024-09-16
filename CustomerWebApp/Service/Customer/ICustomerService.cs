using CustomerWebApp.Service.Customer.Request;
using CustomerWebApp.Service.Customer.Vms;
using WebAppIntegrated.ApiResponse;

namespace CustomerWebApp.Service.Customer;

public interface ICustomerService
{
    Task<Result<CustomerInfoVm>> GetUserProfile(Guid customerId);
    Task<Result<bool>> UpdateProfile(UpdateProfileRequest request);
    Task<Result<bool>> UpdateAvatar(UpdateAvatarRequest request);
}
