using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Services.Product.Vms;

namespace StaffWebApp.Components.Product;

public partial class ProductDetail
{
    [CascadingParameter] 
    MudDialogInstance MudDialog { get; set; }
    [Parameter] 
    public List<ProductDetailVm> ProductDetails { get; set; }
    [Parameter]
    public Guid ProductId { get; set; }

}