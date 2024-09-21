using CustomerWebApp.Service.Customer.Request;
using CustomerWebApp.Service.Customer.Vms;
using WebAppIntegrated.ApiResponse;

namespace CustomerWebApp.Service.Customer;

public interface ICustomerService
{
    Task<Result<CustomerInfoVm>> GetUserProfile(Guid customerId);
    Task<Result<bool>> UpdateProfile(UpdateProfileRequest request);
    Task<Result<bool>> UpdateAvatar(UpdateAvatarRequest request);


    Task<Result<bool>> ChangePassword(ChangePasswordModel model);
    Task<Result<string>> ForgetPassword(string userInput);
    Task<Result<string>> Login(LoginInfo info);
    Task<Result<bool>> Register(RegisterModel model);
    Task<bool> CheckUniqueEmail(string email);
    Task<bool> CheckUniqueUsername(string username);
}
