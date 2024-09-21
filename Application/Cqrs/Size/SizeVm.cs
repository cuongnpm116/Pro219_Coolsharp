using Domain.Enums;

namespace Application.Cqrs.Size;
public class SizeVm
{
    public Guid Id { get; set; }
    public int SizeNumber { get; set; }
    public Status Status { get; set; }
}
