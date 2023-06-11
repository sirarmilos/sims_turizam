using InitialProject.DTO;
using InitialProject.Service;
using System;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using InitialProject.Model;
using System.Windows.Navigation;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight;
using Prism.Commands;
using System.Globalization;
using System.Windows.Data;

namespace InitialProject.View
{
    public partial class ShowGuest1NotificationsViewModel : Page, INotifyPropertyChanged
    {
        private ReservationReschedulingRequestService reservationReschedulingRequestService;
        private string accommodationName;
        private DateTime startDate;
        private DateTime endDate;
        private int guestsNumber;
        private string guest1;


        public int GuestsNumber
        {
            get { return guestsNumber; }
            set
            {
                guestsNumber = value;
            }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
            }
        }

        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
            }
        }

        public string AccommodationName
        {
            get { return accommodationName; }
            set
            {
                accommodationName = value;
            }
        }

        public ObservableCollection<Guest1NotificationDTO> Guest1NotificationDTOs
        {
            get;
            set;
        }

        //private bool notification;
        //public bool Notification
        //{
        //    get { return notification; }
        //    set
        //    {
        //        notification = value;
        //    }
        //}



        //public string NotificationEnable
        //{
        //    get;
        //    set;
        //}


        public ICommand ReadMoreCommand
        {
            get;
            set;
        }

        //public ICommand RescheduleCommand { get; set; }

        //public DelegateCommand GoToGuest1StartCommand { get; }

        //public DelegateCommand GoToSearchAndShowAccommodationsCommand { get; }

        //public DelegateCommand GoToCreateReviewCommand { get; }

        //public DelegateCommand GoToGuest1RequestsCommand { get; }

        //public DelegateCommand GoToLogoutCommand { get; }

        //public DelegateCommand CheckNotificationCommand { get; }

        private Visibility isNotificationVisible;

        public Visibility IsNotificationVisible
        {
            get { return isNotificationVisible; }
            set
            {
                isNotificationVisible = value;
                RaisePropertyChanged();
            }
        }

        private Visibility isRegularBellVisible;

        public Visibility IsRegularBellVisible
        {
            get { return isRegularBellVisible; }
            set
            {
                isRegularBellVisible = value;
                RaisePropertyChanged();
            }
        }


        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public NavigationService NavService { get; set; }

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
                IsNotificationVisible = Visibility.Visible;
                IsRegularBellVisible = Visibility.Collapsed;
            }
            else
            {
                IsNotificationVisible = Visibility.Collapsed;
                IsRegularBellVisible = Visibility.Visible;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        Page ViewPage { get; set; }

        public ShowGuest1NotificationsViewModel(string username, ShowGuest1NotificationsView showGuest1NotificationsView, Page page, NavigationService navService)
        {
            Guest1 = username;
            reservationReschedulingRequestService = new ReservationReschedulingRequestService(Guest1);

            //ShowReservationDTOs = new ObservableCollection<ShowReservationDTO>(reservationService.FindAll(Guest1));
            //CancelCommand = new RelayCommand<ShowReservationDTO>(Cancel);
            ReadMoreCommand = new RelayCommand<Guest1NotificationDTO>(ReadMore);

            Guest1NotificationDTOs = new ObservableCollection<Guest1NotificationDTO>(reservationReschedulingRequestService.FindAllNotificationsByGuest1Username());

            reservationReschedulingRequestService.UpdateViewedRequestsByGuest1();



            SetUsernameHeader();

            SetComboBoxes(page);

            NavService = navService;

            ViewPage = showGuest1NotificationsView;

            ComboBoxCommand = new DelegateCommand<object>(ComboBoxAction);
            ComboBoxSGCommand = new DelegateCommand<object>(ComboBoxSuperGuestAction);

            GoToGuest1CreateForumCommand = new RelayCommand(GoToGuest1CreateForum);
            GoToShowReservationsCommand = new RelayCommand(GoToShowReservations);
            GoToAnywhereAnytimeCommand = new RelayCommand(GoToAnywhereAnytime);
            GoToSearchAndShowAccommodationsCommand = new RelayCommand(GoToSearchAndShowAccommodations);
            GoToForumCommand = new RelayCommand(GoToForum);
        }


        private void ReadMore(Guest1NotificationDTO guest1NotificationDTO)
        {
            Guest1RebookingRequestsDTO guest1RebookingRequestsDTO = 
                reservationReschedulingRequestService.FindRebookingRequestByRequestId(guest1NotificationDTO.RequestId);

            //NavService?.Navigate(new Guest1RequestPreview(Guest1, guest1RebookingRequestsDTO, "ShowGuest1Notifications", this));
            NavService?.Navigate(new Guest1RequestPreviewView(Guest1, this, NavService, guest1RebookingRequestsDTO, "ShowGuest1Notifications"));

        }

        private string usernameAndSuperGuestText;

        public string UsernameAndSuperGuestText
        {
            get { return usernameAndSuperGuestText; }
            set
            {
                usernameAndSuperGuestText = value;
                RaisePropertyChanged();
            }
        }

        private string superGuestText;

        public string SuperGuestText
        {
            get { return superGuestText; }
            set
            {
                superGuestText = value;
                RaisePropertyChanged();
            }
        }

        private void SetUsernameHeader()
        {
            Notification = reservationReschedulingRequestService.Guest1HasNotification();
            CheckNotification();
            UsernameAndSuperGuestText = Guest1;
            SuperGuestText = CheckSuperType();
        }

        private string CheckSuperType()
        {
            string superType = string.Empty;

            if (reservationReschedulingRequestService.IsSuperGuest(Guest1))
            {
                superType = "(Super guest)";
            }

            return superType;
        }


        // CB 1 -----------------------------------------------
        // view model

        public DelegateCommand<object> ComboBoxCommand { get; }
        public DelegateCommand<object> ComboBoxSGCommand { get; }


        private void ComboBoxAction(object parameter)
        {
            if (parameter != null)
            {
                if (parameter.Equals("Create review"))
                {
                    GoToCreateReview(null, null);

                }
                else if (parameter.Equals("Reviews"))
                {
                    GoToShowOwnerReviews(null, null);

                }
                else if (parameter.Equals("Requests"))
                {
                    GoToGuest1Requests(null, null);

                }
            }
        }

        private void ComboBoxSuperGuestAction(object parameter)
        {
            if (parameter != null)
            {
                if (parameter.Equals("Super-guest"))
                {
                    GoToShowSuperGuest(null, null);

                }
                else if (parameter.Equals("Logout"))
                {
                    GoToLogout(null, null);

                }
            }
        }


        public ICommand GoToShowOwnerReviewsCommand { get; set; }
        private void GoToShowOwnerReviews(object sender, RoutedEventArgs e)
        {
            NavService?.Navigate(new ShowOwnerReviews(Guest1, this));
        }

        public ICommand GoToForumCommand { get; set; }
        private void GoToForum()
        {
            NavService.Navigate(new Guest1ForumView(Guest1, this, NavService));
        }

        public ICommand GoToSearchAndShowAccommodationsCommand { get; set; }
        private void GoToSearchAndShowAccommodations()
        {
            NavService?.Navigate(new SearchAndShowAccommodations(Guest1, this));
        }


        public ICommand GoToGuest1CreateForumCommand { get; set; }

        private void GoToGuest1CreateForum()
        {
            NavService?.Navigate(new Guest1CreateForum(Guest1, this));
        }


        private void GoToForumPreview(object sender, RoutedEventArgs e)
        {
            NavService?.Navigate(new Guest1ForumPreview(Guest1, this));
        }

        public ICommand GoToAnywhereAnytimeCommand { get; set; }
        private void GoToAnywhereAnytime()
        {
            NavService?.Navigate(new Guest1AnywhereAnytime(Guest1, this));
        }

        public ICommand GoToShowReservationsCommand { get; set; }
        private void GoToShowReservations()
        {
            NavService?.Navigate(new ShowReservationsView(Guest1, this, NavService));
        }

        private void GoToCreateReview(object sender, RoutedEventArgs e)
        {
            NavService?.Navigate(new CreateReview(Guest1, this));
        }

        private void GoToGuest1Requests(object sender, RoutedEventArgs e)
        {
            NavService?.Navigate(new Guest1RequestsView(Guest1, this, this.NavigationService));
        }

        private DelegateCommand goToShowGuest1NotificationsCommand;
        public DelegateCommand GoToShowGuest1NotificationsCommand => goToShowGuest1NotificationsCommand ?? (goToShowGuest1NotificationsCommand = new DelegateCommand(GoToShowGuest1Notifications));
        private void GoToShowGuest1Notifications()
        {
            NavService?.Navigate(new ShowGuest1NotificationsView(Guest1, this, NavService));
        }

        private void GoToShowSuperGuest(object sender, RoutedEventArgs e)
        {
            NavService?.Navigate(new ShowSuperGuest(Guest1, this));
        }

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            Window currentWindow = Window.GetWindow(ViewPage);

            LoginForm window = new LoginForm();
            window.Show();
            currentWindow.Close();
        }

        private int selectedComboBox1Index;
        public int SelectedComboBox1Index
        {
            get { return selectedComboBox1Index; }
            set
            {
                if (selectedComboBox1Index != value)
                {
                    selectedComboBox1Index = value;
                    OnPropertyChanged();
                }
            }
        }

        private int selectedComboBox2Index;
        public int SelectedComboBox2Index
        {
            get { return selectedComboBox2Index; }
            set
            {
                if (selectedComboBox2Index != value)
                {
                    selectedComboBox2Index = value;
                    OnPropertyChanged();
                }
            }
        }


        private void SetComboBoxes(Page page)
        {
            //{
            //    var comboBox = searchAndShowPage.SelectedComboBox1Index;
            //    if (comboBox != null)
            //    {
            //        SelectedComboBox1Index = comboBox;
            //    }

            //    comboBox = searchAndShowPage.SelectedComboBox2Index;
            //    if (comboBox != null)
            //    {
            //        SelectedComboBox2Index = comboBox;
            //    }
            //}

            if (page is SearchAndShowAccommodations searchAndShowPage)
            {
                var comboBox = searchAndShowPage.CBCreateReview;
                if (comboBox != null)
                {
                    SelectedComboBox1Index = comboBox.SelectedIndex;
                }

                comboBox = searchAndShowPage.CBSuperGuest;
                if (comboBox != null)
                {
                    SelectedComboBox2Index = comboBox.SelectedIndex;
                }
            }
            //else if (page is AccommodationReservation accommodationReservationPage)
            //{
            //    //var comboBox = accommodationReservationPage.CBCreateReview;
            //    //if (comboBox != null)
            //    //{
            //    //    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
            //    //}

            //    //comboBox = accommodationReservationPage.CBSuperGuest;
            //    //if (comboBox != null)
            //    //{
            //    //    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
            //    //}
            //}
            //else if (page is CreateReservationReschedulingRequest createReschedulingRequestPage)
            //{
            //    var comboBox = createReschedulingRequestPage.CBCreateReview;
            //    if (comboBox != null)
            //    {
            //        CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
            //    }

            //    comboBox = createReschedulingRequestPage.CBSuperGuest;
            //    if (comboBox != null)
            //    {
            //        CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
            //    }
            //}
            //else if (page is CreateReview createReviewPage)
            //{
            //    //var comboBox = createReviewPage.CBCreateReview;
            //    //if (comboBox != null)
            //    //{
            //    //    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
            //    //}

            //    //comboBox = createReviewPage.CBSuperGuest;
            //    //if (comboBox != null)
            //    //{
            //    //    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
            //    //}
            //}
            //else if (page is Guest1RequestPreview guest1RequestPreviewPage)
            //{
            //    var comboBox = guest1RequestPreviewPage.CBCreateReview;
            //    if (comboBox != null)
            //    {
            //        CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
            //    }

            //    comboBox = guest1RequestPreviewPage.CBSuperGuest;
            //    if (comboBox != null)
            //    {
            //        CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
            //    }
            //}
            //else if (page is Guest1Requests guest1RequestsPage)
            //{
            //    var comboBox = guest1RequestsPage.CBCreateReview;
            //    if (comboBox != null)
            //    {
            //        CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
            //    }

            //    comboBox = guest1RequestsPage.CBSuperGuest;
            //    if (comboBox != null)
            //    {
            //        CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
            //    }
            //}
            //else if (page is ShowGuest1Notifications showGuest1NotificationsPage)
            //{
            //    var comboBox = showGuest1NotificationsPage.CBCreateReview;
            //    if (comboBox != null)
            //    {
            //        CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
            //    }

            //    comboBox = showGuest1NotificationsPage.CBSuperGuest;
            //    if (comboBox != null)
            //    {
            //        CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
            //    }
            //}
            //else if (page is ShowOwnerReviews showOwnerReviewsPage)
            //{
            //    //var comboBox = showOwnerReviewsPage.CBCreateReview;
            //    //if (comboBox != null)
            //    //{
            //    //    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
            //    //}

            //    //comboBox = showOwnerReviewsPage.CBSuperGuest;
            //    //if (comboBox != null)
            //    //{
            //    //    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
            //    //}
            //}
            //else if (page is ShowReservations showReservationsPage)
            //{
            //    var comboBox = showReservationsPage.CBCreateReview;
            //    if (comboBox != null)
            //    {
            //        CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
            //    }

            //    comboBox = showReservationsPage.CBSuperGuest;
            //    if (comboBox != null)
            //    {
            //        CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
            //    }
            //}
        }

    }
}