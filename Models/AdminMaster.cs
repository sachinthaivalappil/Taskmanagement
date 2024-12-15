using System;
using System.Collections.Generic;

namespace Task_app.Models;

public partial class AdminMaster
{
    public int AdminId { get; set; }

    public string Name { get; set; } = null!;

    public int? Age { get; set; }

    public string? Department { get; set; }

    public string? Password { get; set; }
}
