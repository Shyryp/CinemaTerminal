using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для HallPage.xaml
    /// </summary>
    public partial class HallPage
    {
        MainWindow mainWindow;
        int places = 0;
        List<Button> placeButtons = new List<Button>();
        public HallPage(MainWindow mainWindow)
        {
            InitializeComponent();
            
            this.mainWindow = mainWindow;
            this.mainWindow.bBack.Click += new System.Windows.RoutedEventHandler(this.PressButtonBack);
            this.mainWindow.bBack.Visibility = Visibility.Visible;
            DateTime nowTime = DateTime.Now; // узнаем системную дату
            for (int j = 0; j < 11; j++)
            {
                for (int i = 1; i < 11; i++)
                {
                    Button bufButton = new Button
                    {
                        Name = "bPlace" + i+"r"+j,
                        Content = i,
                        Style = (Style)this.Resources["PlaceB"]
                    };

                    placeButtons.Add(bufButton);
                    placeButtons.Last().Click += ClickPlace;
                    placeButtons.Last().SetValue(Grid.ColumnProperty, i);
                    placeButtons.Last().SetValue(Grid.RowProperty, j);
                    gridButtonsPlace.Children.Add(placeButtons.Last());
                }
            }
            for (int j = 0; j < 10; j++)
            {
                Label bufLabel = new Label
                {
                    Name = "lPlace" + j,
                    Content = (j+1) + " ряд",
                    Style = (Style)this.Resources["PlaceL"]
                };
                bufLabel.SetValue(Grid.ColumnProperty, 0);
                bufLabel.SetValue(Grid.RowProperty, j);
                gridButtonsPlace.Children.Add(bufLabel);
            }
            for (int j = 0; j < 10; j++)
            {
                Label bufLabel = new Label
                {
                    Name = "lPlace" + j + "i",
                    Content = (j + 1) + " ряд",
                    Style = (Style)this.Resources["PlaceL"]
                };
                bufLabel.SetValue(Grid.ColumnProperty, 11);
                bufLabel.SetValue(Grid.RowProperty, j);
                gridButtonsPlace.Children.Add(bufLabel);
            }
            UpdateData();
        }
        private void PressButtonBack(object sender, RoutedEventArgs e)
        {
            //TODO : удаляем данные текущей страницы
            this.mainWindow.bBack.Click -= new System.Windows.RoutedEventHandler(this.PressButtonBack);
            mainWindow.Main.Content = new FilmsPage(mainWindow);
        }
        private void ClickPlace(object sender, RoutedEventArgs e)
        {
            Button bufButton = (Button)sender;
            if (bufButton.Style == (Style)this.Resources["PlaceBPressed"])
            {
                places -= 1;
                bufButton.Style = (Style)this.Resources["PlaceB"];
            }
            else {
                places += 1;
                bufButton.Style = (Style)this.Resources["PlaceBPressed"];
            }
            UpdateData();
        }

        private void UpdateData()
        {
            this.lTotalPrice.Content = "Итого: " + places* (int)mainWindow.time.Cost + " р.";
            int hours = (int)mainWindow.time.SessionTime.TotalHours;
            int minuts = (int)mainWindow.time.SessionTime.TotalMinutes - 60 * (int)mainWindow.time.SessionTime.TotalHours;
            String str = "";
            if (minuts > 9)
            {
                if (hours > 9)
                {
                    str = hours + ":" + minuts;
                }
                else
                {
                    str = "0" + hours + ":" + minuts;
                }
            }
            else
            {
                if (hours > 9)
                {
                    str = hours + ":0" + minuts;
                }
                else
                {
                    str = "0" + hours + ":0" + minuts;
                }
            }
            this.lNameFilm.Content = mainWindow.film.Name;
            this.lTime.Content ="Время: " + str;
            this.lDate.Content = "Дата: " + mainWindow.time.SessionDate.ToString("dd MMMM");
            this.lPlaceSet.Content =  "Выбрано мест: " + places;
            this.lCostPlace.Content = "Цена за место: " + (int)mainWindow.time.Cost + " р.";
        }

        private void ClickBasketPage(object sender, RoutedEventArgs e)
        {
            mainWindow.places = places;
            mainWindow.Main.Content = new BasketPage(mainWindow, "Оплата билета", "Оплатить");
        }
    }
}
