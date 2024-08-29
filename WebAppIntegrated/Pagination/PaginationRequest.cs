namespace WebAppIntegrated.Pagination;
public class PaginationRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 8;
    public string? SearchString { get; set; } = string.Empty;
}
