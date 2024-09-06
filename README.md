# Ứng dụng bán hàng online CoolSharp
## Giới thiệu
Ứng dụng Bán Hàng Online được phát triển bằng C# nhằm cung cấp cho người dùng một nền tảng mua sắm trực tuyến, nơi họ có thể duyệt sản phẩm, đặt hàng, thanh toán và theo dõi trạng thái đơn hàng. Ứng dụng hỗ trợ quản lý sản phẩm, quản lý đơn hàng, thông báo trạng thái và tích hợp thanh toán trực tuyến(test).
## Tính năng chính
- Quản lý sản phẩm: thêm, sửa, xóa và xem chi tiết sản phẩm.
- Hỗ trợ giỏ hàng: thêm sản phẩm vào giỏ hàng và quản lý giỏ hàng.
- Xử lý thanh toán: hỗ trợ phương thức thanh toán trực tuyến VNPay.
- Quản lý voucher: thêm, sửa, xóa và xem chi tiết voucher và áp dụng dựa trên tổng đơn hàng.
- Quản lý đơn hàng: theo dõi và thay đổi trạng thái đơn hàng.
- Gửi thông báo: thông báo cho người dùng về tình trạng đơn hàng qua email.
- Tìm kiếm và lọc sản phẩm.
- Thống kê.
## Yêu cầu hệ thống
- .NET SDK 6.0 trở lên
- SQL Server (hoặc bất kỳ cơ sở dữ liệu nào bạn đang sử dụng)
- Visual Studio 2022 (hoặc IDE hỗ trợ .NET)
## Cài đặt và triển khai
### Bước 1: Clone repository
git clone https://github.com/cuongnpm116/Pro219_Coolsharp.git
### Bước 2: cập nhật database
dotnet ef database update
### Bước 3: Set up project run
Api > customerapp > staffapp
### Bước 4: Sử dụng
Mở trình duyệt và truy cập địa chỉ http://localhost:2000 cho người mua hàng.
Đăng ký hoặc đăng nhập để bắt đầu mua sắm.
Tìm kiếm và thêm sản phẩm vào giỏ hàng.
Tiến hành thanh toán bằng phương thức mong muốn.
Theo dõi tình trạng đơn hàng trong trang cá nhân.
Mở trình duyệt và truy cập địa chỉ http://localhost:3000 cho người bán hàng.
Tài khoản admin: admin -- 12345678
Quản lý sản phẩm.
Quản lý, cập nhật đơn hàng.
Xem thống kê.
Quản lý voucher.
## Công nghệ sử dụng
C# và .NET 8.0
ASP.NET Core Web API
Entity Framework Core
MudBlazor (cho UI)
SQL Server
