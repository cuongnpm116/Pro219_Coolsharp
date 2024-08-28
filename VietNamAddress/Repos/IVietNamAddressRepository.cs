using VietNamAddress.ViewModels;

namespace VietNamAddress.Repos;

public interface IVietNamAddressRepository
{
    Task<IReadOnlyList<ProvinceVm>> GetListProvince();
    Task<IReadOnlyList<DistrictVm>> GetListDistrict(string provinceCode);
    Task<IReadOnlyList<WardVm>> GetListWard(string districtCode);
}
