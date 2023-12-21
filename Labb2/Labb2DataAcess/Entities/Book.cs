
using System;
using System.Collections.Generic;

namespace Labb2DataAcess.Entities;

public partial class Book 
{
    public string Isbn13 { get; set; } = null!;

    public string? Title { get; set; }

    public string? Language { get; set; }

    public double? Price { get; set; }

    public DateOnly? PublishingDate { get; set; }

    public int? GenreId { get; set; }

    public int? DescriptionId { get; set; }

    public virtual Description? Description { get; set; }

    public virtual Genre? Genre { get; set; }

    public virtual ICollection<IventoryBalance> IventoryBalances { get; set; } = new List<IventoryBalance>();

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
