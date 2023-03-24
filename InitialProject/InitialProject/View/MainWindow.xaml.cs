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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GoToAddNewAccommodation(object sender, RoutedEventArgs e)
        {
            AddNewAccommodation window = new AddNewAccommodation();
            window.Show();
        }

        private void GoToSearchAndShowAccommodation(object sender, RoutedEventArgs e)
        {
            SearchAndShowAccommodations window = new SearchAndShowAccommodations();
            window.Show();
        }

        private void GoToSearchAndShowTours(object sender, RoutedEventArgs e)
        {
            SearchAndShowTours window = new SearchAndShowTours();
            window.Show();
        }

        private void GoToAddNewTour(object sender, RoutedEventArgs e)
        {
            AddNewTour window = new AddNewTour();
            window.Show();
        }

        private void GoToRateGuests(object sender, RoutedEventArgs e)
        {
            RateGuests window = new RateGuests();
            if (window.dgRateGuests.Items.Count > 0)
            {
                window.Show();
            }
        }

        private void GoToShowTourGuidences(object sender, RoutedEventArgs e)
        {
            ShowTourGuidences window = new ShowTourGuidences();
            window.Show();
        }
    }
}
