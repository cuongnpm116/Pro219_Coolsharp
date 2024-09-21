using CustomerWebApp.Service.Address.Dtos;
using CustomerWebApp.Service.Address.ViewModel;
using WebAppIntegrated.AddressService;
using WebAppIntegrated.ApiResponse;

namespace CustomerWebApp.Service.Address;

public interface IAddressService
{
    Task<Result<List<AddressModel>>> GetUserAddress(Guid customerId);
    Task<Result<bool>> AddUserAddress(AddAddressRequest model);
    Task<Result<bool>> UpdateUserAddress(UpdateAddressRequest model);
    Task<Result<AddressModel>> GetAddressById(Guid addressId);
    Task<Result<bool>> MakeDefaultAddress(MakeDefaultAddressModel model);
    Task<Result<bool>> DeleteAddress(DeleteAddressRequest model);
    Task<Result<AddressModel>> GetDefaultAddress(Guid customerId);
}
