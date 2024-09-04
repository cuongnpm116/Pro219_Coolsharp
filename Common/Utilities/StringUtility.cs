using System.Text;
using System.Text.RegularExpressions;

namespace Common.Utilities;
public partial class StringUtility
{
    // mảng ký tự để chuyển đổi
    private static readonly string[] VietnameseSigns =
        [
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        ];

    // chuyển chữ tiếng việt sang alphabet
    public static string RemoveSign4VietnameseString(string str)
    {
        for (int i = 1; i < VietnameseSigns.Length; i++)
        {
            for (int j = 0; j < VietnameseSigns[i].Length; j++)
                str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
        }
        return str;
    }

    // combo thay khoảng trắng với 'replacement' sử dụng regex
    [GeneratedRegex(@"\s+")]
    private static partial Regex MyRegex();
    private static readonly Regex sWhitespace = MyRegex();
    public static string ReplaceWhitespace(string input, string replacement)
    {
        return sWhitespace.Replace(input, replacement);
    }

    public static string DateNowToString()
    {
        DateTime now = DateTime.Now;
        string result = $"{now.Year}{now.Month:00}{now.Day:00}";
        return result;
    }
    private static readonly char[] _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
    private static readonly Random _random = new Random();

    public static string GenerateVoucherCode(int length)
    {
        var voucherCode = new char[length];

        for (int i = 0; i < length; i++)
        {
            voucherCode[i] = _chars[_random.Next(_chars.Length)];
        }

        return new string(voucherCode);
    }
    public static string GenerateOrderCode(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder sb = new();
        Random rd = new();
        for (int i = 0; i < length; i++)
        {
            sb.Append(chars[rd.Next(chars.Length)]);
        }
        return sb.ToString();
    }
}
