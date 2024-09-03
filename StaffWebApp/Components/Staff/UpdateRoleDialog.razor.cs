using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Services.Role;
using StaffWebApp.Services.Role.Vms;
using StaffWebApp.Services.Staff;

namespace StaffWebApp.Components.Staff;
public partial class UpdateRoleDialog
{
    [CascadingParameter]
    public MudDialogInstance Dialog { get; set; } = null!;

    [Parameter]
    public Guid StaffId { get; set; } = new("B48703E5-2BC4-4996-88DD-4369D76FD61D");

    [Inject]
    private IRoleService RoleService { get; set; } = null!;

    [Inject]
    private IStaffService StaffService { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    private IReadOnlyList<RoleVmForGetAll> _availableRoles = [];
    private Dictionary<Guid, bool> _checkedRoles = [];

    protected override async Task OnInitializedAsync()
    {
        await GetAvailableRoles();
        await GetStaffRoleIds();
    }

    private async Task UpdateStaffRole()
    {
        Guid[] selectedRoleIds = _checkedRoles
            .Where(x => x.Value is true)
            .Select(x => x.Key)
            .ToArray();
        if (selectedRoleIds.Length == 0)
        {
            Snackbar.Add("Hãy chọn ít nhất 1 vai trò cho nhân viên", Severity.Error);
            return;
        }
        bool? confirm = await DialogService.ShowMessageBox(
            "Xác nhận",
            "Bạn chắc chắn muốn cập nhật?",
            yesText: "Cập nhật",
            cancelText: "Hủy");

        if (confirm is true)
        {
            var result = await StaffService.UpdateStaffRole(new(StaffId, selectedRoleIds));
            if (result.IsSuccess)
            {
                Snackbar.Add("Cập nhật thông tin thành công", Severity.Success);
                Dialog.Close(DialogResult.Ok(true));
            }
            else
            {
                Snackbar.Add("Cập nhật thông tin thất bại", Severity.Error);
            }
        }

    }

    private async Task GetAvailableRoles()
    {
        var result = await RoleService.GetRoles();
        _availableRoles = result.Value;
        foreach (var item in _availableRoles)
        {
            _checkedRoles[item.Id] = false;
        }
    }

    private async Task GetStaffRoleIds()
    {
        var result = await RoleService.GetRoleIdsByStaffId(StaffId);
        foreach (var item in result.Value)
        {
            _checkedRoles[item] = true;
        }
    }

    private void CheckedChange(bool value, Guid roleId)
    {
        _checkedRoles[roleId] = value;
    }
}