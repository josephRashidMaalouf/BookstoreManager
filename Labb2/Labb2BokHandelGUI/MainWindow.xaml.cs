using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Labb2BokHandelGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void GoBackBtn_OnClick(object sender, RoutedEventArgs e)
        {
            EditView.Visibility = Visibility.Hidden;
            StoreView.Visibility = Visibility.Visible;
            GoToEditBtn.Visibility = Visibility.Visible;
            GoBackBtn.Visibility = Visibility.Hidden;
        }

        private void GoToEditBtn_OnClick(object sender, RoutedEventArgs e)
        {
            EditView.Visibility = Visibility.Visible;
            StoreView.Visibility = Visibility.Hidden;
            GoToEditBtn.Visibility = Visibility.Hidden;
            GoBackBtn.Visibility = Visibility.Visible;
        }
    }
}