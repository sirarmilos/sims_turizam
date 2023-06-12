using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Guest2RegisterPage.xaml
    /// </summary>
    public partial class Guest2RegisterPage : Page
    {
        public Guest2RegisterPage()
        {
            InitializeComponent();
            labelWarning.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserService userService = new UserService();
            string username = username1.Text;

            if (userService.IsUsernameExist(username) == true)
            {
                labelWarning.Visibility = Visibility.Visible;
            }

            else if(!password1.Password.Equals(password1_Copy.Password))
            {
                labelWarning.Visibility = Visibility.Visible;
            }

            else if(!IsEmailFormatValid(username1_Copy.Text))
            {
                labelWarning.Visibility= Visibility.Visible;
            }

            else
            {
                Guest2LogInPage guest2LogInPage = new Guest2LogInPage();
                NavigationService.Navigate(guest2LogInPage);
            }
        }

        private bool IsEmailFormatValid(string email)
        {
            // Regular expression pattern for email validation
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Create a Regex object with the pattern
            Regex regex = new Regex(pattern);

            // Use the IsMatch method to check if the email matches the pattern
            return regex.IsMatch(email);
        }
    }
}
