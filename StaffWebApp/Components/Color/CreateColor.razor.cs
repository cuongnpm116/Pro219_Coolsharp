using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Services.Color;
using StaffWebApp.Services.Color.Requests;
using StaffWebApp.Services.Color.Vms;

namespace StaffWebApp.Components.Color;

public partial class CreateColor
{
    [Inject]
    private IColorService ColorService { get; set; } = null!;

    [Inject]
    private IDialogService DiagService { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public Guid Id { get; set; }

    private CreateColorRequest _createRequest = new();
    private UpdateColorRequest _updateRequest = new();

    private ColorVm _colorVm = new();
    bool success;

    protected override async Task OnInitializedAsync()
    {
        if (Id != Guid.Empty)
        {
            var result = await ColorService.GetColorById(Id);
            if (result.Value != null)
            {
                _colorVm = result.Value;

            }
        }
    }

    private async Task Submit()
    {
        bool? messageResult = await DiagService.ShowMessageBox(
            "Xác nhận",
            $"Bạn chắc chắn muốn thêm màu {_colorVm.Name}?",
            yesText: "Lưu",
            cancelText: "Hủy");
        if (messageResult == true && Validate())
        {
            if (Id == Guid.Empty)
            {
                _createRequest.Name = _colorVm.Name.Trim(); ;
                var result = await ColorService.CreateColor(_createRequest);
                if (result.Value == false)
                {
                    Snackbar.Add("Tên màu bị trùng", Severity.Error);
                    return;
                }
                success = result.Value;
                Snackbar.Add("Thêm màu thành công", Severity.Success);
            }
            else
            {
                _updateRequest.Id = Id;
                _updateRequest.Name = _colorVm.Name.Trim(); ;
                _updateRequest.Status = _colorVm.Status;
                var result = await ColorService.UpdateColor(_updateRequest);
                if (result.Value == false)
                {
                    Snackbar.Add("Tên màu bị trùng", Severity.Error);
                    return;
                }
                success = result.Value;
                Snackbar.Add("Cập nhật màu thành công", Severity.Success);
            }
            MudDialog.Close(DialogResult.Ok(success));
        }
    }

    private bool Validate()
    {
        if (string.IsNullOrWhiteSpace(_colorVm.Name))
        {
            Snackbar.Add("Vui lòng nhập màu.", Severity.Error);
            return false;
        }
        if (_colorVm.Name.Length > 100)
        {
            Snackbar.Add("Tên màu không được vượt quá 100 ký tự.", Severity.Error);
            return false;
        }
        return true;
    }

    private void OnClickCloseBtn()
    {
        MudDialog.Close(DialogResult.Ok(false));
    }
}
