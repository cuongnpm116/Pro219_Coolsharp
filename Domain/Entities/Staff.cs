using Domain.Enums;

namespace Domain.Entities;
public class Staff
{
    public Guid Id { get; set; }
    public Guid? CreateBy { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public Guid? DeletedBy { get; set; }
    public DateTime DeletedOn { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = "default.png";
    public DateTime DateOfBirth { get; set; }
    public Status Status { get; set; } = Status.Active;

    public virtual ICollection<Order>? Orders { get; set; }
    public virtual ICollection<StaffRole>? StaffRoles { get; set; }

    public void UpdateInfo(string firstName, string lastName, DateTime dob)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dob;
    }
}
