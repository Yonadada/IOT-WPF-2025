using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using MQTTnet;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Windows.Threading;
using WpfMqttSubApp.Models;

namespace WpfMqttSubApp.ViewModels
{
    public partial class MainViewModel : ObservableObject, IDisposable
    {
        // 필드
        private IMqttClient mqttClient;
        private readonly IDialogCoordinator dialogCoordinator;
        private readonly DispatcherTimer timer;
        private int LineCounter = 1; // TODO : 나중에 텍스트가 많아져서 느려지면 초기화시 사용

        private string connString = string.Empty;
        private MySqlConnection connection;

        private string _brokerHost;
        private string _databaseHost;
        private string _logText;

        // ⭐ 무한루프 방지 및 안정적인 재연결을 위한 제어 변수들
        private bool isReconnecting = false;           // 현재 재연결 시도 중인지 확인하는 플래그
        private int reconnectAttempts = 0;             // 현재까지 재연결 시도 횟수
        private const int MAX_RECONNECT_ATTEMPTS = 5;  // 최대 재연결 시도 횟수 제한
        private bool isConnecting = false;             // 현재 연결 시도 중인지 확인 (중복 연결 방지)

        //생성자
        public MainViewModel(IDialogCoordinator coordinator)
        {
            this.dialogCoordinator = coordinator;
            BrokerHost = "210.119.12.61";
            DatabaseHost = "210.119.12.61";

            connection = new MySqlConnection(); //예외처리용

            //-- RichTextBox 테스트용 --
            //timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick += (sender, e) =>
            //{
            //    // RichTextBox 추가내용
            //    LogText += $"Log[{DateTime.Now:HH:mm:ss}] - {counter++}\n";
            //    Debug.WriteLine($"Log[{DateTime.Now:HH:mm:ss}] - {counter++}"); // Debug용
            //};
            //timer.Start();
        }

        //속성
        public string LogText
        {
            get => _logText;
            set => SetProperty(ref _logText, value);
        }

        public string BrokerHost
        {
            get => _brokerHost;
            set => SetProperty(ref _brokerHost, value);
        }

        public string DatabaseHost
        {
            get => _databaseHost;
            set => SetProperty(ref _databaseHost, value);
        }

        // 연결 상태 표시용 속성
        [ObservableProperty]
        private bool isBrokerConnected;

        [ObservableProperty]
        private bool isDatabaseConnected;

