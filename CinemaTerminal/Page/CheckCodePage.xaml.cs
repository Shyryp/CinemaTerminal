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
    /// Логика взаимодействия для CheckCodePage.xaml
    /// </summary>
    public partial class CheckCodePage
    {
        MainWindow mainWindow;
        int k;
        int[] numbers = new int[6];
        public CheckCodePage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.mainWindow.bBack.Click += new System.Windows.RoutedEventHandler(this.PressButtonBack);
            this.mainWindow.bBack.Visibility = Visibility.Visible;
            k = 0;
            for (int i = 0; i < 6; i++)
                numbers[i] = 0;
        }
        private void PressButtonBack(object sender, RoutedEventArgs e)
        {
            //TODO : удаляем данные текущей страницы
            this.mainWindow.bBack.Click -= new System.Windows.RoutedEventHandler(this.PressButtonBack);
            mainWindow.Main.Content = new StartPage(mainWindow);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Main.Content = new BasketPage(mainWindow, "Печать билета", "Распечатать");
        }

        private void Button_Click_Number(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (k < 6)
            {
                numbers[k] = Convert.ToInt32(button.Content.ToString());
                String str = "Введите код: ";
                for (int i = 0; i < 6; i++)
                {
                    if (i <= k)
                    {
                        str += " "+ numbers[i] + " ";
                    }
                    else
                    {
                        str += " __";
                    }
                }
                k++;
                codeText.Text = str;
            }
        }

        private void Button_Click_Clear(object sender, RoutedEventArgs e)
        {
            String str = "Введите код: __ __ __ __ __ __";
            codeText.Text = str;
            for (int i = 0; i < 6; i++)
                numbers[i] = 0;
            k = 0;
        }
    }
}
