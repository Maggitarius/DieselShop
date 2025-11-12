using System;
using System.Collections.Generic;

namespace apidiesel.Models;

public partial class Warehouse
{
    public int WarehouseId { get; set; }

    public string WarehouseName { get; set; } = null!;

    public string? Address { get; set; }

   
}
