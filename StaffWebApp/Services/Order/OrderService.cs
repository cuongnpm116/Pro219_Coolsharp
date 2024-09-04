using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using StaffWebApp.Services.Order.Requests;
using StaffWebApp.Services.Order.Vms;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Enum;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Order;

public class OrderService : IOrderService
{
    private const string apiUrl = "/api/Orders";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IWebHostEnvironment _hostingEnvironment;
    public OrderService(IHttpClientFactory httpClientFactory, IWebHostEnvironment hostingEnvironment)
    {
        _httpClientFactory = httpClientFactory;
        _hostingEnvironment = hostingEnvironment;
    }

 

    public async Task<Result<PaginationResponse<OrderVm>>> GetOrders(OrderPaginationRequest request)
    {
        var httpClient = _httpClientFactory.CreateClient("eShopApi");
        //var url = $"/api/Orders/get-orders-for-Staff?PageNumber={request.PageNumber}&PageSize={request.PageSize}";
        var url = apiUrl + $"/get-orders-for-Staff?";
        if (request.OrderStatus != null)
        {

            url += $"OrderStatus={Uri.EscapeDataString(request.OrderStatus.ToString())}";
        }
        if (request.PageNumber != null || request.PageSize != null)
        {
            url += $"&PageNumber={Uri.EscapeDataString(request.PageNumber.ToString())}&PageSize={Uri.EscapeDataString(request.PageSize.ToString())}";
        }
        if (!string.IsNullOrEmpty(request.SearchString))
        {
            url += $"&SearchString={Uri.EscapeDataString(request.SearchString)}";
        }

        var result = await httpClient.GetFromJsonAsync<Result<PaginationResponse<OrderVm>>>(url);
        return result;
    }

    public async Task<Result<OrderVm>> GetOrderDetais(Guid orderId)
    {
        var httpClient = _httpClientFactory.CreateClient("eShopApi");
        var response = await httpClient.GetFromJsonAsync<Result<OrderVm>>(apiUrl + $"/get-order-detail-staff?orderId={orderId}");
        return response;
    }

    public async Task UpdateOrderStatus(OrderVm request)
    {
        var httpClient = _httpClientFactory.CreateClient("eShopApi");
        var response = await httpClient.PutAsJsonAsync(apiUrl + $"/update-order-status-staff", request);
        response.EnsureSuccessStatusCode();
    }
    public async Task CancelOrderStatus(Guid orderId)
    {
        var httpClient = _httpClientFactory.CreateClient("eShopApi");
        var response = await httpClient.DeleteAsync(apiUrl + $"/cancel-order-status-staff?Id={orderId}");
        response.EnsureSuccessStatusCode();
    }
    public async Task<Result<List<OrderDetailVm>>> TopProducts()
    {
        var httpClient = _httpClientFactory.CreateClient("eShopApi");
        string url = apiUrl + $"/top-products ";
        var result = await httpClient.GetFromJsonAsync<Result<List<OrderDetailVm>>>(url);
        return result;
    }

    public async Task<Result<List<OrderVm>>> Statistical()
    {
        var httpClient = _httpClientFactory.CreateClient("eShopApi");
        string url = apiUrl + $"/statistical";
        var result = await httpClient.GetFromJsonAsync<Result<List<OrderVm>>>(url);
        return result;
    }


 public async Task<string> PrintOrder(Guid orderId)
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        // Giả định: Bạn có cách nào đó để lấy thông tin đơn hàng ở đây
        var order = await GetOrderDetais(orderId);

        var orderDetails = order.Value.Details;

        using (var memoryStream = new MemoryStream())
        {
            var document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter pdfWriter = PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.ttf");
            // Đường dẫn đến font tùy chỉnh

            // Taoj object Font
            BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, false);
            Font titleFont = new Font(baseFont, 18, Font.BOLD);
            Font subTitleFont = new Font(baseFont, 14, Font.BOLD);
            Font bodyFont = new Font(baseFont, 12, Font.BOLD);



