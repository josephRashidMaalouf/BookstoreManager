
using System;
using System.Collections.Generic;

namespace Labb2DataAcess.Entities;

public partial class Description 
{
    public int Id { get; set; }

    public string? Description1 { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
