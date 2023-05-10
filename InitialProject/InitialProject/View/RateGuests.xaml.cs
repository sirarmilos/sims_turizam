using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
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
    /// Interaction logic for RateGuests.xaml
    /// </summary>
    public partial class RateGuests : Window
    {
        private readonly RateGuestsService rateGuestsService;

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

        private int cleanliness;
        private int followRules;
        private int behavior;
        private bool cash;
        private bool creditCard;
        private bool deferredPayment;
        private string typePayment;
        private int communicativeness;
        private string comment;

        public List<RateGuestsDTO> RateGuestsDTOs
        {
            get;
            set;
        }

        public RateGuestsDTO SelectedGuest
        {
            get;
            set;
        }

        public int Cleanliness
        {
            get { return cleanliness; }
            set
            {
                cleanliness = value;
            }
        }

        public int FollowRules
        {
            get { return followRules; }
            set
            {
                followRules = value;
            }
        }

        public int Behavior
        {
            get { return behavior; }
            set
            {
                behavior = value;
            }
        }

        public bool Cash
        {
            get { return cash; }
            set
            {
                if (value != cash)
                {
                    cash = value;
                    if (value) TypePayment = "Cash";
                }
            }
        }

        public bool CreditCard
        {
            get { return creditCard; }
            set
            {
                if (value != creditCard)
                {
                    creditCard = value;
                    if (value) TypePayment = "CreditCard";
                }
            }
        }

        public bool DeferredPayment
        {
            get { return deferredPayment; }
            set
            {
                if (value != deferredPayment)
                {
                    deferredPayment = value;
                    if (value) TypePayment = "DeferredPayment";
                }
            }
        }

        public string TypePayment
        {
            get { return typePayment; }
            set
            {
                if (value != typePayment)
                {
                    typePayment = value;
                }
            }
        }

        public int Communicativeness
        {
            get { return communicativeness; }
            set
            {
                communicativeness = value;
            }
        }

        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
            }
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

        public RateGuests(string ownerUsername, string ownerHeader)
        {
            InitializeComponent();

            OwnerUsername = ownerUsername;

            DataContext = this;

            rateGuestsService = new RateGuestsService(OwnerUsername);

            RateGuestsDTOs = new List<RateGuestsDTO>();

            RateGuestsDTOs = rateGuestsService.FindAllGuestsToRate();

            SetDefaultValue();

            SetMenu(ownerHeader);
        }

        private void SetMenu(string ownerHeader)
        {
            usernameAndSuperOwner.Header = ownerHeader;

            rateGuestsNotifications.Header = "Number of unrated guests: " + RateGuestsDTOs.Count + ".";

            UnreadCancelledReservations = new List<string>();

            UnreadCancelledReservations = rateGuestsService.FindUnreadCancelledReservations(OwnerUsername);
        }

        private void SetDefaultValue()
        {
            rbCash.IsChecked = true;
            sliderCleanliness.Value = 3;
            sliderFollowRules.Value = 3;
            sliderBehavior.Value = 3;
            sliderCommunicativeness.Value = 3;
            Comment = string.Empty;
            tbComment.Text = string.Empty;
            buttonRate.IsEnabled = false;
            groupBoxRateFields.IsEnabled = false;
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
                rateGuestsService.SaveViewedCancelledReservation(FindDTO(viewedCancelledReservation));

                UnreadCancelledReservations = rateGuestsService.FindUnreadCancelledReservations(OwnerUsername);

                cancelledReservationsNotificationsList.DataContext = UnreadCancelledReservations;
            }
        }

        private void ReadCancelledReservationNotification(object sender, RoutedEventArgs e)
        {
            string viewedCancelledReservation = ((MenuItem)sender).Header.ToString();

            if (viewedCancelledReservation.Equals("There are currently no new booking cancellations.") == false)
            {
                rateGuestsService.SaveViewedCancelledReservation(FindDTO(viewedCancelledReservation));

                UnreadCancelledReservations = rateGuestsService.FindUnreadCancelledReservations(OwnerUsername);

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

        private void Rate_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(SelectedGuest != null)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            } 
        }

        private void Rate_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            rateGuestsService.SaveNewRateGuest(new SaveNewRateGuestDTO(SelectedGuest.ReservationId, Cleanliness, FollowRules, Behavior, TypePayment, Communicativeness, Comment));

            RateGuestsDTOs.Remove(SelectedGuest);
            dgRateGuests.Items.Refresh();

            SetDefaultValue();

            rateGuestsNotifications.Header = "Number of unrated guests: " + RateGuestsDTOs.Count;

            MessageBox.Show("You have successfully rated a guest.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Cancel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Cancel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SetDefaultValue();
            dgRateGuests.SelectedItem = null;
        }

        private void RateEnable(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedGuest == null)
            {
                buttonRate.IsEnabled = false;
                groupBoxRateFields.IsEnabled = false;
            }
            else
            {
                buttonRate.IsEnabled = true;
                groupBoxRateFields.IsEnabled = true;
            }
        }

        void LoadingRowForDgRateGuests(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void labeltbFocus(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                var label = (Label)sender;
                Keyboard.Focus(label.Target);
            }
        }

        private void SliderCleanlinessValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Cleanliness = Convert.ToInt32(sliderCleanliness.Value);
        }

        private void SliderFollowRulesValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            FollowRules = Convert.ToInt32(sliderFollowRules.Value);
        }

        private void SliderBehaviorValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Behavior = Convert.ToInt32(sliderBehavior.Value);
        }

        private void SliderCommunicativenessValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Communicativeness = Convert.ToInt32(sliderCommunicativeness.Value);
        }
    }
}
