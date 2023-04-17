using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Service;

namespace InitialProject.View
{
    public partial class ShowReservations : Window
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

        private void CheckNotification()
        {
            if (Notification)
            {
                NotificationMenuItem.Visibility = Visibility.Visible;
            }
            else
            {
                NotificationMenuItem.Visibility = Visibility.Collapsed;
            }
        }

        public ICommand CancelCommand { get; set; }

        public ICommand RescheduleCommand { get; set; }

        public ShowReservations(string username)
        {
            InitializeComponent();
            Guest1 = username;
            DataContext = this;

            reservationService = new ReservationService(Guest1);
            Notification = reservationService.Guest1HasNotification();
            CheckNotification();


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
            dgShowReservations.Items.Refresh();
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
            Close();
        }

        private void GoToGuest1Start(object sender, RoutedEventArgs e)
        {
            Guest1Start window = new Guest1Start(Guest1);
            window.Show();
            Close();
        }

        private void GoToSearchAndShowAccommodations(object sender, RoutedEventArgs e)
        {
            SearchAndShowAccommodations window = new SearchAndShowAccommodations(Guest1);
            window.Show();
            Close();
        }

        private void GoToCreateReview(object sender, RoutedEventArgs e)
        {
            CreateReview window = new CreateReview(Guest1);
            window.Show();
            Close();
        }

        private void GoToShowReservations(object sender, RoutedEventArgs e)
        {
            ShowReservations window = new ShowReservations(Guest1);
            window.Show();
            Close();
        }

        private void GoToGuest1Requests(object sender, RoutedEventArgs e)
        {
            Guest1Requests window = new Guest1Requests(Guest1);
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
