using System.Windows;
using WpfBookRentalShoop01.Views;
using WpfBookRentalShop01.ViewModels;

namespace WpfBookRentalShop01
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var viewModel = new MainViewModel();
            var view = new MainView()
            {
                DataContext = viewModel
            };
            //var view = new MainView();
            //view.DataContext = viewModel;
            view.ShowDialog();
        }
    }
}
