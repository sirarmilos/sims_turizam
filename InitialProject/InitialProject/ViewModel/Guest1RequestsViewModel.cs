using GalaSoft.MvvmLight.Command;
using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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

    public partial class Guest1RequestsViewModel : Page, INotifyPropertyChanged
    {
        private readonly ReservationReschedulingRequestService reservationReschedulingRequestService;
        private string accommodationName;
        private DateTime oldStartDate;
        private DateTime oldEndDate;
        private DateTime newStartDate;
        private DateTime newEndDate;
        private string guest1;

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

        public DateTime OldStartDate
        {
            get { return oldStartDate; }
            set
            {
                oldStartDate = value;
            }
        }

        public DateTime OldEndtDate
        {
            get { return oldEndDate; }
            set
            {
                oldEndDate = value;
            }
        }

        public DateTime NewStartDate
        {
            get { return newStartDate; }
            set
            {
                newStartDate = value;
            }
        }

        public DateTime NewEndDate
        {
            get { return newEndDate; }
            set
            {
                newEndDate = value;
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

        private Visibility isNotificationVisible;

        public Visibility IsNotificationVisible
        {
            get { return isNotificationVisible; }
            set
            {
                isNotificationVisible = value;
                OnPropertyChanged();
            }
        }

        private Visibility isRegularBellVisible;

        public Visibility IsRegularBellVisible
        {
            get { return isRegularBellVisible; }
            set
            {
                isRegularBellVisible = value;
                OnPropertyChanged();
            }
        }


        public NavigationService NavService { get; set; }

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

        public ObservableCollection<Guest1RebookingRequestsDTO> Guest1RebookingRequestsDTOs
        {
            get;
            set;
        }

        Page ViewPage { get; set; }

        public Guest1RequestsViewModel(string username, Guest1RequestsView guest1RequestsView, Page page, NavigationService navService)
        {
            Guest1 = username;

            reservationReschedulingRequestService = new ReservationReschedulingRequestService(Guest1);

            Guest1RebookingRequestsDTOs = new ObservableCollection<Guest1RebookingRequestsDTO>(reservationReschedulingRequestService.FindAllByGuest1Username());

            MoreDetailsCommand = new RelayCommand<Guest1RebookingRequestsDTO>(MoreDetails);




            SetUsernameHeader();

            SetComboBoxes(page);

            NavService = navService;

            ViewPage = guest1RequestsView;

            ComboBoxCommand = new DelegateCommand<object>(ComboBoxAction);
            ComboBoxSGCommand = new DelegateCommand<object>(ComboBoxSuperGuestAction);
            ComboBoxFilterCommand = new DelegateCommand<object>(ComboBoxFilterAction);
            SelectedComboBox3Index = 0;

            GoToShowReservationsCommand = new RelayCommand(GoToShowReservations);
            GoToAnywhereAnytimeCommand = new RelayCommand(GoToAnywhereAnytime);
            GoToSearchAndShowAccommodationsCommand = new RelayCommand(GoToSearchAndShowAccommodations);
            GoToForumCommand = new RelayCommand(GoToForum);

            GoToCreateRequestCommand = new RelayCommand(GoToShowReservations);
        }
        public ICommand GoToCreateRequestCommand { get; set; }

        private string _selectedReview;

        public string SelectedReview
        {
            get { return _selectedReview; }
            set
            {
                if (_selectedReview != value)
                {
                    _selectedReview = value;
                    OnPropertyChanged(nameof(SelectedReview));
                }
            }
        }

        private bool _isDropDownOpen;
        public bool IsDropDownOpen
        {
            get { return _isDropDownOpen; }
            set
            {
                _isDropDownOpen = value;
                OnPropertyChanged(nameof(IsDropDownOpen));
            }
        }

        //public ICommand ComboBoxFilterCommand { get; } 

        public void ShowAllRequests(object sender, MouseButtonEventArgs e)
        {
            IsDropDownOpen = false;
            SelectedComboBox3Index = 0;

            Guest1RebookingRequestsDTOs.Clear();
            var requests = reservationReschedulingRequestService.FindAllByGuest1Username();
            foreach (var request in requests)
            {
                Guest1RebookingRequestsDTOs.Add(request);
            }
        }


        public void ShowAllPendingRequests(object sender, MouseButtonEventArgs e)
        {
            IsDropDownOpen = false;
            SelectedComboBox3Index = 2;

            Guest1RebookingRequestsDTOs.Clear();
            var requests = reservationReschedulingRequestService.FindAllPendingByGuest1Username();
            foreach (var request in requests)
            {
                Guest1RebookingRequestsDTOs.Add(request);
            }
        }

        public void ShowAllRejectedRequests(object sender, MouseButtonEventArgs e)
        {
            IsDropDownOpen = false;
            SelectedComboBox3Index = 1;

            Guest1RebookingRequestsDTOs.Clear();
            var requests = reservationReschedulingRequestService.FindAllRejectedByGuest1Username();
            foreach (var request in requests)
            {
                Guest1RebookingRequestsDTOs.Add(request);
            }
        }

        public void ShowAllAcceptedRequests(object sender, MouseButtonEventArgs e)
        {
            IsDropDownOpen = false;
            SelectedComboBox3Index = 3;

            Guest1RebookingRequestsDTOs.Clear();
            var requests = reservationReschedulingRequestService.FindAllAcceptedByGuest1Username();
            foreach (var request in requests)
            {
                Guest1RebookingRequestsDTOs.Add(request);
            }
        }

        public ICommand MoreDetailsCommand { get; set; }

        private void MoreDetails(Guest1RebookingRequestsDTO guest1RebookingRequestsDTO)
        {

            //NavService?.Navigate(new Guest1RequestPreview(Guest1, guest1RebookingRequestsDTO, "Guest1Requests", this));
            NavService?.Navigate(new Guest1RequestPreviewView(Guest1, this, NavService, guest1RebookingRequestsDTO, "Guest1Requests"));
        }




        private string usernameAndSuperGuestText;

        public string UsernameAndSuperGuestText
        {
            get { return usernameAndSuperGuestText; }
            set
            {
                usernameAndSuperGuestText = value;
                OnPropertyChanged();
            }
        }

        private string superGuestText;

        public string SuperGuestText
        {
            get { return superGuestText; }
            set
            {
                superGuestText = value;
                OnPropertyChanged();
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
                    GoToGuest1Requests();

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


        public DelegateCommand<object> ComboBoxFilterCommand { get; }
        private void ComboBoxFilterAction(object parameter)
        {
            if (parameter != null)
            {
                if (parameter.Equals("All"))
                {
                    ShowAllRequests(null, null);
                    //SelectedComboBox3Index = 0;
                }
                else if (parameter.Equals("Pending"))
                {
                    //SelectedComboBox3Index = 2;
                    ShowAllPendingRequests(null, null);

                }
                else if (parameter.Equals("Rejected"))
                {
                    //SelectedComboBox3Index = 1;
                    ShowAllRejectedRequests(null, null);
                }
                else if (parameter.Equals("Accepted"))
                {
                    //SelectedComboBox3Index = 3;
                    ShowAllAcceptedRequests(null, null);
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

        private void GoToGuest1Requests()
        {
            NavService?.Navigate(new Guest1RequestsView(Guest1, this, NavService));
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

        private int selectedComboBox3Index;
        public int SelectedComboBox3Index
        {
            get { return selectedComboBox3Index; }
            set
            {
                if (selectedComboBox3Index != value)
                {
                    selectedComboBox3Index = value;
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

