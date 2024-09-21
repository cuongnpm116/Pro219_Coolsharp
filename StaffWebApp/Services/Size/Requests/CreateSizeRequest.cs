using WebAppIntegrated.Enum;

namespace StaffWebApp.Services.Size.Requests;

public class CreateSizeRequest
{
    public Guid Id { set; get; }
    public int SizeNumber { set; get; }
    public Status Status { get; set; }
}
