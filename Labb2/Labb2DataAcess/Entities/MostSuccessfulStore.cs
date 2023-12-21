using System;
using System.Collections.Generic;

namespace Labb2DataAcess.Entities;

public partial class MostSuccessfulStore
{
    public string BookStore { get; set; } = null!;

    public int? Sales { get; set; }

    public double? TotalRevenue { get; set; }
}
