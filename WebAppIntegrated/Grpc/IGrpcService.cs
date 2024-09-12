using Microsoft.AspNetCore.Components.Forms;

namespace WebAppIntegrated.Grpc;
public interface IGrpcService
{
    Task<string> UploadToServer(IBrowserFile file, string extendDir);
}
