using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using WpfMqttSubApp.Helpers;
using WpfMqttSubApp.ViewModels;
using WpfMqttSubApp.Views;

namespace WpfMqttSubApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        var coordinator = DialogCoordinator.Instance;
        var viewModel = new MainViewModel(coordinator);
        var view = new MainView
        {
            DataContext = viewModel
        };
        view.ShowDialog();
        Common.LOGGER.Info("스마트홈 모니터링 앱");
    }

    
}

