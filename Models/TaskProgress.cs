using System;
using System.Collections.Generic;

namespace Task_app.Models;

public partial class TaskProgress
{
    public int TaskId { get; set; }

    public int SubTaskId { get; set; }

    public DateTime ProgressDatetime { get; set; }

    public decimal? PercentageOfCompletion { get; set; }
}
