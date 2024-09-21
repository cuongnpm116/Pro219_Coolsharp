using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazor;
using StaffWebApp.Components.Color;
using StaffWebApp.Services.Order;
using StaffWebApp.Services.Order.Vms;
using WebAppIntegrated.Constants;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Components.Order;

public partial class OrderDetail
{
    [CascadingParameter]
    private Task<AuthenticationState> AuthStateTask { get; set; } = null;
    [Inject]
    private IDialogService DialogService { get; set; }
    [Inject]
    private IJSRuntime JSRuntime { get; set; }
    [Inject]
    ISnackbar snackbar { get; set; }
    [Inject]
    private IOrderService OrderService { get; set; }
    private PaginationResponse<OrderVm> _lstOrder = new();
    private OrderVm orderVm = new();
    private decimal _reduceAmount = 0;
    private List<OrderDetailVm> _lstOrderDetail = new();
    private decimal _totalAmount = 0;
    [Parameter]
    public string OrderId { get; set; }
    public string PdfUrl { get; set; }
    public Guid _userId;
    private string _imageUrl = ShopConstants.EShopApiHost + "/product-content/";
    protected override async Task OnInitializedAsync()
    {
        AuthenticationState? authState = await AuthStateTask;
        _userId = new(authState.User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value);

        await LoadOrder();
        if (orderVm.VoucherCode != "N/A")
        {
            _reduceAmount = _totalAmount - orderVm.TotalPrice;
        }
        StateHasChanged();
    }
    private async Task LoadOrder()
    {
        var response = await OrderService.GetOrderDetais(Guid.Parse(OrderId));
        if (response.Value != null)
        {

            orderVm = response.Value;
            _lstOrderDetail = response.Value.Details;

            foreach (var item in _lstOrderDetail)
            {
                _totalAmount += (item.Price * item.Quantity);
            }
        }
    }

    private async Task PrintInvoice(Guid orderId)
    {
        try
        {
            // Lấy dữ liệu PDF dưới dạng chuỗi Base64
            var base64String = await OrderService.PrintOrder(orderId);

            if (!string.IsNullOrEmpty(base64String))
            {
                // Ghi log kích thước chuỗi Base64
                string wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                string pdfFolder = Path.Combine(wwwrootPath, "pdf-invoice");

                if (!Directory.Exists(pdfFolder))
                {
                    Directory.CreateDirectory(pdfFolder); // Tạo thư mục nếu chưa tồn tại
                }

                byte[] pdfBytes = Convert.FromBase64String(base64String);
                string fileName = $"invoice-{orderId}.pdf"; // Đặt tên file theo orderId
                string outputPath = Path.Combine(pdfFolder, fileName);

                await File.WriteAllBytesAsync(outputPath, pdfBytes);

                // Đường dẫn URL cho file PDF
                PdfUrl = $"/pdf-invoice/{fileName}";

                snackbar.Add("Xuất ra file PDF thành công", Severity.Success);

                var parameter = new DialogParameters();
                parameter.Add("PdfUrl", PdfUrl);
                var dialog = DialogService.Show<PdfOrder>("", parameter);
                var result = await dialog.Result;
                if (Convert.ToBoolean(result.Data))
                {
                    await LoadOrder();
                }

            }

        }
        catch (Exception ex)
        {         
            snackbar.Add("Xuất ra file PDF thất bại", Severity.Error);
        }
    }
}
