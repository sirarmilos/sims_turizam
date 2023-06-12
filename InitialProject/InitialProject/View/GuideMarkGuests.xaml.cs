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
    /// Interaction logic for GuideMarkGuests.xaml
    /// </summary>
    public partial class GuideMarkGuests : Window
    {
        public TourKeyPoint TourKeyPoint { get; set; }

        public TourGuidence TourGuidence { get; set; }

        public static List<Dto.ReservationDisplayDto> tourReservations { get; set; }

        public List<User> users { get; set; }



        public string WelcomeText
        {
            get; set;
        }

        private string guide;

        public string Guide
        {
            get { return guide; }
            set
            {
                guide = value;
            }
        }

        public GuideMarkGuests(TourKeyPoint tkp, TourGuidence tg, string guide)
        {
            InitializeComponent();
            DataContext = this;
            TourKeyPoint = tkp;
            TourGuidence = tg;
            Guide = guide;
            WelcomeText = "Mark New Guests - " + TourKeyPoint.KeyPointName.ToString();
            TourReservationService tourReservationService = new TourReservationService();
            tourReservations = new List<Dto.ReservationDisplayDto>(tourReservationService.GetAllForOneTourGuidence(TourGuidence.Id));
            UserService userService = new UserService();
            users = new List<User>();
            users.Add(userService.FindByUsername("Guest21"));
            //addedGuest.ItemsSource = users;

        }

        private void GoToBack(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void GoToAddGuest(object sender, RoutedEventArgs e)
        {
            UserService userService = new UserService();
            Dto.ReservationDisplayDto selectedItem = (Dto.ReservationDisplayDto)allGuests.SelectedItem;
            users.Add(userService.FindByUsername(selectedItem.userId));
            addedGuest.ItemsSource = users;
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
