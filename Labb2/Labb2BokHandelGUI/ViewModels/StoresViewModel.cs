using System.Collections.ObjectModel;
using Common.Managers;
using Common.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb2DataAcess.Entities;
using Labb2DataAcess.Services;

namespace Labb2BokHandelGUI.ViewModels;

public class StoresViewModel : ObservableObject
{
    private readonly StoreRepository _storeRepository;
    private readonly BookRepository _bookRepository;
    private readonly InventoryRepository _inventoryRepository;


    private StoreModel _selectedStore;
    private BookModel _selectedBook;
    private string _editQuantity = "0";
    private ObservableCollection<StoreModel> _stores;
    private BookModel _selectedAddNewBook;
    private ObservableCollection<BookModel> _allBooks;
    private ObservableCollection<BookModel> _selectedStoreBooks = new ObservableCollection<BookModel>();

    #region Props

    public ObservableCollection<BookModel>? SelectedStoreBooks
    {
        get => _selectedStoreBooks;
        set
        {
            if (Equals(value, _selectedStoreBooks)) return;
            _selectedStoreBooks = value;
            OnPropertyChanged();
        }
    }

    public StoreModel SelectedStore
    {
        get => _selectedStore;
        set
        {
            if (Equals(value, _selectedStore)) return;
            _selectedStore = value;
            OnPropertyChanged();

            
            SelectedStoreBooks.Clear();
              var books =  _bookRepository.GetAllBooksByStore(value);

              foreach (var bookModel in books)
              {
                  SelectedStoreBooks.Add(bookModel);
              }

            foreach (var book in SelectedStoreBooks)
            {
                book.AmountInStore = _inventoryRepository.GetAmountOfBookInStore(value, book.Isbn13);
                int i = 0;
                
            }

            AddBookCommand.NotifyCanExecuteChanged();
        }
    }

    public BookModel SelectedBook
    {
        get => _selectedBook;
        set
        {
            if (Equals(value, _selectedBook)) return;
            _selectedBook = value;
            IncreaseQuantityCommand.NotifyCanExecuteChanged();
            DecreaseQuantityCommand.NotifyCanExecuteChanged();
            DeleteBookCommand.NotifyCanExecuteChanged();
            OnPropertyChanged();
        }
    }

    public BookModel SelectedAddNewBook
    {
        get => _selectedAddNewBook;
        set
        {
            if (Equals(value, _selectedAddNewBook)) return;
            _selectedAddNewBook = value;

            
            OnPropertyChanged();
            AddBookCommand.NotifyCanExecuteChanged();
        }
    }

    public string EditQuantity
    {
        get => _editQuantity;
        set
        {
            if (value == _editQuantity) return;
            _editQuantity = value;
            OnPropertyChanged();

            IncreaseQuantityCommand.NotifyCanExecuteChanged();
            DecreaseQuantityCommand.NotifyCanExecuteChanged();
        }
    }

