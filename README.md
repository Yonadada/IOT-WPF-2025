# IOT-WPF-2025
lot 개발자 WPF 학습리포지토리 2025

## 1일차

### WPF 개요
- Windows Presentaion Foundation
    - WinForms의 디자인의 미약한 부분, 속도 개선, `개발과 디자인의 분리를` 개선하고자 MS의 새로운 프레임워크
    - 화면 디자인을 XML 기반의 xaml

### WPF DB 바인딩
1. 프로젝트 생성 - [디자인](./day01/Day01WPF/WpfBasicApp01/MainWindow.xaml), [소스](./day01/Day01WPF/WpfBasicApp01/MainWindow.xaml.cs)
2. MahApps.Metro(디자인) 라이브러리 설치
3. 디자인
    - App.xaml
    - MainWindow.xaml
    - MainWindow.xaml.cs의 기반클래스를 변경하는 것 -  디자인
4. UI 구현
5. DB연결 사전 준비
    - MySQL.Data 라이브러리 설치

6. DB 연결
    1. DB연결 문자열(Connection String) :  DB 종류마다 연결 문자열 포맷이 다르고 무조건 있어야함

    2. 쿼리 : 실행할 쿼리(보통 SELECT, INSERT, UPDATE, DELETE)

    3. 데이터를 담을 객체 : 리스트 형식

    4. DB 연결 객체(`SqlConnection`) : 연결 문자열을 처리하는 객체. DB연결 - DB 끊기, 연결 확인
        - DB 종류 별로 MySqlConnection, SqlConnection, OracleConnection...

    5. DB명령객체(`SqlCommand`) : 쿼리를 컨트롤하는 객체
        - ExcuteReader() : SELECT문 실행, 결과 데이터를 담는 메서드
        - ExcuteScalar() : SELECT문 중, count() 등 함수로 1 row/1 column 데이터만 가져오는 메서드 
        - ExcuteNonQuery() : INSERT, UPDATE, DELETE문과 같이 transaction이 발생하는 쿼리실행 사용 메서드 
    
    6. DB 데이터 어댑터(`DataAdapter`) : 연결 이후 데이터 처리를 쉽게 도와주는 객체
        - DB 명령 객체처럼 쿼리를 직접 실행할 필요가 없음
        - DataTable, DataSet 객체에 fill() 메서드로 자동으로 채워줌 
        - 거의 SELECT문에만 사용 
    
    6. DB 데이터리더(`DataReader`) 
        - DataReader : ExcuteReader()로 가져온 데이터를 조작하는 객체 
        - DataAdapter : 좀 더 간단하게 데이터를 처리해주는 객체 

7. 실행결과

    <img src="./image/wpf0001.png">

8. MahApps.Metro 방식 다이얼로그 처리

    <img src="./image/wpf0004.png" width="600">

9. `전통적인 C# 윈앱 개발과` 차이가 없음

### WPF MVVM
- `디자인패턴`
    - 소프트웨어 공학에서 공통적으로 발생하는 문제를 재사용 가능하게 해결한 방식들
    - 반복적으로 되풀이되는 개발디자인의 문제를 해결하도록 맞춤화 시킨 양식(템플릿)
    - 여러 디자인패턴 중 아키텍쳐패턴, 그 중 디자인과 개발을 분리해 개발 할 수 있는 패턴을 준비
        - MV* : MVC, MVP, MVVM...

- `MVC : Model-View-Controller 패턴`
    - 사용자 인터페이스(View)와 비즈니스 로직(Controller, Model) 분리해서 앱을 개발
    - 디자이너에게 최소한의 개발에 참여 시킴
    - 개발자는 디자인은 고려하지 않고 개발만 할 수 있음
    - 사용자는 Controller에게 요청
    - Controller가 Model에게 Data를 요청
    - Model이 DB에 데이터를 가져와 Controller로 전달
    - Controller는 데이터를 비즈니스 로직에 따라 처리하고 View로 전달
    - View는 데이터를 화면에 뿌려주고, 화면 상에 처리할 것을 마친 후 사용자에게 응답

    - 구조는 복잡, 각 부분별 개발코드는 간결
    - Spring Boot, `ASP.NET`, jDango 등 웹개발 아키텍처 패턴 표준으로 사용

    <img src="./image/wpf0002.png" width="600">

