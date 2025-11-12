using System;
using System.Collections.Generic;

namespace apidiesel.Models;

public partial class OperationDetail
{
    public int DetailId { get; set; }

    public int OperationId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

  
}
