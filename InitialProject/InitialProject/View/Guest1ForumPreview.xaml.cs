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
using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace InitialProject.View
{

    public partial class Guest1ForumPreview : Page
    {
        private readonly ForumService forumService;

        public string Guest1
        {
            get;
            set;
        }

        public string Guest1UsernameShow
        {
            get;
            set;
        }

        public ShowGuest1ForumsDTO ShowGuest1ForumsDTO
        {
            get;
            set;
        }

        public List<ShowGuest1ForumCommentsDTO> ShowGuest1ForumCommentsDTOs
        {
            get;
            set;
        }

        public string Comment
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

        private Brush _labelColor;
        public Brush LabelColor
        {
            get { return _labelColor; }
            set
            {
                _labelColor = value;
                OnPropertyChanged(nameof(LabelColor));
            }
        }

        private void CheckNotification()
        {
            if (Notification)
            {
                NotificationMenuItemImageNotificationBell.Visibility = Visibility.Visible;
                NotificationMenuItemImageRegularBell.Visibility = Visibility.Collapsed;
            }
            else
            {
                NotificationMenuItemImageNotificationBell.Visibility = Visibility.Collapsed;
                NotificationMenuItemImageRegularBell.Visibility = Visibility.Visible;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Guest1ForumPreview(string username, Page page)
        {
            InitializeComponent();

            this.DataContext = this;

            Guest1 = username;

            forumService = new ForumService(Guest1);

            Guest1Forum guest1Forum = (Guest1Forum)page;

            SetDefaultValue(guest1Forum.ShowGuest1ForumsDTO);

            SetComboBoxes(page);

            SetUsernameHeader();

            CheckWindowModeVisibilty();
        }


        private void SetDefaultValue(ShowGuest1ForumsDTO showGuest1ForumsDTO)
        {
            showGuest1ForumsDTO.CreatorUsername = showGuest1ForumsDTO.CreatorUsername.Substring(0, showGuest1ForumsDTO.CreatorUsername.Length - 7) + ":";

            ShowGuest1ForumsDTO = showGuest1ForumsDTO;

            Guest1UsernameShow = Guest1 + ":";

            ShowGuest1ForumCommentsDTOs = forumService.FindGuest1ForumComments(showGuest1ForumsDTO.ForumId);
        }

        private void AddComment(object sender, RoutedEventArgs e)
        {
            forumService.AddGuest1Comment(Guest1, Comment, ShowGuest1ForumsDTO.ForumId);

            forumService.CheckIsUseful(ShowGuest1ForumsDTO.ForumId);

            ShowGuest1ForumCommentsDTOs = forumService.FindGuest1ForumComments(ShowGuest1ForumsDTO.ForumId);

            ShowGuest1ForumCommentsDTOsItems.ItemsSource = ShowGuest1ForumCommentsDTOs;

            tbComment.Text = string.Empty;
            tbComment.Focus();
        }

        private void CloseForum(object sender, RoutedEventArgs e) 
        {
            forumService.CloseForum(ShowGuest1ForumsDTO.ForumId);

            CheckWindowModeVisibilty();
        }
        
        private void CheckWindowModeVisibilty()
        {
            if (forumService.CheckIsForumClosed(ShowGuest1ForumsDTO.ForumId)) // mode: closed forum
            {
                AddCommentGrid.Visibility = Visibility.Collapsed;
                ForumIsClosedMessage.Visibility = Visibility.Visible;
                OneButtonMode.Visibility = Visibility.Visible;
                TwoButtonsMode.Visibility = Visibility.Collapsed;

                return;
            }

            if (!(Guest1+":").Equals(ShowGuest1ForumsDTO.CreatorUsername)) // mode: opened forum, but not creator
            {
                AddCommentGrid.Visibility = Visibility.Visible;
                ForumIsClosedMessage.Visibility = Visibility.Collapsed;
                OneButtonMode.Visibility = Visibility.Visible;
                TwoButtonsMode.Visibility = Visibility.Collapsed;
            }
            else // mode: opened forum and creator
            {
                AddCommentGrid.Visibility = Visibility.Visible;
                ForumIsClosedMessage.Visibility = Visibility.Collapsed;
                OneButtonMode.Visibility = Visibility.Collapsed;
                TwoButtonsMode.Visibility = Visibility.Visible;
            }
        }

        private void BackToCaller(object sender, RoutedEventArgs e)
        {
            GoToForum(sender, e);
        }

        private void SetUsernameHeader()
        {
            Notification = forumService.Guest1HasNotification();
            CheckNotification();
            usernameAndSuperGuest.Text = $"{Guest1}";
            superGuest.Text = $"{CheckSuperType()}";
        }

        private string CheckSuperType()
        {
            string superType = string.Empty;

            if (forumService.IsSuperGuest(Guest1))
            {
                superType = "(Super guest)";
            }

            return superType;
        }



        private bool comboBoxClicked = false;
        private bool itemClicked = false;

        private void CBPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            comboBoxClicked = true;
        }

        private void CBCreateReviewDropDownClosed(object sender, EventArgs e)
        {
            if (comboBoxClicked && itemClicked)
            {
                ComboBox comboBox = (ComboBox)sender;
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;

                if (selectedItem.Content.ToString() == "Create review")
                {
                    GoToCreateReview(sender, null);
                }
                else if (selectedItem.Content.ToString() == "Reviews")
                {
                    GoToShowOwnerReviews(sender, null);
                }
                else if (selectedItem.Content.ToString() == "Requests")
                {
                    GoToGuest1Requests(sender, null);
                }
            }

            comboBoxClicked = false;
            itemClicked = false;
        }

        private void CBSuperGuestDropDownClosed(object sender, EventArgs e)
        {
            if (comboBoxClicked && itemClicked)
            {
                ComboBox comboBox = (ComboBox)sender;
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;

                if (selectedItem.Content.ToString() == "Super-guest")
                {
                    GoToSearchAndShowAccommodations(sender, null);
                }
                else if (selectedItem.Content.ToString() == "Logout")
                {
                    GoToLogout(sender, null);
                }
            }

            comboBoxClicked = false;
            itemClicked = false;
        }

        private void CBItemPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            itemClicked = true;
        }

        private void GoToShowOwnerReviews(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowOwnerReviews(Guest1, this));
        }

        //private void GoToGuest1Start(object sender, RoutedEventArgs e)
        //{
        //    NavigationService?.Navigate(new Guest1Start(Guest1, this));
        //}

        private void GoToSearchAndShowAccommodations(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new SearchAndShowAccommodations(Guest1, this));
        }

        private void GoToAnywhereAnytime(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Guest1AnywhereAnytime(Guest1, this));
        }

        private void GoToForum(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Guest1Forum(Guest1, this));
        }

        private void GoToShowReservations(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowReservations(Guest1, this));
        }

        private void GoToCreateReview(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CreateReview(Guest1, this));
        }

        private void GoToGuest1Requests(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Guest1Requests(Guest1, this));
        }

        private void GoToShowGuest1Notifications(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowGuest1Notifications(Guest1, this));
        }

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            Window currentWindow = Window.GetWindow(this);

            LoginForm window = new LoginForm();
            window.Show();
            currentWindow.Close();
        }

        private void SetComboBoxes(Page page)
        {
            if (page is SearchAndShowAccommodations searchAndShowPage)
            {
                var comboBox = searchAndShowPage.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = searchAndShowPage.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
            else if (page is AccommodationReservation accommodationReservationPage)
            {
                //var comboBox = accommodationReservationPage.CBCreateReview;
                //if (comboBox != null)
                //{
                //    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                //}

                //comboBox = accommodationReservationPage.CBSuperGuest;
                //if (comboBox != null)
                //{
                //    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                //}
            }
            else if (page is CreateReservationReschedulingRequest createReschedulingRequestPage)
            {
                var comboBox = createReschedulingRequestPage.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = createReschedulingRequestPage.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
            else if (page is CreateReview createReviewPage)
            {
                //var comboBox = createReviewPage.CBCreateReview;
                //if (comboBox != null)
                //{
                //    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                //}

                //comboBox = createReviewPage.CBSuperGuest;
                //if (comboBox != null)
                //{
                //    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                //}
            }
            else if (page is Guest1RequestPreview guest1RequestPreviewPage)
            {
                var comboBox = guest1RequestPreviewPage.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = guest1RequestPreviewPage.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
            else if (page is Guest1Requests guest1RequestsPage)
            {
                var comboBox = guest1RequestsPage.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = guest1RequestsPage.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
            else if (page is ShowGuest1Notifications showGuest1NotificationsPage)
            {
                var comboBox = showGuest1NotificationsPage.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = showGuest1NotificationsPage.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
            else if (page is ShowOwnerReviews showOwnerReviewsPage)
            {
                //var comboBox = showOwnerReviewsPage.CBCreateReview;
                //if (comboBox != null)
                //{
                //    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                //}

                //comboBox = showOwnerReviewsPage.CBSuperGuest;
                //if (comboBox != null)
                //{
                //    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                //}
            }
            else if (page is ShowReservations showReservationsPage)
            {
                var comboBox = showReservationsPage.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = showReservationsPage.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
        }

    }
}
