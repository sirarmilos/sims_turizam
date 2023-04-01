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
    /// Interaction logic for Guest1Start.xaml
    /// </summary>
    public partial class Guest1Start : Window
    {
        public Guest1Start()
        {
            InitializeComponent();
        }

        private void GoToSearchAndShowAccommodation(object sender, RoutedEventArgs e)
        {
            SearchAndShowAccommodations window = new SearchAndShowAccommodations();
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
