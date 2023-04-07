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
    /// Interaction logic for GuideStart.xaml
    /// </summary>
    public partial class GuideStart : Window
    {
        public GuideStart()
        {
            InitializeComponent();
        }

        private void GoToAddNewTour(object sender, RoutedEventArgs e)
        {
            AddNewTour window = new AddNewTour();
            window.Show();
        }

        private void GoToShowTourGuidences(object sender, RoutedEventArgs e)
        {
            ShowTourGuidences window = new ShowTourGuidences();
            window.Show();
        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            LoginForm window = new LoginForm();
            window.Show();
            Close();
        }
    }
}
