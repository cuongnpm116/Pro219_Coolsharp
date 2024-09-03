using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using StaffWebApp.Services.Color;
using StaffWebApp.Services.Color.Requests;
using StaffWebApp.Services.Color.Vms;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Components.Color;

public partial class ListColor
{
    [Inject]
    private IColorService ColorService { get; set; }
    [Inject]
    private IDialogService DialogService { get; set; }
    [Inject]
    private ISnackbar Snackbar { get; set; }
    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }
    private PaginationResponse<ColorVm> _lstColor = new();
    private ColorPaginationRequest _paginationRequest = new();
    bool success;
    protected override async Task OnInitializedAsync()
    {
        await LoadColor();
        StateHasChanged();
    }
    private async Task LoadColor()
    {
        var response = await ColorService.GetColorsWithPagination(_paginationRequest);

        if (response.Value != null)
        {
            _lstColor = response.Value;
            _lstColor.Data = response.Value.Data;
        }
    }
    protected async override Task OnParametersSetAsync()
    {
        if (!string.IsNullOrEmpty(_paginationRequest.SearchString))
        {
            _paginationRequest.PageNumber = 1;
            await LoadColor();
            StateHasChanged();
        }

    }

    private async Task HandleKeyPress(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await HandleSearchValueChanged();
        }
    }
    private async Task HandleSearchValueChanged()
    {
        if (!string.IsNullOrEmpty(_paginationRequest.SearchString))
        {
            await LoadColor();
            StateHasChanged();
        }
        else
        {
            _paginationRequest.PageNumber = 1;
            await LoadColor();
            StateHasChanged();
        }
    }

    private async Task OnNextPageClicked()
    {
        if (_lstColor.HasNext)
        {
            _paginationRequest.PageNumber++;
            await LoadColor();
            StateHasChanged();
        }
    }

    private async Task OnPreviousPageClicked()
    {
        if (_lstColor.PageNumber > 0)
        {

            _paginationRequest.PageNumber--;
            await LoadColor();
            StateHasChanged();
        }
    }


    private async Task OpenDialog(Guid id)
    {
        var parameter = new DialogParameters();
        parameter.Add("Id", id);
        var dialog = DialogService.Show<CreateColor>("", parameter);
        var result = await dialog.Result;
        if (Convert.ToBoolean(result.Data))
        {
            await LoadColor();
        }
    }
}
