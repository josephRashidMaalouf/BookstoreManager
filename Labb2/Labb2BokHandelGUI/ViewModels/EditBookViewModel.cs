using System.Collections.ObjectModel;
using System.Windows;
using Common.Managers;
using Common.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb2DataAcess.Entities;
using Labb2DataAcess.Services;

namespace Labb2BokHandelGUI.ViewModels;

public class EditBookViewModel : ObservableObject
{
    private readonly StoreRepository _storeRepository;
    private readonly BookRepository _bookRepository;
    private readonly InventoryRepository _inventoryRepository;
    private readonly AuthorRepository _authorRepository;


    private AuthorModel? _selectedAuthorBookEdit;
    private AuthorModel? _selectedCoAuthor;
    private BookModel _selectedEditBook;
    private string _editIsbn;
    private string _editTitle;
    private string _editPrice;
    private string _editLanguage;
    private string _editDate;
    private ObservableCollection<AuthorModel> _authors;
    private ObservableCollection<BookModel> _books;
    private AuthorModel _coAuthor;


    #region PropsBook


    public ObservableCollection<AuthorModel> Authors
    {
        get => _authors;
        set
        {
            if (Equals(value, _authors)) return;
            _authors = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<BookModel> Books
    {
        get => _books;
        set
        {
            if (Equals(value, _books)) return;
            _books = value;
            OnPropertyChanged();
        }
    }

    public AuthorModel? SelectedAuthorBookEdit
    {
        get => _selectedAuthorBookEdit;
        set
        {
            if (Equals(value, _selectedAuthorBookEdit)) return;
            _selectedAuthorBookEdit = value;
            OnPropertyChanged();

        }
    }

    public AuthorModel? SelectedCoAuthor
    {
        get => _selectedCoAuthor;
        set
        {
            if (Equals(value, _selectedCoAuthor)) return;
            _selectedCoAuthor = value;
            OnPropertyChanged();
        }
    }

    public BookModel? SelectedEditBook
    {
        get => _selectedEditBook;
        set
        {
            if (Equals(value, _selectedEditBook)) return;
            _selectedEditBook = value;
            OnPropertyChanged();

            if (SelectedEditBook != null)
            {
                if (SelectedEditBook.Isbn13 != null)
                    EditIsbn = SelectedEditBook.Isbn13;

                if (SelectedEditBook.Title != null)
                    EditTitle = SelectedEditBook.Title;

                if (SelectedEditBook.Price != null)
                    EditPrice = SelectedEditBook.Price.ToString();

                if (SelectedEditBook.Language != null)
                    EditLanguage = SelectedEditBook.Language;

                if (SelectedEditBook.PublishingDate != null)
                    EditDate = SelectedEditBook.PublishingDate.ToString();
            }

            UpdateCommand.NotifyCanExecuteChanged();
            DeleteCommand.NotifyCanExecuteChanged();
        }
    }

    public string EditIsbn
    {
        get => _editIsbn;
        set
        {
            if (value == _editIsbn) return;
            _editIsbn = value;
            OnPropertyChanged();
            AddCommand.NotifyCanExecuteChanged();
            UpdateCommand.NotifyCanExecuteChanged();
        }
    }

    public string EditTitle
    {
        get => _editTitle;
        set
        {
            if (value == _editTitle) return;
            _editTitle = value;
            OnPropertyChanged();
        }
    }

    public string EditPrice
    {
        get => _editPrice;
        set
        {
            if (value == _editPrice) return;
            _editPrice = value;
            OnPropertyChanged();
            AddCommand.NotifyCanExecuteChanged();
            UpdateCommand.NotifyCanExecuteChanged();
        }
    }

    public string EditLanguage
    {
        get => _editLanguage;
        set
        {
            if (value == _editLanguage) return;
            _editLanguage = value;
            OnPropertyChanged();
        }
    }

    public string EditDate
    {
        get => _editDate;
        set
        {
            if (value == _editDate) return;
            _editDate = value;

            OnPropertyChanged();
            AddCommand.NotifyCanExecuteChanged();
            UpdateCommand.NotifyCanExecuteChanged();
        }
    }

    #endregion


    #region Commands

    public IRelayCommand UpdateCommand { get; }
    public IRelayCommand AddCommand { get; }
    public IRelayCommand DeleteCommand { get; }

    #endregion

    public EditBookViewModel(StoreRepository storeRepository, BookRepository bookRepository, InventoryRepository inventoryRepository, AuthorRepository authorRepository)
    {
        _storeRepository = storeRepository;
        _bookRepository = bookRepository;
        _inventoryRepository = inventoryRepository;
        _authorRepository = authorRepository;

        Authors = new ObservableCollection<AuthorModel>(_authorRepository.GetAllAuthors());
        Authors.Add(new AuthorModel() { FirstName = "(Blank)" });

        Books = _bookRepository.GetAllBooks();

        UpdateCommand = new RelayCommand(UpdateCommandExecute, UpdateCommandCanExecute);
        AddCommand = new RelayCommand(AddCommandExecute, AddCommandCanExecute);
        DeleteCommand = new RelayCommand(DeleteCommandExecute, DeleteCommandCanExecute);

        //implement these
        AuthorManager.AuthorUpdated += UpdateAuthor;
        AuthorManager.AuthorRemoved += RemoveAuthor;
        AuthorManager.AuthorAdded += AddAuthor;
        


    }

    private void AddAuthor(AuthorModel obj)
    {
        Authors.Add(obj);
    }

    private void RemoveAuthor(AuthorModel obj)
    {
        var author = Authors.First(a => a.Id == obj.Id);

        Authors.Remove(author);
    }

    private void UpdateAuthor(AuthorModel obj)
    {
        var author = Authors.First(a => a.Id == obj.Id);

        author.FirstName = obj.FirstName;
        author.LastName = obj.LastName;
        author.Birthday = obj.Birthday;

        //updates combobox
        var authorArray = Authors.ToArray();
        Authors.Clear();
        Authors = new ObservableCollection<AuthorModel>(authorArray);

        //update listView
        Books.Clear();
        Books = new ObservableCollection<BookModel>(_bookRepository.GetAllBooks());

    }


    private bool DeleteCommandCanExecute()
    {
        if (SelectedEditBook != null)
            return true;

        return false;
    }

    private bool AddCommandCanExecute() //ISBN must be valid, and not in db to add new book
    {
        if (EditIsbn == null)
        {
            return false;
        }

        if (VerifyISBN(EditIsbn) == false)
        {
            return false;
        }

        if (Books.Any(b => b.Isbn13 == EditIsbn))
        {
            return false;
        }

        if (double.TryParse(EditPrice, out var price) == false)
        {
            return false;
        }

        if (DateOnly.TryParse(EditDate, out var date) == false)
        {
            return false;
        }


        return true;
    }

    private bool UpdateCommandCanExecute()
    {
        if (SelectedEditBook == null)
        {
            return false;
        }

        if (VerifyISBN(EditIsbn) == false)
        {
            return false;
        }

        if (Books.Any(b => b.Isbn13 == EditIsbn))
        {
            if (SelectedEditBook.Isbn13 != EditIsbn)
                return false;
        }

        if (double.TryParse(EditPrice, out var price) == false)
        {
            return false;
        }

        if (DateOnly.TryParse(EditDate, out var date) == false)
        {
            return false;
        }

        return true;
    }

    private void DeleteCommandExecute()
    {

        _bookRepository.DeleteBook(SelectedEditBook);
        BookManager.OnBookDeleted(SelectedEditBook);
        Books.Remove(SelectedEditBook);

    }

    private void AddCommandExecute()
    {
        var newBook = new BookModel()
        {
            Isbn13 = EditIsbn,
            Title = EditTitle,
            Price = double.Parse(EditPrice),
            Language = EditLanguage,
            PublishingDate = DateOnly.Parse(EditDate)
        };
        if (SelectedAuthorBookEdit != null && SelectedAuthorBookEdit.FirstName != "(Blank)")
        {
            newBook.Authors.Add(SelectedAuthorBookEdit);
        }


        if (SelectedCoAuthor != null && SelectedCoAuthor.FirstName != "(Blank)")
        {
            newBook.Authors.Add(SelectedCoAuthor);
        }

        _bookRepository.AddBook(newBook);
        Books.Add(newBook);
        BookManager.OnBookAdded(newBook);

        ClearTextFields();
    }

    private void UpdateCommandExecute()
    {
        var updatedAuthorsList = new List<AuthorModel>();

        bool authorSelected = (SelectedAuthorBookEdit != null && SelectedAuthorBookEdit.FirstName != "(Blank)");
        bool coAuthorSelected = (SelectedCoAuthor != null && SelectedCoAuthor.FirstName != "(Blank)");

        if (authorSelected && coAuthorSelected)
        {
            updatedAuthorsList.AddRange(new AuthorModel[] { SelectedAuthorBookEdit, SelectedCoAuthor });

        }
        else
        {
            if (authorSelected)
                updatedAuthorsList.Add(SelectedAuthorBookEdit);
            if (coAuthorSelected)
                updatedAuthorsList.Add(SelectedCoAuthor);
        }




        string oldIsbn = SelectedEditBook.Isbn13;

        SelectedEditBook.Isbn13 = EditIsbn;
        SelectedEditBook.Title = EditTitle;
        SelectedEditBook.Price = double.Parse(EditPrice);
        SelectedEditBook.Language = EditLanguage;
        SelectedEditBook.PublishingDate = DateOnly.Parse(EditDate);
        SelectedEditBook.Authors = new ObservableCollection<AuthorModel>(updatedAuthorsList);
        SelectedEditBook.AuthorsString = SelectedEditBook.AuthorsString;

        _bookRepository.UpdateBook(SelectedEditBook);
        BookManager.OnBookUpdated(SelectedEditBook, oldIsbn);


    }

    private bool VerifyISBN(string? isbn)
    {
        if (EditIsbn == null)
        {
            return false;
        }

        if (int.TryParse(EditIsbn, out var result))
        {
            return false;
        }

        if (EditIsbn.Length != 13)
        {
            return false;
        }

        return true;
    }

    private void ClearTextFields()
    {
        EditIsbn = string.Empty;
        EditTitle = string.Empty;
        EditPrice = string.Empty;
        EditLanguage = string.Empty;
        EditDate = string.Empty;
    }
}