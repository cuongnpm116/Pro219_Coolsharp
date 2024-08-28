namespace VietNamAddress.Models;

public partial class District
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string NameEn { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string FullNameEn { get; set; } = string.Empty;

    public string CodeName { get; set; } = string.Empty;

    public string ProvinceCode { get; set; } = string.Empty;

    public int? AdministrativeUnitId { get; set; }

    public virtual AdministrativeUnit? AdministrativeUnit { get; set; }

    public virtual Province? ProvinceCodeNavigation { get; set; }

    public virtual ICollection<Ward> Wards { get; set; } = [];
}
