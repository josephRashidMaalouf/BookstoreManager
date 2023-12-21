
using System;
using System.Collections.Generic;

namespace Labb2DataAcess.Entities;

public partial class Store 
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string City { get; set; } = null!;

    public int PostalNo { get; set; }

    public virtual ICollection<IventoryBalance> IventoryBalances { get; set; } = new List<IventoryBalance>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
