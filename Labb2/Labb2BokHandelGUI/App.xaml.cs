using System.Configuration;
using System.Data;
using System.Windows;
using Labb2BokHandelGUI.ViewModels;
using Labb2DataAcess.Entities;
using Labb2DataAcess.Services;

namespace Labb2BokHandelGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var dbContext = new Labb1Bokhandel2Context();
            var storeRepository = new StoreRepository(dbContext);
            var bookRepository = new BookRepository(dbContext);
            var invRepository = new InventoryRepository(dbContext);
            var authorRepository = new AuthorRepository(dbContext);

            var storeViewModel = new StoresViewModel(storeRepository, bookRepository, invRepository);

            var editBookViewModel = new EditBookViewModel(storeRepository, bookRepository, invRepository, authorRepository);
            var editAuthorViewModel = new EditAuthorViewModel(authorRepository);
            var editViewModel = new EditViewModel(editBookViewModel, editAuthorViewModel);

            var mainViewModel = new MainViewModel(editViewModel, storeViewModel);

            var mainWindow = new MainWindow()
            {
                DataContext = mainViewModel
            };

            mainWindow.Show();
        }
    }

}
