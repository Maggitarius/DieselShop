using System;
using System.Collections.Generic;

namespace apidiesel.Models;

public partial class Operation
{
    public int OperationId { get; set; }

    public string OperationType { get; set; } = null!;

    public DateTime Date { get; set; }

    public int EmployeeId { get; set; }

    public int? CustomerId { get; set; }

    public int WarehouseId { get; set; }

   
}
