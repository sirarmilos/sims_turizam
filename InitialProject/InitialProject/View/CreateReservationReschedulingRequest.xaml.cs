using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace InitialProject.View
{
    public partial class CreateReservationReschedulingRequest : Window
    {
        private readonly ReservationReschedulingRequestService reservationReschedulingRequestService;
        private ShowReservationDTO showReservationDTO;
        private DateTime startDate;
        private DateTime endDate;
        private string guest1;

        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
            }
        }

        public ShowReservationDTO ShowReservationDTO
        {
            get { return showReservationDTO; }
            set
            {
                showReservationDTO = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged();
            }
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


        public CreateReservationReschedulingRequest(ShowReservationDTO showReservationDTO, string username)
        {
            InitializeComponent();
            DataContext = this;
            Guest1 = username;

            ShowReservationDTO = showReservationDTO;    
            reservationReschedulingRequestService = new ReservationReschedulingRequestService(Guest1);
            Notification = reservationReschedulingRequestService.Guest1HasNotification();
            CheckNotification();

            AccommodationNameLabel.Content = showReservationDTO.Accommodation.AccommodationName.ToString();
            OldStartDateLabel.Content = showReservationDTO.StartDate.ToString("dd.MM.yyyy.");
            OldEndDateLabel.Content = showReservationDTO.EndDate.ToString("dd.MM.yyyy.");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Create(object sender, RoutedEventArgs e)
        {
            SuggestedDatesMessage.Content = "";

            if (StartDatePicker.SelectedDate != null && EndDatePicker.SelectedDate != null) // nez da l ove validacije u neki servis da se urade?
            {
                if (!IsSearchInputValid()) return;

                CreateReservationReschedulingRequestDTO request = 
                    new CreateReservationReschedulingRequestDTO(showReservationDTO, StartDate, EndDate);

                reservationReschedulingRequestService.CreateRequest(request);

                MessageBox.Show(
                    $"The request has been successfully created!");
            }
            else
            {
                MessageBox.Show($"No dates are selected.");
                return;
            }
        }

        private bool IsSearchInputValid()
        {
            StartDate = StartDatePicker.SelectedDate.Value;
            EndDate = EndDatePicker.SelectedDate.Value;

            int newReservationDays = EndDate.Subtract(StartDate).Days;

            if (newReservationDays < ShowReservationDTO.Accommodation.MinDaysReservation)
            {
                MessageBox.Show(
                    $"Number of reservation days couldn't be less than minimal days of reservation which is: {ShowReservationDTO.Accommodation.MinDaysReservation}. Try again.");
                return false;
            }

            if (StartDate > EndDate)
            {
                MessageBox.Show($"Start date is greater than end date. Try again.");
                return false;
            }

            if ((StartDate == ShowReservationDTO.StartDate) & (EndDate == ShowReservationDTO.EndDate))
            {
                MessageBox.Show($"Choose different dates. Try again.");
                return false;
            }

            if (StartDate < DateTime.Now)
            {
                MessageBox.Show($"The start date is in the past. Try again.");
                return false;
            }

            return true;
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
