using CustomerWebApp.Components.Categorys.ViewModel;
using CustomerWebApp.Components.Products.Dtos;
using CustomerWebApp.Components.Products.ViewModel;
using CustomerWebApp.Service.Category;
using CustomerWebApp.Service.Product;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebAppIntegrated.Constants;
using WebAppIntegrated.Pagination;

namespace CustomerWebApp.Components.Products
{
    public partial class Products
    {
        #region Variable, parameter, Inject
        [Inject]
        private IProductService ProductService { get; set; }
        [Inject]
        private ICategoryService CategoryService { get; set; }
        [Parameter]
        public string SearchString { get; set; } = "";
        [Parameter]
        public Guid ProductId { get; set; }

        private PaginationResponse<ProductVm> _lstProduct = new();
        
        private ProductPaginationRequest _pagingnationRequest = new();
        private string _imageUrl = ShopConstants.EShopApiHost + "/user-content/";

        //private CategoryVm category = new();
        private List<CategoryVm> _categories = new();
        private Dictionary<Guid, bool> _categoriesDictionary = new Dictionary<Guid, bool>();
        private List<Guid> _categoryIds = new();
       
        
        private Transition transition = Transition.Slide;
        #endregion

        #region Category
        private async Task ListCategory()
        {
            var categoriesResponse = await CategoryService.Categories();
            if (categoriesResponse.Value != null)
            {
                _categories = categoriesResponse.Value;
            }
        }
        private void InitializeCategoriesDictionary()
        {
            _categoriesDictionary.Clear();
            foreach (var category in _categories)
            {
                if (!_categoriesDictionary.ContainsKey(category.CategoryId))
                {
                    _categoriesDictionary.Add(category.CategoryId, false);
                }
            }
        }
        private async void UpdateSelectedIds(bool value, Guid id)
        {
            if (value)
            {
                if (_categoriesDictionary.ContainsKey(id) && !_categoryIds.Contains(id))
                {
                    _categoryIds.Add(id);
                }
            }
            else
            {
                if (_categoriesDictionary.ContainsKey(id) && _categoryIds.Contains(id))
                {
                    _categoryIds.Remove(id);
                }
            }
            _categoriesDictionary[id] = value;
            _pagingnationRequest.PageNumber = 1;
            _pagingnationRequest.CategoryIds = new List<Guid>(_categoryIds);

            await ListProduct();
            StateHasChanged();
        }
        #endregion

        #region Product

        private async Task ListProduct()
        {
            _pagingnationRequest.SearchString = SearchString;

            if (_categoryIds != null)
                _pagingnationRequest.CategoryIds = _categoryIds;
            var response = await ProductService.ShowProductOnCustomerAppVm(_pagingnationRequest);

            if (response.Value != null)
            {
                _lstProduct = response.Value;
                _lstProduct.Data = response.Value.Data;
            }
        }
        private async Task FilterByPrice()
        {
            _pagingnationRequest.PageNumber = 1; // Reset to first page for new filter
            await ListProduct();
        }
        #endregion

        #region Lifecycle
        protected async override Task OnParametersSetAsync()
        {
            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                _pagingnationRequest.PageNumber = 1;
                _pagingnationRequest.CategoryIds = _categoryIds;
                await ListProduct();
                StateHasChanged();
            }
        }
        protected override async Task OnInitializedAsync()
        {
            await ListCategory();
            InitializeCategoriesDictionary();
            await ListProduct();
            StateHasChanged();
        }
        #endregion

        #region Pagingnation
        private async Task OnNextPageClicked()
        {
            if (_lstProduct.HasNext)
            {
                _pagingnationRequest.PageNumber++;
                await ListProduct();
            }
        }

        private async Task OnPreviousPageClicked()
        {
            _pagingnationRequest.PageNumber--;
            await ListProduct();
        }
        #endregion
    }
}