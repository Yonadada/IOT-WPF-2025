using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using MovieFinder2025.Helpers;
using MovieFinder2025.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Web;

namespace MovieFinder2025.ViewModels
{
    public partial class MoviesViewModel : ObservableObject
    {
        private readonly IDialogCoordinator dialogCoordinator;
        public MoviesViewModel(IDialogCoordinator coordinator)
        {
            this.dialogCoordinator = coordinator;

            Common.LOGGER.Info("MovieFinder2025 Start.");
            PosterUri = new Uri("/NoPicture.png", UriKind.RelativeOrAbsolute);
        }

        private string _movieName;

        private ObservableCollection<MovieItem> _movieItems;
        
        private MovieItem _selectedMovieItem;

        private Uri _posterUri;
        
        private string _base_url = "https://image.tmdb.org/t/p/w300_and_h450_bestv2"; // TMDB에서 제공하는 이미지 URL 기본 주소

        public string MovieName {
            get => _movieName;
            set => SetProperty(ref _movieName, value); 
        }

        public ObservableCollection<MovieItem> MovieItems { 
            get => _movieItems; 
            set =>SetProperty(ref _movieItems, value);
        }

        public MovieItem SelectedMovieItem {
            get => _selectedMovieItem;
            set
            {

                SetProperty(ref _selectedMovieItem, value);

                if (value == null)
                {
                    PosterUri = new Uri("Views/NoPicture.png", UriKind.Relative);
                    Common.LOGGER.Info("선택된 영화가 null → 기본 이미지 사용");
                    return;
                }

                if (string.IsNullOrWhiteSpace(value.Poster_path))
                {
                    PosterUri = new Uri("Views/NoPicture.png", UriKind.Relative);
                    Common.LOGGER.Warn("포스터 경로 없음 → 기본 이미지 사용");
                    return;
                }

                try
                {
                    PosterUri = new Uri($"{_base_url}{value.Poster_path}", UriKind.Absolute);
                    Common.LOGGER.Info($"포스터 설정: {_base_url}{value.Poster_path}");
                }
                catch (Exception ex)
                {
                    PosterUri = new Uri("Views/NoPicture.png", UriKind.Relative);
                    Common.LOGGER.Error($"포스터 URI 예외 발생 → 기본 이미지 사용: {ex.Message}");
                }
            }
        }
        
        public Uri PosterUri {
            get => _posterUri; 
            set =>SetProperty(ref _posterUri, value);
        }



        [RelayCommand]
        public async Task SearchMovie()
        {
            //await this.dialogCoordinator.ShowMessageAsync(this, "영화검색", "영화를 검색합니다.");
            if (string.IsNullOrEmpty(MovieName))
            {
                await this.dialogCoordinator.ShowMessageAsync(this, "영화검색", "영화 제목을 입력하세요");
                return;
            }

            var controller = await dialogCoordinator.ShowProgressAsync(this, "대기중", "검색중");
            controller.SetIndeterminate();

            await SearchMovie(MovieName);
            await Task.Delay(1000);
            await controller.CloseAsync();
            //await Task.Delay(1000); // 1.0초 대기
            //await controller.CloseAsync();


            //SearchMovie(MovieName);
        }

        private async Task SearchMovie(string movieName)
        {
            string tmdb_apikey = "b17de1c30acbfdc46242b35f980c87ed"; // TMDB에서 신청한 API Key 
            string encoding_movieName = HttpUtility.UrlEncode(movieName, Encoding.UTF8); // 입력한 한글을 UTF-8로 인코딩
            string openApiUri = $"https://api.themoviedb.org/3/search/movie?api_key={tmdb_apikey}" +
                                $"&language=ko-KR&page=1&include_adult=false&query={encoding_movieName}";
           
            // Debug.WriteLine($"openApiUri: {openApiUri}");
           Common.LOGGER.Info($"TMDB Uri: {openApiUri}");

            string result = string.Empty;

            //OpenApi 실행할 웹 객체 - WebRequest, WebResponse -> Deprecated 추후 삭제될 예정 
            // HttpClient로 대체할 것 !!!!!
            //WebRequest req = null;
            //WebResponse res = null;

            HttpClient client = new HttpClient(); // HttpClient 객체 생성
            ObservableCollection<MovieItem> movieItems = new ObservableCollection<MovieItem>(); // 영화 목록을 담을 ObservableCollection 객체 생성
            string reader; // 응답을 받은 결과를 담는 객체

            try
            {
                //response = await client.GetAsync(openApiUri); // 비동기적으로 OpenAPI를 호출
                var response = await client.GetFromJsonAsync<MovieSearchResponse>(openApiUri); // 비동기적으로 OpenAPI를 호출
               
                foreach(var movie in response.Results)
                {
                    Common.LOGGER.Info($"{movie.Title}, {movie.Release_date.ToString("yyyy-MM-dd")}");
                    movieItems.Add(movie); // 영화 목록에 추가
                }
            }
            catch (Exception ex)
            {
                await this.dialogCoordinator.ShowMessageAsync(this, "예외", ex.Message);
                Common.LOGGER.Fatal($"API 요청 에러: {ex.Message}");
            }
            MovieItems = movieItems; // View에 가져갈 속성에 데이터를 할당
        }

        [RelayCommand]
        public async Task MovieItemDoubleClick()
        {
            var currMovie = SelectedMovieItem;
            if(currMovie != null)
            {
                StringBuilder sb = new StringBuilder();

                //Environment.NewLine은 줄바꿈을 의미
                //sb.Append(currMovie.Original_title + "(" + currMovie.Release_date.ToString("yyyy-MM-dd") + ")" + Environment.NewLine);
                sb.Append($"{currMovie.Original_title} ({currMovie.Release_date.ToString("yyyy-MM-dd")})\n");
                sb.Append($"평점 : {currMovie.Vote_average.ToString("F2")}\n");
                sb.Append(currMovie.Overview);

                await this.dialogCoordinator.ShowMessageAsync(this, currMovie.Title, sb.ToString());
            }
        }
    }
}
