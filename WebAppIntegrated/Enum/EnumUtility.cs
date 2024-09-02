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
}
