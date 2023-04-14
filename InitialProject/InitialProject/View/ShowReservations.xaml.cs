using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using InitialProject.DTO;
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

        public ICommand CancelCommand { get; set; }

        public ShowReservations(string username)
        {
            InitializeComponent();
            Guest1 = username;
            DataContext = this;

            reservationService = new ReservationService(Guest1);
            ShowReservationDTOs = reservationService.FindAll(Guest1);
            CancelCommand = new RelayCommand<ShowReservationDTO>(Cancel);

            // todo: validacije
        }

        private void Cancel(ShowReservationDTO showReservationDTO)
        {
            bool isReservationCancelEligible = reservationService.IsRemoved(showReservationDTO);
            
            if (!isReservationCancelEligible)
            {
                MessageBox.Show("Reservation is not eligible for canceling.", "Error", MessageBoxButton.OK, MessageBoxImage.Error); // todo: mogu preciznije da ispisem greske
                return;
            }

            ShowReservationDTOs.Remove(showReservationDTO);
            dgShowReservations.Items.Refresh();
        }

        private void GoToCreateReview(object sender, RoutedEventArgs e)
        {
            CreateReview window = new CreateReview(Guest1);
            window.Show();
        }

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            LoginForm window = new LoginForm();
            window.Show();
            Close();
        }

    }

}
