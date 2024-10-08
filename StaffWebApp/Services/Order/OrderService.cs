﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using OfficeOpenXml;
using StaffWebApp.Services.Order.Requests;
using StaffWebApp.Services.Order.Vms;
using WebAppIntegrated.ApiResponse;
using WebAppIntegrated.Constants;
using WebAppIntegrated.Enum;
using WebAppIntegrated.Pagination;

namespace StaffWebApp.Services.Order;

public class OrderService : IOrderService
{
    private const string apiUrl = "/api/Orders";
    private readonly HttpClient _httpClient;
    private readonly IWebHostEnvironment _hostingEnvironment;
    public OrderService(IHttpClientFactory httpClientFactory, IWebHostEnvironment hostingEnvironment)
    {
        _httpClient = httpClientFactory.CreateClient(ShopConstants.EShopClient);
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task<Result<PaginationResponse<OrderVm>>> GetOrders(OrderPaginationRequest request)
    {

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

        var result = await _httpClient.GetFromJsonAsync<Result<PaginationResponse<OrderVm>>>(url);
        return result;
    }

    public async Task<Result<OrderVm>> GetOrderDetais(Guid orderId)
    {

        var response = await _httpClient.GetFromJsonAsync<Result<OrderVm>>(apiUrl + $"/get-order-detail-staff?orderId={orderId}");
        return response;
    }

    public async Task UpdateOrderStatus(OrderVm request)
    {

        var response = await _httpClient.PutAsJsonAsync(apiUrl + $"/update-order-status-staff", request);
        response.EnsureSuccessStatusCode();
    }
    public async Task<Result<bool>> CancelOrderStatus(CancelOrderRequest request)
    {
        string url = apiUrl + $"/cancel-order";
        var apiRes = await _httpClient.PutAsJsonAsync(url, request);
        string content = await apiRes.Content.ReadAsStringAsync();
        Result<bool> result = JsonConvert.DeserializeObject<Result<bool>>(content);
        return result;
    }
    public async Task<Result<List<OrderDetailVm>>> TopProducts(OrderPaginationRequest request)
    {
        string url = apiUrl + $"/top-products?Stock={request.Stock}";
        if (request.Begin.HasValue)
        {
            url += $"&Begin={Uri.EscapeDataString(request.Begin.ToString())}";
        }
        if (request.End.HasValue)
        {
            url += $"&End={Uri.EscapeDataString(request.End.ToString())}";
        }

        var result = await _httpClient.GetFromJsonAsync<Result<List<OrderDetailVm>>>(url);
        return result;
    }
    public async Task<Result<List<ProductDetailInOrderVm>>> LowStockProducts()
    {
        var url = apiUrl + $"/low-stock-products";
        var result = await _httpClient.GetFromJsonAsync<Result<List<ProductDetailInOrderVm>>>(url);
        return result;
    }
    public async Task<Result<List<OrderVm>>> Statistical()
    {

        string url = apiUrl + $"/statistical";
        var result = await _httpClient.GetFromJsonAsync<Result<List<OrderVm>>>(url);
        return result;
    }


    public async Task<string> PrintOrder(Guid orderId)
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


        var order = await GetOrderDetais(orderId);
        decimal _totalPrice = 0;
        var orderDetails = order.Value.Details;

        string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdf-invoice");

        if (!Directory.Exists(webRootPath))
        {
            Directory.CreateDirectory(webRootPath);
        }
        string fileName = $"{order.Value.OrderCode}.pdf";
        string filePath = Path.Combine(webRootPath, fileName);

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
            document.Add(new Paragraph($"Số HĐ: {order.Value.OrderCode} - {order.Value.CompletedDate:yyyy-MM-dd}", bodyFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20
            });

            // Tạo bảng chi tiết sản phẩm
            var table = new PdfPTable(5) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 1, 4, 2, 2, 2 });

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
            AddCellToTable(table, "Tổng Tiền", bodyFont, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY);

            // Thêm chi tiết sản phẩm vào bảng
            int index = 1;
            foreach (var detail in orderDetails)
            {
                string productInfo = $"{detail.ProductName}\nMàu sắc: {detail.ColorName} - Size: {detail.SizeNumber}";
                AddCellToTable(table, index.ToString(), bodyFont, Element.ALIGN_CENTER);
                AddCellToTable(table, productInfo, bodyFont, Element.ALIGN_CENTER);
                AddCellToTable(table, detail.Quantity.ToString(), bodyFont, Element.ALIGN_CENTER);
                AddCellToTable(table, $"{detail.Price:N0} VND", bodyFont, Element.ALIGN_CENTER);
                AddCellToTable(table, $"{detail.Price * detail.Quantity:N0} VND", bodyFont, Element.ALIGN_CENTER);
                _totalPrice += (detail.Price * detail.Quantity);
                index++;

            }
            document.Add(table);

            document.Add(new Paragraph($"{_totalPrice:N0} VND", bodyFont)
            {
                Alignment = Element.ALIGN_RIGHT,
                SpacingBefore = 5,
                SpacingAfter = 5
            });

            document.Add(new Paragraph($"Voucher đã dùng: {order.Value.VoucherCode} ", bodyFont)
            {
                Alignment = Element.ALIGN_RIGHT,
                SpacingBefore = 20,
                SpacingAfter = 5
            });
            document.Add(new Paragraph($"Số tiền giảm: {(_totalPrice - order.Value.TotalPrice):N0} VND ", bodyFont)
            {
                Alignment = Element.ALIGN_RIGHT,
                SpacingBefore = 5,
                SpacingAfter = 5
            });
            // Thêm tổng tiền
            document.Add(new Paragraph($"Thành tiền: {order.Value.TotalPrice:N0} VND", bodyFont)
            {
                Alignment = Element.ALIGN_RIGHT,
                SpacingBefore = 5,
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

            //File.WriteAllBytes(filePath, memoryStream.ToArray());
            // Chuyển đổi mảng byte thành chuỗi Base64
            var pdfBytes = memoryStream.ToArray();
            return Convert.ToBase64String(pdfBytes);
        }
    }

    public async Task<Result<CustomerInOrderVm>> GetCustomerId(Guid customerId)
    {

        var response = await _httpClient.GetFromJsonAsync<Result<CustomerInOrderVm>>($"https://localhost:1000/api/Customers/get-customer-by-id?customerId={customerId}");
        return response;
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

            var confirmedOrders = orders.Value.Data.Where(o => o.OrderStatus == OrderStatus.Completed).ToList();
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

            var topProducts = listDetail.GroupBy(t => t.ProductName)
                .OrderByDescending(o => o.Sum(q => q.Quantity))
                .Select(p => new { ProductName = p.Key, Quantity = p.Sum(s => s.Quantity) })
                .Take(5).ToList();

            var topColors = listDetail.GroupBy(t => t.ColorName)
                .OrderByDescending(o => o.Sum(q => q.Quantity))
                .Select(p => new { ColorName = p.Key, Quantity = p.Sum(s => s.Quantity) })
                .Take(5).ToList();

            var topSizes = listDetail.GroupBy(t => t.SizeNumber)
                .OrderByDescending(o => o.Sum(q => q.Quantity))
                .Select(p => new { SizeNumber = p.Key, Quantity = p.Sum(s => s.Quantity) })
                .Take(5).ToList();

            if (!confirmedOrders.Any())
            {
                return false; // Không có đơn hàng nào để xuất
            }

            var exportPath = Path.Combine(_hostingEnvironment.WebRootPath, "export_invoice");

            // Kiểm tra và tạo thư mục nếu không tồn tại
            if (!Directory.Exists(exportPath))
            {
                Directory.CreateDirectory(exportPath);
            }

            // Tạo tên tệp với thời gian hiện tại
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");
            var filePath = Path.Combine(exportPath, $"Orders-{timestamp}.xlsx");

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Orders");

                // Set column headers
                worksheet.Cells[1, 1].Value = "Ngày tạo";
                worksheet.Cells[1, 2].Value = "OrderCode";
                worksheet.Cells[1, 3].Value = "SĐT";
                worksheet.Cells[1, 4].Value = "Ngày xác nhận";
                worksheet.Cells[1, 5].Value = "Ngày vận chuyển";
                worksheet.Cells[1, 6].Value = "Ngày hoàn thành";
                worksheet.Cells[1, 7].Value = "TotalCost";
                worksheet.Cells[1, 8].Value = "Phương thức thanh toán";
                worksheet.Cells[1, 9].Value = "Khách hàng";
                worksheet.Cells[1, 10].Value = "Trạng thái hóa đơn";

                // Set data
                for (int i = 0; i < confirmedOrders.Count; i++)
                {
                    var order = confirmedOrders[i];
                    if (order.CreatedOn.HasValue)
                    {
                        worksheet.Cells[i + 2, 1].Value = order.CreatedOn.Value.ToOADate();
                        worksheet.Cells[i + 2, 1].Style.Numberformat.Format = "dd/MM/yyyy"; // Định dạng ngày tháng
                    }
                    worksheet.Cells[i + 2, 2].Value = order.OrderCode;
                    worksheet.Cells[i + 2, 3].Value = order.PhoneNumber;
                    // Kiểm tra nếu ConfirmedDate có giá trị
                    if (order.ConfirmedDate.HasValue)
                    {
                        worksheet.Cells[i + 2, 4].Value = order.ConfirmedDate.Value.ToOADate();
                        worksheet.Cells[i + 2, 4].Style.Numberformat.Format = "dd/MM/yyyy"; // Định dạng ngày tháng
                    }

                    // Kiểm tra nếu ShippedDate có giá trị
                    if (order.ShippedDate.HasValue)
                    {
                        worksheet.Cells[i + 2, 5].Value = order.ShippedDate.Value.ToOADate();
                        worksheet.Cells[i + 2, 5].Style.Numberformat.Format = "dd/MM/yyyy"; // Định dạng ngày tháng
                    }

                    // Kiểm tra nếu CompletedDate có giá trị
                    if (order.CompletedDate.HasValue)
                    {
                        worksheet.Cells[i + 2, 6].Value = order.CompletedDate.Value.ToOADate();
                        worksheet.Cells[i + 2, 6].Style.Numberformat.Format = "dd/MM/yyyy"; // Định dạng ngày tháng
                    }
                    worksheet.Cells[i + 2, 7].Value = order.TotalPrice;
                    worksheet.Cells[i + 2, 8].Value = order.PaymentMethod;
                    string customer = "";
                    if (order.CustomerId.HasValue)
                    {
                        var customerResult = await GetCustomerId(order.CustomerId.Value);
                        if (customerResult != null) // Kiểm tra nếu customerResult không phải null
                        {
                            customer = customerResult.Value.FirstName + " " + customerResult.Value.LastName; // Gán giá trị cho biến customer
                        }
                    }
                    worksheet.Cells[i + 2, 9].Value = customer;
                    worksheet.Cells[i + 2, 10].Value = order.OrderStatus;
                }

                // Add summary row
                worksheet.Cells[confirmedOrders.Count + 3, 6].Value = "Tổng tiền";
                worksheet.Cells[confirmedOrders.Count + 3, 7].Value = totalCost;
                worksheet.Cells[confirmedOrders.Count + 4, 6].Value = "Tổng số đơn hàng";
                worksheet.Cells[confirmedOrders.Count + 4, 7].Value = invoiceCount;

                // Dòng bắt đầu cho phần Top 5
                int startRow = confirmedOrders.Count + 7;
                int currentRow = startRow;

                // Thêm tiêu đề và dữ liệu cho Top 5 Products
                worksheet.Cells[currentRow, 1].Value = "Top 5 Products";
                currentRow++;
                for (int i = 0; i < topProducts.Count; i++)
                {
                    worksheet.Cells[currentRow, 1].Value = topProducts[i].ProductName;
                    worksheet.Cells[currentRow, 2].Value = topProducts[i].Quantity;
                    currentRow++;
                }

                // Đặt lại currentRow cho phần Top 5 Colors
                currentRow = startRow;
                worksheet.Cells[currentRow, 4].Value = "Top 5 Colors";
                currentRow++;
                for (int i = 0; i < topColors.Count; i++)
                {
                    worksheet.Cells[currentRow, 4].Value = topColors[i].ColorName;
                    worksheet.Cells[currentRow, 5].Value = topColors[i].Quantity;
                    currentRow++;
                }

                // Đặt lại currentRow cho phần Top 5 Sizes
                currentRow = startRow;
                worksheet.Cells[currentRow, 7].Value = "Top 5 Sizes";
                currentRow++;
                for (int i = 0; i < topSizes.Count; i++)
                {
                    worksheet.Cells[currentRow, 7].Value = topSizes[i].SizeNumber;
                    worksheet.Cells[currentRow, 8].Value = topSizes[i].Quantity;
                    currentRow++;
                }

                // Tự động điều chỉnh kích thước cột
                worksheet.Cells.AutoFitColumns();

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
