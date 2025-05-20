using System.Windows;
using System.Windows.Input;
using WpfSmartHomeApp.ViewModels;

namespace WpfSmartHomeApp.Views
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
        // 원래 없는 속성을 사용자가 추가하는 방법
        // 의존속성 (depandency property)
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();// 앱 완전 종료
        }

        private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 제목 표시줄 x버튼 누를때, Alt+F4 눌렀을때 발생하는 이벤트
            e.Cancel = true; // 앱 종료를 막는 기능
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            if(DataContext is MainViewModel vm)
            {
                vm.LoadedCommand.Execute(null); // MainViewModels의 OnLoaded를 실행
            }
        }
    }
}