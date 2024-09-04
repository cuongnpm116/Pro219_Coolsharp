using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Services.Role;
using StaffWebApp.Services.Role.Vms;
using StaffWebApp.Services.Staff;
using StaffWebApp.Services.Staff.Requests;
using StaffWebApp.Services.Staff.Vms;

namespace StaffWebApp.Components.Staff;
public partial class AddStaffDialog
{
    [CascadingParameter]
    public MudDialogInstance Dialog { get; set; } = null!;

    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private IRoleService RoleService { get; set; } = null!;

    [Inject]
    private IStaffService StaffService { get; set; } = null!;

    private IReadOnlyList<RoleVmForGetAll> _roles = [];
    private AddStaffVm _addStaffVm = new();

    private AddStaffVmValidator _validator = new();
    private MudSelect<RoleVmForGetAll> _roleSelect;
    private MudForm _addStaffForm;

    private bool isShow;
    private InputType PasswordInput = InputType.Password;
    private string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    protected override async Task OnInitializedAsync()
    {
        await GetRoles();
    }

    private async Task OnAddButtonClick()
    {
        await _addStaffForm.Validate();
        if (!_addStaffForm.IsValid)
        {
            Snackbar.Add("Thông tin chưa hợp lệ. Vui lòng kiểm tra lại", Severity.Error);
            return;
        }
        bool? confirm = await DialogService.ShowMessageBox(
            "Xác nhận",
            $"Bạn chắc chắn muốn thêm nhân viên {_addStaffVm.LastName + " " + _addStaffVm.FirstName}",
            yesText: "Thêm",
            cancelText: "Hủy");

        if (confirm == true)
        {
            var request = new AddStaffRequest(
                _addStaffVm.FirstName,
                _addStaffVm.LastName,
                (DateTime)_addStaffVm.DateOfBirth,
                _addStaffVm.Email,
                _addStaffVm.Username,
                _addStaffVm.Password,
                _addStaffVm.Roles);
            var result = await StaffService.AddStaff(request);
            if (result.IsSuccess && result.Value)
            {
                Snackbar.Add("Thêm nhân viên thành công", Severity.Success);
                Dialog.Close(DialogResult.Ok(true));
            }
            else
            {
                Snackbar.Add("Thêm nhân viên thất bại", Severity.Error);
                Dialog.Close(DialogResult.Cancel());
            }
        }
    }

    private async Task GetRoles()
    {
        var result = await RoleService.GetRoles();
        _roles = result.Value;
    }

    private async Task RoleSelectionChanged(IEnumerable<RoleVmForGetAll> values)
    {
        _addStaffVm.Roles = values;
        await _roleSelect.Validate();
    }

    private void OnShowPasswordClick()
    {
        if (isShow)
        {
            isShow = false;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInput = InputType.Password;
        }
        else
        {
            isShow = true;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInput = InputType.Text;
        }
    }
}