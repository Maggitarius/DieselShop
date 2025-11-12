using System;
using System.Collections.Generic;

namespace apidiesel.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Sku { get; set; }

    public decimal PurchasePrice { get; set; }

    public decimal SalePrice { get; set; }

    public int CategoryId { get; set; }

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

   
}
