using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for GuideMarkKeyPoints.xaml
    /// </summary>
    public partial class GuideMarkKeyPoints : Window
    {

        private string guide;

        public string Guide
        {
            get { return guide; }
            set
            {
                guide = value;
            }
        }

        public TourGuidence TourGuidence { get; set; }

        public static ObservableCollection<TourKeyPoint> tourKeyPoints { get; set; }

        public GuideMarkKeyPoints(TourGuidence guidence, string guideUsername)
        {
            InitializeComponent();
            DataContext = this;
            TourGuidence = guidence;
            Guide = guideUsername;
            TourKeyPointService tourKeyPointService = new TourKeyPointService();
            tourKeyPoints = new ObservableCollection<TourKeyPoint>(tourKeyPointService.FindByTourGuidance(guidence.Id));
            tourKeyPoints[0].Passed = true;

        }

        private void GoToMarkGuests(object sender, RoutedEventArgs e)
        {
            TourKeyPoint selectedItem = (TourKeyPoint)dataGridKP.SelectedItem;
            GuideMarkGuests window = new GuideMarkGuests(selectedItem, TourGuidence, Guide);
            window.Show();

        }

        private void FinishTourGuidence(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to finish Tour before reaching last key point?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            TourGuidenceService tourGuidenceService = new TourGuidenceService();

            if (result == MessageBoxResult.Yes)
            {
                tourGuidenceService.UpdateFinishedField(TourGuidence.Id);
                MessageBox.Show("Tour successfully finished");

                GuideStart1 window = new GuideStart1(Guide);
                this.Close();
                window.Show();

            }
            else if (result == MessageBoxResult.No)
            {
                return;
            }


            /*TourGuidenceService tourGuidenceService = new TourGuidenceService();
            if (TourGuidence.Started == false)
            {
                MessageBox.Show("Start your tour first!!");
            }
            else
            {
                tourGuidenceService.UpdateFinishedField(TourGuidence.Id);
                MessageBox.Show("Tour successfully finished");
                
                GuideStart1 window = new GuideStart1(Guide);
                this.Close();
                window.Show();
            }*/


        }

        private void SaveCheckedKeyPoints(object sender, RoutedEventArgs e)
        {
            TourKeyPointRepository tourKeyPointRepository = new TourKeyPointRepository();
            TourGuidenceService tourGuidenceService = new TourGuidenceService();
            if (TourGuidence.Started == true)
            {
                tourKeyPointRepository.UpdateCheckedKeyPoints(tourKeyPoints.ToList());
                if (tourKeyPoints[tourKeyPoints.Count - 1].Passed == true)
                {
                    tourGuidenceService.UpdateFinishedField(TourGuidence.Id);
                }
                GuideStart2 window = new GuideStart2(Guide, TourGuidence);
                this.Close();
                window.Show();
            }
            else
            {
                MessageBox.Show("First start your tour!!!");
            }

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


        private void GoToAllTourOccurences(object sender, RoutedEventArgs e)
        {
            AllTourOccurences window = new AllTourOccurences();
            window.Show();
            Close();
        }

        private void GoToHomePage(object sender, RoutedEventArgs e)
        {
            TourGuidenceService tourGuidenceService = new TourGuidenceService();
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
