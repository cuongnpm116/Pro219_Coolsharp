using CustomerWebApp.Components.Address;
using CustomerWebApp.Service.Address;
using CustomerWebApp.Service.Address.ViewModel;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CustomerWebApp.Components.Payment;

public partial class CheckOutAddress
{
    [CascadingParameter]
    public MudDialogInstance Dialog { get; set; } = null!;

    [Parameter]
    public Guid CustomerId { get; set; }

    [Parameter]
    public Guid SelectedAddressId { get; set; }

    [Inject]
    private IAddressService AddressService { get; set; } = null!;

    [Inject]
    private IDialogService DialogService { get; set; }

    private IEnumerable<AddressModel> _addresses = [];
    private AddressModel _selectedAddress;

    protected override async Task OnInitializedAsync()
    {
        await GetAddresses();
        if (SelectedAddressId != Guid.Empty)
        {
            _selectedAddress = _addresses.FirstOrDefault(x => x.Id == SelectedAddressId);
        }
    }

    private async Task GetAddresses()
    {
        var result = await AddressService.GetUserAddress(CustomerId);
        _addresses = result.Value;
        StateHasChanged();
    }

    private async Task EditAddress(Guid addressId, Guid modifierId, bool isDefault)
    {

        DialogParameters dialogParams = [];
        dialogParams.Add("AddressId", addressId);
        dialogParams.Add("UserId", modifierId);
        dialogParams.Add("IsDefault", isDefault);

        string title = "Cập nhật địa chỉ";

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
            await GetAddresses();
        }
    }

    private void SelectAddress()
    {
        Dialog.Close(DialogResult.Ok(_selectedAddress.Id));
    }

    private async Task AddAddress(Guid userId)
    {
        DialogParameters dialogParams = [];
        dialogParams.Add("AddressId", Guid.Empty);
        dialogParams.Add("UserId", userId);
        dialogParams.Add("IsDefault", false);

        string title = "Cập nhật địa chỉ";

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
            await GetAddresses();
        }
    }
}
