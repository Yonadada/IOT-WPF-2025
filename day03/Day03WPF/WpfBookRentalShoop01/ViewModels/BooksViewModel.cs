
using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfBookRentalShoop01.ViewModels
{
    class BooksViewModel : ObservableObject
    {
        private string _message;


        public string Message { 
            get => _message; 
            set => SetProperty(ref _message, value); 
        }
        public BooksViewModel()
        {
           Message = "도서 관리 화면입니다~!";
        }

    }
}
