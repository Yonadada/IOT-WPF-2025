﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBasicApp02.Model;


namespace WpfBasicApp02.ViewModel
{
    // MainViewModel에 속하는 속성의 값이 변경되면 이벤트가 발생
    internal class MainViewModel : INotifyPropertyChanged
    {
        // 속성 추가 get; set;
        // ObservableCollection -> 리스트의 변형 (변화를 감지할 수 있도록 처리된 클래스)
        public ObservableCollection<Book> Books { get; set; }
        //List<KeyValuePair<string, string>> divisions
        public ObservableCollection<KeyValuePair<string, string>> Divisions { get; set; }

        // 선택된 값에 대한 멤버변수, 멤버변수는 _를 붙이거나, 소문자로 변수명 시작
        private Book _selectedBook;
        // 선택된 값에 대한 속성
        public Book SelectedBook
        {
            // 람다식 표현 
            get => _selectedBook; // == get { return _selectedBook; }
            set
            {
                _selectedBook = value;
                // 값이 변경된 것을 알아차리도록 해줘야 함!!
                OnPropertyChanged(nameof(SelectedBook)); // nameof(SelectedBook) == "SelectedBook""
            }
        }

        public MainViewModel()
        {
            LoadControlFromDb();
            LoadGridFromDb();
        }

        // DB에서 콤보박스에 넣을 데이터를 불러오기
        private void LoadControlFromDb()
        {
            // 1. 연결 문자열(DB 연결 문자열은 필수)
            string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
            // 2. 사용 쿼리
            string query = "SELECT division, names FROM divtbl;";

            // Dictionary나 KeyValuePair 둘 다 상관없음
            ObservableCollection<KeyValuePair<string, string>> divisions = new ObservableCollection<KeyValuePair<string, string>>();

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
                    // 나중에
                }
            } // conn.Close() 자동 발생

            Divisions = divisions;
            OnPropertyChanged(nameof(Divisions)); // Divisions 속성 값이 변경됨
        }

        //DB에서 데이터 로드 후 Books 속성에 집어넣기
        private void LoadGridFromDb()
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
            OnPropertyChanged(nameof(books));
        }

        // 속성값이 변경되면 이벤트를 발생
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            // 기본적인 이벤트핸들러 파라미터와 동일(object sender, EventArgs e)
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