        //private 메서드들
        private async Task ConnectMqttBroker()
        {
            // ⭐ 중복 연결 시도 방지
            if (isConnecting)
            {
                LogText += "⚠️ 이미 연결 시도 중입니다. 대기 중...\n";
                return;
            }

            try
            {
                isConnecting = true; // 연결 시도 시작

                // ⭐ 기존 MQTT 클라이언트 정리 - 중복 이벤트 핸들러 등록 방지
                if (mqttClient != null)
                {
                    try
                    {
                        // 기존 연결이 있다면 먼저 끊기
                        if (mqttClient.IsConnected)
                        {
                            await mqttClient.DisconnectAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        // DisconnectAsync에서 발생하는 예외는 로그만 기록하고 계속 진행
                        Debug.WriteLine($"기존 연결 해제 중 예외: {ex.Message}");
                    }

                    // 리소스 해제로 메모리 누수 방지
                    mqttClient.Dispose();
                    mqttClient = null; // 중요: null로 설정
                }

                // 잠시 대기 (기존 연결이 완전히 정리될 시간 확보)
                await Task.Delay(1000);

                //MQTT 클라이언트 생성
                var mqttFactory = new MqttClientFactory();
                mqttClient = mqttFactory.CreateMqttClient();

                // ⭐ 더 안정적인 MQTT 클라이언트 접속 설정
                var mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer(BrokerHost, 1883)
                    .WithCleanSession(true)
                    .WithTimeout(TimeSpan.FromSeconds(15)) // 타임아웃을 15초로 증가
                    .WithKeepAlivePeriod(TimeSpan.FromSeconds(30)) // Keep-alive 추가로 연결 유지 강화
                    .Build();

                // MQTT 접속 후 이벤트 처리
                mqttClient.ConnectedAsync += async e =>
                {
                    IsBrokerConnected = true;
                    // ⭐ 연결 성공 시 재연결 관련 상태 초기화
                    reconnectAttempts = 0;     // 재연결 시도 횟수 리셋
                    isReconnecting = false;    // 재연결 플래그 리셋
                    isConnecting = false;      // 연결 완료 플래그 해제
                    LogText += "✅ MQTT Broker에 성공적으로 연결되었습니다!\n";

                    try
                    {
                        await mqttClient.SubscribeAsync("smarthome/61/topic");
                        LogText += "✅ Topic 구독이 완료되었습니다!\n";
                    }
                    catch (Exception ex)
                    {
                        LogText += $"⚠️ Topic 구독 실패: {ex.Message}\n";
                    }
                };

                // ⭐ 개선된 연결 끊김 이벤트 처리 - 스마트한 재연결 로직 추가
                mqttClient.DisconnectedAsync += async e =>
                {
                    IsBrokerConnected = false;
                    isConnecting = false; // 연결 플래그 해제

                    // ⭐ 버전 호환성을 위한 안전한 연결 끊김 처리
                    LogText += $"❌ MQTT Broker 연결이 끊어졌습니다\n";

                    // 무한루프 방지: 이미 재연결 중이거나 최대 시도 횟수 초과 시 중단
                    if (!isReconnecting && reconnectAttempts < MAX_RECONNECT_ATTEMPTS)
                    {
                        await HandleMqttDisconnection(); // 재연결 시도
                    }
                    else if (reconnectAttempts >= MAX_RECONNECT_ATTEMPTS)
                    {
                        // 사용자에게 수동 연결 요청 안내
                        LogText += "❌ 최대 재연결 시도 횟수를 초과했습니다. 수동으로 연결하세요.\n";
                    }
                };

                mqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    var topic = e.ApplicationMessage.Topic;
                    var payload = e.ApplicationMessage.ConvertPayloadToString();

                    try
                    {
                        var data = JsonConvert.DeserializeObject<FakeInfo>(payload);
                        Debug.WriteLine($"{data.Count} / {data.Sensing_Dt} / {data.Humid} / {data.Human}");

                        SaveSensingData(data);

                        LogText += $"📡 데이터 수신: {LineCounter++}\n";
                        LogText += $"{payload}\n";
                    }
                    catch (JsonException ex)
                    {
                        // ⭐ JSON 파싱 오류 전용 예외 처리
                        LogText += $"⚠️ JSON 파싱 오류: {ex.Message}\n";
                    }

                    return Task.CompletedTask;
                };

                LogText += "🔄 MQTT Broker 연결을 시도하고 있습니다...\n";

                // ⭐ CancellationToken 사용으로 더 안전한 연결
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                await mqttClient.ConnectAsync(mqttClientOptions, cts.Token);

            }
            catch (OperationCanceledException)
            {
                // ⭐ Canceled 예외는 별도 처리 (덜 중요한 오류로 처리)
                IsBrokerConnected = false;
                LogText += "⏱️ MQTT 연결 시간 초과 또는 취소되었습니다\n";

                if (!isReconnecting && reconnectAttempts < MAX_RECONNECT_ATTEMPTS)
                {
                    await HandleMqttDisconnection();
                }
            }
            catch (Exception ex)
            {
                IsBrokerConnected = false;
                LogText += $"❌ MQTT Broker 접속 실패: {ex.Message}\n";

                // ⭐ 연결 실패 시에도 재연결 시도 (네트워크 일시적 문제 대응)
                if (!isReconnecting && reconnectAttempts < MAX_RECONNECT_ATTEMPTS)
                {
                    await HandleMqttDisconnection();
                }
            }
            finally
            {
                isConnecting = false; // ⭐ 반드시 플래그 해제
            }
        }

