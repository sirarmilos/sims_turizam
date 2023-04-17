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
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest2Start.xaml
    /// </summary>
    public partial class Guest2Start : Window
    {

        private readonly string username;
        public Guest2Start(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void GoToSearchAndShowTours(object sender, RoutedEventArgs e)
        {
            SearchAndShowTours window = new SearchAndShowTours(username);
            window.Show();
        }
        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            LoginForm window = new LoginForm();
            window.Show();
            Close();
        }

        private void ShowTourAttendance(object sender, RoutedEventArgs e)
        {
            ShowTourAttendance window = new ShowTourAttendance(username);
            window.Show();
        }

        private void ShowUserInfo(object sender, RoutedEventArgs e)
        {
            Guest2Info window = new Guest2Info(username);
            window.Show();
        }

        private void ShowNotifications(object sender, RoutedEventArgs e)
        {
            ShowGuest2Notifications window = new ShowGuest2Notifications(username);
            window.Show();
        }

        private void RateGuide(object sender, RoutedEventArgs e)
        {
           // RateGuide window = new RateGuide(username);
            //window.Show();
        }
    }
}
