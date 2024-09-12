namespace StaffWebApp.Services.Product.Requests;

public class CreateImageRequest
{
    public Guid Id { get; set; }
    public string Path { get; set; } = string.Empty;

    public CreateImageRequest(Guid id, string path)
    {
        Id = id;
        Path = path;
    }
}
