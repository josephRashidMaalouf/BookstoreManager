using System;
using System.Collections.Generic;


namespace Labb2DataAcess.Entities;

public partial class Author 
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? Birthday { get; set; }

    public virtual ICollection<Book> Isbn13s { get; set; } = new List<Book>();

    public virtual ICollection<Publisher> Publishers { get; set; } = new List<Publisher>();
}
