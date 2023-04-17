using InitialProject.DTO;
using InitialProject.Service;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Prism.Commands;
using GalaSoft.MvvmLight.Command;

namespace InitialProject.ViewModel
{
    public class ShowReservationsViewModel : Window
    {
        private ReservationService reservationService;
        private string accommodationName;
        private DateTime startDate;
        private DateTime endDate;
        private int guestsNumber;
        private string guest1;

        public int GuestsNumber
        {
            get { return guestsNumber; }
            set
            {
                guestsNumber = value;
            }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
            }
        }

        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
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

        public List<ShowReservationDTO> ShowReservationDTOs
        {
            get;
            set;
        }

        private bool notification;
        public bool Notification
        {
            get { return notification; }
            set
            {
                notification = value;
            }
        }

        public string NotificationEnable
        {
            get;
            set;
        }

        private void CheckNotification()
        {
            if (Notification)
            {
                NotificationEnable = "You have notification!";
                // NotificationMenuItem.Visibility = Visibility.Visible;
            }
            else
            {
                NotificationEnable = "";
                // NotificationMenuItem.Visibility = Visibility.Collapsed;
            }
        }

        public ICommand CancelCommand { get; set; }

        public ICommand RescheduleCommand { get; set; }

        public DelegateCommand GoToGuest1StartCommand { get; }

        public DelegateCommand GoToSearchAndShowAccommodationsCommand { get; }

        public DelegateCommand GoToCreateReviewCommand { get; }

        public DelegateCommand GoToGuest1RequestsCommand { get; }

        public DelegateCommand GoToLogoutCommand { get; }

        public DelegateCommand CheckNotificationCommand { get; }

        public ShowReservationsViewModel(string username)
        {
            GoToGuest1StartCommand = new DelegateCommand(GoToGuest1Start);
            GoToSearchAndShowAccommodationsCommand = new DelegateCommand(GoToSearchAndShowAccommodations);
            GoToCreateReviewCommand = new DelegateCommand(GoToCreateReview);
            GoToGuest1RequestsCommand = new DelegateCommand(GoToGuest1Requests);
            GoToLogoutCommand = new DelegateCommand(GoToLogout);

            Guest1 = username;

            reservationService = new ReservationService(Guest1);
            Notification = reservationService.Guest1HasNotification();
            // CheckNotification();
            CheckNotificationCommand = new DelegateCommand(CheckNotification);

            ShowReservationDTOs = reservationService.FindAll(Guest1);
            CancelCommand = new RelayCommand<ShowReservationDTO>(Cancel);
            RescheduleCommand = new RelayCommand<ShowReservationDTO>(Reschedule);
        }

        private void Cancel(ShowReservationDTO showReservationDTO)
        {
            bool isReservationCancelEligible = reservationService.IsRemoved(showReservationDTO);

            if (showReservationDTO.StartDate <= DateTime.Now)
            {
                MessageBox.Show("Reservation is not eligible for cancellation since the start date is in the past.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!isReservationCancelEligible)
            {
                MessageBox.Show("Reservation is not eligible for cancellation. Since the number of left cancelation days is 0.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ShowReservationDTOs.Remove(showReservationDTO);
            // dgShowReservations.Items.Refresh();
        }

        private void Reschedule(ShowReservationDTO showReservationDTO)
        {

            if (showReservationDTO.StartDate < DateTime.Now)
            {
                MessageBox.Show("Reservation is not eligible for rescheduling since it already started.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CreateReservationReschedulingRequest window = new CreateReservationReschedulingRequest(showReservationDTO, Guest1);
            window.Show();
            // Close();
        }

        private void GoToGuest1Start()
        {
            Guest1Start window = new Guest1Start(Guest1);
            window.Show();
        }

        private void GoToSearchAndShowAccommodations()
        {
            SearchAndShowAccommodations window = new SearchAndShowAccommodations(Guest1);
            window.Show();
        }

        private void GoToCreateReview()
        {
            CreateReview window = new CreateReview(Guest1);
            window.Show();
        }

        private void GoToGuest1Requests()
        {
            Guest1Requests window = new Guest1Requests(Guest1);
            window.Show();
        }


        private void GoToLogout()
        {
            LoginForm window = new LoginForm();
            window.Show();
        }
    }
}
