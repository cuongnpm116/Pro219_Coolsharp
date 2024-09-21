using Microsoft.AspNetCore.Components.Forms;

namespace CustomerWebApp.Service.Customer.Request;

public class UpdateAvatarRequest
{
    public Guid CustomerId { get; set; }
    public string OldImageUrl { get; set; } = string.Empty;
    public IBrowserFile NewImage { get; set; } = null!;
}
