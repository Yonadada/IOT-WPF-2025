
using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfBookRentalShoop01.ViewModels
{
    class BookGenreViewModel : ObservableObject
    {
        private string _message;


        public string Message { 
            get => _message;
            set => SetProperty(ref _message, value);
        }
        public BookGenreViewModel()
        {
           Message = "도서 장르 화면입니다~!";
        }

    }
}
