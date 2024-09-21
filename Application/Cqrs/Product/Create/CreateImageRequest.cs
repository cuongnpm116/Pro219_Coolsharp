namespace Application.Cqrs.Product.Create;

public class CreateImageRequest
{
    public Guid Id { get; set; }
    public string Path { get; set; }
}