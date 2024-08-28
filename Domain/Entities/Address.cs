namespace Domain.Entities;
public class Address
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string ProvinceCode { get; set; } = string.Empty;
    public string DistrictCode { get; set; } = string.Empty;
    public string WardCode { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public Guid CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }
}
