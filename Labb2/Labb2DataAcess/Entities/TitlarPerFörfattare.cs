using System;
using System.Collections.Generic;

namespace Labb2DataAcess.Entities;

public partial class TitlarPerFörfattare
{
    public string? Name { get; set; }

    public int? Age { get; set; }

    public int? Titles { get; set; }

    public double? InventoryValue { get; set; }
}
