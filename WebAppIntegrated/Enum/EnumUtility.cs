namespace WebAppIntegrated.Enum;
public static class EnumUtility
{
    public static string ConvertUserStatus(this Status status)
    {
        return status switch
        {
            Status.Active => "Hoạt động",
            Status.InActive => "Ngừng hoạt động",
            Status.Locked => "Khóa",
            Status.Deleted => "Nghỉ việc",
            Status.None => "Không đề cập",
            _ => "N/A"
        };
    }

    public static string ConvertGender(this int gender)
    {
        return gender switch
        {
            0 => "Không đề cập",
            1 => "Nam",
            2 => "Nữ",
            _ => "N/A"
        };
    }

    public static string ConvertOrderStatus(OrderStatus OrderStatus)
    {
        return OrderStatus switch
        {
            OrderStatus.None => "Không có",
            OrderStatus.Pending => "Chờ xác nhận",
            OrderStatus.AwaitingShipment => "Chờ lấy hàng",
            OrderStatus.AWaitingPickup => "Chờ giao hàng",
            OrderStatus.Completed => "Hoàn thành",
            OrderStatus.Cancelled => "Đã hủy",
            _ => "Không xác định"
        };
    }
    public static string ConvertPaymentStatus(PaymentStatus status)
    {
        return status switch
        {
            PaymentStatus.None => "Không có",
            PaymentStatus.Pending => "Chờ thanh toán",
            PaymentStatus.Completed => "Đã thanh toán",
            PaymentStatus.Failed => "Thanh toán không thành công",
            PaymentStatus.Canceled => "Đã huỷ",
            _ => "N/A"
        };
    }
    public static string ConvertPaymentMethod(PaymentMethod status)
    {
        return status switch
        {
            PaymentMethod.None => "Không có",
            PaymentMethod.OnlinePayment => "Thanh toán online (VnPay)",
            PaymentMethod.CashOnDelivery => "Thanh toán khi nhận hàng",
            _ => "N/A"
        };
    }
    public static string ConvertStatus(Status status)
    {
        return status switch
        {
            Status.None => "Không có",
            Status.Active => "Đang hoạt động",
            Status.InActive => "Ngừng hoạt động",
            Status.Locked => "Khóa",
            Status.Deleted => "Đã xóa",
            _ => "N/A"
        };
    }
}
