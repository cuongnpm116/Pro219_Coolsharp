using Microsoft.AspNetCore.Components.Forms;

namespace WebAppIntegrated.Services;
public interface IGrpcService
{
    Task<string> SayHelloAsync(string name);
    Task<string> UploadFileAsync(IBrowserFile file);
}
