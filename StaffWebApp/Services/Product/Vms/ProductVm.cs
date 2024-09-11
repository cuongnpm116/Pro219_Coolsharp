﻿using WebAppIntegrated.Enum;

namespace StaffWebApp.Services.Product.Vms;

public class ProductVm
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal MinPrice { get; set; }
    public int TotalStock { get; set; }
    public bool IsShow { get; set; } = false;
    public Status Status { get; set; }
    public List<ProductDetailVm> ProductDetails { get; set; }
}
