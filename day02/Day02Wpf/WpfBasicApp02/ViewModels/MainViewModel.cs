﻿using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using WpfBasicApp02.Models;

namespace WpfBasicApp02.ViewModels
{

    public class MainViewModel : Conductor<object>
    {
        private IDialogCoordinator _dialogCoordinator; // 메세지박스, 다이얼로그 실행을 위한 변수

        public ObservableCollection<KeyValuePair<string, string>> Divisions { get; set; }

        public ObservableCollection<Book> Books{ get; set; }
        
        private Book _selectedBook;

        public Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                NotifyOfPropertyChange(() => SelectedBook);
            }
        }
        public MainViewModel()
        {
            _dialogCoordinator = new DialogCoordinator();
            LoadControlFromDb();
            LoadGridFromDb();
        }

        private void LoadGridFromDb()
        {
            // 1. 연결문자열(DB연결문자열은 필수)
            string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
            // 2. 사용쿼리
            string query = "SELECT division, names FROM divtbl";

            ObservableCollection<KeyValuePair<string, string>> divisions = new ObservableCollection<KeyValuePair<string, string>>();

            // 3. DB연결, 명령, 리더
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader(); // 데이터 가져올때                    

                    while (reader.Read())
                    {
                        var division = reader.GetString("division");
                        var names = reader.GetString("names");

                        divisions.Add(new KeyValuePair<string, string>(division, names));
                    }
                }
                catch (MySqlException ex)
                {
                    // 나중에...
                }
            } // conn.Close() 자동발생

            Divisions = divisions;
            NotifyOfPropertyChange(() => Divisions); // Caliburn.Micro에서 UI에 바인딩된 속성의 변경을 알림
        }

        private void LoadControlFromDb()
        {
            // 1. 연결 문자열(DB 연결 문자열은 필수)
            string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
            // 2. 사용 쿼리, 기본 쿼리로 먼저 작업 후 필요한 실제 쿼리로 변경
            string query = @"SELECT b.Idx, b.Author, b.Division, b.Names, b.ReleaseDate, b.ISBN, b.Price, d.Names AS dNames
                               FROM bookstbl AS b, divtbl AS d
                              WHERE b.Division = d.Division
                              ORDER BY b.idx";

            ObservableCollection<Book> books = new ObservableCollection<Book>();

            // 3. DB 연결, 명령, 리더
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        books.Add(new Book
                        {
                            Idx = reader.GetInt32("Idx"),
                            Division = reader.GetString("Division"),
                            Names = reader.GetString("Names"),
                            DNames = reader.GetString("dNames"),
                            Author = reader.GetString("Author"),
                            ISBN = reader.GetString("ISBN"),
                            ReleaseDate = reader.GetDateTime("ReleaseDate"),
                            Price = reader.GetInt32("Price")
                        });
                    }

                }
                catch (MySqlException ex)
                {
                    // 나중에
                }
            } // conn.Close() 자동 발생

            Books = books;
            NotifyOfPropertyChange(() => Books);
        }    
       public async void DoAction()
        {
            await _dialogCoordinator.ShowMessageAsync(this,"데이터로드!!!!!", "로드");
        }
    }
}
