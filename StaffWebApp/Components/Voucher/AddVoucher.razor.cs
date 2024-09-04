using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Services.Voucher;
using StaffWebApp.Services.Voucher.Dtos;

namespace StaffWebApp.Components.Voucher;

public partial class AddVoucher
{
    [Inject]
    private IVoucherSevice VoucherService { get; set; }

    [Inject]
    private IDialogService DiagService { get; set; }
    [Inject]
    private ISnackbar Snackbar { get; set; }
    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }

    private AddVoucherRequest _createRequest = new();

    private int _typeDiscount = 0;
    bool success;

    private async Task Submit()
    {
        bool? messageResult = await DiagService.ShowMessageBox("Cảnh báo",
                                                         "Bạn chắc chắn muốn thêm mới khuyến mãi?",
                                                         yesText: "Có",
                                                         cancelText: "Hủy");
        if (messageResult == true && Validate())
        {

            if (_typeDiscount == 0)
            {
                _createRequest.DiscountAmount = 0;
            }
            else if (_typeDiscount == 1)
            {
                _createRequest.DiscountPercent = 0;
            }
            var result = await VoucherService.AddVoucher(_createRequest);
            if (result.IsSuccess)
            {
                Snackbar.Add($"{result.Message}", Severity.Success);
                MudDialog?.Close(DialogResult.Ok(true));
            }
            else
            {
                Snackbar.Add("Không thành công", Severity.Error);
            }
        }
    }
    private bool Validate()
    {
        if (string.IsNullOrWhiteSpace(_createRequest.Name))
        {
            Snackbar.Add("Vui lòng nhập tên giảm giá.", Severity.Error);
            return false;
        }
        if (_createRequest.DiscountAmount < 0 || _createRequest.DiscountPercent < 0 || _createRequest.DiscountCondition < 0)
        {
            Snackbar.Add("Giá trị giảm không được âm.", Severity.Error);
            return false;
        }
        if (_createRequest.DiscountPercent > 100)
        {
            Snackbar.Add("Giảm giá theo phần trăm chỉ từ 0 - 100.", Severity.Error);
            return false;
        }
        if (_createRequest.StartedDate < DateTime.Now.Date)
        {
            Snackbar.Add("Ngày bắt đầu không được là ngày trong quá khứ.", Severity.Error);
            return false;
        }
        if (_createRequest.StartedDate >= _createRequest.FinishedDate)
        {
            Snackbar.Add("Thời gian kết thúc đợt giảm giá phải lớn hơn thời gian bắt đầu đợt giảm giá.", Severity.Error);
            return false;
        }

        return true;
    }

    private void OnClickCloseBtn()
    {
        MudDialog?.Close(DialogResult.Ok(false));
    }
}