using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace WpfSmartHomeApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private double _homeTemp;
        private double _homeHumid;

        private string _detectResult;
        private bool _isDetectOn;
        
        private string _rainResult;
        private bool _isRainOn;
        
        private string _airConResult;
        private bool _isAirConOn;
        
        private string _LightResult;
        private bool _isLightOn;

        //온도 속성
        public double HomeTemp
        {
            get => _homeTemp;
            set => SetProperty(ref _homeTemp, value);

        }


        //습도 속성
        public double HomeHumid
        {
            get => _homeHumid; 
            set => SetProperty(ref _homeHumid, value); 
        }

        // 사람 인지
         public string DetectResult
        {
            get => _detectResult;
            set => SetProperty(ref _detectResult, value);
        }

        // 사람인지 여부
        public bool IsDetectOn
        {
            get => _isDetectOn;
            set => SetProperty(ref _isDetectOn, value);
        }

        // 비가 오는지 여부
        public string RainResult
        {
            get => _rainResult;
            set => SetProperty(ref _rainResult, value);
        }

        //비 인지
        public bool IsRainOn
        {
            get => _isRainOn;
            set => SetProperty(ref _isRainOn, value);
        }

        // 에어컨 
        public string AirConResult
        {
            get => _airConResult;
            set => SetProperty(ref _airConResult, value);
        }

        // 에어컨 
        public bool IsAirConOn
        {
            get => _isAirConOn;
            set => SetProperty(ref _isAirConOn, value);
        }

        // 조명
        public string LightResult
        {
            get => _LightResult;
            set => SetProperty(ref _LightResult, value);
        }

        // 조명
        public bool IsLightOn
        {
            get => _isLightOn;
            set => SetProperty(ref _isLightOn, value);
        }

        [RelayCommand]
        public void OnLoaded()
        {
            // 초기화 작업
            // 예를 들어, 데이터베이스 연결, API 호출 등
            HomeTemp = 27;
            HomeHumid = 50.43;

            DetectResult = "Detected Human";
            IsDetectOn = true;

            RainResult = "Raining";
            IsRainOn = true;

            AirConResult = "Aircon On!";
            IsAirConOn = true;

            LightResult = "Light On!";
            IsLightOn = true;
        }
    }
}
