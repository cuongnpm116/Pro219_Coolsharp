using Application.Cqrs.Address;
using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using VietNamAddress.Models;

namespace Infrastructure.Repositories;
internal sealed class AddressRepository : IAddressRepository
{
    private readonly AppDbContext _context;
    private readonly VietNamAddressContext _vietNamAddressContext;

    public AddressRepository(AppDbContext context, VietNamAddressContext vietNamAddressContext)
    {
        _context = context;
        _vietNamAddressContext = vietNamAddressContext;
    }

    public async Task<IReadOnlyList<AddressVm>> GetAddresses(Guid userId, CancellationToken cancellationToken)
    {
        var userAddresses = await GetUserAddressesAsync(userId, cancellationToken);
        var addressVms = await ConvertToAddressViewModels(userAddresses, cancellationToken);
        return addressVms.AsReadOnly();
    }

    private async Task<List<Address>> GetUserAddressesAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await (from u in _context.Customers
                      join a in _context.Addresses on u.Id equals a.CustomerId
                      where u.Id == userId
                      select new Address
                      {
                          Id = a.Id,
                          IsDefault = a.IsDefault,
                          ProvinceCode = a.ProvinceCode,
                          DistrictCode = a.DistrictCode,
                          WardCode = a.WardCode,
                          PhoneNumber = a.PhoneNumber,
                          Detail = a.Detail
                      })
                      .ToListAsync(cancellationToken);
    }

    private async Task<List<AddressVm>> ConvertToAddressViewModels(List<Address> userAddresses, CancellationToken cancellationToken)
    {
        List<AddressVm> addressVms = [];

        foreach (var ua in userAddresses)
        {
            Province? province = await GetProvinceByCode(ua.ProvinceCode, cancellationToken);
            District? district = await GetDistrictByCode(ua.DistrictCode, cancellationToken);
            Ward? ward = await GetWardByCode(ua.WardCode, cancellationToken);
            AddressVm addressVm = new(ua.Id, province, district, ward, ua.Detail, ua.IsDefault, ua.PhoneNumber);

            addressVms.Add(addressVm);
        }

        return addressVms;
    }

    private async Task<Province?> GetProvinceByCode(string provinceCode, CancellationToken token)
        => await _vietNamAddressContext.Provinces.AsNoTracking().FirstOrDefaultAsync(x => x.Code == provinceCode, token);

    private async Task<District?> GetDistrictByCode(string districtCode, CancellationToken token)
        => await _vietNamAddressContext.Districts.AsNoTracking().FirstOrDefaultAsync(x => x.Code == districtCode, token);

    private async Task<Ward?> GetWardByCode(string wardCode, CancellationToken token)
        => await _vietNamAddressContext.Wards.AsNoTracking().FirstOrDefaultAsync(w => w.Code == wardCode, token);
}
