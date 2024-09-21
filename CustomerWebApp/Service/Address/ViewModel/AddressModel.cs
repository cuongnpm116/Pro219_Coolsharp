using WebAppIntegrated.AddressService;

namespace CustomerWebApp.Service.Address.ViewModel;

public class AddressModel
{
    public Guid Id { get; set; }
    public ProvinceVm Province { get; set; }
    public DistrictVm District { get; set; }
    public WardVm Ward { get; set; }
    public string Detail { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
}
