namespace StaffWebApp.Services.Size.Vms;
public class SizeForSelectVm
{
    public Guid Id { get; set; }
    public int SizeNumber { get; set; }

    public SizeForSelectVm()
    {
    }

    public SizeForSelectVm(Guid id, int sizeNumber)
    {
        Id = id;
        SizeNumber = sizeNumber;
    }
}
