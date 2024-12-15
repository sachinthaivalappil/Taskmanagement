using System;
using System.Collections.Generic;

namespace Task_app.Models;

public partial class TaskMaster
{
    public int TaskId { get; set; }

    public string TaskName { get; set; } = null!;

    public string TaskDcr { get; set; } = null!;

    public string? Department { get; set; }

    public string? AssignedBy { get; set; }

    public string? AssignedTo { get; set; }

    public DateOnly? DateOfAssignment { get; set; }

    public string? TaskProgress { get; set; }

    public DateOnly? TaskTargetdate { get; set; }
}
