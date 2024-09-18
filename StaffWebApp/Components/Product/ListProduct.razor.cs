using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Components.Product.Update;
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
    private IProductService ProductService { get; set; } = null!;

    private GetProductPaginationRequest _request = new();
    private PaginationResponse<ProductVm> _paginatedProduct = new();

    protected override async Task OnInitializedAsync()
    {
        await GetProducts();
    }

    private async Task GetProducts()
    {
        _paginatedProduct = await ProductService.GetProducts(_request);
    }

    private async Task ShowUpdateProductInfoDialog(Guid productId, string productName)
    {
        var dialogOptions = new DialogOptions { MaxWidth = MaxWidth.Large };
        var parameters = new DialogParameters();
        parameters.Add("ProductId", productId);
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
            { "ProductId", id }
        };
        var dialog = DialogService.Show<ListProductDetail>($"Chi tiết sản phẩm {name}", parameters, dialogOptions);
        var result = await dialog.Result;
        if (result is not null && !result.Canceled)
        {
            await GetProducts();
        }
    }

    private void OnOpenCreateProductClick()
    {
        NavigationManager.NavigateTo("/create-product");
    }
}