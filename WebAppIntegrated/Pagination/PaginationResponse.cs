namespace WebAppIntegrated.Pagination;
public class PaginationResponse<T>
{
    public int PageNumber { get; set; }
    public bool HasNext { get; set; }
    public IReadOnlyCollection<T>? Data { get; set; }
}
