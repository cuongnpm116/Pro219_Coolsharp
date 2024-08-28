namespace Domain.Entities;
public class Role
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<StaffRole>? StaffRoles { get; set; }
}
