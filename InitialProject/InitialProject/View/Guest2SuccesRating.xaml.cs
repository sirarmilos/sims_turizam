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
using static InitialProject.View.Guest2MainWindow;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest2SuccesRating.xaml
    /// </summary>
    public partial class Guest2SuccesRating : Page
    {
        private string Username;

        public Guest2SuccesRating(string username)
        {
            InitializeComponent();
            Username = username;
        }


        private void HyperLinkHomePage(object sender, RoutedEventArgs e)
        {
            Guest2PageTours guest2PageTours = new Guest2PageTours(Username);
            NavigationService.Navigate(guest2PageTours);
        }

        private void HyperLinkBackToTourAttendance(object sender, RoutedEventArgs e)
        {
            Guest2TourAttendance guest2TourAttendance = new Guest2TourAttendance();
            NavigationService.Navigate(guest2TourAttendance);   
        }
    }
}
