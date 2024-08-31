using CustomerWebApp.Components.Orders.Dtos;
using CustomerWebApp.Components.Payment.Dtos;
using CustomerWebApp.Service.Order;
using CustomerWebApp.Service.Payment;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;
using Newtonsoft.Json;
using WebAppIntegrated.Constants;
using CustomerWebApp.Components.Carts.ViewModel;
using CustomerWebApp.Service.Address;
using CustomerWebApp.Components.Orders.ViewModel;
using CustomerWebApp.Components.Address.ViewModel;
using CustomerWebApp.Components.Address;

namespace CustomerWebApp.Components.Payment;

public partial class Checkout
{
    [CascadingParameter]
    private Task<AuthenticationState> AuthStateTask { get; set; } = null!;
    [Inject]
    private IHttpContextAccessor HttpContextAccessor { get; set; } = null!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    private IDialogService DialogService { get; set; } = null!;
    [Inject]
    private NavigationManager Navigation { get; set; } = null!;
    [Inject]
    private IPaymentService PaymentService { get; set; }
    [Inject]
    private IOrderService OrderService { get; set; } = null!;
    [Inject]
    private IAddressService AddressService { get; set; }

    private CreateOrderRequest _orderRequest = new();
    private CreatePaymentRequest _paymentRequest = new();
    private OrderWithPaymentVm _orderVm = new();
    private List<CartItemVm> _selectedCartItems = [];
    private AddressModel _deliveryAddress;
    public decimal _totalPayment;
    private string _imageUrl = ShopConstants.EShopApiHost + "/user-content/";
    //public decimal totalPrice;
    public Guid CustomerId = Guid.Parse("BCF83D3E-BC97-4813-8E2C-96FD34863EA8");
    public Guid OrderId;
    public int OrderType { get; set; } = 0;

    protected override async Task OnInitializedAsync()
    {
        //AuthenticationState? authState = await AuthStateTask;
        //var stringUserId = authState.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;
        //if (!string.IsNullOrWhiteSpace(stringUserId))
        //{
        //    UserId = new(stringUserId);
        //}
        await GetDefaultAddress();
        //Request
        var uri = new Uri(Navigation.Uri);
        var queryParams = QueryHelpers.ParseQuery(uri.Query);

        if (queryParams.TryGetValue("cartItems", out var cartItemsJson)
            && queryParams.TryGetValue("totalPayment", out var totalPaymentValue))
        {
            _selectedCartItems = JsonConvert.DeserializeObject<List<CartItemVm>>(Uri.UnescapeDataString(cartItemsJson));
            _totalPayment = decimal.Parse(totalPaymentValue);
        }

        
    }

    private async Task MakePaymentOnline()
    {
        await CreatOrder();
        var model = new VnPayRequest
        {
            Amount = (double)_totalPayment, // Amount in VND
            CreatedDate = DateTime.Now,
            OrderCode = _orderVm.OrderCode,
        };

        var paymentUrl = PaymentService.CreatePaymentUrl(HttpContextAccessor.HttpContext, model);
        Navigation.NavigateTo(paymentUrl, true);
    }

    private async Task MakePayment()
    {
        await CreatOrder();
        await CreatePayment();
        Navigation.NavigateTo("/payment-response?success=true", true);
    }

    private async Task CreatOrder()
    {
        //goi orderservice
        _orderRequest.Carts = _selectedCartItems;
        _orderRequest.TotalPrice = _totalPayment;
        _orderRequest.CustomerId = CustomerId;
        _orderRequest.PhoneNumber = _deliveryAddress.PhoneNumber;
        _orderRequest.ShipAddress = _deliveryAddress.Ward.Name + ", " + _deliveryAddress.District.Name + ", " + _deliveryAddress.Province.Name;
        _orderRequest.ShipAddressDetail = _deliveryAddress.Detail;

        var response = await OrderService.CreateOrder(_orderRequest);
        if (response.IsSuccess && response.Value != null)
        {
            _orderVm = response.Value;
            Snackbar.Add("Đặt hàng thành công", Severity.Warning);
        }
        else if (!response.IsSuccess)
        {
            Snackbar.Add("Mua hàng không thành công! Vui lòng thử lại.", Severity.Warning);
        }
        
    }

    private async Task CreatePayment()
    {
        _paymentRequest.PaymentDate = DateTime.Now;
        _paymentRequest.OrderCode = _orderVm.OrderCode;
        _paymentRequest.CustomerId = CustomerId;
        _paymentRequest.PaymentMethod = 2;
        _paymentRequest.Amount = _totalPayment;
        _paymentRequest.PaymentStatus = 1;
        var result = await PaymentService.CreatePayment(_paymentRequest);
        if (result.IsSuccess)
        {
            Snackbar.Add("Đặt hàng thành công", Severity.Warning);
        }
    }

    private async Task GetDefaultAddress()
    {
        var getDefaultAddressResponse = await AddressService.GetDefaultAddress(CustomerId);
        _deliveryAddress = getDefaultAddressResponse.Value is not null ? getDefaultAddressResponse.Value : null;
    }

    private async Task OpenChangeDeliveryAddressDialog()
    {
        DialogParameters dialogParams = [];
        dialogParams.Add("CustomerId", CustomerId);
        dialogParams.Add("SelectedAddressId", _deliveryAddress.Id);
        string title = "Thay đổi địa chỉ giao hàng";

        DialogOptions dialogOptions = new()
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseButton = true,
            CloseOnEscapeKey = true,
        };

        IDialogReference dialog = await DialogService.ShowAsync<CheckOutAddress>(title, dialogParams, dialogOptions);
        DialogResult result = await dialog.Result;
        if (!result.Canceled)
        {
            Guid selectedAddressId = (Guid)result.Data;
            var getSelectedAddress = await AddressService.GetAddressById(selectedAddressId);
            _deliveryAddress = getSelectedAddress.Value;
            Snackbar.Add("Thay đổi địa chỉ giao hàng thành công", Severity.Success);
        }
    }

    private async Task OpenAddAddressDialog(Guid customerId)
    {
        DialogParameters dialogParams = [];
        dialogParams.Add("AddressId", Guid.Empty);
        dialogParams.Add("CustomerId", CustomerId);
        dialogParams.Add("IsDefault", true);

        string title = "Thêm địa chỉ";

        DialogOptions dialogOptions = new()
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseButton = true,
            CloseOnEscapeKey = true,
        };

        IDialogReference dialog = await DialogService.ShowAsync<AddOrEditAddress>(title, dialogParams, dialogOptions);
        DialogResult dialogResult = await dialog.Result;
        if (!dialogResult.Canceled)
        {
            await GetDefaultAddress();
        }
    }

}
