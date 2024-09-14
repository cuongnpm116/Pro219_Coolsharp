using Application.Cqrs.Address;
using Application.Cqrs.Address.AddAddress;
using Application.Cqrs.Address.DeleteAddress;
using Application.Cqrs.Address.MakeDefaultAddress;
using Application.Cqrs.Address.UpdateAddress;
using Application.IRepositories;
using Domain.Entities;
using Domain.Primitives;
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
    private async Task<Address?> GetDefaultAddress(Guid customerId, CancellationToken token)
        => await _context.Addresses.FirstOrDefaultAsync(x => x.CustomerId == customerId && x.IsDefault, token);
    private async Task<Address?> GetAddressById(Guid addressId, CancellationToken token)
        => await _context.Addresses.FirstOrDefaultAsync(x => x.Id == addressId, token);

    public async Task<Result<AddressVm>> GetAddressVmById(Guid addressId, CancellationToken cancellationToken)
    {
        Address? exist = await GetAddressById(addressId, cancellationToken);
        if (exist is null)
        {
            return Result<AddressVm>.Invalid("Địa chỉ không tồn tại");
        }

        Province? province = await GetProvinceByCode(exist.ProvinceCode, cancellationToken);
        District? district = await GetDistrictByCode(exist.DistrictCode, cancellationToken);
        Ward? ward = await GetWardByCode(exist.WardCode, cancellationToken);

        AddressVm vm = new(
            exist.Id,
            province,
            district,
            ward,
            exist.Detail,
            exist.IsDefault,
            exist.PhoneNumber);

        return Result<AddressVm>.Success(vm);
    }

    public async Task<Result<AddressVm>> GetDefaultAddressVm(Guid customerId, CancellationToken cancellationToken)
    {
        Address? exist = await GetDefaultAddress(customerId, cancellationToken);
        if (exist is null)
        {
            return Result<AddressVm>.Invalid("");
        }

        Province? province = await GetProvinceByCode(exist.ProvinceCode, cancellationToken);
        District? district = await GetDistrictByCode(exist.DistrictCode, cancellationToken);
        Ward? ward = await GetWardByCode(exist.WardCode, cancellationToken);

        AddressVm vm = new(
            exist.Id,
            province,
            district,
            ward,
            exist.Detail,
            exist.IsDefault,
            exist.PhoneNumber);
        return Result<AddressVm>.Success(vm);
    }

    public async Task<Result<bool>> AddUserAddress(AddCustomerAddressCommand request, CancellationToken cancellationToken)
    {
        Address newAddress = new()
        {
            Id = Guid.NewGuid(),
            PhoneNumber = request.PhoneNumber,
            ProvinceCode = request.ProvinceCode,
            DistrictCode = request.DistrictCode,
            WardCode = request.WardCode,
            Detail = request.Detail,
            IsDefault = request.IsDefault,
            CustomerId = request.CreatedBy,

        };

        var existingAddresses = await _context.Addresses
            .AsNoTracking()
            .Where(x => x.CustomerId == request.CreatedBy)
            .ToListAsync(cancellationToken);

        if (existingAddresses.Count == 0)
        {
            newAddress.IsDefault = true;
        }
        else if (request.IsDefault)
        {
            var defaultAddress = existingAddresses.FirstOrDefault(x => x.IsDefault);
            if (defaultAddress != null)
            {
                defaultAddress.IsDefault = false;
                _context.Addresses.Update(defaultAddress);
            }
        }

        await _context.Addresses.AddAsync(newAddress, cancellationToken);
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> UpdateUserAddress(UpdateCustomerAddressCommand request, CancellationToken cancellationToken)
    {
        Address? exist = await GetAddressById(request.Id, cancellationToken);
        if (exist is null)
        {
            return Result<bool>.Invalid("Người dùng không tồn tại");
        }

        bool isModified = false;
        if (request.ProvinceCode != exist.ProvinceCode)
        {
            exist.ProvinceCode = request.ProvinceCode;
            isModified = true;
        }
        if (request.DistrictCode != exist.DistrictCode)
        {
            exist.DistrictCode = request.DistrictCode;
            isModified = true;
        }
        if (request.WardCode != exist.WardCode)
        {
            exist.WardCode = request.WardCode;
            isModified = true;
        }
        if (request.PhoneNumber != exist.PhoneNumber)
        {
            exist.PhoneNumber = request.PhoneNumber;
            isModified = true;
        }
        if (request.Detail != exist.Detail)
        {
            exist.Detail = request.Detail;
            isModified = true;
        }

        // Kiểm tra và cập nhật địa chỉ mặc định
        if (request.IsDefault && !exist.IsDefault)
        {
            // Tìm địa chỉ mặc định hiện tại
            Address? currentDefaultAddress = await GetDefaultAddress(
                request.ModifiedBy,
                cancellationToken);
            if (currentDefaultAddress != null)
            {
                // Đặt địa chỉ mặc định hiện tại thành không mặc định
                currentDefaultAddress.IsDefault = false;
                _context.Addresses.Update(currentDefaultAddress);
            }

            // Đặt địa chỉ hiện tại thành mặc định
            exist.IsDefault = true;
            isModified = true;
        }

        if (isModified)
        {
            _context.Addresses.Update(exist);
        }

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> MakeDefaultAddress(MakeDefaultAddressCommand request, CancellationToken cancellationToken)
    {
        Address? currentDefaultAddress = await GetAddressById(
           request.CurrentDefaultAddressId,
           cancellationToken);

        Address? newDefaultAddress = await GetAddressById(
            request.NewDefaultAddressId,
            cancellationToken);

        if (currentDefaultAddress is null || newDefaultAddress is null)
        {
            return Result<bool>.Invalid("Địa chỉ không tồn tại");
        }
        if (newDefaultAddress.IsDefault == true)
        {
            return Result<bool>.Invalid("Địa chỉ này đã được đặt làm mặc định");
        }

        currentDefaultAddress.IsDefault = false;
        newDefaultAddress.IsDefault = true;

        _context.Addresses.UpdateRange([currentDefaultAddress, newDefaultAddress]);

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteAddress(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        Address? exist = await GetAddressById(request.AddressId, cancellationToken);
        if (exist is null)
        {
            return Result<bool>.Invalid("Địa chỉ không tồn tại");
        }
        if (exist.IsDefault)
        {
            return Result<bool>.Invalid("Không thể xóa địa chỉ mặc định");
        }

        _context.Addresses.Remove(exist);
        return Result<bool>.Success(true);
    }
}
