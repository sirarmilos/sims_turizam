using InitialProject.DTO;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for OwnerForumActiveTopic.xaml
    /// </summary>
    public partial class OwnerForumActiveTopic : Window
    {
        private readonly ForumService forumService;

        public string OwnerUsername
        {
            get;
            set;
        }

        public string OwnerUsernameShow
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

        public ShowOwnerForumsDTO ShowOwnerForumsDTO
        {
            get;
            set;
        }

        public List<ShowOwnerForumCommentsDTO> ShowOwnerForumCommentsDTOs
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

        public OwnerForumActiveTopic(string ownerUsername, string ownerHeader, ShowOwnerForumsDTO showOwnerForumsDTO)
        {
            InitializeComponent();

            OwnerUsername = ownerUsername;

            DataContext = this;

            forumService = new ForumService(OwnerUsername);

            SetDefaultValue(showOwnerForumsDTO);

            SetMenu(ownerHeader);
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

        private void SetDefaultValue(ShowOwnerForumsDTO showOwnerForumsDTO)
        {
            showOwnerForumsDTO.CreatorUsername = showOwnerForumsDTO.CreatorUsername.Substring(0, showOwnerForumsDTO.CreatorUsername.Length - 7) + ":";

            ShowOwnerForumsDTO = showOwnerForumsDTO;

            OwnerUsernameShow = OwnerUsername + ":";

            ShowOwnerForumCommentsDTOs = forumService.FindComments(showOwnerForumsDTO.ForumId);
        }

        private void AddComment_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AddComment_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Close_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }
    }

    public class ReportVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                if (parameter is string parameterValue)
                {
                    if (parameterValue == "Past")
                    {
                        return dateTime < DateTime.Now ? Visibility.Collapsed : Visibility.Visible;
                    }
                    else if (parameterValue == "Future")
                    {
                        return dateTime > DateTime.Now ? Visibility.Collapsed : Visibility.Visible;
                    }
                }
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
