namespace StaffWebApp.Services.Color.Vms;
public class ColorForSelectVm
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ColorForSelectVm()
    {
    }

    public ColorForSelectVm(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}
