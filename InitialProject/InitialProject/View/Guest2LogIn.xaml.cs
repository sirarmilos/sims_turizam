using InitialProject.Service;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest2LogIn.xaml
    /// </summary>
    public partial class Guest2LogIn
    {
        private readonly UserService userService;
        public Guest2LogIn()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = username1.Text;
            string password = password1.Text;
            UserService userService = new UserService();
            UserClass.Username = username;

            Guest2MainWindow guest2MainWindow = new Guest2MainWindow(username);
            guest2MainWindow.Left = Left;
            guest2MainWindow.Top = Top;
            guest2MainWindow.Width = Width;
            guest2MainWindow.Height = Height;

            guest2MainWindow.WindowStartupLocation = WindowStartupLocation.Manual;

            Hide();
            guest2MainWindow.Show();
            Dispatcher.BeginInvoke(new Action(() => Close()), DispatcherPriority.ApplicationIdle);
        }

    }
}
