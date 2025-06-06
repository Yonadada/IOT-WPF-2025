﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;
using WpfBookRentalShoop01.ViewModels;
using WpfBookRentalShoop01.Views;

namespace WpfBookRentalShop01.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private string _greeting;

        public string Greeting
        {
            get => _greeting;
            set => SetProperty(ref _greeting, value);
        }

        private string _currentStatus;

        public string CurrentStatus { 
            get => _currentStatus; 
            set => _currentStatus = value; }

        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public MainViewModel()
        {
            Greeting = "BookRentalShop!!";
        }
        #region '화면 기능(이벤트 처리)'

        [RelayCommand]
        public void AppExit()
        {
            MessageBox.Show("종료합니다.");
        }

        [RelayCommand]
        public void ShowBookGenre()
        {
            var vm = new BookGenreViewModel();
            var v = new BookGenreView
            {
                DataContext = vm,
            };
            CurrentView = v;
        }    

            [RelayCommand]
        public void ShowBooks()
        {
            var vm = new BooksViewModel();
            var v = new BooksView
            {
                DataContext = vm,
            };
            CurrentView = v;
        }
        #endregion
    }
}
