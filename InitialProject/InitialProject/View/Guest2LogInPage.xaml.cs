using InitialProject.Dto;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest2LogInPage.xaml
    /// </summary>
    public partial class Guest2LogInPage : Page
    {
        public Guest2LogInPage()
        {
            InitializeComponent();
            labelWarning.Visibility = Visibility.Hidden;
        }

        private void HyperLinkRegister(object sender, RoutedEventArgs e)
        {
            Guest2RegisterPage guest2RegisterPage = new Guest2RegisterPage();
            NavigationService.Navigate(guest2RegisterPage);
        }

        private void HyperLinkForgotPassword(object sender, RoutedEventArgs e)
        {
            Guest2RegistrationPage guest2RegistrationPage = new Guest2RegistrationPage();   
            NavigationService.Navigate(guest2RegistrationPage);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window currentWindow = Application.Current.MainWindow;

            string username = username1.Text;
            string password = password1.Password;
            UserService userService = new UserService();
      


            if (userService.IsUsernameExist(username) == false)
            {
                labelWarning.Visibility = Visibility.Visible;
            }
            else if (userService.IsPasswordCorrect(username, password) == false)
            {
                labelWarning.Visibility = Visibility.Visible;
            }
            else
            {
                UserClass.Username = username;
                Guest2MainWindow guest2MainWindow = new Guest2MainWindow(username);
                guest2MainWindow.Left = currentWindow.Left;
                guest2MainWindow.Top = currentWindow.Top;
                guest2MainWindow.Width = currentWindow.Width;
                guest2MainWindow.Height = currentWindow.Height;

                guest2MainWindow.WindowStartupLocation = WindowStartupLocation.Manual;

                currentWindow.Hide();
                guest2MainWindow.Show();
                Dispatcher.BeginInvoke(new Action(() => currentWindow.Close()), DispatcherPriority.ApplicationIdle);
            }
        }
    }
}
