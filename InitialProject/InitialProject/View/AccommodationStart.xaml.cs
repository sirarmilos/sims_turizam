using InitialProject.DTO;
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
using InitialProject.View;
using InitialProject.Model;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AccommodationStart.xaml
    /// </summary>
    public partial class AccommodationStart : Window
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

        public ObservableCollection<ShowAccommodationDTO> ShowAccommodationDTOs
        {
            get;
            set;
        }

        public ShowAccommodationDTO SelectedShowAccommodationDTO
        {
            get;
            set;
        }

        public AccommodationStart(string ownerUsername)
        {
            InitializeComponent();

            OwnerUsername = ownerUsername;

            DataContext = this;

            accommodationService = new AccommodationService(OwnerUsername);

            SetDefaultValue();

            SetMenu();

            ShowAccommodationDTOs = accommodationService.FindOwnerAccommodations(OwnerUsername);

            dgAccommodations.ItemsSource = ShowAccommodationDTOs;
        }

        private void SetMenu()
        {
            usernameAndSuperOwner.Header = OwnerUsername + CheckSuperType();

            rateGuestsNotifications.Header = "Number of unrated guests: " + accommodationService.FindNumberOfUnratedGuests(OwnerUsername) + ".";

            UnreadCancelledReservations = new List<string>();

            UnreadCancelledReservations = accommodationService.FindUnreadCancelledReservations(OwnerUsername);
        }

        private void SetDefaultValue()
        {
            ShowAccommodationDTOs = new ObservableCollection<ShowAccommodationDTO>();
            SelectedShowAccommodationDTO = null;

            buttonRenovate.IsEnabled = false;
            buttonStatistics.IsEnabled = false;
        }

        private string CheckSuperType()
        {
            string superType = string.Empty;

            if (accommodationService.FindSuperTypeByOwnerName(OwnerUsername).Equals("super") == true)
            {
                superType = " (Super owner)";
            }

            return superType;
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

        private void AddNewAccommodation(object sender, RoutedEventArgs e)
        {
            InitialProject.View.AddNewAccommodation window = new AddNewAccommodation(OwnerUsername);
            window.ShowDialog();
        }

        private void ButtonsEnable(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedShowAccommodationDTO == null)
            {
                buttonRenovate.IsEnabled = false;
                buttonStatistics.IsEnabled = false;
            }
            else
            {
                buttonRenovate.IsEnabled = true;
                buttonStatistics.IsEnabled = true;
            }
        }

        void LoadingRowForDgAccommodations(object sender, DataGridRowEventArgs e)
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

        private void RenovateAccommodation(object sender, RoutedEventArgs e)
        {

        }

        private void AccommodationStatistics(object sender, RoutedEventArgs e)
        {
            ShowStatisticsAccommodationDTO showStatisticsAccommodationDTO = accommodationService.FindSelectedAccommodation(SelectedShowAccommodationDTO.Id);

            InitialProject.View.AccommodationStatistics window = new AccommodationStatistics(OwnerUsername, usernameAndSuperOwner.Header.ToString(), showStatisticsAccommodationDTO);
            window.Show();
            Close();
        }
    }
}
