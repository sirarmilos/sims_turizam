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

        public List<string> UnreadCancelledReservations
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

        private void OwnerHomePageLogin_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OwnerHomePageLogin_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OwnerHomePageLogin window = new OwnerHomePageLogin(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void AccommodationStart_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AccommodationStart_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AccommodationStart window = new AccommodationStart(OwnerUsername);
            window.Show();
            Close();
        }

        private void ShowOwnerManageBookingMoveRequests_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ShowOwnerManageBookingMoveRequests_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OwnerManageBookingMoveRequests window = new OwnerManageBookingMoveRequests(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void ShowAndCancellationRenovation_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ShowAndCancellationRenovation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ShowAndCancellationRenovation window = new ShowAndCancellationRenovation(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void RateGuests_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void RateGuests_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RateGuests window = new RateGuests(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void ShowGuestReviews_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ShowGuestReviews_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ShowGuestReviews window = new ShowGuestReviews(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void OwnerForum_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OwnerForum_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OwnerForum window = new OwnerForum(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void Logout_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Logout_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LoginForm window = new LoginForm();
            window.Show();
            Close();
        }

        private void Notifications_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Notifications_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            notifications.IsSubmenuOpen = true;
            rateGuestsNotifications.Focus();
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

            UnreadCancelledReservations = new List<string>();

            UnreadCancelledReservations = reservationReschedulingRequestService.FindUnreadCancelledReservations(OwnerUsername);
        }

        private void SetDefaultValue()
        {
            buttonAcceptRequest.IsEnabled = false;
            buttonDeclineRequest.IsEnabled = false;
            SelectedBookingMoveRequest = null;
        }

        private void ReadCancelledReservationNotification_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ReadCancelledReservationNotification_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string viewedCancelledReservation = ((MenuItem)sender).Header.ToString();

            if (viewedCancelledReservation.Equals("There are currently no new booking cancellations.") == false)
            {
                reservationReschedulingRequestService.SaveViewedCancelledReservation(FindDTO(viewedCancelledReservation));

                UnreadCancelledReservations = reservationReschedulingRequestService.FindUnreadCancelledReservations(OwnerUsername);

                cancelledReservationsNotificationsList.DataContext = UnreadCancelledReservations;
            }
        }

        private void ReadCancelledReservationNotification(object sender, RoutedEventArgs e)
        {
            string viewedCancelledReservation = ((MenuItem)sender).Header.ToString();

            if (viewedCancelledReservation.Equals("There are currently no new booking cancellations.") == false)
            {
                reservationReschedulingRequestService.SaveViewedCancelledReservation(FindDTO(viewedCancelledReservation));

                UnreadCancelledReservations = reservationReschedulingRequestService.FindUnreadCancelledReservations(OwnerUsername);

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

        private void AcceptRequest_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(SelectedBookingMoveRequest == null)
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
        }

        private void AcceptRequest_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OwnerBookingMoveRequestsDTOs = reservationReschedulingRequestService.SaveAcceptedRequest(SelectedBookingMoveRequest);

            dgBookingMoveRequests.Items.Refresh();

            dgBookingMoveRequests.ItemsSource = OwnerBookingMoveRequestsDTOs;
        }

        private void DeclineRequest_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedBookingMoveRequest == null)
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
        }

        private void DeclineRequest_Executed(object sender, ExecutedRoutedEventArgs e)
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
    }
}
