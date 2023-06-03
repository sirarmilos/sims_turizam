using GalaSoft.MvvmLight.Command;
using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
using System.Windows.Threading;

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

        public string Comment
        {
            get;
            set;
        }

        public string CommentCheck
        {
            get;
            set;
        }

        public int ForumId
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

            textBlockErrorComment1.Visibility = Visibility.Hidden;

            ReportCommand = new RelayCommand<ShowOwnerForumCommentsDTO>(Report);
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
            ForumId = showOwnerForumsDTO.ForumId;

            showOwnerForumsDTO.CreatorUsername = showOwnerForumsDTO.CreatorUsername.Substring(0, showOwnerForumsDTO.CreatorUsername.Length - 7) + ":";

            ShowOwnerForumsDTO = showOwnerForumsDTO;

            OwnerUsernameShow = OwnerUsername + ":";

            ShowOwnerForumCommentsDTOs = forumService.FindComments(ForumId);
        }

        private void AddComment_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AddComment_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(Comment) == false)
            {
                forumService.AddOwnerComment(OwnerUsername, Comment, ShowOwnerForumsDTO.ForumId);

                MessageBox.Show("Comment successfully added.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                Comment = string.Empty;
                tbComment.Text = string.Empty;
                tbComment.Focus();

                ShowOwnerForumCommentsDTOs = forumService.FindComments(ForumId);
                itemsControlShowOwnerForumComments.ItemsSource = ShowOwnerForumCommentsDTOs;
            }
        }

        private void Close_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OwnerForum window = new OwnerForum(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        public ICommand ReportCommand { get; set; }

        private void Report(ShowOwnerForumCommentsDTO showOwnerForumCommentsDTO)
        {
            if(forumService.ReportGuest(showOwnerForumCommentsDTO.CommentId, OwnerUsername) == true)
            {
                MessageBox.Show("Guest is reported.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Guest is already reported.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ShowOwnerForumCommentsDTOs = forumService.FindComments(ForumId);
            itemsControlShowOwnerForumComments.ItemsSource = ShowOwnerForumCommentsDTOs;
        }

        private void CheckErrorComment(object sender, TextChangedEventArgs e)
        {
            if(CommentCheck.Equals(string.Empty) == true)
            {
                Comment = string.Empty;
                textBlockErrorComment0.Visibility = Visibility.Visible;
                textBlockErrorComment1.Visibility = Visibility.Hidden;
            }
            else
            {
                textBlockErrorComment0.Visibility = Visibility.Hidden;

                if(CommentCheck.Length <= 20)
                {
                    Comment = string.Empty;
                    textBlockErrorComment1.Visibility = Visibility.Visible;
                }
                else
                {
                    textBlockErrorComment1.Visibility = Visibility.Hidden;
                    Comment = CommentCheck;
                }
            }
        }
    }

    public class ReportVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string visited)
            {
                if(visited.Equals("Not visited this location") == true)
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NumberOfUserReportVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string numberOfUserReport)
            {
                if(numberOfUserReport.Equals(string.Empty) == false)
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
