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
    /// Interaction logic for GuideStart2.xaml
    /// </summary>
    public partial class GuideStart2 : Window
    {
        public TourGuidence TourGuidence { get; set; }

        private readonly TourGuidenceService tourGuidenceService;

        private string guide;

        public string Guide
        {
            get { return guide; }
            set
            {
                guide = value;
            }
        }

        public string WelcomeText
        {
            get; set;
        }

        public GuideStart2(string username, TourGuidence tourGuidence)
        {
            InitializeComponent();
            DataContext = this;
            Guide = username;
            TourGuidence = tourGuidence;
            tourGuidenceService = new TourGuidenceService();
            WelcomeText = "WELCOME, " + Guide;
        }

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            LoginForm window = new LoginForm();
            window.Show();
            Close();
        }

        private void GoToMostPopularTour(object sender, RoutedEventArgs e)
        {
            ShowMostPopularTour window = new ShowMostPopularTour();
            window.Show();
            Close();
        }

        private void MoreInfo(object sender, RoutedEventArgs e)
        {
            TourGuidenceInformation window = new TourGuidenceInformation(TourGuidence);
            window.Show();
            Close();
        }

        private void GoToAllTourOccurences(object sender, RoutedEventArgs e)
        {
            AllTourOccurences window = new AllTourOccurences();
            window.Show();
            Close();
        }

        private void GoToHomePage(object sender, RoutedEventArgs e)
        {
            TourGuidence tg = tourGuidenceService.CheckIfStartedAndNotFinished();
            if (tg != null)
            {
                GuideStart2 window = new GuideStart2(Guide, tg);
                window.Show();
                Close();
            }
            else
            {
                GuideStart1 window = new GuideStart1(Guide);
                window.Show();
                Close();
            }
        }

        private void GoToMarkReachedKeyPoints(object sender, RoutedEventArgs e)
        {
            GuideMarkKeyPoints window = new GuideMarkKeyPoints(TourGuidence, Guide);
            window.Show();
            Close();
        }

        private void GoToCreateNewTour(object sender, RoutedEventArgs e)
        {
            GuideCreateNewTour window = new GuideCreateNewTour(Guide);
            window.Show();
            Close();
        }

        private void GoToTourRequests(object sender, RoutedEventArgs e)
        {
            SearchAndShowTourRequests window = new SearchAndShowTourRequests(Guide);
            window.Show();
            Close();
        }
    }
}
