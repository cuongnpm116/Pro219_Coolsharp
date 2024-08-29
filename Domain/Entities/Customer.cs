using Domain.Enums;

namespace Domain.Entities;
public class Customer
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string EmailAddress { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = "default.png";
    public Status Status { get; set; } = Status.Active;

    public virtual ICollection<Order>? Orders { get; set; }
    public virtual ICollection<Address>? Addresses { get; set; }
    public virtual ICollection<Payment>? Payments { get; set; }
    public virtual ICollection<Notification>? Notifications { get; set; }
    public virtual Cart? Cart { get; set; }
}
