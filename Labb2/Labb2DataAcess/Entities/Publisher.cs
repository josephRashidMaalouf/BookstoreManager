using System;
using System.Collections.Generic;

namespace Labb2DataAcess.Entities;

public partial class Publisher
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? HeadOfficeCity { get; set; }

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();
}
