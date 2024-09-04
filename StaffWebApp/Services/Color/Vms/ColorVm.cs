using WebAppIntegrated.Enum;

namespace StaffWebApp.Services.Color.Vms;

public class ColorVm
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Status Status { get; set; }
}
