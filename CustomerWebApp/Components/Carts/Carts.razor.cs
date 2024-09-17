using CustomerWebApp.Service.Cart;
using CustomerWebApp.Service.Cart.Dtos;
using CustomerWebApp.Service.Cart.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Newtonsoft.Json;
using WebAppIntegrated.Constants;


namespace CustomerWebApp.Components.Carts;

public partial class Carts
{
    [CascadingParameter]
    private Task<AuthenticationState> AuthStateTask { get; set; }
    [Inject]
    NavigationManager Navigation { get; set; }
    [Inject]
    private ISnackbar Snackbar { get; set; }
    [Inject]
    private ICartService Cartservice { get; set; }
    [Inject]
    private CartState CartState { get; set; }
    [Inject]
    private SelectedProductState SelectedProductState { get; set; }

    public Guid UserId;
    private bool selectAllChecked = false;
    private string _imageUrl = ShopConstants.EShopApiHost + "/product-content/";
    private bool IsDisable { get; set; } = true;
    private CartVm _cartVm = new();
    private CartItemVm _cartDto = new();
    private UpdateCartRequest _updateRequest = new();
    private DeleteCartRequest _deleteRequest = new();
    private List<Guid> _productDetailIds = [];
    private List<Guid> _cartItemIds = [];
    private Dictionary<Guid, bool> _productDetailDictionaryChecked = [];

    protected override async Task OnInitializedAsync()
    {

        AuthenticationState? authState = await AuthStateTask;
        var stringUserId = authState.User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
        UserId = new(stringUserId);

        await LoadCarts();
        foreach (var itemId in SelectedProductState.SelectedProductDetailIds)
        {
            var item = _cartVm.ListCart.FirstOrDefault(x => x.ProductDetailId == itemId);
            if (item != null)
            {
                item.IsChecked = true;
                _productDetailIds.Add(item.ProductDetailId);
                _cartItemIds.Add(item.CartItemId);
                _productDetailDictionaryChecked[item.ProductDetailId] = true;
            }
        }

        SelectedProductState.Clear();
    }

    public async Task LoadCarts()
    {
        var response = await Cartservice.GetCart(UserId);
        if (response.IsSuccess && response.Value != null)
        {
            _cartVm = response.Value;
            //khoi tao dictinary
            foreach (var item in _cartVm.ListCart)
            {
                if (!_productDetailDictionaryChecked.ContainsKey(item.ProductDetailId))
                {
                    _productDetailDictionaryChecked[item.ProductDetailId] = false;
                }
            }
        }
    }
    private bool isDisablebutton = true;
    private void UpdateHeaderCheckbox(bool value)
    {
        selectAllChecked = value;
        IsDisable = !value;

        foreach (var item in _cartVm.ListCart)
        {
            item.IsChecked = value;

            if (value)
            {
                if (!_productDetailDictionaryChecked.ContainsKey(item.ProductDetailId))
                {
                    _productDetailDictionaryChecked[item.ProductDetailId] = true;
                    _productDetailIds.Add(item.ProductDetailId);
                    _cartItemIds.Add(item.CartItemId);
                }
                else if (!_productDetailIds.Contains(item.ProductDetailId))
                {
                    _productDetailIds.Add(item.ProductDetailId);
                    _cartItemIds.Add(item.CartItemId);
                    _productDetailDictionaryChecked[item.ProductDetailId] = true;
                }
            }
            else
            {
                if (_productDetailDictionaryChecked.ContainsKey(item.ProductDetailId))
                {
                    _productDetailDictionaryChecked[item.ProductDetailId] = false;
                }
            }
        }

        if (!value)
        {
            _productDetailIds.Clear();
            _cartItemIds.Clear();
        }
    }
    private void UpdateSelectedIds(bool value, Guid id)
    {
        var item = _cartVm.ListCart.FirstOrDefault(x => x.ProductDetailId == id);
        if (item != null)
        {
            item.IsChecked = value;
            if (value)
            {
                if (!_productDetailIds.Contains(id))
                {
                    _productDetailIds.Add(id);
                    _cartItemIds.Add(item.CartItemId);
                }
                if (_productDetailIds.Count == _cartVm.ListCart.Count)
                {
                    selectAllChecked = true;
                }
            }
            else
            {
                _productDetailIds.Remove(id);
                _cartItemIds.Remove(item.CartItemId);
                if (_productDetailIds.Count < _cartVm.ListCart.Count)
                {
                    selectAllChecked = false;
                }
            }
            if (!_cartVm.ListCart.Any(x => x.IsChecked))
            {
                IsDisable = true;
            }
            else
            {
                IsDisable = false;
            }
            StateHasChanged();
        }
    }

    private async Task RemoveCarts()
    {
        if (_productDetailIds != null)
        {
            _deleteRequest.ProductDetailIds ??= [];
            _deleteRequest.ProductDetailIds = _productDetailIds;
            var response = await Cartservice.DeleteCartItem(_deleteRequest);
            if (response.IsSuccess)
            {
                await LoadCarts();
                CartState.DecreaseQuantity(_productDetailIds.Count);
                _cartDto.IsChecked = false;
                _productDetailIds.Clear();
                StateHasChanged();
            }
        }
    }

    private async Task RemoveCartItem(Guid productDetailId)
    {
        if (productDetailId != Guid.Empty)
        {

            _deleteRequest.ProductDetailIds ??= [];

            _deleteRequest.ProductDetailIds.Add(productDetailId);
            var existingProduct = _cartVm.ListCart.FirstOrDefault(item => item.ProductDetailId == productDetailId);
            var response = await Cartservice.DeleteCartItem(_deleteRequest);
            if (response.IsSuccess)
            {
                await LoadCarts();
                CartState.DecreaseQuantity();
            }
        }
    }

    private async Task UpdateCart(Guid cartId, Guid productDetailId, int quantity)
    {

        _updateRequest.CartId = cartId;
        _updateRequest.ProductDetailId = productDetailId;
        _updateRequest.Quantity = quantity;
        await Cartservice.UpdateCartItemQuantity(_updateRequest);


    }

    private bool isUpdating = false;

    private async Task AdjustQuantity(CartItemVm context, int change)
    {
        if (isUpdating)
        {
            return;
        }

        isUpdating = true;
        int newQuantity = Math.Clamp(context.Quantity + change, 1, context.ProductQuantity);

        if (newQuantity != context.Quantity)
        {
            context.Quantity = newQuantity;
            await UpdateCart(context.CartId, context.ProductDetailId, context.Quantity);
            StateHasChanged();
        }

        isUpdating = false;
    }

    private async Task HandleQuantityChange(CartItemVm context, ChangeEventArgs e)
    {
        if (isUpdating)
        {
            return;
        }

        if (int.TryParse(e.Value?.ToString(), out int newQuantity))
        {
            await AdjustQuantity(context, newQuantity - context.Quantity);
        }
    }

    private void NavigateToCheckout()
    {
        if (_cartItemIds.Count != 0)
        {
            var selectedCartItems = _cartVm.ListCart.Where(item => item.IsChecked).ToList();
            var selectedCartItemsJson = JsonConvert.SerializeObject(selectedCartItems);
            var encodedCartItems = Uri.EscapeDataString(selectedCartItemsJson);
            var totalPayment = _cartVm.TotalPayment;

            Navigation.NavigateTo($"/checkout?cartItems={encodedCartItems}&totalPayment={totalPayment}");
        }
        else
        {
            Snackbar.Add("Vui lòng chọn sản phẩm bạn muốn mua.", Severity.Warning);
        }
    }
}