using System;
using System.Collections.Generic;

namespace Labb2DataAcess.Entities;

public partial class Order
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public int? StoreId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Store? Store { get; set; }

    public virtual ICollection<Book> Isbn13s { get; set; } = new List<Book>();
}
