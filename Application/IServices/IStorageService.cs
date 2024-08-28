using Microsoft.AspNetCore.Http;

namespace Application.IServices;
/// <summary>
/// Represents a storage service for saving and deleting files.
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Saves a file asynchronously.
    /// </summary>
    /// <param name="file">The file to be saved.</param>
    /// <param name="directory">The directory where the file will be saved.</param>
    /// <returns>A task that represents the asynchronous save operation. The task result contains the saved file path.</returns>
    Task<string> SaveFileAsync(IFormFile file, string directory);

    /// <summary>
    /// Deletes a file asynchronously.
    /// </summary>
    /// <param name="fileName">The name of the file to be deleted.</param>
    /// <param name="directory">The directory where the file is located.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteFileAsync(string fileName, string directory);
}
