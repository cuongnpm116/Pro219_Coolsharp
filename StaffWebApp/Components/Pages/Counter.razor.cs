using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using WebAppIntegrated.Constants;
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
        string newFileName = Path.GetRandomFileName() + Path.GetExtension(file.Name);
        await GrpcService.UploadFileAsync(file, DirectoryConstants.ProductContent, newFileName);
        Snackbar.Add($"File uploaded successfully {newFileName}", Severity.Success);
    }
}