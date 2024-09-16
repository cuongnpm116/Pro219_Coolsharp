namespace Domain.Entities;
public class Role
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<StaffRole>? StaffRoles { get; set; }

    public Role()
    {
    }

    public Role(string code, string name)
    {
        Code = code;
        Name = name;
    }
}
