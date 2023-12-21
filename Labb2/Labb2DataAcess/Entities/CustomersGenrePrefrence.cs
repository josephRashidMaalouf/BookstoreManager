using System;
using System.Collections.Generic;

namespace Labb2DataAcess.Entities;

public partial class CustomersGenrePrefrence
{
    public string Name { get; set; } = null!;

    public string? Genre { get; set; }

    public int? BooksOfThisGenreBought { get; set; }
}
