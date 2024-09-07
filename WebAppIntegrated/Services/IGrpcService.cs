using Microsoft.AspNetCore.Components.Forms;

namespace WebAppIntegrated.Services;
public interface IGrpcService
{
    Task<string> SayHelloAsync(string name);
    Task UploadFileAsync(IBrowserFile file, string extendDir, string newFileName);
}
