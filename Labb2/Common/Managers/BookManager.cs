using Common.Models;

namespace Common.Managers;

public static class BookManager
{
    public static event Action<BookModel> BookAdded;

    public static event Action<BookModel, string> BookUpdated;

    public static event Action<BookModel> BookDeleted;

    public static async Task OnBookDeleted(BookModel book)
    {
        BookDeleted.Invoke(book);
    }

    public static async Task OnBookUpdated(BookModel book, string oldIsbn)
    {
        BookUpdated.Invoke(book, oldIsbn);
    }

    public static async Task OnBookAdded(BookModel book)
    {
        BookAdded.Invoke(book);
    }
}