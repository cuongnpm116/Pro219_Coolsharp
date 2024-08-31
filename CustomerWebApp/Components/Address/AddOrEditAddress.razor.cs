using CustomerWebApp.Components.Address.Dtos;
using CustomerWebApp.Service.Address;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebAppIntegrated.AddressService;
using WebAppIntegrated.ApiResponse;

namespace CustomerWebApp.Components.Address;

public partial class AddOrEditAddress
{
    [CascadingParameter]
    public MudDialogInstance DialogInstance { get; set; } = null!;

    [Parameter]
    public Guid AddressId { get; set; }

    [Parameter]
    public Guid CustomerId { get; set; }

    [Parameter]
    public bool IsDefault { get; set; }

    [Inject]
    private IVietNamAddressService VietNamAddressService { get; set; } = null!;

    [Inject]
    private IAddressService AddressService { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    private IEnumerable<ProvinceVm> _provinces = [];
    private IEnumerable<DistrictVm> _districts = [];
    private IEnumerable<WardVm> _wards = [];

    private AddressAddOrEditVm _vm = new();

    private readonly AddressAddOrEditVmValidator validator = new();
    private MudForm _form = null!;

    protected override async Task OnInitializedAsync()
    {
        var provinceResult = await VietNamAddressService.GetProvinceListAsync();
        _provinces = provinceResult;
        if (AddressId != Guid.Empty)
        {
            var result = await AddressService.GetAddressById(AddressId);

            _vm.Province = result.Value.Province;
            _vm.District = result.Value.District;
            _vm.Ward = result.Value.Ward;

            _vm.Detail = result.Value.Detail;
            _vm.PhoneNumber = result.Value.PhoneNumber;
            _vm.IsDefault = result.Value.IsDefault;
        }
    }

    private async Task OnProvinceChanged(ProvinceVm selected)
    {
        _vm.Province = selected;
        _vm.District = default;
        _vm.Ward = default;

        var districtResult = await VietNamAddressService.GetDistrictListAsync(_vm.Province.Code);
        _districts = districtResult;
    }

    private async Task OnDistrictChanged(DistrictVm selected)
    {
        _vm.District = selected;
        _vm.Ward = default;

        var wardResult = await VietNamAddressService.GetWardListAsync(_vm.District.Code);
        _wards = wardResult;
    }

    private async Task<IEnumerable<ProvinceVm>> SearchProvince(string value, CancellationToken token)
    {
        await Task.Delay(1, token);
        IEnumerable<ProvinceVm> provinceNames = _provinces;

        if (!string.IsNullOrWhiteSpace(value))
        {
            provinceNames = provinceNames
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
        return provinceNames;
    }

    private async Task<IEnumerable<DistrictVm>> SearchDistrict(string value, CancellationToken token)
    {
        await Task.Delay(1, token);
        IEnumerable<DistrictVm> districtNames = _districts;
        if (!string.IsNullOrWhiteSpace(value))
        {
            districtNames = districtNames
                    .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
        return districtNames;
    }

    private async Task<IEnumerable<WardVm>> SearchWard(string value, CancellationToken token)
    {
        await Task.Delay(1, token);
        IEnumerable<WardVm> wardNames = _wards;
        if (!string.IsNullOrWhiteSpace(value))
        {
            wardNames = wardNames
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
        return wardNames;
    }

    private async Task OnSaveButtonClick()
    {
        await _form.Validate();

        if (_form.IsValid)
        {
            if (AddressId == Guid.Empty)
            {
                await AddNewAddress();
            }
            else
            {
                await UpdateExistingAddress();
            }
        }
    }

    private async Task AddNewAddress()
    {
        AddAddressRequest model = new()
        {
            CreatedBy = CustomerId,
            PhoneNumber = _vm.PhoneNumber,
            ProvinceCode = _vm.Province.Code,
            DistrictCode = _vm.District.Code,
            WardCode = _vm.Ward.Code,
            Detail = _vm.Detail,
            IsDefault = _vm.IsDefault,
        };
        Result<bool> result = await AddressService.AddUserAddress(model);
        if (result.Value)
        {
            Snackbar.Add("Thêm địa chỉ thành công", Severity.Success);
            DialogInstance.Close();
            StateHasChanged();
        }
        else
        {
            Snackbar.Add($"Thêm địa chỉ thất bại {result.Message}", Severity.Error);
        }
        HandleOperationResult(result, "Thêm địa chỉ thành công", "Thêm địa chỉ thất bại");
    }

    private async Task UpdateExistingAddress()
    {
        UpdateAddressRequest model = new()
        {
            Id = AddressId,
            ModifiedBy = CustomerId,
            PhoneNumber = _vm.PhoneNumber,
            ProvinceCode = _vm.Province.Code,
            DistrictCode = _vm.District.Code,
            WardCode = _vm.Ward.Code,
            Detail = _vm.Detail,
            IsDefault = _vm.IsDefault,
        };
        Result<bool> result = await AddressService.UpdateUserAddress(model);
        HandleOperationResult(result, "Cập nhật địa chỉ thành công", "Cập nhật địa chỉ thất bại");
    }

    private void HandleOperationResult(Result<bool> result, string successMessage, string failureMessage)
    {
        if (result.IsSuccess)
        {
            Snackbar.Add(successMessage, Severity.Success);
            DialogInstance.Close();
        }
        else
        {
            Snackbar.Add($"{failureMessage}: {result.Message}", Severity.Error);
        }
    }
}