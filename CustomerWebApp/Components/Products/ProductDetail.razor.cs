using CustomerWebApp.Components.Carts;
using CustomerWebApp.Service.Cart;
using CustomerWebApp.Service.Cart.Dtos;
using CustomerWebApp.Service.Cart.ViewModel;
using CustomerWebApp.Service.Product;
using CustomerWebApp.Service.Product.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using WebAppIntegrated.Constants;

namespace CustomerWebApp.Components.Products;

public partial class ProductDetail
{
    #region bien
    [Inject]
    private IProductService ProductService { get; set; }
    [Inject]
    NavigationManager Navigation { get; set; }
    [Inject]
    private ICartService CartService { get; set; }
    [Inject]
    private ISnackbar Snackbar { get; set; }
    [Inject]
    private CartState CartState { get; set; }
    [Inject]
    private SelectedProductState SelectedProductState { get; set; }
    [CascadingParameter]
    private Task<AuthenticationState> AuthStateTask { get; set; }

    [Parameter]
    public Guid ProductId { get; set; }
    private const string _imageUrl = ShopConstants.EShopApiHost + "/user-content/";
    private AddCartRequest _addCartRequest = new();
    private CartVm _cartVm = new();

    private ProductVm _product = new();


    private Dictionary<Guid, List<string>> ImageDict = new Dictionary<Guid, List<string>>();
    private List<string> ImagePaths = new();
    private string defaultImage;

    private List<ProductVm> _lstProduct = new(); //featured product

    //goi api cart

    private Guid ColorId;
    private Guid SizeId;
    private int cart;
    private string Error = "";

    private int SizeNumber;
    private string ColorName;
    private ProductDetailVm _productDetail = new();
    private ProductPriceVm _productPriceVm = new();
    private decimal Price;
    private ProductDetailVm _productDetailVm = new();

    private List<(Guid, string)> colorsList = [];
    private List<(Guid, int)> sizesList = [];
    public static string ProductName { get; set; } = "";
    private Transition transition = Transition.None;

    public Guid CustomerId = Guid.Parse("BCF83D3E-BC97-4813-8E2C-96FD34863EA8");
    #endregion

    #region product Method

    private async Task ProductDetailVm()
    {
        var response = await ProductService.ShowProductDetail(ProductId);

        if (response.Value != null)
        {
            _productDetail = response.Value;

            ProductName = _productDetail.ProductName;
            if (_productDetail.ColorsDictionary != null)
            {
                colorsList = _productDetail.ColorsDictionary.Select(kvp => (kvp.Key, kvp.Value)).ToList();
            }

            if (_productDetail.SizesDictionary != null)
            {
                sizesList = _productDetail.SizesDictionary.Select(kvp => (kvp.Key, kvp.Value)).ToList();
            }
        }
        StateHasChanged();
    }
    private async Task Images()
    {
        var result = await ProductService.GetDetailImage(ProductId);
        if (result.IsSuccess && result.Value != null)
        {
            ImageDict = result.Value;

            foreach (var kvp in ImageDict)
            {
                ImagePaths.AddRange(kvp.Value);
            }
            OnInitImage();
        }
    }
    private void OnInitImage()
    {
        defaultImage = ImagePaths.FirstOrDefault();
    }
    private void SelectImage(int index)
    {
        defaultImage = ImagePaths[index];
    }

    private async Task UpdatePrice()
    {
        if (ColorId != Guid.Empty && SizeId != Guid.Empty)
        {
            var priceResponse = await ProductService.GetProductDetailPrice(ProductId, ColorId, SizeId);
            if (priceResponse.IsSuccess && priceResponse.Value != null)
            {
                _productPriceVm = priceResponse.Value;
                _productDetail.Price = _productPriceVm.Price;
            }

        }
        StateHasChanged();
    }

    private async Task UpdateStock()
    {
        if (ColorId != Guid.Empty && SizeId != Guid.Empty)
        {
            var stockResponse = await ProductService.GetProductDetailStock(ProductId, ColorId, SizeId);
            if (stockResponse != null)
            {
                _productDetail.Stock = stockResponse.Value;
            }
        }
        StateHasChanged();
    }

    private async Task HandleButtonSizeClick(Guid id, object value)
    {

        if (SizeId == id)
        {
            SizeId = Guid.Empty;
        }
        else
        {
            SizeId = id;
        }
        SizeNumber = (int)value;
        _addCartRequest.SizeId = SizeId;
        await UpdateStock();
        await UpdatePrice();
        StateHasChanged();
    }

    private async Task HandleButtonColorClick(Guid id, object value)
    {

        if (ColorId == id)
        {
            ColorId = Guid.Empty;
        }
        else
        {
            ColorId = id;
        }
        _addCartRequest.ColorId = ColorId;
        ColorName = (string)value;

        await UpdateStock();
        await UpdatePrice();
        if (ColorId != Guid.Empty)
        {
            ImagePaths = ImageDict[ColorId];
            OnInitImage();
        }
        if (ColorId == Guid.Empty)
        {
            ImagePaths.Clear();
            await Images();
            StateHasChanged();
        }
        StateHasChanged();
    }
    #endregion

