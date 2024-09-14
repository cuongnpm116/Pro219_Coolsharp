using Domain.Enums;

namespace Application.Cqrs.Color;

public class ColorVm
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Status Status { get; set; }
}
