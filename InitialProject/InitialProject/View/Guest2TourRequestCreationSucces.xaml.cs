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
    /// Interaction logic for Guest2TourRequestCreationSucces.xaml
    /// </summary>
    public partial class Guest2TourRequestCreationSucces : Page
    {
        public Guest2TourRequestCreationSucces()
        {
            InitializeComponent();
        }

        private void HyperLinkSeeRequests(object sender, RoutedEventArgs e)
        {
            Guest2DisplayRequestedTours guest2TourRequestsDisplay = new Guest2DisplayRequestedTours(UserClass.Username);
            NavigationService.Navigate(guest2TourRequestsDisplay);
        }

        private void HyperLinkCreateMoreRequests(object sender, RoutedEventArgs e)
        {
            Guest2ComplexTourRequestCreation guest2ComplexTourRequestCreation = new Guest2ComplexTourRequestCreation(UserClass.Username);
            NavigationService.Navigate(guest2ComplexTourRequestCreation);
        }
    }
}
