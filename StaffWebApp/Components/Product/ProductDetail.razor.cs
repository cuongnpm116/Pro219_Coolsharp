using Microsoft.AspNetCore.Components;
using MudBlazor;
using StaffWebApp.Services.Product.Vms;

namespace StaffWebApp.Components.Product;

public partial class ProductDetail
{
    [CascadingParameter] 
    MudDialogInstance MudDialog { get; set; }
    [Parameter] 
    public List<ProductDetailStaffVm> ProductDetails { get; set; }
    [Parameter]
    public Guid ProductId { get; set; }

}