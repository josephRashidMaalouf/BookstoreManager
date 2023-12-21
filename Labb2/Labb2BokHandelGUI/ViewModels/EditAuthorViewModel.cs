using System.Collections.ObjectModel;
using Common.Managers;
using Common.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb2DataAcess.Entities;
using Labb2DataAcess.Services;

namespace Labb2BokHandelGUI.ViewModels;

public class EditAuthorViewModel : ObservableObject
{
    private readonly AuthorRepository _authorRepository;

    private string? _editFirstName;
    private string? _editLastName;
    private string? _editDate;
    private AuthorModel? _selectedAuthor;
    private ObservableCollection<AuthorModel> _authors;

    #region Props

    public string? EditFirstName
    {
        get => _editFirstName;
        set
        {
            if (value == _editFirstName) return;
            _editFirstName = value;
            OnPropertyChanged();

            AddCommand.NotifyCanExecuteChanged();
            UpdateCommand.NotifyCanExecuteChanged();
        }
    }

    public string? EditLastName
    {
        get => _editLastName;
        set
        {
            if (value == _editLastName) return;
            _editLastName = value;
            OnPropertyChanged();

            AddCommand.NotifyCanExecuteChanged();
            UpdateCommand.NotifyCanExecuteChanged();
        }
    }

    public string? EditDate
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

    public AuthorModel? SelectedAuthor
    {
        get => _selectedAuthor;
        set
        {
            if (Equals(value, _selectedAuthor)) return;
            _selectedAuthor = value;
            OnPropertyChanged();

            if (SelectedAuthor != null)
            {
                EditFirstName = SelectedAuthor.FirstName;
                EditLastName = SelectedAuthor.LastName;
                EditDate = SelectedAuthor.Birthday.ToString();
            }

            AddCommand.NotifyCanExecuteChanged();
            UpdateCommand.NotifyCanExecuteChanged();
            DeleteCommand.NotifyCanExecuteChanged();
        }
    }

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


    #endregion

    #region Commands

    public IRelayCommand AddCommand { get; }
    public IRelayCommand UpdateCommand { get; }
    public IRelayCommand DeleteCommand { get; }


    #endregion

    public EditAuthorViewModel(AuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;

        Authors = new ObservableCollection<AuthorModel>(_authorRepository.GetAllAuthors());

        UpdateCommand = new RelayCommand(UpdateCommandExecute, UpdateCommandCanExecute);
        AddCommand = new RelayCommand(AddCommandExecute, AddCommandCanExecute);
        DeleteCommand = new RelayCommand(DeleteCommandExecute, DeleteCommandCanExecute);
    }

    private bool DeleteCommandCanExecute()
    {
        return SelectedAuthor != null;
    }

    private bool AddCommandCanExecute()
    {
        if (EditFirstName != null && EditLastName != null && DateOnly.TryParse(EditDate, out var date))
        {
            return true;
        }

        return false;
    }

    private bool UpdateCommandCanExecute()
    {
        if (EditFirstName != null 
            && EditLastName != null 
            && DateOnly.TryParse(EditDate, out var date) 
            && SelectedAuthor != null)
        {
            return true;
        }

        return false;
    }

    private void DeleteCommandExecute()
    {
        _authorRepository.DeleteAuthor(SelectedAuthor);
        AuthorManager.OnAuthorRemoved(SelectedAuthor);
        Authors.Remove(SelectedAuthor);
        
    }

    private void AddCommandExecute()
    {
        var author = new AuthorModel()
        {
            FirstName = EditFirstName,
            LastName = EditLastName,
            Birthday = DateOnly.Parse(EditDate)
        };

        _authorRepository.AddAuthor(author);
        AuthorManager.OnAuthorAdded(author);
        Authors.Add(author);
    }

    private void UpdateCommandExecute()
    {
        SelectedAuthor.FirstName = EditFirstName;
        SelectedAuthor.LastName = EditLastName;
        SelectedAuthor.Birthday = DateOnly.Parse(EditDate);

        _authorRepository.UpdateAuthor(SelectedAuthor);
        AuthorManager.OnAuthorUpdated(SelectedAuthor);
    }
}