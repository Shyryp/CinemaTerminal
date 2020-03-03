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
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace CinemaTerminal
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Film film;
        public Time time;
        public Hall hall;
        public int places;
        public string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Cimena;Integrated Security=True";

        
        public MainWindow()
        {
            InitializeComponent();

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.WindowState = WindowState.Maximized;
            Main.Content = new StartPage(this);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void InsertImage(object sender, RoutedEventArgs e)
        {
            int i = 0;
            while (i != 5)
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"UPDATE [Film] SET [Poster] = @poster WHERE [Id] = @id";

                command.Parameters.Add("@id", SqlDbType.Int);
                command.Parameters.Add("@poster", SqlDbType.Image, 1000000);

                // путь к файлу для загрузки
                string filename = @"PicturesPoster/shazam.jpg"; ;
                if (i == 0)
                {
                    filename = @"PicturesPoster/shazam.jpg";
                }
                else if (i == 1)
                {
                    filename = @"PicturesPoster/kladbishe.jpg";
                }
                else if (i == 2)
                {
                    filename = @"PicturesPoster/plazh.jpg";
                }
                else if (i == 3)
                {
                    filename = @"PicturesPoster/dambo.jpg";
                }
                else if (i == 4)
                {
                    filename = @"PicturesPoster/mi.jpg"; ;
                }
                // заголовок файла
                int id = i;
                i++;
                byte[] poster;
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    poster = new byte[fs.Length];
                    fs.Read(poster, 0, poster.Length);
                }
                // передаем данные в команду через параметры
                command.Parameters["@Id"].Value = id;
                command.Parameters["@poster"].Value = poster;

                command.ExecuteNonQuery();
            }
        }
    }
}
