namespace VietNamAddress.Models;

public partial class Ward
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string NameEn { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string FullNameEn { get; set; } = string.Empty;

    public string CodeName { get; set; } = string.Empty;

    public string DistrictCode { get; set; } = string.Empty;

    public int? AdministrativeUnitId { get; set; }

    public virtual AdministrativeUnit? AdministrativeUnit { get; set; }

    public virtual District? DistrictCodeNavigation { get; set; }
}
