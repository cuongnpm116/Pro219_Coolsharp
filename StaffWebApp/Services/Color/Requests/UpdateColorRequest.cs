using WebAppIntegrated.Enum;

namespace StaffWebApp.Services.Color.Requests;

public class UpdateColorRequest
{
    public Guid Id { set; get; }
    public string Name { set; get; }
    public Status Status { get; set; }
}
