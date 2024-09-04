using Google.Protobuf;
using Grpc.Net.Client;
using Hello;
using Microsoft.AspNetCore.Components.Forms;
using Upload;
using WebAppIntegrated.Constants;

namespace WebAppIntegrated.Services;
public class GrpcService : IGrpcService
{
    private readonly GrpcChannel _channel;

    public GrpcService()
    {
        _channel = GrpcChannel.ForAddress("https://localhost:1001");
    }

    public async Task<string> SayHelloAsync(string name)
    {
        var client = new Greeter.GreeterClient(_channel);
        var reply = await client.SayHelloAsync(new HelloRequest { Name = name });
        return reply.Message;
    }

    public async Task<string> UploadFileAsync(IBrowserFile file)
    {
        using var channel = GrpcChannel.ForAddress(ShopConstants.GrpcApiHost);
        var client = new Uploader.UploaderClient(channel);
        var call = client.UploadFile();

        string fileEx = Path.GetExtension(file.Name);

        await call.RequestStream.WriteAsync(new UploadFileRequest
        {
            FileName = Path.GetRandomFileName() + Path.GetExtension(fileEx),
            ExtendDir = DirectoryConstants.UserContent
        });

        await using var readStream = file.OpenReadStream();

        var buffer = new byte[1024 * 32];
        while (true)
        {
            int count = await readStream.ReadAsync(buffer);
            if (count == 0)
            {
                break;
            }

            await call.RequestStream.WriteAsync(new UploadFileRequest
            {
                Data = UnsafeByteOperations.UnsafeWrap(buffer.AsMemory(0, count))
            });
        }

        await call.RequestStream.CompleteAsync();
        var response = await call;
        return response.Id;
    }
}
