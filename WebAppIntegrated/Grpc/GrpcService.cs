using Google.Protobuf;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components.Forms;
using Upload;

namespace WebAppIntegrated.Grpc;
public class GrpcService : IGrpcService
{
    private readonly GrpcChannel _channel;

    public GrpcService()
    {
        _channel = GrpcChannel.ForAddress("https://localhost:1001");
    }

    public async Task<string> UploadToServer(IBrowserFile file, string extendDir)
    {
        var client = new Uploader.UploaderClient(_channel);
        var call = client.UploadFile();

        // Sửa lại metadata để bao gồm thông tin về thư mục đích (SpecificDirectory)
        await call.RequestStream.WriteAsync(new UploadFileRequest
        {
            Metadata = new()
            {
                FileExtension = Path.GetExtension(file.Name),
                SpecificDirectory = extendDir
            }
        });

        byte[] buffer = new byte[1024 * 32];
        await using var readStream = file.OpenReadStream();
        while (true)
        {
            int count = await readStream.ReadAsync(buffer);
            if (count == 0)
                break;
            await call.RequestStream.WriteAsync(new UploadFileRequest
            {
                Data = UnsafeByteOperations.UnsafeWrap(buffer.AsMemory(0, count))
            });
        }

        await call.RequestStream.CompleteAsync();
        var response = await call.ResponseAsync;
        return response.Id;
    }
}
