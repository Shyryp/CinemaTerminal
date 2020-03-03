using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CinemaTerminal
{
    /// <summary>
    /// Логика взаимодействия для FilmsPage.xaml
    /// </summary>
    public partial class FilmsPage
    {
        SqlConnection sqlConnection; //объект для создания соединения с базой данных
        List<Time> dataTimeTable = new List<Time>(); //список временных объектов для каждой записи таблицы "Time"
        List<Film> filmTable = new List<Film>(); //список временных объектов для каждой записи таблицы "Film"
        List<Button> dateButtons = new List<Button>(); //список кнопок для дат
        List<Hall> hallsList = new List<Hall>(); //список залов
        int offsetDate = -3; //смещение даты (тестирование)
        MainWindow mainWindow;
        public FilmsPage(MainWindow mainWindow) //Конструктор страницы
        {
            InitializeComponent();
            this.mainWindow = mainWindow; //сохраняем ссылку на mainWindow
            this.mainWindow.bBack.Click += new System.Windows.RoutedEventHandler(this.PressButtonBack); //активируем кнопку "Назад"
            this.mainWindow.bBack.Visibility = Visibility.Visible; //делаем кнопку "Назад" выдимой на странице
            DateTime nowTime = DateTime.Now; // узнаем системную дату
            for (int i = 0; i < 5; i++) //выводим пять кнопок для дат
            {
                Button bufButton = new Button //создаём кнопку и задаём ей значения
                {
                    Name = "bData" + nowTime.AddDays(i+offsetDate).ToString("dd"), //название кнопки
                    Content = nowTime.AddDays(i+offsetDate).ToString("dd MMMM"), //текст для кнопки
                    Height = 60, //высота
                    Width = 230, //ширина
                    Style = (Style)this.Resources["StyleNotPress"], // задаётся стиль, который прописан в xaml
                };
                dateButtons.Add(bufButton); //добавляем кнопку в список кнопок
                dateButtons.Last().Click += ClickDate; //даём кнопке ссылку на функцию при клике по ней
                dateButtons.Last().SetValue(Grid.ColumnProperty, i); //устанавливается для неё положение
                gridDateButtons.Children.Add(dateButtons.Last()); //добавляется на область
            }
            ClickDate(dateButtons.First(),null); //производится автоматическое нажатие на первую кнопку
        }

        private void PressButtonBack(object sender, RoutedEventArgs e)
        {
            //TODO : удаляем данные текущей страницы
            this.mainWindow.bBack.Click -= new System.Windows.RoutedEventHandler(this.PressButtonBack);
            mainWindow.Main.Content = new StartPage(mainWindow);
        }

        private void RestartButtons()
        {
            foreach (Button button in dateButtons)
            {
                button.Style = (Style)this.Resources["StyleNotPress"];
            }
        }

        private async void ClickDate(object sender, RoutedEventArgs e) // обработчик события нажатия на кнопку с датой
        {
            RestartButtons();


            //Обнуляем листы
            dataTimeTable = new List<Time>();
            filmTable = new List<Film>();

            Button clickedButton = sender as Button;
            clickedButton.Style = (Style)this.Resources["StyleYesPress"];

            DateTime selectedDay = CheckButton(clickedButton);

            sqlConnection = new SqlConnection(mainWindow.connectionString); //создаем подключение
            await sqlConnection.OpenAsync(); // открываем соединение
            SqlDataReader sqlDataReader = null;
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Time] WHERE [SessionDate] = @sessionDate", sqlConnection);
            sqlCommand.Parameters.AddWithValue("sessionDate", selectedDay); // определяем параметр id_film
            sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                Time bufTime = new Time();
                bufTime.Id = sqlDataReader.GetInt32(0);
                bufTime.SessionTime = sqlDataReader.GetTimeSpan(1); // в переменную Time из класса timetable записываем результат запроса
                bufTime.SessionDate = sqlDataReader.GetDateTime(2);
                bufTime.Cost = sqlDataReader.GetDecimal(3);
                bufTime.IdFilm = sqlDataReader.GetInt32(4);

                dataTimeTable.Add(bufTime);
            }
            sqlDataReader.Close();
            sqlConnection.Close();



            int[] buffFilmArray = new int[dataTimeTable.Count];
            for (int i = 0; i < dataTimeTable.Count; i++)
            {
                buffFilmArray[i] = dataTimeTable[i].IdFilm;
            }
            int[] filmArray = buffFilmArray.Distinct().ToArray();

            for (int i = 0; i < filmArray.Length; i++)
            {
                Console.WriteLine(filmArray[i].ToString());
            }


            sqlConnection = new SqlConnection(mainWindow.connectionString); //создаем подключение
            await sqlConnection.OpenAsync(); // открываем соединение
            for (int i = 0; i < filmArray.Length; i++)
            {
                sqlDataReader = null;
                sqlCommand = new SqlCommand("SELECT * FROM [Film] WHERE [Id] = @Id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", filmArray[i]); // определяем параметр id_film
                sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    Film bufFilm = new Film();
                    bufFilm.Id = sqlDataReader.GetInt32(0);
                    bufFilm.Name = sqlDataReader.GetString(1);
                    bufFilm.Length = sqlDataReader.GetInt32(2);
                    byte[] data = (byte[])sqlDataReader.GetValue(3);
                    bufFilm.Poster = data;
                    bufFilm.Age = sqlDataReader.GetInt32(4);
                    bufFilm.ShortDescription = sqlDataReader.GetString(5);
                    bufFilm.IdGenre = sqlDataReader.GetInt32(6);
                    filmTable.Add(bufFilm);
                }
                sqlDataReader.Close();
            }
            sqlConnection.Close();

            filmTime.Children.Clear();
            //Выводим на экран все гриды и кнопки
            for (int i = 0; i < filmTable.Count; i++)
            {
                Grid gridRoot = new Grid
                {
                    Height = 300,
                    Margin = new Thickness(10, 20, 10, 20),
                    Name = "GridRoot" + filmTable[i].Id.ToString(),
                    //ShowGridLines = true
                };
                
                gridRoot.RowDefinitions.Add(new RowDefinition());
                gridRoot.RowDefinitions.Add(new RowDefinition());

                ColumnDefinition c1 = new ColumnDefinition();
                c1.Width = new GridLength(250, GridUnitType.Pixel);
                gridRoot.ColumnDefinitions.Add(c1);
                ColumnDefinition c2 = new ColumnDefinition();
                gridRoot.ColumnDefinitions.Add(c2);
                filmTime.Children.Add(gridRoot);
                Image posterFilm = new Image
                {
                    Source = LoadImage(filmTable[i].Poster)
                };
                posterFilm.SetValue(Grid.ColumnProperty, 0);
                posterFilm.SetValue(Grid.RowProperty, 0);
                posterFilm.SetValue(Grid.RowSpanProperty, 2);
                gridRoot.Children.Add(posterFilm);


                Grid gridDescription = new Grid
                {
                    Name = "GridDescription" + filmTable[i].Id.ToString(),
                    //ShowGridLines = true
                };
                gridDescription.SetValue(Grid.ColumnProperty, 1);
                gridDescription.SetValue(Grid.RowProperty, 0);
                gridDescription.RowDefinitions.Add(new RowDefinition());
                gridDescription.RowDefinitions.Add(new RowDefinition());
                gridRoot.Children.Add(gridDescription);

                SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(150, 255, 255, 255));
                gridDescription.Background = brush;

                

                //Создаём грид для кнопок расписания
                Grid gridButtons = new Grid
                {
                    Name = "GridButtons" + filmTable[i].Id.ToString(),
                    //ShowGridLines = true
                };

                gridButtons.RowDefinitions.Add(new RowDefinition());
                gridButtons.RowDefinitions.Add(new RowDefinition());

                for (int k = 0; k < 7; k++)
                { 
                    ColumnDefinition column = new ColumnDefinition();
                    column.Width = new GridLength(10, GridUnitType.Star);
                    gridButtons.ColumnDefinitions.Add(column);
                }
                gridButtons.SetValue(Grid.ColumnProperty, 1);
                gridButtons.SetValue(Grid.RowProperty, 1);
                gridRoot.Children.Add(gridButtons);

                SolidColorBrush brush1 = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
                gridButtons.Background = brush1;

                dataTimeTable.Sort(delegate (Time tim1, Time tim2)
                { return tim1.SessionTime.CompareTo(tim2.SessionTime); });

                int columnButton = 0;
                for (int l = 0; l < dataTimeTable.Count; l++)
                {
                    if (dataTimeTable[l].IdFilm == filmTable[i].Id)
                    {
                        int cost = (int) dataTimeTable[l].Cost;
                        int hours = (int)dataTimeTable[l].SessionTime.TotalHours;
                        int minuts = (int)dataTimeTable[l].SessionTime.TotalMinutes - 60*(int)dataTimeTable[l].SessionTime.TotalHours;
                        Button buttonNote = new Button
                        {
                            Name = "Button" + filmTable[i].Id + "l" + dataTimeTable[l].Id,
                            Height = 60,
                            Width = 130,
                            Style = (Style)this.Resources["TimeB"]

                        };
                        if (minuts > 9)
                        {
                            if (hours > 9)
                            {
                                buttonNote.Content = hours + ":" + minuts + "\n" + cost + " р.";
                            }
                            else
                            {
                                buttonNote.Content = "0"+hours + ":" + minuts + "\n" + cost + " р.";
                            }
                        }
                        else {
                            if (hours > 9)
                            {
                                buttonNote.Content = hours + ":0" + minuts + "\n" + cost + " р.";
                            }
                            else
                            {
                                buttonNote.Content = "0" + hours + ":0" + minuts + "\n" + cost + " р.";
                            }
                        }
                        buttonNote.Click += NextWindow;
                        if (columnButton < 7)
                        {
                            buttonNote.SetValue(Grid.ColumnProperty, columnButton);
                            buttonNote.SetValue(Grid.RowProperty, 0);
                            gridButtons.Children.Add(buttonNote);
                        }
                        else {
                            buttonNote.SetValue(Grid.ColumnProperty, columnButton - 7);
                            buttonNote.SetValue(Grid.RowProperty, 1);
                            gridButtons.Children.Add(buttonNote);
                        }
                        columnButton++;
                    }
                }



                //Выводим название фильма
                Label labelNameFilm = new Label
                {
                    Content = filmTable[i].Name.ToString(),
                    Style = (Style)this.Resources["LabelName"]
                };
                labelNameFilm.SetValue(Grid.ColumnProperty, 0);
                labelNameFilm.SetValue(Grid.RowProperty, 0);
                gridDescription.Children.Add(labelNameFilm);

                gridDescription.RowDefinitions[0].Height = new GridLength(50);

                //Выводим описание фильма
                Label labelDescriptionFilm = new Label
                {
                    Content = filmTable[i].ShortDescription.ToString()
                };
                labelDescriptionFilm.SetValue(Grid.ColumnProperty, 0);
                labelDescriptionFilm.SetValue(Grid.RowProperty, 1);
                gridDescription.Children.Add(labelDescriptionFilm);


            }

        }
        BitmapSource LoadImage(byte[] imageData)
        {
            using (MemoryStream ms = new MemoryStream(imageData))
            {
                var decoder = BitmapDecoder.Create(ms,
                    BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                return decoder.Frames[0];
            }
        }
        private DateTime CheckButton(Button button)
        {
            DateTime nowTime = DateTime.Now; // узнаем системную дату
            for (int i = 0; i < 5; i++)
            {
                if (button.Name == ("bData"+nowTime.AddDays(i+offsetDate).ToString("dd")))
                {
                    return nowTime.AddDays(i+offsetDate).Date;
                }
            }
            return System.DateTime.Today;
        }
        private void NextWindow(object sender, RoutedEventArgs e)
        {
            Button bufBut = (Button)sender;
            foreach (Film filmb in filmTable)
            {
                foreach (Time timeb in dataTimeTable)
                {
                    if (bufBut.Name == ("Button" + filmb.Id + "l" + timeb.Id))
                    {
                        mainWindow.film = filmb;
                        mainWindow.time = timeb;


                        sqlConnection = new SqlConnection(mainWindow.connectionString); //создаем подключение
                        sqlConnection.OpenAsync(); // открываем соединение
                        SqlDataReader sqlDataReader = null;
                        SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Hall] WHERE [IdTime] = @idtime", sqlConnection);
                        sqlCommand.Parameters.AddWithValue("idtime", timeb.Id); // определяем параметр id_film
                        sqlDataReader = sqlCommand.ExecuteReader();
                        while (sqlDataReader.Read())
                        {
                            Hall bufHall = new Hall();
                            bufHall.Id = sqlDataReader.GetInt32(0);
                            bufHall.Number = sqlDataReader.GetInt32(1); // в переменную Time из класса timetable записываем результат запроса
                            bufHall.NumberOfRow = sqlDataReader.GetInt32(2);
                            bufHall.PlaceInRow = sqlDataReader.GetInt32(3);
                            bufHall.IdTime = sqlDataReader.GetInt32(4);

                            hallsList.Add(bufHall);
                        }
                        sqlDataReader.Close();
                        sqlConnection.Close();


                        foreach (Hall hallb in hallsList)
                        {
                            if (hallb.IdTime == timeb.Id)
                            {
                                mainWindow.hall = hallb;
                            }
                        }
                    }
                }
            }
            mainWindow.Main.Content = new HallPage(mainWindow);
        }
    }
}
