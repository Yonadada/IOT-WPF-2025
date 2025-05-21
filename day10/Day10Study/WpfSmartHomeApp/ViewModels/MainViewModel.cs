using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MQTTnet;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Threading;
using WpfMqttSubApp.Helpers;
using WpfSmartHomeApp.Models;

// 스마트홈 앱
namespace WpfSmartHomeApp.ViewModels
{

    public partial class MainViewModel : ObservableObject, IDisposable
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

        private string? _currDateTime;

        //readonly는 생성자에서만 값을 할당가능하고 그 외는 불가!
        private readonly DispatcherTimer _timer;

        //로그용 타이머
        private readonly DispatcherTimer _logTimer;

        //MQTT 관련 변수들
        private string TOPIC;
        private IMqttClient mqttClient;
        private string BROKERHOST;


        // 생성자
        public MainViewModel()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (sender, e) =>
            {
                CurrDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            };
            _timer.Start();
        }


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

        // 시간 
        public string? CurrDateTime
        {
            get => _currDateTime;
            set => SetProperty(ref _currDateTime, value);
        }

        // LoadedCommand에서 앞에 On을 붙여서 사용, Command 는 삭제
        [RelayCommand]
        public async Task OnLoaded()
        {
            // 초기화 작업
            // 예를 들어, 데이터베이스 연결, API 호출 등

            // 테스트로 집어넣은 가짜 데이터들!
            /*
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
            */

            // MQTT 연결부터 실행까지
            TOPIC = "pknu/sh01/data"; // publish, subscribe 동시에 사용!
            BROKERHOST = "210.119.12.52"; // SmartHome MQTT Broker Ip주소 -> 강사용 

            var mqttFactory = new MqttClientFactory();
            mqttClient = mqttFactory.CreateMqttClient();

            // MQTT 클라이언트 접속 설정변수 만들기
            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(BROKERHOST)
                .WithCleanSession(true)
                .Build();

            // MQTT 접속 확인 이벤트 메서드 선언
            mqttClient.ConnectedAsync += MqttClient_ConnectedAsync;

            // MQTT 구독메시지 확인 메서드 선언
            mqttClient.ApplicationMessageReceivedAsync += MqttClient_ApplicationMessageReceivedAsync;

            await mqttClient.ConnectAsync(mqttClientOptions); // MQTT 브로커에 연결

        }

        // MQTT 구독메세지 확인 이벤트 메서드
        private Task MqttClient_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            var topic = arg.ApplicationMessage.Topic; // pknu/sh01/data
            var payload = arg.ApplicationMessage.ConvertPayloadToString(); // byte -> UTF-8 문자열로 변환

            var data = JsonConvert.DeserializeObject<SensingInfo>(payload);

            //Common.LOGGER.Info($"Light : {data.L}\n Rain : {data.R}\n Temp : {data.T}\n " +
            //    $"Humid : {data.H}\n Fan : {data.F}\n  Vulernability : {data.V}\n " +
            //    $" RealLight : {data.RL}\n ChaimBell : {data.CB}");
            
            HomeTemp = data.T; // 온도
            HomeHumid = data.H; // 습도

            // 사람 인지
            IsDetectOn = data.V == "ON" ? true : false; 
            DetectResult = data.V == "ON" ? "Detection State" : "Normal State";

            // 조명
            IsLightOn = data.RL == "ON" ? true : false; 
            LightResult = data.RL == "ON" ? "Light On!" : "Light Off!";

            // 에어컨
            IsAirConOn = data.F == "ON" ? true : false;
            AirConResult = data.F == "ON" ? "Aircon On!" : "Aircon Off!";

            // 비
            IsRainOn = data.R <= 350 ? true : false; 
            RainResult = data.R <= 350 ? "Raining" : "No Raining";


            return Task.CompletedTask; // 구독이 종료됨을 알려주는 리턴문
        }

        // MQTT 접속 확인 이벤트 메서드
        private async Task MqttClient_ConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            Common.LOGGER.Info($"{arg}");
            Common.LOGGER.Info("MQTT Broker 접속 성공 >< ");

            // 연결 이후 Subscribe 구독 시작
            await mqttClient.SubscribeAsync(TOPIC);
        }

        public void Dispose()
        {
            //TODO : 나중에 리소스 해제 처리 필요!!!!
        }
    }
}