- MVP : Model-View-Presenter 패턴
    - MVC 패턴에서 파생됨
    - Presenter : Supervising Controller 라고 부름
### WPF MVVM
- [디자인 패턴](https://ko.wikipedia.org/wiki/%EC%86%8C%ED%94%84%ED%8A%B8%EC%9B%A8%EC%96%B4_%EB%94%94%EC%9E%90%EC%9D%B8_%ED%8C%A8%ED%84%B4)
    - 소프트웨어 공학에서 공통적으로 발생하는 문제를 재사용 가능한 방식으로 해결한 방식들
    - 반복적으로 되풀이되는 개발 디자인의 문제를 해결하도록 맞춤화시킨 양식(템플릿)
    - 여러 디자인패턴 중 아키텍쳐 패턴, 디자인과 개발을 분리해 개발할 수 있는 패턴을 준비
        - MV* : MVC, MVP, MVVM...

- MVC : Model-View-Controller 패턴
    - 사용자 인터페이스(View)와 비즈니스 로직(Controller, Model) 분리해서 앱을 개발
    - 디자이너에게 최소환의 개발에 참여를 시킴
    - 개발자는 디자인은 고려하지 않고 개발만 할 수 있음 
    - 사용자는 Controller에게 요청
    - Controller가 Mode에게 Data를 요청
    - Model이 DB에 데이터를 가져와 Controller로 전달
    - Controller는 데이터를 비즈니스로직에 따라서 처리하고 View로 전달
    - View는 데이터를 화면에 뿌려주고, 화면 상에서 처리할 것을 모두 마친 후 사용자에게 응답

    - 구조는 복잡, 각 부분별 개발 코드는 간결.
    - Spring Boot, `ASP.Net`, jDango 등 웹개발 아키텍처 패턴을 표준으로 사용
    
    <img src="./image/wpf0002.png" width="600">

- MVP : Model-View-Presenter 패턴
    - MVC 패턴에서 파생됨
    - Presenter : Supervising Controller라고 부름
    
- **MVVM** : Model-View-ViewModel 패턴
    - MVC 패턴에서 파생
    - 마크업 언어로 GUI 코드를 구현하는 아키텍처
    - 사용자는 View로 접근(MVC와 차이점)
    - ViewModel이 Controller 역할(비즈니스로직 처리)
    - Model은 당연히 DB 요청, 응답
    - 연결방식이 MVC와 다름
    - 전통적인 C# 방식은 사용자가 이벤트 발생시키기 때문에 발생 시기를 바로 알 수 있음
    - MVVM 방식은 C#이 변화를 주시하고 있어야 함. 상태가 바뀌면 변화를 줘야 함

    <img src="./image/wpf0003.png" width="600">

### WPF MVVM 연습
1. 프로젝트 생성 - [디자인](./day01/Day01WPF/WpfBasicApp02/View/MainWindow.xaml), [소스](./day01/Day01WPF/WpfBasicApp02/ViewModel/MainViewModel.cs)
2. WPF DB 바인딩 연습시 사용한 UI 복사
3. Model, View, ViewModel 폴더 생성
4. MainWindow.Xaml을 View로 이동
5. App.xaml StartUri 수정
6. Model 폴더 내 Book 클래스 생성
    - INotifyPropertyChanged 인터페이스 : 객체내의 어떠한 속성 값이 변경되면 상태를 C#에게 알려주는 기능
    - PropertyChangedEventHandler 이벤트 생성
7. ViewModel 폴더 내 MainViewModel 클래스 생성
    - INotifyPropertyChanged 인터페이스 구현
    - OnPropertyChanged 이벤트 핸들러 메서드 코딩
8. MainView.xaml에  연결
    ```xml
        ...
        xmlns:vm="clr-namespace:WpfBasicApp02.ViewModel"
        DataContext="{DynamicResource MainVM}"
        ...
    <mah:MetroWindow.IconTemplate>
        <!-- MainViewModel을 가져와서 사용하겠다!! -->
        <vm:MainViewModel x:Key="MainVM" />
    </mah:MetroWindow.Resources>
    ```

9. MainView.xaml 컨트롤에 바인딩 작업
    -  전통적인 c# 방식 : x:Name 사용(비하인드 사용필요), 마우스 이벤트 추가
   
    ```xml
    <!-- UI 컨트롤 구성 -->
    <DataGrid x:Name="GrdBooks" 
        Grid.Row="0" Grid.Column="0" Margin="5" 
        AutoGenerateColumns="False" IsReadOnly="True" 
        MouseDoubleClick="GrdBooks_MouseDoubleClick">
    <DataGrid.Columns>
        <DataGridTextColumn Binding="{Binding Idx}" Header="순번" />
    ```

    - WPF MVVM 바인딩 방식 

    ```xml
    <!-- UI 컨트롤 구성 -->
    <DataGrid Grid.Row="0" Grid.Column="0" Margin="5" 
          AutoGenerateColumns="False" IsReadOnly="True"
          ItemsSource="{Binding Books}"
          SelectedItem="{Binding SelectedBook, Mode=TwoWay}">
    <DataGrid.Columns>
        <DataGridTextColumn Binding="{Binding Idx}" Header="순번" />
    ```
10. 실행결과

    <img src="./image/wpf0005.png" width="600">

- MVVM의 장.단점
    (장점)
    - View  <-> ViewModel 간 데이터 자동 연동
    - 로직 분리로 구조가 명확해짐. 디자이너와 개발자의 업무가 명확해짐 
    - 팀 프로젝트로 개발할 시, 역할 분담이 확실하다
    - 테스트와 유지보수가 쉽다
    
    (단점)
    - 구조가 복잡하며 디버깅이 어렵다
    - 스케일이 커진다

## 2일차

### MVVM Framework
- MVVM 개발자체가 어려움. 초기 개발시  MVVM 템플릿을 만드는데 시간이 많이 소요. 난이도가 꽤 있음
- 조금 쉽게 개발하고자 만든 3rd Party에서 개발한 MVVM 프레임워크 사용
- 종류
    - `Prism` : MS계열에서 직접 개발. 대규모 앱 개발시 사용. 모듈화 잘 되어 있음. 커뮤니티 활발 
        - (단점) 진입장벽이 높음
   
    - **Caliburn.Micro** : 경량화된 프레임워크. 쉽게 개발 할 수 있음. Xaml 바인딩 생략가능.
        - [공식사이트](https://caliburnmicro.com/)
        - [Github](https://github.com/Caliburn-Micro/Caliburn.Micro)
        - MahApps.Metro에서 사용중 
        - (단점) 커뮤니티가 줄어들고 있는 추세, 디버깅이 어려움
        - [문제] MahApps.Metro의 메세지 박스 다이얼로그가 구현이 되지 않고있다!
   
    - `MVVM Light ToolKit` : 가장 가벼운 MVVM 입문용. Command 지원
        - (단점) 개발 종료, 확장성이 떨어짐

    - CommunityToolKit.Mvvm : MS 공식 경량 MVVM, 단순, 빠르다, 커뮤니티 매우 활발, NotifyPropertyChanged를 사용할 필요없음
        - (단점) 모듈 기능이 없음

    - `ReactiveUI` : 최신기술 RX기반 MVVM, 비동기, 스트림처리 강력, 커뮤니티가 활발
        - (단점) 진입장벽이 높음

### Caliburn.Micro 학습
1. WPF 프로젝트 생성 -[소스](./day02/Day02Wpf/WpfBasicApp01/App.xaml)
2. Nuget 패키지 관리에서 Caliburn.Micro 검색 후 설치
3. App.xaml StartupUri 삭제
4. Model, View, ViewModels 폴더 생성 (이름이 똑같아야 한다) 생성
5. MainViewModel 클래스 생성 -[소스](./day02/Day02Wpf/WpfBasicApp01/ViewModels/MainViewModel.cs)
    - MainView 속하는 ViewModel은 반드시 MainViewModel라는 이름을 써야함
6. MainWindow.xaml을 View 로 이동
7. MainWindow를 MainView로 이름 변경
8. Bootstrapper 클래스 생성, 작성 -[소스](./day02/Day02Wpf/WpfBasicApp02/Bootstrapper.cs)
9. App.xaml에서 Resource 추가
10. MahApps.Metro UI  적용
    
    <img src="./image/wpf0006.png" width="600">

### Caliburn.Micro MVVM 연습
1. WPF 프로젝트  생성 -[소스](./day02/Day02Wpf/WpfBasicApp02/ViewModels/MainViewModel.cs)
2. 필요 라이브러리 설치
    - Caliburn.Micro
    - MahApps.Metro
    - MahApps.Metro.IconPacks
    - MySql.Data
3. Models, Views, ViewModels 폴더 생성
4. 이전 작업 소스코드 복사, 네임스페이스 변경 

    <img src="./image/wpf0007.png" width="600">


## 3일차 

### CommunityToolkit.Mvvm 다시
1. Wpf 프로젝트 생성
2. 필요 라이브러리 설치 
    - CommunityToolkit.Mvvm
    - MahApps.Metro
    - MahApps.Metro.IconPacks
3. Models, Views, ViewModels
4. MainWIndow.xaml 삭제
5. App.xaml StartUpUri 삭제
6. Views/MainView.xaml 생성
7. ViewModels/MainViewModel.cs 생성
8. App.xaml Startup 이벤트 추가
    - App.xaml.xs 로직 추가
9. App.xaml MahApps.Metro 관련 리소스 추가
10. MainView에 MetroWindow 로 변경

    <img src="./image/wpf0008.png" width="600">

### Log 라이브러리
- 개발한 앱, 솔루션의 현재상태를 계속 모니터링하는 기능

- Log 사용법
    - 직접 코딩 방식 : 어렵다
    - 로그 라이브러리 사용방식 

- Log 라이브러리 
    - **NLog** : 데스크 톱
        -  (장점) 가볍고 배우기 쉽다, 빠름

    - Serilog : 웹쪽에서 사용
        - (장점) 빠름
        - (단점) 배우기 어려움

    - Log4net : 웹쪽에서 사용
        - Java의 로그를 .NET으로 이전 
        - (단점) 느림

    - `ZLogger` : 게임서버에서 사용 
        - 제일 최신(2021), 초고속(게임서버에 사용)

### NLog 라이브러리 사용
1. Nuget 패키지 > NLog, NLog.Schema 설치
2. 추가 > 새 항목 > NLog.config 생성
3. Info < `Debug` < Warn < Error < Fatal
4. `NLog.config`를 출력 디렉토리 
5. Debug, Trace는 출력이 안된다
6. Info, Warn, Error, Fatal 을 사용

    <img src="./image/wpf0009.png" width="600">

### DB연결 CRUD 연습
1. WPF 프로젝트 생성
2. NuGet 패키지 필요 라이브러리 설치
    - CommunityToolkit.Mvvm 
    - MahApps.Metro / MahApps.Metro.IconPacks
    - MySql.Data
    - NLog
3. Models, Views, ViewModels 생성
4. App.xaml 초기화 작업 
5. MainView.xaml, MainViewModel 메인화면 MvvM 작업
    - 메뉴 작업
    - ContentControl 추가
6. 하위 사용자 컨트롤 작업 
    - BookGenre(View, ViewModel)
    - Books(View, ViewModel)
7. Models > Genre(DivisionTbl) 모델 작업 
8. BookGenreViewModel DB 처리 구현  

    https://github.com/user-attachments/assets/5aa79cf8-0f32-4d41-8d39-ead672d8c4c6

## 4일차

### DB연결 CRUD 연습(계속)
1. BookGenre에서 INSERT, UPDATE 기능 구현
2. NLog.config 생성
3. Helpers.Common 클래스 생성
    - NLog 인스턴스 생성
    - 공통 DB연결문자열 생성
    - MahApps.Metro 다이얼로그 코디네이터 생성
4. 각 ViewModel에 IDialogCoordinator 관련 코딩 추가
    - ViewModel 생성자에 파라미터 추가
    - View, ViewModel 연동시 IDialogCoordinator 연결
5. View에 Dialog관련 네임스페이스, 속성 추가
6. await this.dialogCoordinator.ShowMessageAsync() 사용

    <img src="./image/wpf0011.png" width="650">

7. BookView.xaml 화면작업
8. MemberView.xaml, RentalView.xaml 화면작업
9. ViewModel들 작업

    <img src="./image/wpf0012.png" width="650">

#### DB연결 CRUD 연습시 추가 필요사항
- [x] 여러번 나오는 로직 메서드화
- [x] NLog로 각 기능 동작시 로그남기기. 공통화작업
- [x] 연결문자열 Common으로 이전
- [x] 종료 메뉴 다이얼로그 MetroUI로 변경
- [x] MahApps.Metro 메시지형태로 변경
- [x] 삭제여부 메시지박스 추가

### DB연결 CRUD 실습
- BooksView, BooksViewModel 작업 실습
- 1일차 MVVM 내용, 오늘 학습한것 

## 5일차

### MovieFinder 2025 
- 전체 UI : UI 설계화면 다섯 영역으로 구분 

<img src="./image/wpf0014.png" width="650">

- 영화즐겨찾기앱
    - TMDB 사이트에서 제공하는 OpenAPI로 데이터 가져오기
    - 내가 좋아하는 영화리스트 선택, 즐겨찾기 저장
    - 저장한 영화만 리스트업, 삭제 가능
    - 선택한 영화 더블클릭 > 영화 상세정보 팝업
    - 선택된 영화 선택 > 예고편보기 > Youtube동영상 팝업

- TMDB, Youtube
    - [TMDB](https://www.themoviedb.org/) API 신청
    - Youtube Data API 신청
        -프로젝트 생성 후 API 및 서비스 > 라이브러리
        - YouTube Data API v3 선택
        - 사용버튼 클릭
        - 사용자 인증 정보 입력

### 프로젝트 시작
1. WPF 프로젝트 생성
2. NuGet 패키지 사용할 기본 라이브러리 설치 
    - CommunityToolkit.MvvM 설치
    - MahApps.Metro , MahApps.Metro.IconPacks설치
    - MySql.Data 
    - NLog 
3. 폴더생성 : Helpers, Models, ViewModels, Views
4. MvvM 구조 초기 작업
5. UI  구현

    <img src="./image/wpf0015.png" width="650">

6. 로직 구현
    1. TMDB API 사용 구현
    2. 관련 기능 전부 구현

7. 데이터 그리트 더블 클릭 > 상세정보 표시 
    - NuGet 패키지에서 Microsoft.Xaml.Behaviors.Wpf 설치
8. 텍스트 박스에서 엔터시 이벤트 발생 처리
9. 텍스트 박스 한글 입력 우선 처리
10. 실행시 텍스트박스에서 포커스가 가도록 처리

    https://github.com/user-attachments/assets/612ce0a8-1910-40ed-9dbf-34c0d631e643

### 6일차

### MovieFinder 2025 (계속)
1. 상태표시줄 시계 동작
2. 상태표시줄 검색결과 건수 표시
3. 로그 출력 정리
4. 즐겨찾기 DB연동...
    1. MySQL Workbench에서 moviefinder 데이터베이스(스키마) 생성
    2. movieitems 테이블생성. 컬럼은 MovieItem.cs 속성과 동일
    3. INSERT, UPDATE, DELETE 작업 

    <img src="./image/wpf0017.png" width="650">

5. Youtube 예고편 보기
    1. TrailerView, TrailerViewModel
    2. WPF 기본 WebBrowser는 HTML5 기술이 표현안됨. 오류가 많음
    3. NuGet 패키지 - CefSharp.Wpf.NETCore WebBrowser패키지 설치
    4. CefSharp.Wpf 설치 시 프로젝트 속성>빌드>일반, 플랫폼 대상을 Any CPU에서 x64로 변경!!
    5. NuGet 패키지 - Google.Apis.YouTube.v3 설치

6. 기타 작업 완료
7. 결과 화면

    https://github.com/user-attachments/assets/9ba64ceb-5fc2-4ec3-9330-ff8a47cda6f9

## 7일차
### 부산광역시 부산맛집 정보앱
1. [데이터포털](https://data.go.kr) OpenAPI 신청

    <img src="./image/wpf0020.png" width="600">

2. WPF 프로젝트 생성
3. NuGet 패키지 라이브러리 설치
    - CommunityToolkit.Mvvm
    - MahApps.Metro / MahApps.Metro.IconPacks
    - Newtonsoft.Json
    - CefSharp.Wpf.NETCore (플랫폼 x64로 변경!)
    - NLog 

3. MVVM 초기화
4. UI 디자인 및 구현

    https://github.com/user-attachments/assets/afbb89f4-659a-4d92-8565-0a78d8dde575
 

## 8일차

### 부산광역시 부산 맛집 정보앱 (계속)
1. 그리드 표현 아이템 조정 
2. 메인창 내용을 구글 맵으로 이동
3. CefSharp.Wpf로 구글 맵 지도로 표현
4. 위도(Latitude), 경도(Longitude)로 표현

    https://github.com/user-attachments/assets/1e35ea29-1ae0-420e-ace6-eff6f8c9d5fd



### 스마트홈 연동 모니터링 앱

<img src="./image/wpf0022.jpg" width="650">

- 전면부

<img src="./image/WPF0023.jpg" width="650">

- 후면부
- [개발링크] (https://github.com/hugoMGSung/hungout-with-arduino/tree/main/SmartHomeDIY)

1. Arduino + Raspberry Pi 스마트홈 제작

#### 📡 MQTT (Message Queuing Telemetry Transport)

<img src="./image/wpf0026.png" width="650">

- MQTT는 기계 간 통신(M2M, Machine-to-Machine)을 위한 경량 메시징 프로토콜입니다.
- Publish / Subscribe(출판 / 구독) 방식으로 동작합니다.
    - Publish(발행): 메시지를 생성하여 서버에 전달합니다.

    - Subscribe(구독): 필요한 주제(topic)를 구독하고, 해당 메시지를 수신하여 처리합니다
- Server/Client 프로그램으로 동작서버/클라이언트 구조로 구성되어 있으며, 브로커(Broker)가 중간에서 메시지를 중개합니다. 

- 데이터는 휘발성입니다
    - 구독자가 없는 상태에서 발행된 메시지는 사라지며, 저장되지 않습니다.
    - 필요한 경우 별도로 DB 저장 로직을 구현해야 합니다.

- MQTT를 대체할 수 있는 유사 기술
    - Redis: Pub/Sub 기능을 제공하며, 빠른 인메모리 처리에 강점이 있음.

    - Apache Kafka: 대규모 데이터 스트리밍 및 로그 처리에 적합한 분산 메시지 시스템.

    - RabbitMQ: 다양한 프로토콜과 큐 구조를 지원하는 고급 메시지 브로커.

    - ZeroMQ: 브로커 없이 동작 가능한 경량 메시징 라이브러리. 고성능 네트워크 통신에 적합.

    - 소켓(Socket) 통신 직접 개발: 사용자 정의가 가능하지만, 안정성 및 확장성을 직접 구현해야 함.

#### MQTT 시뮬레이션 프로젝트 시작 
1. MQTT 브로커 설치 
     - Mosquitto (https://mosquitto.org/)
    - mosquitto-2.0.21a-install-windows-x64.exe 설치
    - 설치 후 서비스에서 서비스 중지 

2. 모스키토 설정 파일 수정 
    - mosquitto.conf 문서를 notepad++ 관리자 모드로 실행
    - #listener -> listener 1883
    - #allow_anonymous false -> allow_anonymous true
    - 파일 저장 후, 서비스 재시작
3. Windows 보안
    - 방화벽 및 네트워크 보호 > 고급설정 
    - 인바운드 규칙 > 새 규칙 선택
    - 포트 선택 > 다음 클릭
    - TCP 선택, 특정 포트 1883 입력 
    - 이름 => Mosquitto Broker Port

4. MQTT Explorer 설치 
    - https://mqtt-explorer.com/ 설치 
    - new Connection 생성, Host 127.0.0.1, Port 1883 저장
    - CONNECT

5. VS Code에서 [MqttPub.py](./day08/Pythons/MqttPub.py) 파일 생성

    https://github.com/user-attachments/assets/4e203ae9-9860-4686-ba3c-b40b3e97fb34

### 스마트홈 프로젝트 시작
1. 화면 UI 변경 
2. NuGet 패키지 
    - CommunityToolkit.MvvM 설치
3. Models, Views, ViewModels 폴더 생성
4. MainViewModel에서 바인딩 속성 초기화

    <img src="./image/wpf0025.png" width="650">'

## 9일차

### 스마트홈 연동 모니터링 앱(계속)
- MQTT부터 시작

#### 네트워크 확인 
- telnet 명령어로 서버 서비스가 동작중인지 확인 
- telnet 아이피주소 포트번호

    ```shell
    > telnet 아이피주소 포트번호
    
    # MySQL에 접속 가능한지 여부
    > telnet 127.0.0.1 3306

    # MQTT에 접속 가능한지 여부
    > telnet 127.0.0.1 1883
    ```

#### MQTT 시뮬레이션(계속)
1. MqttPub.py 소스코드에 Fake IoT 센서값 전달 코딩
2. Fake 센싱값을 json으로 Publish
3. C# MahApps.Metro 사용 MQTT 데이터 Subscriber 앱
    - CommunityToolkit.MvvM
    - MahApps.Metro
    - MahApps.Metro.IconPacks
    - Newtonsoft.Json
    - MQTTnet

4. DB 서버에 접속자 정보확인 쿼리 
    ```sql
    SELECT * FROM information_schema.processlist LIMIT 10; -- 데이터가 10건 이상이면 LIMIT 10은 삭제
    ```
5. WPF MvvM 전체 구현

6. MqttPub.py와 Publish된 IoT 데이터 WPF에서 Subscribe 예제


## 10일차

### 스마트홈 연동 모니터링 연동(계속)

#### 스마트홈 WPF 실시간 시각화
- NuGet 패키지
    - MQTTNe
    - Newtonsoft.Json
    - NLog
- WpfMqttSubApp.Models의 SensingInfo.cs 가져오기
- MQTT 센서 데이터로 동작용 변수 값 할당
- 실행화면

    https://github.com/user-attachments/assets/86d07cbd-d9ce-4110-9921-e1ac9588618d

      스마트홈 조작 영상

     https://github.com/user-attachments/assets/c54b7609-306f-4628-8c18-903f61e1b72b

    스마트홈 모니터링앱 동작영상

#### 스마트홈 WPF 실시간 시각화

### Github 대문 꾸미기