            // Thêm thông tin công ty và tiêu đề hóa đơn
            document.Add(new Paragraph("Coolsharp", titleFont) { Alignment = Element.ALIGN_CENTER });
            document.Add(new Paragraph("Phố Trịnh Văn Bô - Nam Từ Liêm - Hà Nội\nTel: 0987654321", bodyFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 10
            });
            document.Add(new Paragraph("HÓA ĐƠN BÁN HÀNG", subTitleFont) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 20 });
            document.Add(new Paragraph($"Số HĐ: {order.Value.OrderCode} - {order.Value.ConfirmedDate:yyyy-MM-dd}", bodyFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20
            });

            // Tạo bảng chi tiết sản phẩm
            var table = new PdfPTable(4) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 1, 4, 2, 2 });

            // Hàm nội tuyến để thêm ô vào bảng
            void AddCellToTable(PdfPTable tbl, string txt, Font fnt, int align, BaseColor bgColor = null)
            {
                var cell = new PdfPCell(new Phrase(txt, fnt))
                {
                    HorizontalAlignment = align,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = bgColor ?? BaseColor.WHITE
                };
                tbl.AddCell(cell);
            }

            // Thêm header cho bảng
            AddCellToTable(table, "STT", bodyFont, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY);
            AddCellToTable(table, "Tên Sản Phẩm", bodyFont, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY);
            AddCellToTable(table, "Số Lượng", bodyFont, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY);
            AddCellToTable(table, "Đơn Giá", bodyFont, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY);

            // Thêm chi tiết sản phẩm vào bảng
            int index = 1;
            foreach (var detail in orderDetails)
            {
                AddCellToTable(table, index.ToString(), bodyFont, Element.ALIGN_CENTER);
                AddCellToTable(table, detail.ProductName, bodyFont, Element.ALIGN_CENTER);
                AddCellToTable(table, detail.Quantity.ToString(), bodyFont, Element.ALIGN_CENTER);
                AddCellToTable(table, $"{detail.Price * detail.Quantity:N0} VND", bodyFont, Element.ALIGN_CENTER);
                index++;
            }
            document.Add(table);

            // Thêm tổng tiền
            document.Add(new Paragraph($"Tổng tiền: {order.Value.TotalPrice:N0} VND", bodyFont)
            {
                Alignment = Element.ALIGN_RIGHT,
                SpacingBefore = 20,
                SpacingAfter = 30
            });

            // Thêm thông tin khách hàng
            document.Add(new Paragraph($"KHÁCH HÀNG: {order.Value.Customer}\nSố điện thoại: {order.Value.PhoneNumber}\nĐịa chỉ: {order.Value.ShipAddressDetail}", bodyFont)
            {
                SpacingBefore = 20
            });

            // Thêm lời cảm ơn
            document.Add(new Paragraph("Xin cảm ơn quý khách!", subTitleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingBefore = 30,
                SpacingAfter = 10
            });

            document.Close();

            // Chuyển đổi mảng byte thành chuỗi Base64
            var pdfBytes = memoryStream.ToArray();
            return Convert.ToBase64String(pdfBytes);
        }
    }
    public async Task<bool> ExportOrdersToExcel(OrderPaginationRequest request)
    {
        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var orders = await GetOrders(request);
            if (orders == null)
            {
                return false;
            }
            var confirmedOrders = orders.Value.Data.Where(o => o.OrderStatus == OrderStatus.Completed)
                .ToList();
            var totalCost = confirmedOrders.Sum(o => o.TotalPrice);
            var invoiceCount = confirmedOrders.Count;
            var listDetail = new List<OrderDetailVm>();
            foreach (var order in confirmedOrders)
            {
                var orderDetailResult = await GetOrderDetais(order.Id);

                if (orderDetailResult?.Value != null && orderDetailResult.Value.Details != null)
                {
                    listDetail.AddRange(orderDetailResult.Value.Details);
                }
            }


            var topProducts = listDetail.GroupBy(t => t.ProductName).OrderByDescending(o => o.Sum(q => q.Quantity)).Select(p => new { ProductName = p.Key, Quantity = p.Sum(s => s.Quantity) }).Take(5).ToList();
            var topColors = listDetail.GroupBy(t => t.ColorName).OrderByDescending(o => o.Sum(q => q.Quantity)).Select(p => new { ColorName = p.Key, Quantity = p.Sum(s => s.Quantity) }).Take(5).ToList();
            var topSizes = listDetail.GroupBy(t => t.SizeNumber).OrderByDescending(o => o.Sum(q => q.Quantity)).Select(p => new { SizeNumber = p.Key, Quantity = p.Sum(s => s.Quantity) }).Take(5).ToList();
            if (!confirmedOrders.Any())
            {
                return false; // Không có đơn hàng nào để xuất
            }
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "export_invoice", "Orders.xlsx");

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Orders");

                // Set column headers
                worksheet.Cells[1, 1].Value = "CreatedOn";
                worksheet.Cells[1, 2].Value = "OrderCode";
                worksheet.Cells[1, 3].Value = "ConfirmedDate";
                worksheet.Cells[1, 4].Value = "ShippedDate";
                worksheet.Cells[1, 5].Value = "CompletedDate";
                worksheet.Cells[1, 6].Value = "TotalCost";
                worksheet.Cells[1, 7].Value = "PaymentMethods";
                worksheet.Cells[1, 8].Value = "Customer";
                //worksheet.Cells[1, 9].Value = "OrderType";
                worksheet.Cells[1, 10].Value = "OrderStatus";

                // Set data
                for (int i = 0; i < confirmedOrders.Count; i++)
                {
                    var order = confirmedOrders[i];
                    worksheet.Cells[i + 2, 1].Value = order.CreatedOn;
                    worksheet.Cells[i + 2, 2].Value = order.OrderCode;
                    worksheet.Cells[i + 2, 3].Value = order.ConfirmedDate;
                    worksheet.Cells[i + 2, 4].Value = order.ShippedDate;
                    worksheet.Cells[i + 2, 5].Value = order.CompletedDate;
                    worksheet.Cells[i + 2, 6].Value = order.TotalPrice;
                    worksheet.Cells[i + 2, 7].Value = order.PaymentMethod;
                    worksheet.Cells[i + 2, 8].Value = order.Customer;
                    //worksheet.Cells[i + 2, 9].Value = order.OrderType;
                    worksheet.Cells[i + 2, 10].Value = order.OrderStatus;
                }

                // Add summary row
                worksheet.Cells[confirmedOrders.Count + 3, 6].Value = "Tổng tiền";
                worksheet.Cells[confirmedOrders.Count + 3, 7].Value = totalCost;
                worksheet.Cells[confirmedOrders.Count + 4, 6].Value = "Tổng số đơn hàng";
                worksheet.Cells[confirmedOrders.Count + 4, 7].Value = invoiceCount;

                // Dòng bắt đầu cho phần Top 5
                int startRow = confirmedOrders.Count + 7;

                // Biến để theo dõi dòng hiện tại
                int currentRow = startRow;

                // Thêm tiêu đề và dữ liệu cho Top 5 Products
                worksheet.Cells[currentRow, 1].Value = "Top 5 Products";
                currentRow++; // Tăng dòng để ghi dữ liệu bên dưới tiêu đề
                for (int i = 0; i < topProducts.Count; i++)
                {
                    worksheet.Cells[currentRow, 1].Value = topProducts[i].ProductName;
                    worksheet.Cells[currentRow, 2].Value = topProducts[i].Quantity;
                    currentRow++; // Tăng dòng sau khi ghi dữ liệu
                }

                // Đặt lại currentRow cho phần Top 5 Colors để in đúng dòng
                currentRow = startRow;
                worksheet.Cells[currentRow, 4].Value = "Top 5 Colors";
                currentRow++; // Tăng dòng để ghi dữ liệu bên dưới tiêu đề
                for (int i = 0; i < topColors.Count; i++)
                {
                    worksheet.Cells[currentRow, 4].Value = topColors[i].ColorName;
                    worksheet.Cells[currentRow, 5].Value = topColors[i].Quantity;
                    currentRow++; // Tăng dòng sau khi ghi dữ liệu
                }

                // Đặt lại currentRow cho phần Top 5 Sizes để in đúng dòng
                currentRow = startRow;
                worksheet.Cells[currentRow, 7].Value = "Top 5 Sizes";
                currentRow++; // Tăng dòng để ghi dữ liệu bên dưới tiêu đề
                for (int i = 0; i < topSizes.Count; i++)
                {
                    worksheet.Cells[currentRow, 7].Value = topSizes[i].SizeNumber;
                    worksheet.Cells[currentRow, 8].Value = topSizes[i].Quantity;
                    currentRow++; // Tăng dòng sau khi ghi dữ liệu
                }

                // Define path to save the file
                var file = new FileInfo(filePath);
                package.SaveAs(file);
                return true;

            }
        }
        catch
        {
            return false;
        }
    }
}
