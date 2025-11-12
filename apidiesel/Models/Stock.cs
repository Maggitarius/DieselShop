using System;
using System.Collections.Generic;

namespace apidiesel.Models;

public partial class Stock
{
    public int StockId { get; set; }

    public int WarehouseId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

}
