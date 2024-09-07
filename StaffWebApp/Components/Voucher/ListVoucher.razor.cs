using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using StaffWebApp.Services.Voucher;
using StaffWebApp.Services.Voucher.Dtos;
using StaffWebApp.Services.Voucher.ViewModel;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Components.Voucher;

public partial class ListVoucher
{
    #region Inject, Parameter

    [Inject]
    private IVoucherSevice VoucherService { get; set; }

    [Inject]
    private ISnackbar Snackbar { get; set; }

    [Inject]
    private IDialogService DiagService { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }


    #endregion Inject, Parameter

    #region Obj

    private PaginationResponse<VoucherVm> _lstVoucher = new PaginationResponse<VoucherVm>();
    private VoucherVm _voucherVm = new VoucherVm();
    private GetVoucherPaginationRequest paginationRequest = new();

    #endregion Obj

    private async Task LoadVoucher()
    {
        var response = await VoucherService.GetVoucherPaging(paginationRequest);

        if (response.Value != null)
        {
            _lstVoucher = response.Value;
            _lstVoucher.Data = response.Value.Data;
        }
    }
    protected async override Task OnParametersSetAsync()
    {
        if (!string.IsNullOrEmpty(paginationRequest.SearchString))
        {
            paginationRequest.PageNumber = 1;
            await LoadVoucher();
            StateHasChanged();
        }
    }
    protected override async Task OnInitializedAsync()
    {
        await LoadVoucher();
        StateHasChanged();
    }

    #region OpenDialog

    private async Task OpenAddVoucherDialog()
    {
        var dialog = DiagService.Show<AddVoucher>("");
        var result = await dialog.Result;
        if (Convert.ToBoolean(result.Data))
        {
            await LoadVoucher();
            StateHasChanged();
        }
    }

    #endregion OpenDialog


    private bool isEnterPressed;
    private async Task HandleKeyPress(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await HandleSearchValueChanged();
        }
    }
    private async Task HandleSearchValueChanged()
    {
        if (!string.IsNullOrEmpty(paginationRequest.SearchString))
        {
            await LoadVoucher();
            StateHasChanged();
        }
    }
    private async Task OnNextPageClicked()
    {
        if (_lstVoucher.HasNext)
        {
            paginationRequest.PageNumber++;
            await LoadVoucher();
        }
    }

    private async Task OnPreviousPageClicked()
    {
        paginationRequest.PageNumber--;
        await LoadVoucher();
    }

    private void NavigateToDetail(Guid voucherId)
    {
        if (voucherId != Guid.Empty)
        {
            NavigationManager.NavigateTo($"vouchers/" + voucherId);
        }
    }
}