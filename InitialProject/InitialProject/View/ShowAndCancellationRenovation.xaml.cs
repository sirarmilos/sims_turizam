﻿using InitialProject.DTO;
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
    /// Interaction logic for ShowAndCancellationRenovation.xaml
    /// </summary>
    public partial class ShowAndCancellationRenovation : Window
    {
        private readonly RenovationService renovationService;

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

        public ShowRenovationDTO SelectedRenovation
        {
            get;
            set;
        }

        public List<ShowRenovationDTO> ShowRenovationDTOs
        {
            get;
            set;
        }

        public ShowAndCancellationRenovation(string ownerUsername, string ownerHeader)
        {
            InitializeComponent();

            OwnerUsername = ownerUsername;

            DataContext = this;

            renovationService = new RenovationService(ownerUsername);

            ShowRenovationDTOs = new List<ShowRenovationDTO>();

            ShowRenovationDTOs = renovationService.FindAllRenovations(ownerUsername);

            SetDefaultValue();

            SetMenu(ownerHeader);
        }

        private void SetMenu(string ownerHeader)
        {
            usernameAndSuperOwner.Header = ownerHeader;

            rateGuestsNotifications.Header = "Number of unrated guests: " + renovationService.FindNumberOfUnratedGuests(OwnerUsername) + ".";

            UnreadCancelledReservations = new List<string>();

            UnreadCancelledReservations = renovationService.FindUnreadCancelledReservations(OwnerUsername);
        }

        private void SetDefaultValue()
        {
            buttonCancelRenovation.IsEnabled = false;
            SelectedRenovation = null;
        }

        private void ReadCancelledReservationNotification(object sender, RoutedEventArgs e)
        {
            string viewedCancelledReservation = ((MenuItem)sender).Header.ToString();

            if (viewedCancelledReservation.Equals("There are currently no new booking cancellations.") == false)
            {
                renovationService.SaveViewedCancelledReservation(FindDTO(viewedCancelledReservation));

                UnreadCancelledReservations = renovationService.FindUnreadCancelledReservations(OwnerUsername);

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

        private void RenovateAccommodation(object sender, RoutedEventArgs e)
        {
            SchedulingRenovation window = new SchedulingRenovation(OwnerUsername);

            window.ShowDialog();
        }

        private void CancelRenovation(object sender, RoutedEventArgs e)
        {
            ShowRenovationDTOs = renovationService.RemoveRenovation(SelectedRenovation, OwnerUsername);

            dgRenovations.Items.Refresh();

            dgRenovations.ItemsSource = ShowRenovationDTOs;
        }

        private void CancelRenovationButtonEnable(object sender, SelectionChangedEventArgs e)
        {
            if(SelectedRenovation != null && SelectedRenovation.Status.Equals("Can be cancelled") == true)
            {
                buttonCancelRenovation.IsEnabled = true;
            }
            else
            {
                buttonCancelRenovation.IsEnabled = false;
            }
        }

        private void LoadingRowForDgRenovations(object sender, DataGridRowEventArgs e)
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
