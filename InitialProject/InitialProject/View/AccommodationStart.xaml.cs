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

        private void AddNewAccommodation_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AddNewAccommodation_Executed(object sender, ExecutedRoutedEventArgs e)
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
    }
}
