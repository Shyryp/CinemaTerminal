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
    /// Логика взаимодействия для StartPage.xaml
    /// </summary>
    public partial class StartPage
    {
        MainWindow mainWindow;
        public StartPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.mainWindow.bBack.Visibility = Visibility.Hidden;
        }

        private void PressButtonListFilms(object sender, RoutedEventArgs e)
        {
            mainWindow.Main.Content = new FilmsPage(mainWindow);
        }

        private void PressButtonCheck(object sender, RoutedEventArgs e)
        {
            mainWindow.Main.Content = new CheckCodePage(mainWindow);
        }
    }
}
