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
    /// Interaction logic for WizardPage2.xaml
    /// </summary>
    public partial class WizardPage2 : Page
    {
        public WizardPage2()
        {
            InitializeComponent();
        }

        private void Footer_Back(object sender, RoutedEventArgs e)
        {
            // Navigate to the previous page
            this.NavigationService.Navigate(new WizardPage1());
        }

        private void Footer_Next(object sender, RoutedEventArgs e)
        {
            // Navigate to the next page
            this.NavigationService.Navigate(new WizardPage3());
        }

        private void Footer_Cancel(object sender, RoutedEventArgs e)
        {
            // Cancel the wizard and close the window
        }
    }
}
