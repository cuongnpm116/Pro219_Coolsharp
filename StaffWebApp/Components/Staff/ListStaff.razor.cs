using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Services.Staff;
using StaffWebApp.Services.Staff.Requests;
using StaffWebApp.Services.Staff.Vms;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Components.Staff;
public partial class ListStaff
{
    [Inject]
    private IStaffService StaffService { get; set; } = null!;

    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    private PaginationResponse<StaffVm> _paginatedStaff = new();
    private GetListStaffRequest _request = new();

    protected override async Task OnInitializedAsync()
    {
        await GetStaff();
    }

    private async Task GetStaff()
    {
        var apiRes = await StaffService.GetListStaffWithPagination(_request);
        _paginatedStaff = apiRes.Value;
    }

    private async Task OpenAddStaffDialog()
    {
        DialogOptions dialogOptions = new()
        {
            MaxWidth = MaxWidth.Medium,
        };
        var dialog = await DialogService.ShowAsync<AddStaffDialog>("Thêm nhân viên", dialogOptions);
        DialogResult result = await dialog.Result;
        if (result is not null && !result.Canceled)
        {
            await GetStaff();
        }
    }

    private async Task OpenUpdateStaffInfoDialog(Guid staffId)
    {
        DialogOptions dialogOptions = new()
        {
            MaxWidth = MaxWidth.Medium,
        };
        var parameters = new DialogParameters
        {
            { "StaffId", staffId }
        };
        var dialog = await DialogService.ShowAsync<UpdateStaffInfoDialog>("Cập nhật nhân viên", parameters, dialogOptions);
        DialogResult result = await dialog.Result;
        if (result is not null && !result.Canceled)
        {
            await GetStaff();
        }
    }

    private async Task OpenUpdateRoleDialog(Guid staffId)
    {
        DialogOptions dialogOptions = new()
        {
            MaxWidth = MaxWidth.Medium,
        };
        var parameters = new DialogParameters
        {
            { "StaffId", staffId }
        };
        var dialog = await DialogService.ShowAsync<UpdateRoleDialog>("Cập nhật vai trò", parameters, dialogOptions);
        DialogResult result = await dialog.Result;
        if (result is not null && !result.Canceled)
        {
            await GetStaff();
        }
    }

    private async Task OnNextPageClicked()
    {
        if (_paginatedStaff.HasNext)
        {
            _request.PageNumber++;
            await GetStaff();
        }
    }

    private async Task OnPreviousPageClicked()
    {
        _request.PageNumber--;
        await GetStaff();
    }
}