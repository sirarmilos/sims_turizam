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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest2RegistrationPage.xaml
    /// </summary>
    public partial class Guest2RegistrationPage : Page
    {
        public Guest2RegistrationPage()
        {
            InitializeComponent();
            labelWarning.Visibility = Visibility.Hidden;
        }

        private void HyperLinkSignIn(object sender, RoutedEventArgs e)
        {
            Guest2LogInPage guest2LogInPage = new Guest2LogInPage();
            NavigationService.Navigate(guest2LogInPage);
        }

        private void HyperLinkRegister(object sender, RoutedEventArgs e)
        {
            Guest2RegisterPage guest2RegisterPage = new Guest2RegisterPage();
            NavigationService.Navigate(guest2RegisterPage);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserService userService = new UserService();
            string username = username1.Text;

            if (userService.IsUsernameExist(username) == false)
            {
                labelWarning.Visibility = Visibility.Visible;
            }
            else
            {
                Guest2LogInPage guest2LogInPage = new Guest2LogInPage();
                NavigationService.Navigate(guest2LogInPage);
            }
        }
    }
}
