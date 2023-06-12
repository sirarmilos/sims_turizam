using InitialProject.Model;
using InitialProject.Service;
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
    /// Interaction logic for CreateTourForRequestStatistics.xaml
    /// </summary>
    public partial class CreateTourForRequestStatistics : Window
    {
        private readonly TourRequestService tourRequestService;

        public string GuideUsername
        {
            get;
            set;
        }

        public CreateTourForRequestStatistics(string username)
        {
            InitializeComponent();
            DataContext = this;
            tourRequestService = new TourRequestService();
            GuideUsername = username;
        }

        private void MostPopularLocationTourRequest(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to proceed?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Location location = tourRequestService.FindMostFrequentLocationInLastYear();
                GuideCreateNewTour window = new GuideCreateNewTour(GuideUsername, location);
                window.Show();
                Close();
            }
            else if (result == MessageBoxResult.No)
            {
                return;
            }
        }

        private void MostPopularLanguageTourRequest(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to proceed?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Language language = tourRequestService.FindMostFrequentLanguageInLastYear();
                GuideCreateNewTour window = new GuideCreateNewTour(GuideUsername, language);
                window.Show();
                Close();
            }
            else if (result == MessageBoxResult.No)
            {
                return;
            }
        }
    }
}
