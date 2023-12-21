using CommunityToolkit.Mvvm.ComponentModel;

namespace Labb2BokHandelGUI.ViewModels;

public class MainViewModel 
{
    public EditViewModel EditViewModel { get; set; }
    public StoresViewModel StoresViewModel { get; set; }
    

    public MainViewModel(EditViewModel editViewModel, StoresViewModel storesViewModel)
    {
        EditViewModel = editViewModel;
        StoresViewModel = storesViewModel;

    }
}