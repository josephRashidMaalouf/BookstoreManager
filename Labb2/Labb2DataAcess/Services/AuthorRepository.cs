
using System.Collections.ObjectModel;
using Common.Models;
using Labb2DataAcess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Labb2DataAcess.Services;

public class AuthorRepository
{
    private readonly Labb1Bokhandel2Context _context;

    public AuthorRepository(Labb1Bokhandel2Context context)
    {
        _context = context;
    }

    public List<AuthorModel> GetAllAuthors()
    {
        var authors = _context.Authors
            .Select
            (
                a => new AuthorModel()
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Birthday = a.Birthday,
                    Isbn13s = new ObservableCollection<BookModel>
                    (
                        a.Isbn13s
                            .Select
                            (
                                b => new BookModel()
                                {
                                    Isbn13 = b.Isbn13,
                                    Title = b.Title,
                                    Language = b.Language,
                                    Price = b.Price,
                                    PublishingDate = b.PublishingDate,
                                    Authors = new ObservableCollection<AuthorModel>
                                    (
                                        b.Authors
                                            .Select
                                            (
                                                a => new AuthorModel()
                                                {
                                                    Id = a.Id,
                                                    Birthday = a.Birthday,
                                                    FirstName = a.FirstName,
                                                    LastName = a.LastName
                                                }
                                            ).ToList()
                                    )

                                }
                            )
                    )
                }
            ).ToList();
        return authors;
    }


    public void UpdateAuthor(AuthorModel author)
    {
        var authorEntity = _context.Authors.Find(author.Id);

        authorEntity.FirstName = author.FirstName;
        authorEntity.LastName = author.LastName;
        authorEntity.Birthday = author.Birthday;
        _context.Authors.Update
            (
                authorEntity
            );

        _context.SaveChanges();
    }

    public void AddAuthor(AuthorModel author)
    {
        var newAuthor = new Author()
        {
            
            FirstName = author.FirstName,
            LastName = author.LastName,
            Birthday = author.Birthday
        };

        _context.Authors.Add(newAuthor);

        _context.SaveChanges();

        author.Id = _context.Authors
            .OrderBy(a => a.Id)
            .Last().Id;
    }

    public void DeleteAuthor(AuthorModel authorModel)
    {
        var author = _context.Authors
            .Include(a => a.Isbn13s)
            .Include(a => a.Publishers)
            .FirstOrDefault(a => a.Id == authorModel.Id);

        author.Isbn13s.Clear();
        author.Publishers.Clear();

        _context.Authors.Remove(author);

        _context.SaveChanges();
    }
}