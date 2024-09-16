using CustomerWebApp.Components.Address;
using CustomerWebApp.Service.Address.ViewModel;
using CustomerWebApp.Service.Address;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebAppIntegrated.AddressService;
using CustomerWebApp.Service.Address.Dtos;

namespace CustomerWebApp.Components.Customer;

public partial class Address
{
    [Parameter]
    public Guid CustomerId { get; set; }
    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private IAddressService AddressService { get; set; } = null!;

    private List<AddressModel> _addresses;
    private Guid _defaultAddressId = Guid.Empty;

    protected override async Task OnInitializedAsync()
    {
        await LoadUserAddress();
    }

    private async Task LoadUserAddress()
    {
        var result = await AddressService.GetUserAddress(CustomerId);
        _addresses = result.Value;
        var defaultAddress = _addresses.FirstOrDefault(x => x.IsDefault == true);
        _defaultAddressId = defaultAddress != null ? defaultAddress.Id : Guid.Empty;
    }

    private async Task OpenAddOrEditAddress(Guid addressId, Guid userId, bool isDefault)
    {
        DialogParameters dialogParams = [];
        dialogParams.Add("AddressId", addressId);
        dialogParams.Add("CustomerId", userId);
        dialogParams.Add("IsDefault", isDefault);

        string title = addressId == Guid.Empty ? "Thêm địa chỉ" : "Cập nhật địa chỉ";

        DialogOptions dialogOptions = new()
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseButton = true,
            CloseOnEscapeKey = true,
        };

        IDialogReference dialog = await DialogService.ShowAsync<AddOrEditAddress>(title, dialogParams, dialogOptions);
        DialogResult dialogResult = await dialog.Result;
        if (!dialogResult.Canceled)
        {
            await LoadUserAddress();
        }
    }

    private async Task SetDefaultAddress(Guid currentDefaultAddressId, Guid newDefaultAddressId)
    {
        MakeDefaultAddressModel model = new(currentDefaultAddressId, newDefaultAddressId);
        var result = await AddressService.MakeDefaultAddress(model);
        if (!result.Value)
        {
            Snackbar.Add($"Đặt địa chỉ mặc định thất bại. {result.Message}", Severity.Error);
        }
        else
        {
            Snackbar.Add("Đặt địa chỉ mặc định thành công", Severity.Success);
            await LoadUserAddress();
        }
    }

    private async Task DeleteAddress(Guid addressId, Guid deletedBy)
    {
        DeleteAddressRequest model = new(addressId, deletedBy);
        var result = await AddressService.DeleteAddress(model);
        if (!result.Value)
        {
            Snackbar.Add($"Xóa địa chỉ thất bại. {result.Message}", Severity.Error);
        }
        else
        {
            Snackbar.Add("Xóa địa chỉ thành công", Severity.Success);
            await LoadUserAddress();
        }
    }
}