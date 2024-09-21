using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using StaffWebApp.Services.Size;
using StaffWebApp.Services.Size.Requests;
using StaffWebApp.Services.Size.Vms;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Components.Size;

public partial class ListSize
{
    [Inject]
    private ISizeService SizeService { get; set; }
    [Inject]
    private IDialogService DialogService { get; set; }
    [Inject]
    private ISnackbar Snackbar { get; set; }
    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }
    private PaginationResponse<SizeVm> _listSize = new();
    private SizePaginationRequest _paginationRequest = new();



    protected override async Task OnInitializedAsync()
    {
        await LoadSize();
        StateHasChanged();
    }
    private async Task LoadSize()
    {
        var response = await SizeService.GetSizesWithPagination(_paginationRequest);

        if (response.Value != null)
        {
            _listSize = response.Value;
            _listSize.Data = response.Value.Data;
        }
    }
    protected async override Task OnParametersSetAsync()
    {
        if (!string.IsNullOrEmpty(_paginationRequest.SearchString))
        {
            _paginationRequest.PageNumber = 1;
            await LoadSize();
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
            await LoadSize();
            StateHasChanged();
        }
        else
        {
            _paginationRequest.PageNumber = 1;
            await LoadSize();
            StateHasChanged();
        }
    }

    private async Task OnNextPageClicked()
    {
        if (_listSize.HasNext)
        {
            _paginationRequest.PageNumber++;
            await LoadSize();
            StateHasChanged();
        }
    }

    private async Task OnPreviousPageClicked()
    {
        if (_listSize.PageNumber > 0)
        {

            _paginationRequest.PageNumber--;
            await LoadSize();
            StateHasChanged();
        }
    }

    private async Task OpenDialog(Guid id)
    {
        var parameter = new DialogParameters();
        parameter.Add("Id", id);
        var dialog = DialogService.Show<CreateSize>("", parameter);
        var result = await dialog.Result;
        if (Convert.ToBoolean(result.Data))
        {
            await LoadSize();
        }
    }
}
