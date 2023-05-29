using GalaSoft.MvvmLight.Command;
using InitialProject.DTO;
using InitialProject.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for OwnerForum.xaml
    /// </summary>
    public partial class OwnerForum : Window
    {
        private readonly ForumService forumService;

        public string OwnerUsername
        {
            get;
            set;
        }

        public List<ShowOwnerForumsDTO> ShowOwnerForumsDTOs
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

        public ShowOwnerForumsDTO SelectedShowOwnerForum
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
                forumService.MarkAsReadNotificationsCancelledReservations(UnreadCancelledReservationsToDelete);
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

        public OwnerForum(string ownerUsername, string ownerHeader)
        {
            InitializeComponent();

            OwnerUsername = ownerUsername;

            DataContext = this;

            forumService = new ForumService(OwnerUsername);

            SetDefaultValue();

            SetMenu(ownerHeader);

            ShowOwnerForumsDTOs = forumService.FindForums();

            ReadMoreCommand = new RelayCommand<ShowOwnerForumsDTO>(ReadMore);
        }

        private void SetMenu(string ownerHeader)
        {
            usernameAndSuperOwner.Header = ownerHeader;

            rateGuestsNotifications.Header = "Number of unrated guests: " + forumService.FindNumberOfUnratedGuests(OwnerUsername) + ".";

            UnreadCancelledReservations = new List<string>();

            UnreadCancelledReservationsToDelete = forumService.FindUnreadCancelledReservations(OwnerUsername);

            if (UnreadCancelledReservationsToDelete.Count == 0)
            {
                UnreadCancelledReservations.Add("There are no new canceled reservations");
            }
            else
            {
                foreach (CancelledReservationsNotificationDTO temporaryCanceledReservationsNotificationDTO in UnreadCancelledReservationsToDelete.ToList())
                {
                    UnreadCancelledReservations.Add(temporaryCanceledReservationsNotificationDTO.AccommodationName + ": " + temporaryCanceledReservationsNotificationDTO.ReservationStartDate.ToShortDateString() + " - " + temporaryCanceledReservationsNotificationDTO.ReservationEndDate.ToShortDateString());
                }
            }
        }

        private void SetDefaultValue()
        {

        }

        private void ReadMore_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(SelectedShowOwnerForum == null)
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
        }

        private void ReadMore_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            /* if(string.IsNullOrEmpty(SelectedShowOwnerForum.Closed) == true)
            {
                OwnerForumActiveTopic window = new OwnerForumActiveTopic();
                window.Show();
                Close();
            }
            else
            {
                OwnerForumClosedTopic window = new OwnerForumClosedTopic();
                window.Show();
                Close();
            }*/
        }

        public ICommand ReadMoreCommand
        {
            get;
            set;
        }

        private void ReadMore(ShowOwnerForumsDTO showOwnerForumsDTO)
        {
            if (string.IsNullOrEmpty(showOwnerForumsDTO.Closed) == true)
            {
                OwnerForumActiveTopic window = new OwnerForumActiveTopic(OwnerUsername, usernameAndSuperOwner.Header.ToString(), showOwnerForumsDTO);
                window.Show();
                Close();
            }
            else
            {
                OwnerForumClosedTopic window = new OwnerForumClosedTopic(showOwnerForumsDTO);
                window.Show();
                Close();
            }
        }

        private void SelectedShowOwnerForumChange(object sender, RoutedEventArgs e)
        {
            // SelectedShowOwnerForum.ForumId = 
        }
    }
}
