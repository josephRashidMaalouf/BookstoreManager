
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;


namespace Common.Models;

public class AuthorModel : ObservableObject
{
    private string? _firstName;
    private string? _lastName;
    private DateOnly? _birthday;
    public int Id { get; set; }

    public string? FirstName
    {
        get => _firstName;
        set
        {
            if (value == _firstName) return;
            _firstName = value;
            OnPropertyChanged();
        }
    }

    public string? LastName
    {
        get => _lastName;
        set
        {
            if (value == _lastName) return;
            _lastName = value;
            OnPropertyChanged();
        }
    }

    public DateOnly? Birthday
    {
        get => _birthday;
        set
        {
            if (Nullable.Equals(value, _birthday)) return;
            _birthday = value;
            OnPropertyChanged();
        }
    }

    public virtual ObservableCollection<BookModel> Isbn13s { get; set; } = new ();

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}