        // ⭐ 대폭 개선된 백오프 알고리즘을 적용한 스마트 재연결 로직
        private async Task HandleMqttDisconnection()
        {
            // 중복 재연결 방지: 이미 재연결 중이거나 연결 시도 중이면 즉시 종료
            if (isReconnecting || isConnecting) return;

            // 재연결 상태 플래그 설정 및 시도 횟수 증가
            isReconnecting = true;
            reconnectAttempts++;

            // 사용자에게 현재 재연결 상태 안내
            LogText += $"🔄 재연결 시도 중... ({reconnectAttempts}/{MAX_RECONNECT_ATTEMPTS}) - {DateTime.Now:HH:mm:ss}\n";

            // ⭐ 백오프 알고리즘: 재연결 간격을 점진적으로 늘려 서버 부하 감소
            // 1차: 3초, 2차: 6초, 3차: 9초, 4차: 12초, 5차: 15초 대기
            int delaySeconds = Math.Min(reconnectAttempts * 3, 15);
            await Task.Delay(TimeSpan.FromSeconds(delaySeconds));

            // 재연결 시도 (성공 시 ConnectedAsync에서 isReconnecting = false 처리됨)
            await ConnectMqttBroker();
        }

        private async Task SaveSensingData(FakeInfo data)
        {
            string query = @"INSERT INTO fakedatas
                                   (sensing_dt, pub_id, count,
                                    temp, humid, light, human)
                            VALUES
                                   (@sensing_dt, @pub_id, @count,
                                   @temp, @humid, @light, @human)";
            try
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    using var cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@sensing_dt", data.Sensing_Dt);
                    cmd.Parameters.AddWithValue("@pub_id", data.Pub_Id);
                    cmd.Parameters.AddWithValue("@count", data.Count);
                    cmd.Parameters.AddWithValue("@temp", data.Temp);
                    cmd.Parameters.AddWithValue("@humid", data.Humid);
                    cmd.Parameters.AddWithValue("@light", data.Light);
                    cmd.Parameters.AddWithValue("@human", data.Human);

                    await cmd.ExecuteNonQueryAsync(); //이전까지는 cmd.ExecuteNonQuery()로 동기식으로 처리했음
                }
            }
            catch (Exception ex)
            {
                // ⭐ 개선된 DB 저장 오류 시 로그 출력
                LogText += $"💾 DB 저장 오류: {ex.Message}\n";
            }
        }

        // DB에 접속하는 메서드
        private async Task ConnectDatabaseServer()
        {
            try
            {
                connection = new MySqlConnection(connString);
                connection.Open();
                IsDatabaseConnected = true; //연결 상태 업데이트
                LogText += $"✅ {DatabaseHost} DB 서버에 성공적으로 연결되었습니다! 상태: {connection.State}\n";
            }
            catch (Exception ex)
            {
                IsDatabaseConnected = false; //연결 상태 업데이트
                LogText += $"❌ {DatabaseHost} DB 서버 접속 실패: {ex.Message}\n";
                throw;
            }
        }

        // Command 메서드들
        [RelayCommand]
        public async Task ConnectBroker()
        {
            if (string.IsNullOrEmpty(BrokerHost))
            {
                // ⭐ 개선된 사용자 안내 메시지
                await this.dialogCoordinator.ShowMessageAsync(this, "브로커연결", "브로커 호스트를 입력하세요!");
                return;
            }

            //MQTT 브로커에 접속해서 데이터를 가져오기 
            await ConnectMqttBroker();
        }

        [RelayCommand]
        public async Task ConnectDatabase()
        {
            if (string.IsNullOrEmpty(DatabaseHost))
            {
                await this.dialogCoordinator.ShowMessageAsync(this, "DB연결", "DB 호스트를 입력하세요!");
                return;
            }

            connString = $"Server={DatabaseHost};Database=smarthome;Uid=root;Pwd=12345;Charset=utf8";

            await ConnectDatabaseServer();
        }

        // ⭐ 대폭 개선된 리소스 해제를 위한 Dispose 메서드 - MQTT 클라이언트도 안전하게 정리
        public void Dispose()
        {
            try
            {
                // MQTT 연결 안전하게 종료 (최대 2초 대기)
                if (mqttClient != null && mqttClient.IsConnected)
                {
                    mqttClient.DisconnectAsync().Wait(2000);
                }
                mqttClient?.Dispose(); // MQTT 클라이언트 리소스 해제
            }
            catch
            {
                // 정리 과정에서 발생하는 예외는 무시 (앱 종료 시 불필요한 오류 방지)
            }

            connection?.Close(); //DB 연결 해제
        }
    }
}