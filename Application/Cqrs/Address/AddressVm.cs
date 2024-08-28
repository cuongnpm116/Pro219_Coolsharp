using VietNamAddress.Models;
using VietNamAddress.ViewModels;

namespace Application.Cqrs.Address;
public class AddressVm
{
    public Guid Id { get; set; }
    public ProvinceVm Province { get; set; }
    public DistrictVm District { get; set; }
    public WardVm Ward { get; set; }
    public string Detail { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;

    public AddressVm()
    {
    }

    public AddressVm(
        Guid id,
        Province province,
        District district,
        Ward ward,
        string detail,
        bool isDefault,
        string phoneNumber)
    {
        Id = id;
        Province = new(province.Code, province.FullName);
        District = new(district.Code, district.FullName);
        Ward = new(ward.Code, ward.FullName);
        Detail = detail;
        IsDefault = isDefault;
        PhoneNumber = phoneNumber;
    }
}
