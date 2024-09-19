using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Components.Product.Update;
using StaffWebApp.Services.Category;
using StaffWebApp.Services.Category.ViewModel;
using StaffWebApp.Services.Product;
using StaffWebApp.Services.Product.Requests;
using StaffWebApp.Services.Product.Vms;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Components.Product;
public partial class ListProduct
{
    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private ICategoryService CategoryService { get; set; } = null!;

    [Inject]
    private IProductService ProductService { get; set; } = null!;

    private GetProductPaginationRequest _request = new();
    private PaginationResponse<ProductVm> _paginatedProduct = new();
    private IEnumerable<CategoryVm> _categories = [];

    protected override async Task OnInitializedAsync()
    {
        await GetListCategories();
        await GetProducts();
    }

    private async Task GetProducts()
    {
        _paginatedProduct = await ProductService.GetProducts(_request);
    }

    private async Task GetListCategories()
    {
        var result = await CategoryService.Categories();
        _categories = result.Value;
    }

    public async Task OnListDetailDialogSuccess()
    {
        await GetProducts();
        StateHasChanged();
    }

    private async Task ShowUpdateProductInfoDialog(Guid productId, string productName)
    {
        var dialogOptions = new DialogOptions { MaxWidth = MaxWidth.Large };
        var parameters = new DialogParameters
        {
            { "ProductId", productId }
        };
        var dialog = DialogService.Show<UpdateProductInfoForm>($"Sửa thông tin chung cho {productName}", parameters, dialogOptions);
        var result = await dialog.Result;
        if (result is not null && !result.Canceled)
        {
            await GetProducts();
        }
    }

    private async Task ShowDetails(Guid id, string name)
    {
        var dialogOptions = new DialogOptions { MaxWidth = MaxWidth.Large };
        var parameters = new DialogParameters
        {
            { "ProductId", id },
        };
        var dialog = DialogService.Show<ListProductDetail>($"Chi tiết sản phẩm {name}", parameters, dialogOptions);
        var result = await dialog.Result;
        if (result is not null && !result.Canceled)
        {
            await GetProducts();
            StateHasChanged();
        }
    }

    private void OnOpenCreateProductClick()
    {
        NavigationManager.NavigateTo("/create-product");
    }

    private async void ClearSearch()
    {
        _request.SearchString = string.Empty;
        _request.CategoryId = Guid.Empty;
        await GetProducts();
        StateHasChanged();
    }

    private async Task OnNextPageClick()
    {
        _request.PageNumber++;
        await GetProducts();
    }

    private async Task OnPreviousPageClick()
    {
        _request.PageNumber--;
        await GetProducts();
    }
}