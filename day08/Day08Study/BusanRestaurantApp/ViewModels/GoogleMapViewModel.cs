
using BusanRestaurantApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using MahApps.Metro.Controls;

namespace BusanRestaurantApp.ViewModels
{
    public class GoogleMapViewModel : ObservableObject
    {
        private BusanItem _selectedMatjibItem;
        private string _matjibLocation;

        // 처음실행시 구글맵에 보여줄 위치
        public GoogleMapViewModel()
        {
            MatjibLocation = "";

        }

        public BusanItem SelectedMatjibItem
        {
            get => _selectedMatjibItem;

            set
            {
                SetProperty(ref _selectedMatjibItem, value);
                //(위도 Latitude, 경도Longitude)
                MatjibLocation = $"https://google.com/maps/place/{SelectedMatjibItem.Lat},{SelectedMatjibItem.Lng}";
            }
        }

        public string MatjibLocation
        {
            get => _matjibLocation;
            set => SetProperty(ref _matjibLocation, value);
        }


    }
}
