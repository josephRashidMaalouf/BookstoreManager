using Common.Models;

namespace Common.Managers;

public class AuthorManager
{
    public static event Action<AuthorModel> AuthorAdded;

    public static event Action<AuthorModel> AuthorRemoved;

    public static event Action<AuthorModel> AuthorUpdated;

    public static async Task OnAuthorAdded(AuthorModel author)
    {
        AuthorAdded.Invoke(author);
    }

    public static async Task OnAuthorRemoved(AuthorModel author)
    {
        AuthorRemoved.Invoke(author);
    }

    public static async Task OnAuthorUpdated(AuthorModel author)
    {
        AuthorUpdated.Invoke(author);
    }

}