using System.Text.Json.Serialization;
using StaffWebApp.Services.Category.ViewModel;

namespace StaffWebApp.Services.Product.Vms.Create;
public class ProductInfoVm
{
    public string Name { get; set; }
    [JsonIgnore]
    public CategoryVm _validateCategories { get; set; }
    public IEnumerable<CategoryVm> Categories { get; set; } = [];
}
