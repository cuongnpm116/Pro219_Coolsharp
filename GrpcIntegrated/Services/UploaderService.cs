using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Upload;

namespace GrpcIntegrated.Services;
public class UploaderService : Uploader.UploaderBase
{
    private readonly string Root;

    public UploaderService(IWebHostEnvironment environment)
    {
        Root = environment.WebRootPath;
    }

    public override async Task<UploadFileResponse> UploadFile(
        IAsyncStreamReader<UploadFileRequest> requestStream,
        ServerCallContext context)
    {
        if (!await requestStream.MoveNext())
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "No data receive"));
        }
        var firstMessage = requestStream.Current;
        string fileExtension = ".bin";
        if (firstMessage.Metadata != null && !string.IsNullOrWhiteSpace(firstMessage.Metadata.FileExtension))
        {
            fileExtension = firstMessage.Metadata.FileExtension;
        }
        string newFileName = Path.GetRandomFileName() + fileExtension;
        string writePath = Path.Combine(Root, firstMessage.Metadata.SpecificDirectory, newFileName);
        await using FileStream writeStream = File.Create(writePath);
        await foreach (var message in requestStream.ReadAllAsync())
        {
            if (message.Data != null)
            {
                await writeStream.WriteAsync(message.Data.Memory);
            }
        }
        return new UploadFileResponse
        {
            Id = newFileName
        };
    }
}