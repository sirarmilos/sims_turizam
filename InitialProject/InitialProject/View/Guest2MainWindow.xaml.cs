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

    public partial class Guest2MainWindow : Window
    {
        private string Username;

        public Page Page { get; set; }

        public Guest2MainWindow(string username)
        {
            InitializeComponent();
            Username = username;

            Guest2PageTours guest2PageTours = new Guest2PageTours(Username);
            mainFrame.Content = guest2PageTours;

        }


        private void ToursButtonClick(object sender, RoutedEventArgs e)
        {
            Guest2PageTours guest2PageTours = new Guest2PageTours(Username);
            mainFrame.Content = guest2PageTours;
        }

        private void ProfilePage(object sender, RoutedEventArgs e)
        {
            Guest2ProfilePreview profilePreview = new Guest2ProfilePreview(Username);
            mainFrame.Content = profilePreview;

        }
    }
}
