using System;
using System.Collections.Generic;

namespace apidiesel.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime AddedDate { get; set; }

    
}
