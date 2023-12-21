using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Common.Models;

public class BookModel : ObservableObject
{
    private double? _price = 0;
    private string? _title = "Untitled";
    private string _isbn13 = null!;
    private string? _language = "unknown";
    private DateOnly? _publishingDate = new DateOnly(1900, 01, 01);
    private int _amountInStore = 1;
    private string _authorsString;

    public string Isbn13
    {
        get => _isbn13;
        set
        {
            if (value == _isbn13) return;
            _isbn13 = value;
            OnPropertyChanged();
        }
    }

    public string? Title
    {
        get => _title;
        set
        {
            if (value == _title) return;
            _title = value;
            OnPropertyChanged();
        }
    }

    public string? Language
    {
        get => _language;
        set
        {
            if (value == _language) return;
            _language = value;
            OnPropertyChanged();
        }
    }

    public double? Price
    {
        get => _price;
        set
        {
            if (Nullable.Equals(value, _price)) return;
            _price = value;
            OnPropertyChanged();
        }
    }

    public DateOnly? PublishingDate
    {
        get => _publishingDate;
        set
        {
            if (Nullable.Equals(value, _publishingDate)) return;
            _publishingDate = value;
            OnPropertyChanged();
        }
    }

    public string AuthorsString
    {
        get
        {
            string output = string.Empty;

            int i = 0;

            foreach (var author in Authors)
            {
                if (i == Authors.Count - 1)
                {
                    output += $"{author.FirstName} {author.LastName}";
                    return output;
                }
                output += $"{author.FirstName} {author.LastName}, ";
                i++;
            }

            AuthorsString = output;
            return output;
        }
        set
        {
            if (value == _authorsString) return;
            _authorsString = value;
            OnPropertyChanged();
        }
    }

    public int AmountInStore
    {
        get => _amountInStore;
        set
        {
            if (value == _amountInStore) return;
            _amountInStore = value;
            OnPropertyChanged();
        }
    }

    public virtual ObservableCollection<InventoryBalanceModel> IventoryBalances { get; set; } = new ();

    public virtual ObservableCollection<AuthorModel> Authors { get; set; } = new ();

    public override string ToString() // kanske ta bort?
    {

        string output = $"{Title} - ";

        int i = 0;

        foreach (var author in Authors)
        {
            if (i == Authors.Count - 1)
            {
                output += $"{author.FirstName} {author.LastName}";
                return output;
            }
            output += $"{author.FirstName} {author.LastName}, ";
            i++;
        }

        return output;
    }
}