namespace StaffWebApp.Services.Product.Dtos;

public class ImageDto
{
    public Guid Id { get; private set; }
    public string Path { get; private set; } = string.Empty;

    public ImageDto(Guid id, string path)
    {
        Id = id;
        Path = path;
    }
}
