using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using WebAppIntegrated.Services;

namespace StaffWebApp.Components.Pages;
public partial class Counter
{
    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private IGrpcService GrpcService { get; set; } = null!;

    private int currentCount = 0;

    private void IncrementCount()
    {
        currentCount++;
    }

    private async Task Hello()
    {
        var message = await GrpcService.SayHelloAsync("cuong nguyen");
        Snackbar.Add(message, Severity.Success);
    }

    private async Task UploadFiles(IBrowserFile file)
    {
        var fileId = await GrpcService.UploadFileAsync(file);
        Snackbar.Add(fileId, Severity.Success);
    }
}