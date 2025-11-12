using System;
using System.Collections.Generic;

namespace apidiesel.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Position { get; set; }

    public string Login { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

}
