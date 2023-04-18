using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for OwnerManageBookingMoveRequests.xaml
    /// </summary>
    public partial class OwnerManageBookingMoveRequests : Window
    {
        private readonly ReservationReschedulingRequestService reservationReschedulingRequestService;

        public string OwnerUsername
        {
            get;
            set;
        }

        private string guestUsername;
        private string accommodationName;
        private DateTime oldStartDate;
        private DateTime oldEndDate;
        private DateTime newStartDate;
        private DateTime newEndDate;
        private string newDateAvailable;

        public string GuestUsername
        {
            get { return guestUsername; }
            set
            {
                guestUsername = value;
            }
        }

        public string AccommodationName
        {
            get { return accommodationName; }
            set
            {
                accommodationName = value;
            }
        }

        public DateTime OldStartDate
        {
            get { return oldStartDate; }
            set
            {
                oldStartDate = value;
            }
        }

        public DateTime OldEndtDate
        {
            get { return oldEndDate; }
            set
            {
                oldEndDate = value;
            }
        }

        public DateTime NewStartDate
        {
            get { return newStartDate; }
            set
            {
                newStartDate = value;
            }
        }

        public DateTime NewEndDate
        {
            get { return newEndDate; }
            set
            {
                newEndDate = value;
            }
        }

        public string NewDateAvailable
        {
            get { return newDateAvailable; }
            set
            {
                newDateAvailable = value;
            }
        }

        public OwnerBookingMoveRequestsDTO SelectedBookingMoveRequest
        {
            get;
            set;
        }

        public List<OwnerBookingMoveRequestsDTO> OwnerBookingMoveRequestsDTOs
        {
            get;
            set;
        }

        public OwnerManageBookingMoveRequests(string ownerUsername, string ownerHeader)
        {
            InitializeComponent();

            OwnerUsername = ownerUsername;

            DataContext = this;

            reservationReschedulingRequestService = new ReservationReschedulingRequestService(OwnerUsername);

            OwnerBookingMoveRequestsDTOs = new List<OwnerBookingMoveRequestsDTO>();

            OwnerBookingMoveRequestsDTOs = reservationReschedulingRequestService.FindPendingRequests();

            SetDefaultValue();

            SetMenu(ownerHeader);
        }

        private void SetMenu(string ownerHeader)
        {
            usernameAndSuperOwner.Header = ownerHeader;

            rateGuestsNotifications.Header = "Number of unrated guests: " + reservationReschedulingRequestService.FindNumberOfUnratedGuests(OwnerUsername) + ".";
        }

        private void SetDefaultValue()
        {
            buttonAcceptRequest.IsEnabled = false;
            buttonDeclineRequest.IsEnabled = false;
            SelectedBookingMoveRequest = null;
        }

        private void AcceptRequest(object sender, RoutedEventArgs e)
        {
            OwnerBookingMoveRequestsDTOs = reservationReschedulingRequestService.SaveAcceptedRequest(SelectedBookingMoveRequest);

            dgBookingMoveRequests.Items.Refresh();

            dgBookingMoveRequests.ItemsSource = OwnerBookingMoveRequestsDTOs;
        }

        private void DeclineRequest(object sender, RoutedEventArgs e)
        {
            DeclineBookingMoveRequest window = new DeclineBookingMoveRequest(reservationReschedulingRequestService, this);

            window.ShowDialog();
        }

        private void AcceptRequestButtonEnable(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedBookingMoveRequest == null)
            {
                buttonAcceptRequest.IsEnabled = false;
                buttonDeclineRequest.IsEnabled = false;
            }
            else
            {
                buttonAcceptRequest.IsEnabled = true;
                buttonDeclineRequest.IsEnabled = true;
            }
        }

        void LoadingRowForDgBookingMoveRequests(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void GoToAddNewAccommodation(object sender, RoutedEventArgs e)
        {
            AddNewAccommodation window = new AddNewAccommodation(OwnerUsername);
            window.ShowDialog();
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

        private void GoToShowOwnerManageBookingMoveRequests(object sender, RoutedEventArgs e)
        {
            OwnerManageBookingMoveRequests window = new OwnerManageBookingMoveRequests(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            LoginForm window = new LoginForm();
            window.Show();
            Close();
        }
    }
}
