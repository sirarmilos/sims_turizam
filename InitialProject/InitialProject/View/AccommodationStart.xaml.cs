﻿using InitialProject.DTO;
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
using System.Windows.Shell;

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

        public List<CancelledReservationsNotificationDTO> UnreadCancelledReservationsToDelete
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

        public string MostPopularLocation
        {
            get;
            set;
        }

        public string NotPopularLocation
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
            if (GlobalOwnerClass.NotificationRead == true)
            {
                accommodationService.MarkAsReadNotificationsCancelledReservations(UnreadCancelledReservationsToDelete);
            }

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
            GlobalOwnerClass.NotificationRead = true;
            notifications.IsSubmenuOpen = true;
            rateGuestsNotifications.Focus();
            accommodationService.MarkAsReadNotificationsForums(OwnerUsername);
        }

        public AccommodationStart(string ownerUsername)
        {
            InitializeComponent();

            OwnerUsername = ownerUsername;

            DataContext = this;

            accommodationService = new AccommodationService(OwnerUsername);

            SetDefaultValue();

            SetMenu();
        }

        private void SetMenu()
        {
            usernameAndSuperOwner.Header = OwnerUsername + CheckSuperType();

            rateGuestsNotifications.Header = "Number of unrated guests: " + accommodationService.FindNumberOfUnratedGuests(OwnerUsername) + ".";

            UnreadCancelledReservations = new List<string>();

            UnreadCancelledReservationsToDelete = accommodationService.FindUnreadCancelledReservations(OwnerUsername);

            if(UnreadCancelledReservationsToDelete.Count == 0)
            {
                UnreadCancelledReservations.Add("There are no new canceled reservations");
            }
            else
            {
                foreach(CancelledReservationsNotificationDTO temporaryCanceledReservationsNotificationDTO in UnreadCancelledReservationsToDelete.ToList())
                {
                    UnreadCancelledReservations.Add(temporaryCanceledReservationsNotificationDTO.AccommodationName + ": " + temporaryCanceledReservationsNotificationDTO.ReservationStartDate.ToShortDateString() + " - " + temporaryCanceledReservationsNotificationDTO.ReservationEndDate.ToShortDateString());
                }
            }

            forumNotifications.Header = "Number of new forums: " + accommodationService.FindNumberOfNewForums(OwnerUsername) + ".";
        }

        private void SetDefaultValue()
        {
            SelectedShowAccommodationDTO = null;

            buttonRenovate.IsEnabled = false;
            buttonStatistics.IsEnabled = false;

            ShowAccommodationDTOs = accommodationService.FindOwnerAccommodations(OwnerUsername);
            dgAccommodations.ItemsSource = ShowAccommodationDTOs;

            MostPopularLocation = accommodationService.FindTopLocation();
            labelMostPopularLocation.Content = MostPopularLocation;
            NotPopularLocation = accommodationService.FindWorstLocation(OwnerUsername);
            labelNotPopularLocation.Content = NotPopularLocation;

            if(MostPopularLocation.Equals(NotPopularLocation) == true)
            {
                NotPopularLocation = "-";
                labelNotPopularLocation.Content = NotPopularLocation;
            }
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

        private void AddNewAccommodation_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AddNewAccommodation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            InitialProject.View.AddNewAccommodation window = new AddNewAccommodation(OwnerUsername);
            window.ShowDialog();

            SetDefaultValue();
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

        private void RenovateAccommodation_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedShowAccommodationDTO == null)
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
        }

        private void RenovateAccommodation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SchedulingSelectedRenovation window = new SchedulingSelectedRenovation(OwnerUsername, SelectedShowAccommodationDTO.AccommodationName);
            window.ShowDialog();
        }

        private void AccommodationStatistics_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(SelectedShowAccommodationDTO == null)
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
        }

        private void AccommodationStatistics_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ShowStatisticsAccommodationDTO showStatisticsAccommodationDTO = accommodationService.FindSelectedAccommodation(SelectedShowAccommodationDTO.Id);

            InitialProject.View.AccommodationStatistics window = new AccommodationStatistics(OwnerUsername, usernameAndSuperOwner.Header.ToString(), showStatisticsAccommodationDTO);
            window.Show();
            Close();
        }

        private void AddTopLocationAccommodation_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AddTopLocationAccommodation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(MostPopularLocation.Equals("-") == true)
            {
                MessageBox.Show("None of your accommodation locations are the worst.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                AddTopLocationAccommodation window = new AddTopLocationAccommodation(OwnerUsername, MostPopularLocation);
                window.ShowDialog();

                SetDefaultValue();
            }
        }

        private void RemoveWorstLocationAccommodation_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void RemoveWorstLocationAccommodation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            bool canRemove = accommodationService.RemoveWorstLocations(OwnerUsername, MostPopularLocation);

            if(canRemove == true)
            {
                MessageBox.Show("Accommodations at this location have been successfully deleted.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                SetDefaultValue();
            }
            else
            {
                MessageBox.Show("It is not possible to delete the accommodation because there are scheduled bookings or renovations in the future or there is no worst location.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void LoadingRowForDgAccommodations(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
