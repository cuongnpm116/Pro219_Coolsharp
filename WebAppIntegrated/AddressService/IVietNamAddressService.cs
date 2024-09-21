namespace WebAppIntegrated.AddressService;
public interface IVietNamAddressService
{
    Task<IEnumerable<ProvinceVm>> GetProvinceListAsync();
    Task<IEnumerable<DistrictVm>> GetDistrictListAsync(string provinceCode);
    Task<IEnumerable<WardVm>> GetWardListAsync(string districtCode);
}
