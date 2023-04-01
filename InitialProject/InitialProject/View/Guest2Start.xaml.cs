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
        public Guest2Start()
        {
            InitializeComponent();
        }

        private void GoToSearchAndShowTours(object sender, RoutedEventArgs e)
        {
            SearchAndShowTours window = new SearchAndShowTours();
            window.Show();
        }
        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            LoginForm window = new LoginForm();
            window.Show();
            Close();
        }
    }
}
