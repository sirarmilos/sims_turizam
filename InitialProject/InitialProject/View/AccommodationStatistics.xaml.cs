using InitialProject.DTO;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for AccommodationStatistics.xaml
    /// </summary>
    public partial class AccommodationStatistics : Window
    {
        private readonly AccommodationService accommodationService;

        public string OwnerUsername
        {
            get;
            set;
        }

        public List<string> UnreadCancelledReservations
        {
            get;
            set;
        }

        public ShowStatisticsAccommodationDTO ShowStatisticsAccommodationDTO
        {
            get;
            set;
        }

        public List<int> Years
        {
            get;
            set;
        }

        public AccommodationStatistics(string ownerUsername, string ownerHeader, ShowStatisticsAccommodationDTO showStatisticsAccommodationDTO)
        {
            InitializeComponent();

            OwnerUsername = ownerUsername;

            DataContext = this;

            accommodationService = new AccommodationService(OwnerUsername);

            ShowStatisticsAccommodationDTO = showStatisticsAccommodationDTO;

            SetDefaultValue();

            SetMenu(ownerHeader);
        }

        private void SetMenu(string ownerHeader)
        {
            usernameAndSuperOwner.Header = ownerHeader;

            rateGuestsNotifications.Header = "Number of unrated guests: " + accommodationService.FindNumberOfUnratedGuests(OwnerUsername) + ".";

            UnreadCancelledReservations = new List<string>();

            UnreadCancelledReservations = accommodationService.FindUnreadCancelledReservations(OwnerUsername);
        }

        private void SetDefaultValue()
        {
            /*
            - prikazuje se statistika po godinama prvo
            - broj rezervacija
            - broj otkazivanja rezervacija
            - broj pomeranja rezervacija
            - broj preporuka za renoviranje
            */

            /*
            - reservations.csv
            - canceledreservations.csv
            - rescheduledreservations.csv
            - renovationrecommedations.csv
            */

            /*
            - sve to trazim za ovaj smestaj sto je dosao
            */

            /*
            - pronaci koje sve godine treba prikazati u combobox-u
            */

            List<int> yearsReservations = accommodationService.FindAccommodationReservationsYears(ShowStatisticsAccommodationDTO.Id);
            List<int> yearsCanceledReservations = accommodationService.FindAccommodationCanceledReservationsYears(ShowStatisticsAccommodationDTO.Id); //
            List<int> yearsRescheduledReservations = accommodationService.FindAccommodationRescheduledReservationsYears(ShowStatisticsAccommodationDTO.Id); //
            // List<int> yearsRenovationRecommedations = accommodationService.FindAccommodationRenovationRecommedationsYears(ShowStatisticsAccommodationDTO.Id);

            List<int> years = new List<int>();

            Years = yearsReservations.Union(yearsCanceledReservations).Union(yearsRescheduledReservations).ToList();

            Years.Sort();

            foreach(int year in years)
            {
                MessageBox.Show(year.ToString());
            }
        }

        private void ReadCancelledReservationNotification(object sender, RoutedEventArgs e)
        {
            string viewedCancelledReservation = ((MenuItem)sender).Header.ToString();

            if (viewedCancelledReservation.Equals("There are currently no new booking cancellations.") == false)
            {
                accommodationService.SaveViewedCancelledReservation(FindDTO(viewedCancelledReservation));

                UnreadCancelledReservations = accommodationService.FindUnreadCancelledReservations(OwnerUsername);

                cancelledReservationsNotificationsList.DataContext = UnreadCancelledReservations;
            }
        }

        private CancelledReservationsNotificationDTO FindDTO(string viewedCancelledReservation)
        {
            string accommodationName = viewedCancelledReservation.Split(":")[0];
            DateTime reservationStartDate = Convert.ToDateTime(viewedCancelledReservation.Split(" ")[1]);
            DateTime reservationEndDate = Convert.ToDateTime(viewedCancelledReservation.Split(" ")[3]);

            CancelledReservationsNotificationDTO cancelledReservationsNotificationDTO = new CancelledReservationsNotificationDTO(accommodationName, reservationStartDate, reservationEndDate);

            return cancelledReservationsNotificationDTO;
        }

        private void labeltbFocus(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                var label = (Label)sender;
                Keyboard.Focus(label.Target);
            }
        }

        void LoadingRowForDgImages(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void GoToOwnerHomePageLogin(object sender, RoutedEventArgs e)
        {
            OwnerHomePageLogin window = new OwnerHomePageLogin(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void GoToAccommodationStart(object sender, RoutedEventArgs e)
        {
            AccommodationStart window = new AccommodationStart(OwnerUsername);
            window.Show();
            Close();
        }

        private void GoToShowOwnerManageBookingMoveRequests(object sender, RoutedEventArgs e)
        {
            OwnerManageBookingMoveRequests window = new OwnerManageBookingMoveRequests(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void GoToShowAndCancellationRenovation(object sender, RoutedEventArgs e)
        {
            ShowAndCancellationRenovation window = new ShowAndCancellationRenovation(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void GoToRateGuests(object sender, RoutedEventArgs e)
        {
            RateGuests window = new RateGuests(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void GoToShowGuestReviews(object sender, RoutedEventArgs e)
        {
            ShowGuestReviews window = new ShowGuestReviews(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void GoToOwnerForum(object sender, RoutedEventArgs e)
        {
            OwnerForum window = new OwnerForum(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void GoToOwnerHomePageNotLogin(object sender, RoutedEventArgs e)
        {
            OwnerHomePageNotLogin window = new OwnerHomePageNotLogin();
            window.Show();
            Close();
        }
    }
}
