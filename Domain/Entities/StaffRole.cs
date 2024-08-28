namespace Domain.Entities;
public class StaffRole
{
    public Guid Id { get; set; }
    public Guid StaffId { get; set; }
    public Guid RoleId { get; set; }

    public virtual Staff? Staff { get; set; }
    public virtual Role? Role { get; set; }
}
