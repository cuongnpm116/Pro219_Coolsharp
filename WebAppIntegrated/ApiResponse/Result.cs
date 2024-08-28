namespace WebAppIntegrated.ApiResponse;
public class Result<T>
{
    public T? Value { get; set; }
    public int Status { get; set; }
    public string Message { get; set; } = string.Empty;
    public IEnumerable<ValidationError> ValidationErrors { get; set; } = [];
    public bool IsSuccess { get; set; }
}
