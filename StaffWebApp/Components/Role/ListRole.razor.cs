using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Services.Role;
using StaffWebApp.Services.Role.Requests;
using StaffWebApp.Services.Role.Vms;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Components.Role;
public partial class ListRole
{
    [Inject]
    private IRoleService RoleService { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    private PaginationResponse<RoleVm> _paginatedRoles = new();
    private GetRolesWithPaginationRequest _request = new();

    private RoleVm _selectedRole;
    private RoleVm _selectedRoleBeforeEdit;

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        var result = await RoleService.GetRolesWithPagination(_request);
        _paginatedRoles = result.Value;
    }

    private async Task OpenCreateRoleDialog()
    {
        DialogOptions options = new()
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
        };
        var dialog = await DialogService.ShowAsync<CreateRoleDialog>("Tạo mới vai trò", options);
        var result = await dialog.Result;
        if (result is not null && !result.Canceled)
        {
            await LoadData();
        }
    }

    private async Task UpdateRole(object role)
    {
        var result = await RoleService.UpdateRole((RoleVm)role);
        if (result.Value is true)
        {
            Snackbar.Add("Cập nhật vai trò thành công", Severity.Success);
            return;
        }
        Snackbar.Add("Cập nhật vai trò thất bại", Severity.Error);
    }

    private void BackupItem(object item)
    {
        _selectedRoleBeforeEdit = new()
        {
            Id = ((RoleVm)item).Id,
            Code = ((RoleVm)item).Code,
            Name = ((RoleVm)item).Name,
        };
    }

    private void SetToOriginValue(object item)
    {
        ((RoleVm)item).Id = _selectedRoleBeforeEdit.Id;
        ((RoleVm)item).Code = _selectedRoleBeforeEdit.Code;
        ((RoleVm)item).Name = _selectedRoleBeforeEdit.Name;
    }
}