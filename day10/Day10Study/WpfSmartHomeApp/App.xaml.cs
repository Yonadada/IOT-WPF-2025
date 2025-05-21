﻿using System.Configuration;
using System.Data;
using System.Windows;
using WpfMqttSubApp.Helpers;
using WpfSmartHomeApp.ViewModels;
using WpfSmartHomeApp.Views;

namespace WpfSmartHomeApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Common.LOGGER.Info("---스마트홈 모니터링 앱 실행---");
            var viewModel = new MainViewModel();
            var view = new MainWindow
            {
                DataContext = viewModel
            };
            view.ShowDialog();
        }
    }

}
