using Common.Consts;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Upload;

namespace GrpcIntegrated.Services;
public class UploaderService : Uploader.UploaderBase
{
    private readonly string _rootPath;
    private readonly string[] _dir;

    public UploaderService(IWebHostEnvironment environment)
    {
        _rootPath = environment.WebRootPath;
        _dir = [StorageDirectory.ProductContent, StorageDirectory.UserContent, StorageDirectory.AppContent];
    }

    public override async Task<UploadFileResponse> UploadFile(IAsyncStreamReader<UploadFileRequest> requestStream, ServerCallContext context)
    {
        await requestStream.MoveNext(CancellationToken.None);
        var initialMessage = requestStream.Current ?? throw new RpcException(new Status(StatusCode.InvalidArgument, "No file uploaded"));
        string extendDir = initialMessage.ExtendDir ?? string.Empty;
        string fileName = initialMessage.FileName ?? Path.GetRandomFileName();
        FileStream writeStream = File.Create(Path.Combine(_rootPath, extendDir, fileName));

        await foreach (var message in requestStream.ReadAllAsync())
        {
            if (message.Data != null)
            {
                await writeStream.WriteAsync(message.Data.Memory);
            }
        }

        return new UploadFileResponse { Id = fileName };
    }
}
