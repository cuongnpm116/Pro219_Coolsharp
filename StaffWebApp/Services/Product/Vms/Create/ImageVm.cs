using Microsoft.AspNetCore.Components.Forms;

namespace StaffWebApp.Services.Product.Vms.Create;
public class ImageVm
{
    public IBrowserFile File { get; set; }
    public string Binary { get; set; } = string.Empty;
}
