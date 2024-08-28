namespace VietNamAddress.Models;

public partial class Province
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string NameEn { get; set; } = string.Empty;

    public string FullName { get; set; } = null!;

    public string FullNameEn { get; set; } = string.Empty;

    public string CodeName { get; set; } = string.Empty;

    public int? AdministrativeUnitId { get; set; }

    public int? AdministrativeRegionId { get; set; }

    public virtual AdministrativeRegion? AdministrativeRegion { get; set; }

    public virtual AdministrativeUnit? AdministrativeUnit { get; set; }

    public virtual ICollection<District> Districts { get; set; } = [];
}