    public ObservableCollection<StoreModel> Stores
    {
        get => _stores;
        set
        {
            if (Equals(value, _stores)) return;
            _stores = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<BookModel> AllBooks
    {
        get => _allBooks;
        set
        {
            if (Equals(value, _allBooks)) return;
            _allBooks = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Commands

    public IRelayCommand IncreaseQuantityCommand { get; }
    public IRelayCommand DecreaseQuantityCommand { get; }
    public IRelayCommand AddBookCommand { get; }
    public IRelayCommand DeleteBookCommand { get; }

    #endregion

    public StoresViewModel(StoreRepository storeRepo, BookRepository bookRepo, InventoryRepository inventoryRepo)
    {
        _storeRepository = storeRepo;
        _bookRepository = bookRepo;
        _inventoryRepository = inventoryRepo;

        Stores = new ObservableCollection<StoreModel>(_storeRepository.GetAllStores());
        AllBooks = _bookRepository.GetAllBooks(); 

        IncreaseQuantityCommand = new RelayCommand(IncreaseQExecute, UpdateBookQuantityCanExecute);
        DecreaseQuantityCommand = new RelayCommand(DecreaseQExecute, UpdateBookQuantityCanExecute);
        AddBookCommand = new RelayCommand(AddBookCommandExecute, AddBookCommandCanExecute);
        DeleteBookCommand = new RelayCommand(DeleteBookCommandExecute, DeleteBookCommandCanExecute);

        BookManager.BookAdded += AddNewBook;
        BookManager.BookUpdated += UpdateBook;
        BookManager.BookDeleted += DeleteBook;

       

    }


    private void DeleteBook(BookModel obj)
    {
        var book = AllBooks.First(b => b.Isbn13 == obj.Isbn13);

        //if (SelectedStoreBooks != null && SelectedStoreBooks.Contains(book))
        //{
        //    SelectedStoreBooks.Remove(book);
        //}
        if (SelectedStore != null)
            SelectedStoreBooks = new ObservableCollection<BookModel>(_bookRepository.GetAllBooksByStore(SelectedStore));

        AllBooks.Remove(book);

        
    }

    private void UpdateBook(BookModel book, string oldisbn)
    {
        var bookToUpdate = AllBooks.First(b => b.Isbn13 == oldisbn);

        bookToUpdate.Isbn13 = book.Isbn13;
        bookToUpdate.Title = book.Title;
        bookToUpdate.Price = book.Price;
        bookToUpdate.Language = book.Language;
        bookToUpdate.PublishingDate = book.PublishingDate;
        bookToUpdate.AuthorsString = book.AuthorsString;

        bookToUpdate.Authors.Clear();
        foreach (var author in book.Authors)
        {
            bookToUpdate.Authors.Add(author);
        }
    }

    private void AddNewBook(BookModel book)
    {
        AllBooks.Add(book);
    }

    private bool UpdateBookQuantityCanExecute()
    {
        if (int.TryParse(EditQuantity, out var result))
        {
            if (SelectedBook != null)
            {
                return true;
            }
            
        }
        return false;
    }

    private bool DeleteBookCommandCanExecute()
    {
        if (SelectedBook == null)
        {
            return false;
        }

        if (SelectedStore == null)
        {
            return false;
        }

        return true;
    }

    private bool AddBookCommandCanExecute()
    {

        if (SelectedAddNewBook == null)
        {
            return false;
        }

        if (SelectedStore == null)
        {
            return false;
        }

        var ibs = _inventoryRepository.GetAllIbs();

        if(ibs.Any(i => (i.Isbn13 == SelectedAddNewBook.Isbn13 && i.StoreId == SelectedStore.Id))) //if the book already exists in store
        {
            return false;
        }
        
        return true;
    }


    private void DeleteBookCommandExecute()
    {
        _inventoryRepository.DeleteBookFromStore(SelectedStore, SelectedBook.Isbn13);
        SelectedStoreBooks.Remove(SelectedBook);
        SelectedBook = null;
    }

    private void AddBookCommandExecute()
    {
        _inventoryRepository.AddBookToStore(SelectedStore, SelectedAddNewBook.Isbn13);
        SelectedStoreBooks.Add(SelectedAddNewBook);

        AddBookCommand.NotifyCanExecuteChanged();
    }

    private void DecreaseQExecute()
    {
        if (int.TryParse(EditQuantity, out var result))
        {

            string book = SelectedBook.Isbn13;

            if (SelectedBook.AmountInStore - result <= 0)
            {
                _inventoryRepository.UpdateBookQuantityForStore(SelectedStore, SelectedBook.Isbn13, (result * -1));
                SelectedBook.AmountInStore = 0;
                return;
            }

            _inventoryRepository.UpdateBookQuantityForStore(SelectedStore, SelectedBook.Isbn13, (result * -1));
            SelectedBook.AmountInStore += (result * -1);
        };
    }

    private void IncreaseQExecute()
    {
        if (int.TryParse(EditQuantity, out var result))
        {

            string book = SelectedBook.Isbn13;

            _inventoryRepository.UpdateBookQuantityForStore(SelectedStore, SelectedBook.Isbn13, result);
            SelectedBook.AmountInStore += result;
        };
        
    }
}