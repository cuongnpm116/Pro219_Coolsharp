namespace WebAppIntegrated.Enum;
public static class EnumUtility
{
    public static string ConvertUserStatus(this Status status)
    {
        return status switch
        {
            Status.Active => "Hoạt động",
            Status.InActive => "Ngừng hoạt động",
            Status.Locked => "Bị khóa",
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
    public static string ConvertStatusColorSize(Status status)
    {
        return status switch
        {
            Status.Active => "Đang hoạt động",
            Status.InActive => "Không hoạt động",
            Status.Locked => "Khóa hoạt động",
            Status.Deleted => "Đã xóa",
            _ => "N/A"
        };
    }


}
