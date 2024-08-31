using WebAppIntegrated.AddressService;

namespace CustomerWebApp.Components.Address.Dtos;

public class AddressAddOrEditVm
{
    public ProvinceVm Province { get; set; }
    public DistrictVm District { get; set; }
    public WardVm Ward { get; set; }
    public string Detail { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
}
