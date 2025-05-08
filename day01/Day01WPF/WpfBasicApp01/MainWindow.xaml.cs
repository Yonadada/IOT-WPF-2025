using System.Text;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;

namespace WpfBasicApp01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 메인 윈도우 로드 후 이벤트 처리 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // DB 연결
            // 데이터 그리드에 데이터를 읽어오기
            LoadControlFromDb();
            LoadGridFromDb();
        }

        private async void LoadControlFromDb()
        {
            // 1. 연결 문자열(DB 연결 문자열은 필수)
            string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
            // 2. 사용 쿼리
            string query = "SELECT division, names FROM divtbl;";

            // Dictionary나 KeyValuePair 둘 다 상관없음
            List<KeyValuePair<string, string>> divisions = new List<KeyValuePair<string, string>>();

            // 3. DB 연결, 명령, 리더
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader(); // 데이터 가져올 때 사용

                    while (reader.Read())
                    {
                        var division = reader.GetString("division");
                        var names = reader.GetString("names");

                        divisions.Add(new KeyValuePair<string, string>(division, names));
                    }
                }
                catch (MySqlException ex)
                {
                    await this.ShowMessageAsync($"에러! {ex.Message}","에러");
                }
            } // conn.Close() 자동 발생

            CboDivisions.ItemsSource = divisions;

        }

        private void LoadGridFromDb()
        {
            // 1. 연결 문자열(DB 연결 문자열은 필수)
            string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";

            // 2. 사용 쿼리, 기본 쿼리로 먼저 작업 후 필요한 실제 쿼리로 변경해도
            string query = @"
                   SELECT b.Idx, b.Author, b.Division, b.Names, b.ReleaseDate, b.ISBN, b.Price,
	                      d.Names AS dNames
                   FROM bookstbl AS b, divtbl AS d
                   WHERE b.Division = d.Division
                   ORDER by b.Idx;";

            // 3. DB 연결, 명령, 리더
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    GrdBooks.ItemsSource = dt.DefaultView; // 데이터 그리드에 데이터 바인딩
                }
                catch (MySqlException ex)
                {
                    // 나중에
                }
            } // conn.Close() 자동 발생

        }
        /// <summary>
        /// 데이터 그리드 더블 클릭 이벤트 핸들러
        /// 선택한 그리드의 레코드값이 오른쪽 상세에 출력됨
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GrdBooks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GrdBooks.SelectedItems.Count == 1)
            {
                //그리드 데이터를 하나만 선택했을 때
                //데이터 그리드의 데이터는 IList형식, 그 중 한건은 DataRowView 객체로 형변환 가능
                var item = GrdBooks.SelectedItems[0] as DataRowView;

                //MessageBox.Show(item.Row["Names"].ToString());
                NudIdx.Value = Convert.ToDouble(item.Row["idx"]);
                CboDivisions.SelectedValue = item.Row["Division"];
                TxtNames.Text = Convert.ToString(item.Row["Names"]);
                TxtAuthor.Text = Convert.ToString(item.Row["Author"]);
                TxtIsbn.Text = Convert.ToString(item.Row["ISBN"]);
                TxtPrice.Text = Convert.ToString(item.Row["Price"]);

                DpcReleaseDate.Text = Convert.ToString(item.Row["ReleaseDate"]);
            }
            await this.ShowMessageAsync($"처리완료", "메세지");
        }

       
    }
}