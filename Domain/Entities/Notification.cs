namespace Domain.Entities;
public class Notification
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public virtual Customer? Customer { get; set; }
}
