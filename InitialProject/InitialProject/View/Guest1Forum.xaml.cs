using GalaSoft.MvvmLight.Command;
using InitialProject.DTO;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace InitialProject.View
{

    public partial class Guest1Forum : Page
    {
        private readonly ForumService forumService;

        public string OwnerUsername
        {
            get;
            set;
        }

        public List<ShowGuest1ForumsDTO> ShowGuest1ForumsDTOs
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

        public ShowGuest1ForumsDTO SelectedShowOwnerForum
        {
            get;
            set;
        }

        public ShowGuest1ForumsDTO ShowGuest1ForumsDTO
        {
            get;
            set;
        }




        private string guest1;
        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
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

        public Guest1Forum(string username, Page page)
        {
            InitializeComponent();

            this.DataContext = this;

            Guest1 = username;


            forumService = new ForumService(Guest1);

            SetDefaultValue();

            //SetMenu(ownerHeader);

            //ShowOwnerForumsDTOs = forumService.FindForums();
            ShowGuest1ForumsDTOs = forumService.FindGuest1Forums();

            ReadMoreCommand = new RelayCommand<ShowGuest1ForumsDTO>(ReadMore);



            SetUsernameHeader();

            SetComboBoxes(page);

        }

        //private void SetMenu(string ownerHeader)
        //{
        //    //usernameAndSuperOwner.Header = ownerHeader;

        //    //rateGuestsNotifications.Header = "Number of unrated guests: " + forumService.FindNumberOfUnratedGuests(OwnerUsername) + ".";

        //    UnreadCancelledReservations = new List<string>();

        //    UnreadCancelledReservationsToDelete = forumService.FindUnreadCancelledReservations(OwnerUsername);

        //    if (UnreadCancelledReservationsToDelete.Count == 0)
        //    {
        //        UnreadCancelledReservations.Add("There are no new canceled reservations");
        //    }
        //    else
        //    {
        //        foreach (CancelledReservationsNotificationDTO temporaryCanceledReservationsNotificationDTO in UnreadCancelledReservationsToDelete.ToList())
        //        {
        //            UnreadCancelledReservations.Add(temporaryCanceledReservationsNotificationDTO.AccommodationName + ": " + temporaryCanceledReservationsNotificationDTO.ReservationStartDate.ToShortDateString() + " - " + temporaryCanceledReservationsNotificationDTO.ReservationEndDate.ToShortDateString());
        //        }
        //    }
        //}

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

        private void ReadMore(ShowGuest1ForumsDTO showGuest1ForumsDTO)
        {
            ShowGuest1ForumsDTO = showGuest1ForumsDTO;

            GoToForumPreview(null, null);

            //if (string.IsNullOrEmpty(showGuest1ForumsDTO.Closed) == true)
            //{
            //OwnerForumActiveTopic window = new OwnerForumActiveTopic(OwnerUsername, usernameAndSuperOwner.Header.ToString(), showOwnerForumsDTO);
            //window.Show();
            //}
            //else
            //{
            //OwnerForumClosedTopic window = new OwnerForumClosedTopic(showOwnerForumsDTO);
            //window.Show();
            //}
        }

        private void SelectedShowOwnerForumChange(object sender, RoutedEventArgs e)
        {
            // SelectedShowOwnerForum.ForumId = 
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

        private void GoToForum(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Guest1Forum(Guest1, this));
        }

        private void GoToForumPreview(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Guest1ForumPreview(Guest1, this));
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
