using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Labb2DataAcess.Entities;

public partial class Labb1Bokhandel2Context : DbContext
{
    public Labb1Bokhandel2Context()
    {
    }

    public Labb1Bokhandel2Context(DbContextOptions<Labb1Bokhandel2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomersGenrePrefrence> CustomersGenrePrefrences { get; set; }

    public virtual DbSet<Description> Descriptions { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<IventoryBalance> IventoryBalances { get; set; }

    public virtual DbSet<MostSuccessfulStore> MostSuccessfulStores { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<TitlarPerFörfattare> TitlarPerFörfattares { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-0M7APTU;Initial Catalog=Labb1_Bokhandel2;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Authors__3214EC0727ECC4AB");

            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .HasColumnName("First Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .HasColumnName("Last Name");

            entity.HasMany(d => d.Isbn13s).WithMany(p => p.Authors)
                .UsingEntity<Dictionary<string, object>>(
                    "AuthorBook",
                    r => r.HasOne<Book>().WithMany()
                        .HasForeignKey("Isbn13")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_BookId_Books"),
                    l => l.HasOne<Author>().WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_AuthorId_Authors2"),
                    j =>
                    {
                        j.HasKey("AuthorId", "Isbn13");
                        j.ToTable("Author_Book");
                        j.IndexerProperty<string>("Isbn13").HasMaxLength(13);
                    });

            entity.HasMany(d => d.Publishers).WithMany(p => p.Authors)
                .UsingEntity<Dictionary<string, object>>(
                    "PublisherAuthor",
                    r => r.HasOne<Publisher>().WithMany()
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PublisherId_Publishers"),
                    l => l.HasOne<Author>().WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_AuthorId_Authors"),
                    j =>
                    {
                        j.HasKey("AuthorId", "PublisherId").HasName("PK_Pub_AUT");
                        j.ToTable("Publisher_Author");
                    });
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Isbn13).HasName("PK__Books__E7B44C946F561F25");

            entity.Property(e => e.Isbn13).HasMaxLength(13);
            entity.Property(e => e.Language).HasMaxLength(20);
            entity.Property(e => e.PublishingDate).HasColumnName("Publishing_Date");
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Description).WithMany(p => p.Books)
                .HasForeignKey(d => d.DescriptionId)
                .HasConstraintName("FK__Books__Descripti__48CFD27E");

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK__Books__GenreId__2F10007B");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC0782ECC128");

            entity.Property(e => e.City).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .HasColumnName("First Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .HasColumnName("Last Name");
            entity.Property(e => e.PostalNo).HasColumnName("Postal No");
            entity.Property(e => e.Street).HasMaxLength(50);
        });

        modelBuilder.Entity<CustomersGenrePrefrence>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("CustomersGenrePrefrences");

            entity.Property(e => e.BooksOfThisGenreBought).HasColumnName("Books of this genre bought");
            entity.Property(e => e.Genre).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(41);
        });

        modelBuilder.Entity<Description>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Descript__3214EC076584C498");

            entity.Property(e => e.Description1).HasColumnName("Description");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genres__3214EC07BE368263");

            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<IventoryBalance>(entity =>
        {
            entity.HasKey(e => new { e.StoreId, e.Isbn13 }).HasName("PK_Store_Book");

            entity.ToTable("IventoryBalance");

            entity.Property(e => e.Isbn13).HasMaxLength(13);

            entity.HasOne(d => d.Isbn13Navigation).WithMany(p => p.IventoryBalances)
                .HasForeignKey(d => d.Isbn13)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Isbn13_Books2");

            entity.HasOne(d => d.Store).WithMany(p => p.IventoryBalances)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__IventoryB__Store__4316F928");
        });

        modelBuilder.Entity<MostSuccessfulStore>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("MostSuccessfulStore");

            entity.Property(e => e.BookStore)
                .HasMaxLength(20)
                .HasColumnName("Book store");
            entity.Property(e => e.TotalRevenue).HasColumnName("Total revenue");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC07B8C40090");

            entity.Property(e => e.CustomerId).HasColumnName("Customer Id");
            entity.Property(e => e.StoreId).HasColumnName("Store Id");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Orders__Customer__3A81B327");

            entity.HasOne(d => d.Store).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK__Orders__Store Id__3B75D760");

            entity.HasMany(d => d.Isbn13s).WithMany(p => p.Orders)
                .UsingEntity<Dictionary<string, object>>(
                    "OrderBook",
                    r => r.HasOne<Book>().WithMany()
                        .HasForeignKey("Isbn13")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_IsbnId_Isbn13"),
                    l => l.HasOne<Order>().WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_OrderId_Orders"),
                    j =>
                    {
                        j.HasKey("OrderId", "Isbn13");
                        j.ToTable("Order_Book");
                        j.IndexerProperty<string>("Isbn13").HasMaxLength(13);
                    });
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Publishe__3214EC07EE79B125");

            entity.Property(e => e.HeadOfficeCity)
                .HasMaxLength(20)
                .HasColumnName("Head Office City");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Stores__3214EC072F8BD9A9");

            entity.Property(e => e.City).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.PostalNo).HasColumnName("Postal No");
            entity.Property(e => e.Street).HasMaxLength(50);
        });

        modelBuilder.Entity<TitlarPerFörfattare>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("TitlarPerFörfattare");

            entity.Property(e => e.InventoryValue).HasColumnName("Inventory value");
            entity.Property(e => e.Name).HasMaxLength(41);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