    protected override async Task OnInitializedAsync()
    {
        await ProductDetailVm();
        OnInitImage();
        await FeaturedProduct();
        await Images();

        StateHasChanged();
    }

    #region quantity cart
    private int Quantity
    {
        get => _addCartRequest.Quantity;
        set
        {
            if (value > 0)
            {
                _addCartRequest.Quantity = value;
            }
            else
            {
                _addCartRequest.Quantity = 1;
            }
        }
    }

    private void IncreaseQuantity()
    {
        Quantity++;
    }

    private void DecreaseQuantity()
    {
        if (Quantity > 0)
        {
            Quantity--;
        }
    }
    #endregion

    private async Task AddToCart()
    {
        //AuthenticationState? authState = await AuthStateTask;
        //if (authState.User.Identity == null || !authState.User.Identity.IsAuthenticated)
        //{
        //    Navigation.NavigateTo("/login");
        //    StateHasChanged();
        //}
        //var stringUserId = authState.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

        //UserId = new(stringUserId);


        _addCartRequest.ProductId = ProductId;

        _addCartRequest.CustomerId = CustomerId;
        if (_addCartRequest.ColorId == Guid.Empty || _addCartRequest.SizeId == Guid.Empty)
        {
            Error = "Vui lòng chọn phân loại giày.";
        }
        else
        {
            var cartResponse = await CartService.GetCart(CustomerId);
            if (cartResponse.IsSuccess && cartResponse.Value != null)
            {
                _cartVm = cartResponse.Value;
            }
            var existingProduct = _cartVm.ListCart.FirstOrDefault(item =>
            item.ProductId == ProductId &&
            item.ColorName == ColorName &&
            item.SizeNumber == SizeNumber);
            //if (_addCartRequest.Quantity <= 0)
            //{
            //    _addCartRequest.Quantity = 1;
            //}
            var response = await CartService.AddToCart(_addCartRequest);
            if (response.IsSuccess)
            {
                if (existingProduct == null)
                {
                    CartState.Quantity++;
                }
                Error = "";
                Snackbar.Add("Thêm mới giỏ hàng thành công", Severity.Success);
            }
            else
            {
                Error = $"{response.Message}";
            }
        }
    }

    private async Task BuyNow()
    {
        //AuthenticationState? authState = await AuthStateTask;
        //var stringUserId = authState.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

        //UserId = new(stringUserId);

        _addCartRequest.ProductId = ProductId;
        _addCartRequest.CustomerId = CustomerId;
        if (_addCartRequest.ColorId == Guid.Empty || _addCartRequest.SizeId == Guid.Empty)
        {
            Error = "Vui lòng chọn phân loại giày.";
        }
        else
        {
            var cartResponse = await CartService.GetCart(CustomerId);
            if (cartResponse.IsSuccess && cartResponse.Value != null)
            {
                _cartVm = cartResponse.Value;
            }
            var existingProduct = _cartVm.ListCart.FirstOrDefault(item =>
            item.ProductId == ProductId &&
            item.ColorName == ColorName &&
            item.SizeNumber == SizeNumber);

            var productDetail = await ProductService.GetProductDetailId(ProductId, ColorId, SizeId);
            //if (productDetail.Value == Guid.Empty)
            //{
            //    Snackbar.Add($"{productDetail.Message}",Severity.Warning);
            //}

            var response = await CartService.AddToCart(_addCartRequest);
            if (response.IsSuccess)
            {
                if (existingProduct == null)
                {
                    CartState.Quantity++;
                    SelectedProductState.AddSelectedProductDetail(productDetail.Value);
                }
                Error = "";

                if (existingProduct != null)
                {
                    SelectedProductState.AddSelectedProductDetail(existingProduct.ProductDetailId);
                }

                Navigation.NavigateTo("/cart");
            }
            else
            {
                Error = $"{response.Message}";
            }
        }
    }

    //image
    private int MaxPage => (ImagePaths.Count - 1) / 5;
    private int CurrentPage { get; set; } = 0;

    private IEnumerable<string> GetVisibleImages()
    {
        return ImagePaths.Skip(CurrentPage * 5).Take(5);
    }

    private void PreviousPage()
    {
        if (CurrentPage > 0)
        {
            CurrentPage--;
        }
    }

    private void NextPage()
    {
        if (CurrentPage < MaxPage)
        {
            CurrentPage++;
        }
    }
    //featured product
    private async Task FeaturedProduct()
    {
        var result = await ProductService.GetFeaturedProducts();
        if (result.IsSuccess && result.Value != null)
        {
            _lstProduct = result.Value;
        }
    }

    private void NavigateToProductDetail(Guid productId)
    {
        Navigation.NavigateTo($"/productdetail/{productId}", true);
    }
}