using Microsoft.EntityFrameworkCore;
using VietNamAddress.Models;
using VietNamAddress.ViewModels;

namespace VietNamAddress.Repos;

public class VietNamAddressRepository : IVietNamAddressRepository
{
    private readonly VietNamAddressContext _context;

    public VietNamAddressRepository(VietNamAddressContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ProvinceVm>> GetListProvince()
    {
        var provinces = await _context.Provinces
            .AsNoTracking()
            .Select(x => new ProvinceVm(x.Code, x.FullName))
            .ToListAsync();

        return provinces;
    }

    public async Task<IReadOnlyList<DistrictVm>> GetListDistrict(string provinceCode)
    {
        var districts = await _context.Districts
            .AsNoTracking()
            .Where(x => x.ProvinceCode == provinceCode)
            .Select(x => new DistrictVm(x.Code, x.FullName))
            .ToListAsync();

        return districts;
    }

    public async Task<IReadOnlyList<WardVm>> GetListWard(string districtCode)
    {
        var wards = await _context.Wards
            .AsNoTracking()
            .Where(x => x.DistrictCode == districtCode)
            .Select(x => new WardVm(x.Code, x.FullName))
            .ToListAsync();

        return wards;
    }
}
