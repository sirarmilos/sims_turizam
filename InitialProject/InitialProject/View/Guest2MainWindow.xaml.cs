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
    /// Interaction logic for Guest2MainWindow.xaml
    /// </summary>
    public partial class Guest2MainWindow : Window
    {
        private string Username;
        public Guest2MainWindow(string username)
        {
            InitializeComponent();
            Username = username;

            Guest2PageTours guest2PageTours = new Guest2PageTours();
            page.Content = guest2PageTours;

        }


        private void ToursButtonClick(object sender, RoutedEventArgs e)
        {
            Guest2PageTours guest2PageTours = new Guest2PageTours();
            page.Content = guest2PageTours;

           
        }
    }
}
