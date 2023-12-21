using System.Collections.ObjectModel;
using Common.Models;
using Labb2DataAcess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Labb2DataAcess.Services;

public class BookRepository
{
    private readonly Labb1Bokhandel2Context _context;

    public BookRepository(Labb1Bokhandel2Context context)
    {
        _context = context;
    }

    public BookModel? GetBookByIsbn(string isbn) 
    {
        var book = _context.Books
            .Include(b => b.Authors)
            .FirstOrDefault(b => b.Isbn13 == isbn);

        if (book == null)
        {
            return null;
        }

        return new BookModel()
        {
            Isbn13 = book.Isbn13,
            Title = book.Title,
            Language = book.Language,
            Price = book.Price,
            PublishingDate = book.PublishingDate,
            Authors = new ObservableCollection<AuthorModel>(book.Authors
                .Select(a => new AuthorModel()
                {   Id = a.Id,
                    Birthday = a.Birthday,
                    FirstName = a.FirstName,
                    LastName = a.LastName
                })) 

        };
    }

    public ObservableCollection<BookModel> GetAllBooks()
    {
        var books = new ObservableCollection<BookModel>
            (
                _context.Books
                    .Select
                        (
                            b => new BookModel()
                            {
                                Isbn13 = b.Isbn13,
                                Title = b.Title,
                                Language = b.Language,
                                Price = b.Price,
                                PublishingDate = b.PublishingDate,
                                Authors = new ObservableCollection<AuthorModel>(b.Authors
                                    .Select(a => new AuthorModel()
                                    {
                                        Id = a.Id,
                                        Birthday = a.Birthday,
                                        FirstName = a.FirstName,
                                        LastName = a.LastName
                                    }))
                            }
                        )
            );

        return books;
    }

    public ObservableCollection<BookModel> GetAllBooksByStore(StoreModel store)
    {
        
        var ib = _context.IventoryBalances.Where(i => i.StoreId == store.Id).ToList();

        var books = new ObservableCollection<BookModel>();

        foreach (var isbn in ib)
        {
            var book = GetBookByIsbn(isbn.Isbn13);

            if(book != null) 
                books.Add(book);
        }

        return books;
    }

    public void UpdateBook(BookModel book)
    {
        var entityBook = _context.Books.Include(b => b.Authors).FirstOrDefault(b => b.Isbn13 == book.Isbn13);

        //clear old authors
        entityBook.Authors.Clear();

        foreach (var authorModel in book.Authors)
        {
            var entityAuthor = _context.Authors.Find(authorModel.Id);
            entityBook.Authors.Add(entityAuthor);
        }



        entityBook.Isbn13 = book.Isbn13;
        entityBook.Title = book.Title;
        entityBook.Price = book.Price;
        entityBook.Language = book.Language;
        entityBook.PublishingDate = book.PublishingDate;

        _context.SaveChanges();
    }

    public void AddBook(BookModel book)
    {
        var newBook = new Book()
        {
            Isbn13 = book.Isbn13,
            Title = book.Title,
            Language = book.Language,
            Price = book.Price,
            PublishingDate = book.PublishingDate,
        };

        var authors = book.Authors;

        foreach (var author in authors)
        {
            var authorEntity = _context.Authors.Find(author.Id);
            newBook.Authors.Add(authorEntity);
        }

        _context.Books.Add(newBook);
        _context.SaveChanges();
    }

    public void DeleteBook(BookModel bookModel)
    {
        var book = _context.Books
            .Include(b => b.Authors)
            .Include(b => b.IventoryBalances)
            .Include(b => b.Orders)
            .FirstOrDefault(b => b.Isbn13 == bookModel.Isbn13);

        book.Authors.Clear();
        book.IventoryBalances.Clear();
        book.Orders.Clear();

        _context.Books.Remove(book);
        _context.SaveChanges();
    }

  
}