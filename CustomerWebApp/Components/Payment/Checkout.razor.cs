using CustomerWebApp.Components.Address;
using CustomerWebApp.Service.Address;
using CustomerWebApp.Service.Address.ViewModel;
using CustomerWebApp.Service.Cart.ViewModel;
using CustomerWebApp.Service.Order;
using CustomerWebApp.Service.Order.Dtos;
using CustomerWebApp.Service.Order.ViewModel;
using CustomerWebApp.Service.Payment;
using CustomerWebApp.Service.Payment.Dtos;
using CustomerWebApp.Service.Voucher;
using CustomerWebApp.Service.Voucher.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;
using Newtonsoft.Json;

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
    private IVoucherService VoucherService { get; set; }
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
    public decimal totalPriceProduct;
    public decimal reducedAmount = 0;
    public decimal totalPayment;
    //private string _imageUrl = ShopConstants.EShopApiHost + "/user-content/";
    //public decimal totalPrice;
    public Guid CustomerId = Guid.Parse("BCF83D3E-BC97-4813-8E2C-96FD34863EA8");
    public Guid OrderId;
    public int OrderType { get; set; } = 0;
    #region Voucher
    private string voucherCode;
    public bool _isOpen;

    public void ToggleOpen()
    {
        if (_isOpen)
            _isOpen = false;
        else
            _isOpen = true;
    }

    private List<VoucherVm> _lstVoucher = new();
    private async Task LoadVoucher()
    {
        var result = await VoucherService.GetListVoucher();
        if (result.IsSuccess && result.Value != null)
        {
            _lstVoucher = result.Value;
            AutoSelectBestVoucher();
        }
    }

    private VoucherVm? selectedVoucher;
    private void AutoSelectBestVoucher()
    {
        decimal maxDiscount = 0;

        foreach (var voucher in _lstVoucher)
        {
            if (voucher.DiscountCondition > totalPriceProduct)
                continue;

            decimal discountValue = 0;

            if (voucher.DiscountPercent.HasValue && voucher.DiscountPercent != 0)
            {
                discountValue = totalPriceProduct * (voucher.DiscountPercent.Value / 100m);
            }
            else if (voucher.DiscountAmount.HasValue && voucher.DiscountAmount != 0)
            {
                discountValue = voucher.DiscountAmount.Value;
            }

            if (discountValue > maxDiscount)
            {
                maxDiscount = discountValue;
                selectedVoucher = voucher;
            }
        }

        if (selectedVoucher != null)
        {
            voucherCode = selectedVoucher.VoucherCode;
            reducedAmount = maxDiscount;
            totalPayment = totalPriceProduct - reducedAmount;
        }
    }
    private void ToggleVoucherSelection(VoucherVm voucher)
    {
        if (voucher.StartedDate > DateTime.Now)
        {
            Snackbar.Add("Voucher này chưa đến ngày sử dụng!", Severity.Warning);
            return;
        }
        if (selectedVoucher != null && selectedVoucher.Id == voucher.Id)
        {
            // Bỏ chọn voucher
            selectedVoucher = null;
            voucherCode = string.Empty;
            reducedAmount = 0;
        }
        else
        {
            // Chọn voucher
            selectedVoucher = voucher;
            voucherCode = voucher.VoucherCode;

            if (voucher.DiscountPercent.HasValue)
            {
                reducedAmount = totalPriceProduct * (voucher.DiscountPercent.Value / 100m);
            }
            else if (voucher.DiscountAmount.HasValue)
            {
                reducedAmount = voucher.DiscountAmount.Value;
            }
        }

        totalPayment = totalPriceProduct - reducedAmount;
    }

    #endregion
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
            totalPriceProduct = decimal.Parse(totalPaymentValue);
        }
        totalPayment = totalPriceProduct - reducedAmount;
        await LoadVoucher();

    }

    private async Task MakePaymentOnline()
    {
        var dialog = DialogService.Show<LoadResponse>("");

        try
        {
            await CreatOrder();
            var model = new VnPayRequest
            {
                Amount = (double)totalPayment, // Amount in VND
                CreatedDate = DateTime.Now,
                OrderCode = _orderVm.OrderCode,
            };

            var paymentUrl = PaymentService.CreatePaymentUrl(HttpContextAccessor.HttpContext, model);
            Navigation.NavigateTo(paymentUrl, true);
        }
        catch (Exception ex)
        {

            Snackbar.Add($"Đã xảy ra lỗi: {ex.Message}", Severity.Error);
        }
        finally
        {
            DialogService.Close(dialog);
        }

    }
    private async Task MakePayment()
    {
        var dialog = DialogService.Show<LoadResponse>("");

        try
        {
            await CreatOrder();
            await CreatePayment();
            Navigation.NavigateTo("/payment-response?success=true", true);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Không thể tạo đơn hàng. Vui lòng thử lại!", Severity.Error);
        }
        finally
        {
            DialogService.Close(dialog);
        }
    }

    private async Task CreatOrder()
    {
        //goi orderservice
        _orderRequest.Carts = _selectedCartItems;
        _orderRequest.TotalPrice = totalPayment;
        _orderRequest.CustomerId = CustomerId;
        if (selectedVoucher?.Id != Guid.Empty)
        {
            _orderRequest.VoucherId = selectedVoucher?.Id;
        }
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
        _paymentRequest.Amount = totalPayment;
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
