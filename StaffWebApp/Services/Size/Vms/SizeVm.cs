using WebAppIntegrated.Enum;

namespace StaffWebApp.Services.Size.Vms;

public class SizeVm
{
    public Guid Id { get; set; }
    public int SizeNumber { get; set; }
    public Status Status { get; set; }
}
