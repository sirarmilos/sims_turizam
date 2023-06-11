using InitialProject.Dto;
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
    /// Interaction logic for Guest2ComplexRequestSuccess.xaml
    /// </summary>
    public partial class Guest2ComplexRequestSuccess : Page
    {
        public Guest2ComplexRequestSuccess()
        {
            InitializeComponent();
        }

        private void HyperLinkCreateMoreComplex(object sender, RoutedEventArgs e)
        {
            Guest2ComplexTourRequestCreation guest2ComplexTourRequestCreation = new Guest2ComplexTourRequestCreation(UserClass.Username);
            NavigationService.Navigate(guest2ComplexTourRequestCreation);
        }


        private void HyperLinkDisplayComplex(object sender, RoutedEventArgs e)
        {
            Guest2ComplexTourDisplay guest2ComplexTourDisplay = new Guest2ComplexTourDisplay(UserClass.Username);
            NavigationService.Navigate(guest2ComplexTourDisplay);
        }
    }
}
