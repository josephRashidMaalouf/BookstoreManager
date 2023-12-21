using System.Collections.ObjectModel;
using Common.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Labb2DataAcess.Services;

namespace Labb2BokHandelGUI.ViewModels;

public class EditViewModel : ObservableObject
{
    public EditBookViewModel EditBookViewModel { get; set; }

    public EditAuthorViewModel EditAuthorViewModel { get; set; }

    public EditViewModel(EditBookViewModel editBookViewModel, EditAuthorViewModel editAuthorViewModel)
    {
        EditBookViewModel = editBookViewModel;
        EditAuthorViewModel = editAuthorViewModel;
    }
}