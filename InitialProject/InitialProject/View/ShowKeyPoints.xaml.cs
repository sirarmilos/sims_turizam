using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Linq;
using System.Security.Policy;
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
    /// Interaction logic for ShowKeyPoints.xaml
    /// </summary>
    public partial class ShowKeyPoints : Window
    {
        public static ObservableCollection<TourKeyPoint> tourKeyPoints { get; set; }

        private readonly TourGuidenceService tourGuidenceService;

        public string TitleText
        {
            get;
            set;
        }

        public ObservableCollection<string> GuestsList
        {
            get;
            set;
        }
        public ShowKeyPoints(TourGuidence tourGuidence)
        {
            InitializeComponent();
            DataContext = this;
            TourKeyPointService tourKeyPointService = new TourKeyPointService();
            tourGuidenceService = new TourGuidenceService();
            tourKeyPoints = new ObservableCollection<TourKeyPoint>(tourKeyPointService.FindByTourGuidance(tourGuidence.Id));
            TitleText = "Tour Key Points for " + tourGuidence.Tour.TourName;
            
            GuestsList = new ObservableCollection<string>();
            GuestsList.Add("Guest21");
            GuestsList.Add("Guest23");
            GuestsList.Add("Guest22");
            GuestsDataGrid.ItemsSource = null;
            GuestsDataGrid.Visibility = Visibility.Hidden;
            Label1.Visibility = Visibility.Hidden;

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

        private void GoToPreviousWindow(object sender, RoutedEventArgs e)
        {
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
                GuideStart2 window = new GuideStart2("Guide1", tg);
                window.Show();
                Close();
            }
            else
            {
                GuideStart1 window = new GuideStart1("Guide1");
                window.Show();
                Close();
            }
        }

        private void ViewGuestsDataGrid(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            TourKeyPoint selectedItem = (TourKeyPoint)clickedButton.DataContext;
            if(selectedItem == dataGridKP.Items[0])
            {
                Label1.Visibility = Visibility.Hidden;
                GuestsDataGrid.ItemsSource = GuestsList;
                GuestsDataGrid.Visibility = Visibility.Visible;
            }
            else
            {
                GuestsDataGrid.Visibility = Visibility.Hidden;
                Label1.Visibility = Visibility.Visible;
            }
            
        }

        
    }
}
