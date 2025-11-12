using System;
using System.Collections.Generic;

namespace apidiesel.Models;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string SupplierName { get; set; } = null!;

    public string? Contacts { get; set; }

}
