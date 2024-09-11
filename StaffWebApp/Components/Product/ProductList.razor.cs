using System.Reflection.Metadata;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Service.Category;
using StaffWebApp.Service.Category.ViewModel;
using StaffWebApp.Services.Order.Vms;
using StaffWebApp.Services.Product;
using StaffWebApp.Services.Product.Requests;
using StaffWebApp.Services.Product.Vms;
using WebAppIntegrated.Pagination;
using static Org.BouncyCastle.Crypto.Digests.SkeinEngine;

namespace StaffWebApp.Components.Product;
public partial class ProductList
{
    [Parameter]
    public string SearchString { get; set; } = "";

    [Parameter]
    public string FilterCategory { get; set; } = "";

    [Inject]
    private IProductService ProductService { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    [Inject]
    private ICategoryService CategoryService { get; set; } = null!;
    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }

    public string _emptyString = "";
    private PaginationResponse<ProductVm> _lstProduct = new();
    private ProductPaginationRequest _pagingnationRequest = new();
    List<CategoryVm> _categoryVms = [];

    protected override async Task OnInitializedAsync()
    {
        var result = await CategoryService.Categories();
        _categoryVms = result.Value;
        await ListProduct();
        StateHasChanged();
    }

    protected async override Task OnParametersSetAsync()
    {
        var uri = new Uri(NavigationManager.Uri);
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        SearchString = query.ContainsKey("SearchString") ? query["SearchString"].ToString() : "";
        FilterCategory = query.ContainsKey("FilterCategory") ? query["FilterCategory"].ToString() : "";

        // Ensure the product list is updated with the new search and filter parameters
        _pagingnationRequest.SearchString = SearchString;
        _pagingnationRequest.CategoryName = FilterCategory;
        await ListProduct();
        StateHasChanged();
    }

    private async Task OpenProductDetail(Guid productId)
    {
        var parameters = new DialogParameters();

        if (_lstProduct.Data != null)
        {
            var productDetails = _lstProduct.Data.FirstOrDefault(x=> x.Id  == productId)?.ProductDetails;

            parameters.Add("ProductDetails",productDetails);
            parameters.Add("ProductId", productId);
        }
        var dialog = DialogService.Show<ProductDetail>("", parameters);
        var result = await dialog.Result;
        if (Convert.ToBoolean(result.Data))
        {
            await ListProduct();
        }
    }
    //private async Task OpenUpdateDialog(Guid Id, Guid ProductId, string Name)
    //{
    //    var parameter = new DialogParameters
    //    {
    //        { "Id", Id },
    //        { "ProductId", ProductId },
    //        { "ProductName", Name }
    //    };

    //    var dialog = DialogService.Show<UpdateProductDetail>("", parameter);
    //    var result = await dialog.Result;
    //    if (Convert.ToBoolean(result.Data))
    //    {
    //        await ListProduct();
    //    }
    //}

    private async Task ListProduct()
    {
        _pagingnationRequest.SearchString = SearchString;
        _pagingnationRequest.CategoryName = FilterCategory;

        var response = await ProductService.ShowProduct(_pagingnationRequest);

        if (response != null && response.Value != null)
        {
            _lstProduct = response.Value;
            _lstProduct.Data = response.Value.Data;
        }
    }

    private void ShowBtnPress(Guid id)
    {
        ProductVm showProduct = _lstProduct.Data.FirstOrDefault(x => x.Id == id);
        showProduct.IsShow = !showProduct.IsShow;
    }

    private async Task OnPageChangedNext()
    {
        if (_lstProduct.HasNext)
        {
            _pagingnationRequest.PageNumber++;
            await ListProduct();
            StateHasChanged();
        }
    }

    private async Task OnPageChangedPrevious()
    {
        _pagingnationRequest.PageNumber--;
        await ListProduct();
        StateHasChanged();
    }

    private void AddNew()
    {
        NavigationManager.NavigateTo("/create-product");
    }

    private async void Search()
    {
        if (!string.IsNullOrWhiteSpace(SearchString) || !string.IsNullOrWhiteSpace(FilterCategory))
        {
            var uri = $"/products/product-staffapp?SearchString={Uri.EscapeDataString(SearchString)}&FilterCategory={Uri.EscapeDataString(FilterCategory)}";
            NavigationManager.NavigateTo(uri);
            _pagingnationRequest.PageNumber = 1; // Reset to first page
            await ListProduct();
        }
        else
        {
            NavigationManager.NavigateTo($"/products/product-staffapp");
            _pagingnationRequest.PageNumber = 1; // Reset to first page
            await ListProduct();
        }
        StateHasChanged();
    }

    private async Task ClearSearch()
    {
        SearchString = string.Empty;
        NavigationManager.NavigateTo($"/products/product-staffapp");
        _pagingnationRequest.PageNumber = 1; // Reset to first page
        await ListProduct();
        StateHasChanged();
    }
}