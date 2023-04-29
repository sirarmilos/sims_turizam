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

        /* public List<string> Years
        {
            get;
            set;
        }*/

        public int SelectedYear
        {
            get;
            set;
        }

        public List<AccommodationStatisticsDataDTO> AccommodationStatisticsDataDTOs
        {
            get;
            set;
        }

        public string MostBusyPeriodTime
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
            SelectedYear = 0;

            Years = new List<int>();

            Years = accommodationService.FindAccommodationYears(ShowStatisticsAccommodationDTO.Id);

            // treba da prebacis da Years bude string da bi moglo da ima i "all year" da bi moglo da se vrati i gleda opet za sve godine kada se jednom promeni na jednu konkretnu godinu
            // ShowMonthsStatistics se nece vise zvati tako jer sada moze da se promeni i na sve godine, i tu ce trebati provera, neki if ako je allyears da radi jedno i ispisuje dole u label-u jedno,
            // a ako je neka konkretna godina onda u tu labelu se upisuje nesto drugo tj. mesec, a ako su sve godine onda se ispise godina

            // Years = accommodationService.FindAccommodationYears(ShowStatisticsAccommodationDTO.Id);

            AccommodationStatisticsDataDTOs = new List<AccommodationStatisticsDataDTO>();

            // kada nema nijedna godina, onda treba da se disable combobox i label-e dole levo i da se desno gde treba da budu grafici ispise poruka da nema nicega za taj accommodation umesto grafikona

            AccommodationStatisticsDataDTOs = accommodationService.FindAccommodationYearStatistics(ShowStatisticsAccommodationDTO.Id, Years);

            MostBusyPeriodTime = accommodationService.FindMostBusyYear(ShowStatisticsAccommodationDTO.Id, Years).ToString();
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

        private void ShowMonthsStatistics(object sender, SelectionChangedEventArgs e)
        {
            AccommodationStatisticsDataDTOs = accommodationService.FindAccommodationMonthStatistics(ShowStatisticsAccommodationDTO.Id, SelectedYear);

            MostBusyPeriodTime = accommodationService.FindMostBusyMonth(ShowStatisticsAccommodationDTO.Id, SelectedYear);

            labelMostBusyPeriodTime.Content = MostBusyPeriodTime; //
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
