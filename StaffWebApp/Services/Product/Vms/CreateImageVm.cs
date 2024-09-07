using Microsoft.AspNetCore.Components.Forms;

namespace StaffWebApp.Services.Product.Vms;
public class CreateImageVm
{
    public IBrowserFile File { get; set; }
    public string Binary { get; set; } = string.Empty;
}
