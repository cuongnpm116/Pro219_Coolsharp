using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Services.Voucher;
using StaffWebApp.Services.Voucher.Requests;
using StaffWebApp.Services.Voucher.ViewModel;

namespace StaffWebApp.Components.Voucher;

public partial class VoucherDetail
{
    [Inject]
    private IVoucherSevice VoucherService { get; set; }

    [Inject]
    private ISnackbar Snackbar { get; set; }
    [Inject]
    private IDialogService DialogService { get; set; }
    [Parameter]
    public Guid VoucherId { get; set; }
    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }

    public string VoucherName = "";
    private VoucherDetailVm _voucherDetailVm = new();

    private UpdateVoucherRequest _updateRequest = new();
    private int _typeDiscount = 0;
    private bool IsReadOnly = true;
    protected override async Task OnInitializedAsync()
    {
        await GetVoucher();
    }
    private void OpenUpdate()
    {
        IsReadOnly = false;
    }
    private bool? messageResult;

    private async Task HandleButtonClick()
    {
        if (IsReadOnly)
        {
            OpenUpdate();
        }
        else
        {
            messageResult = await DialogService.ShowMessageBox("Cảnh báo",
                                            "Bạn chắc chắn muốn sửa khuyến mãi này?",
                                            yesText: "Sửa",
                                            cancelText: "Hủy");
            if (messageResult == true && Validate())
            {
                _updateRequest.Id = _voucherDetailVm.Id;
                _updateRequest.Name = _voucherDetailVm.Name;
                _updateRequest.DiscountAmount = _voucherDetailVm.DiscountAmount;
                _updateRequest.DiscountPercent = _voucherDetailVm.DiscountPercent;
                if (_typeDiscount == 0)
                {
                    _updateRequest.DiscountAmount = 0;
                }
                else if (_typeDiscount == 1)
                {
                    _updateRequest.DiscountPercent = 0;
                }

                _updateRequest.StartedDate = _voucherDetailVm.StartedDate;
                _updateRequest.Stock = _voucherDetailVm.Stock;
                _updateRequest.FinishedDate = _voucherDetailVm.FinishedDate;
                _updateRequest.DiscountCondition = _voucherDetailVm.DiscountCondition;
                _updateRequest.Status = _voucherDetailVm.Status;

                var result = await VoucherService.UpdateVoucher(_updateRequest);
                if (result.IsSuccess)
                {
                    Snackbar.Add("Cập nhật thành công!", Severity.Success);
                    await GetVoucher();
                    IsReadOnly = true;
                    StateHasChanged();
                }
                else
                {
                    Snackbar.Add("Cập nhật không thành công.", Severity.Error);
                }
            }
        }
    }

    private bool Validate()
    {
        if (string.IsNullOrWhiteSpace(_voucherDetailVm.Name))
        {
            Snackbar.Add("Vui lòng nhập tên giảm giá.", Severity.Error);
            return false;
        }
        if (_voucherDetailVm.DiscountAmount < 0 || _voucherDetailVm.DiscountPercent < 0)
        {
            Snackbar.Add("Giá trị giảm không được âm.", Severity.Error);
            return false;
        }
        if (_voucherDetailVm.DiscountPercent > 100)
        {
            Snackbar.Add("Giảm giá theo phần trăm chỉ từ 0 - 100.", Severity.Error);
            return false;
        }

        if (_voucherDetailVm.StartedDate >= _voucherDetailVm.FinishedDate)
        {
            Snackbar.Add("Thời gian kết thúc đợt giảm giá phải lớn hơn thời gian bắt đầu đợt giảm giá.", Severity.Error);
            return false;
        }

        return true;
    }

    private async Task GetVoucher()
    {
        var result = await VoucherService.GetVoucherById(VoucherId);
        if (result.Value != null)
        {
            _voucherDetailVm = result.Value;
            VoucherName = result.Value.Name;
            if (_voucherDetailVm.DiscountPercent != 0)
            {
                _typeDiscount = 0;
            }
            else if (_voucherDetailVm.DiscountAmount != 0)
            {
                _typeDiscount = 1;
            }
        }
    }
}