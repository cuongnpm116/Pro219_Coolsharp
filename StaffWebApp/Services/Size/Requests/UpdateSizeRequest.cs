using WebAppIntegrated.Enum;

namespace StaffWebApp.Services.Size.Requests;

public class UpdateSizeRequest
{
    public Guid Id { set; get; }
    public int SizeNumber { set; get; }
    public Status Status { get; set; }
}
