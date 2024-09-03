using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Services.Size;
using StaffWebApp.Services.Size.Requests;
using StaffWebApp.Services.Size.Vms;

namespace StaffWebApp.Components.Size;

public partial class CreateSize
{
    [Inject]
    private ISizeService SizeService { get; set; }

    [Inject]
    private IDialogService DiagService { get; set; }
    [Inject]
    private ISnackbar Snackbar { get; set; }
    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }
    [Parameter]
    public Guid Id { get; set; }

    private CreateSizeRequest _createRequest = new();
    private UpdateSizeRequest _updateRequest = new();

    private SizeVm _sizeVm = new();
    bool success;

    protected override async Task OnInitializedAsync()
    {
        if (Id != Guid.Empty)
        {
            var result = await SizeService.GetSizeById(Id);
            if (result.Value != null)
            {
                _sizeVm = result.Value;

            }

        }
    }
    private async Task Submit()
    {
        bool? messageResult = await DiagService.ShowMessageBox("Cảnh báo",
                                                         "Bạn chắc chắn với thay đổi?",
                                                         yesText: "Lưu",
                                                         cancelText: "Hủy");
        if (messageResult == true && Validate())
        {
            if (Id == Guid.Empty)
            {


                _createRequest.SizeNumber = _sizeVm.SizeNumber;
                _createRequest.Status = _sizeVm.Status;
                var result = await SizeService.CreateSize(_createRequest);
                success = result.Value;
                Snackbar.Add($"{result.Message}", Severity.Success);
            }
            else
            {
                _updateRequest.Id = Id;
                _updateRequest.SizeNumber = _sizeVm.SizeNumber;
                _updateRequest.Status = _sizeVm.Status;
                var result = await SizeService.UpdateSize(_updateRequest);
                success = result.Value;
                Snackbar.Add($"{result.Message}", Severity.Success);
            }


            MudDialog?.Close(DialogResult.Ok(success));

        }
    }
    private bool Validate()
    {
        if (_sizeVm.SizeNumber == null || string.IsNullOrWhiteSpace(_sizeVm.SizeNumber.ToString()))
        {
            Snackbar.Add("Vui lòng nhập size.", Severity.Error);
            return false;
        }

        if (_sizeVm.SizeNumber <= 0)
        {
            Snackbar.Add("Size không được là số âm và không được bằng 0.", Severity.Error);
            return false;
        }

        if (_sizeVm.SizeNumber % 1 != 0)
        {
            Snackbar.Add("Size phải là số nguyên.", Severity.Error);
            return false;
        }

        return true;
    }


    private void OnClickCloseBtn()
    {
        MudDialog?.Close(DialogResult.Ok(false));
    }
}
