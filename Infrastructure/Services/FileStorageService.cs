using Application.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;
internal sealed class FileStorageService : IStorageService
{
    private readonly string RootPath = string.Empty;

    public FileStorageService(IWebHostEnvironment environment)
    {
        RootPath = environment.WebRootPath;
        if (!Directory.Exists(RootPath))
        {
            Directory.CreateDirectory(RootPath);
        }
    }

    public async Task<string> SaveFileAsync(IFormFile file, string directory)
    {
        string newFileName = Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(file.FileName));
        string newPath = Path.Combine(RootPath, directory, newFileName); // wwwroot/user-content/filename.fileExtension
        await using FileStream fs = new(newPath, FileMode.Create);
        await file.CopyToAsync(fs);
        return newFileName;
    }

    public async Task DeleteFileAsync(string fileName, string directory)
    {
        string filePath = Path.Combine(RootPath, directory, fileName);
        if (File.Exists(filePath))
        {
            await Task.Run(() => File.Delete(filePath));
        }
    }
}
