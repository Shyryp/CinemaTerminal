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
    /// Логика взаимодействия для BasketPage.xaml
    /// </summary>
    public partial class BasketPage
    {
        MainWindow mainWindow;
        public BasketPage(MainWindow mainWindow, String str, String str2)
        {
            InitializeComponent();
            lName.Content = str;
            bSell.Content = str2;
            this.mainWindow = mainWindow;
            if (str2 == "Оплатить")
            {
                this.mainWindow.bBack.Click += new System.Windows.RoutedEventHandler(this.PressButtonBack);
            }
            else {
                this.mainWindow.bBack.Click += new System.Windows.RoutedEventHandler(this.PressButtonBack2);
            }
            this.mainWindow.bBack.Visibility = Visibility.Visible;
            this.lNameFilm.Content += mainWindow.film.Name;
            this.lDateStart.Content += mainWindow.time.SessionDate.ToString("dd MMMM");
            this.lNumberHall.Content += mainWindow.hall.Number.ToString();
            this.lNumberPlace.Content += mainWindow.places.ToString();
            this.lEndPrice.Content += Convert.ToString(mainWindow.places * (int)mainWindow.time.Cost);
            int cost = (int)mainWindow.time.Cost;
            int hours = (int)mainWindow.time.SessionTime.TotalHours;
            int minuts = (int)mainWindow.time.SessionTime.TotalMinutes - 60 * (int)mainWindow.time.SessionTime.TotalHours;
            if (minuts > 9)
            {
                if (hours > 9)
                {
                    this.lTimeStart.Content += hours + ":" + minuts;
                }
                else
                {
                    this.lTimeStart.Content += "0" + hours + ":" + minuts;
                }
            }
            else
            {
                if (hours > 9)
                {
                    this.lTimeStart.Content += hours + ":0" + minuts;
                }
                else
                {
                    this.lTimeStart.Content += "0" + hours + ":0" + minuts;
                }
            }
        }

        private void PressButtonBack(object sender, RoutedEventArgs e)
        {
            //TODO : удаляем данные текущей страницы
            this.mainWindow.bBack.Click -= new System.Windows.RoutedEventHandler(this.PressButtonBack);
            mainWindow.Main.Content = new HallPage(mainWindow);
        }
        private void PressButtonBack2(object sender, RoutedEventArgs e)
        {
            //TODO : удаляем данные текущей страницы
            this.mainWindow.bBack.Click -= new System.Windows.RoutedEventHandler(this.PressButtonBack2);
            mainWindow.Main.Content = new CheckCodePage(mainWindow);
        }

        private void BSell_Click(object sender, RoutedEventArgs e)
        {
            bSell.Content = "Билет печатается!";
        }
    }
}
