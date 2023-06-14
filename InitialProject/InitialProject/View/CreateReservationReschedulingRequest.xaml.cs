using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Windows.Media;
using System.Windows.Navigation;

namespace InitialProject.View
{
    public partial class CreateReservationReschedulingRequest : Page, INotifyPropertyChanged
    {
        private readonly ReservationReschedulingRequestService reservationReschedulingRequestService;
        private ShowReservationDTO showReservationDTO;
        private DateTime startDate;
        private DateTime endDate;
        private DateTime oldStartDate;
        private DateTime oldendDate;
        private string guest1;

        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
            }
        }

        public ShowReservationDTO ShowReservationDTO
        {
            get { return showReservationDTO; }
            set
            {
                showReservationDTO = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime OldStartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime OldEndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged();
            }
        }

        public string Caller { get; set; }

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
                NotificationMenuItemImageNotificationBell.Visibility = Visibility.Visible;
                NotificationMenuItemImageRegularBell.Visibility = Visibility.Collapsed;
            }
            else
            {
                NotificationMenuItemImageNotificationBell.Visibility = Visibility.Collapsed;
                NotificationMenuItemImageRegularBell.Visibility = Visibility.Visible;
            }
        }


        public CreateReservationReschedulingRequest(ShowReservationDTO showReservationDTO, string username, string callingWindow, Page page)
        {
            InitializeComponent();
            DataContext = this;
            Guest1 = username;
            Caller = callingWindow;

            StackPanel1.Visibility = Visibility.Visible;
            StackPanel2.Visibility = Visibility.Collapsed;

            ShowReservationDTO = showReservationDTO;    
            reservationReschedulingRequestService = new ReservationReschedulingRequestService(Guest1);
            
            SetUsernameHeader();

            AccommodationNameLabel.Content = showReservationDTO.Accommodation.AccommodationName.ToString();
            OldStartDate = showReservationDTO.StartDate;
            OldEndDate = showReservationDTO.EndDate;

            SetComboBoxes(page);

        }

        private void SetUsernameHeader()
        {
            Notification = reservationReschedulingRequestService.Guest1HasNotification();
            CheckNotification();
            usernameAndSuperGuest.Text = $"{Guest1}";
            superGuest.Text = $"{CheckSuperType()}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ReturnBackToCaller(object sender, RoutedEventArgs e)
        {
            //if (Caller.Equals("ShowReservations"))
            //    GoToShowReservations(sender, e);
            //else
            //    GoToGuest1Requests(sender, e);
            GoToGuest1Requests(sender, e);
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

        private void Create(object sender, RoutedEventArgs e)
        {
            SuggestedDatesMessage.Text = "";

            if (StartDatePicker.SelectedDate != null && EndDatePicker.SelectedDate != null) 
            {
                if (!IsSearchInputValid()) return;

                CreateReservationReschedulingRequestDTO request = 
                    new CreateReservationReschedulingRequestDTO(showReservationDTO, StartDate, EndDate);

                reservationReschedulingRequestService.CreateRequest(request);

                SuccesfullyCreatedMessage.Content = "The request has been successfully created!";

                StackPanel1.Visibility = Visibility.Collapsed;
                StackPanel2.Visibility = Visibility.Visible;

            }
            else
            {
                SetErrorMessageShow("No dates are selected.");

                return;
            }
        }

        private bool IsSearchInputValid()
        {
            StartDate = StartDatePicker.SelectedDate.Value;
            EndDate = EndDatePicker.SelectedDate.Value;

            int newReservationDays = EndDate.Subtract(StartDate).Days;

            if (StartDate > EndDate)
            {
                SetErrorMessageShow("Start date is greater than end date. Try again.");

                return false;
            }

            if ((StartDate == ShowReservationDTO.StartDate) & (EndDate == ShowReservationDTO.EndDate))
            {
                SetErrorMessageShow("Choose different dates. Try again.");

                return false;
            }

            if (StartDate.Date < DateTime.Now.Date)
            {
                SetErrorMessageShow("The start date is in the past. Try again.");

                return false;
            }

            if (EndDate.Date < DateTime.Now.Date)
            {
                SetErrorMessageShow("The end date is in the past. Try again.");

                return false;
            }

            if (newReservationDays + 1 < ShowReservationDTO.Accommodation.MinDaysReservation)
            {
                SetErrorMessageShow($"Number of reservation days couldn't be less than minimal days of reservation which is: {ShowReservationDTO.Accommodation.MinDaysReservation}. Try again.");

                return false;
            }

            return true;
        }

        private void SetErrorMessageShow(string message, SolidColorBrush messageColor = null)
        {
            if (messageColor == null)
                LabelColor = Brushes.Red;
            else
                LabelColor = messageColor;

            SuggestedDatesMessage.Text = message;
        }



        private bool comboBoxClicked = false;
        private bool itemClicked = false;

        private void CBPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            comboBoxClicked = true;
        }


        public string SelectedCreateReviewCBItem { get; set; } // za azurnu verziju comboboxova
        private void CBCreateReviewDropDownClosed(object sender, EventArgs e)
        {
            if (comboBoxClicked && itemClicked)
            {
                ComboBox comboBox = (ComboBox)sender;
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;

                if (selectedItem.Content.ToString() == "Create review")
                {
                    SelectedCreateReviewCBItem = selectedItem.Content.ToString();
                    GoToCreateReview(sender, null);
                }
                else if (selectedItem.Content.ToString() == "Reviews")
                {
                    SelectedCreateReviewCBItem = selectedItem.Content.ToString();

                    //NavigationService?.Navigate(new Guest1GenerateReport(Guest1, this));
                    GoToShowOwnerReviews(sender, null);

                }
                else if (selectedItem.Content.ToString() == "Requests")
                {
                    SelectedCreateReviewCBItem = selectedItem.Content.ToString();
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
                    GoToShowSuperGuest(sender, null);
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

        private void SeeAvailability(Accommodation accommodation)
        {
            NavigationService?.Navigate(new AccommodationReservation(accommodation, Guest1, this));
        }

        private void GoToShowOwnerReviews(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowOwnerReviewsView(Guest1, this, this.NavigationService));
        }

        private void GoToSearchAndShowAccommodations(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new SearchAndShowAccommodations(Guest1, this));
        }
        private void GoToForum(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Guest1ForumView(Guest1, this, this.NavigationService));

        }

        private void GoToShowSuperGuest(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowSuperGuest(Guest1, this));
        }

        private void GoToAnywhereAnytime(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Guest1AnywhereAnytime(Guest1, this));
        }


        private void GoToShowReservations(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowReservationsView(Guest1, this, this.NavigationService));


        }

        private void GoToCreateReview(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CreateReview(Guest1, this));
        }

        private void GoToGuest1Requests(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Guest1RequestsView(Guest1, this, this.NavigationService));
        }

        private void GoToShowGuest1Notifications(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowGuest1NotificationsView(Guest1, this, this.NavigationService));
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
                var comboBox = accommodationReservationPage.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = accommodationReservationPage.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
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
                var comboBox = createReviewPage.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = createReviewPage.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
            else if (page is Guest1GenerateReport guest1GenerateReport)
            {
                var comboBox = guest1GenerateReport.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = guest1GenerateReport.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
            else if (page is Guest1AnywhereAnytime anywhereAnytime)
            {
                var comboBox = anywhereAnytime.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = anywhereAnytime.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
            else if (page is ShowSuperGuest showSuperGuest)
            {
                var comboBox = showSuperGuest.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = showSuperGuest.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
            else if (page is Guest1CreateForum guest1CreateForum)
            {
                var comboBox = guest1CreateForum.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = guest1CreateForum.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
            else if (page is Guest1ForumPreview guest1ForumPreview)
            {
                var comboBox = guest1ForumPreview.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = guest1ForumPreview.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
            else if (page is Guest1RequestPreviewViewModel guest1RequestPreviewPage) // 6. MVVM 
            {
                CBCreateReview.SelectedIndex = guest1RequestPreviewPage.SelectedComboBox1Index;
                CBSuperGuest.SelectedIndex = guest1RequestPreviewPage.SelectedComboBox2Index;
            }
            else if (page is Guest1RequestsViewModel guest1RequestsPage) // 5. MVVM
            {
                CBCreateReview.SelectedIndex = guest1RequestsPage.SelectedComboBox1Index;
                CBSuperGuest.SelectedIndex = guest1RequestsPage.SelectedComboBox2Index;
            }
            else if (page is ShowGuest1NotificationsViewModel showGuest1NotificationsPage) // 2. MVVM
            {
                CBCreateReview.SelectedIndex = showGuest1NotificationsPage.SelectedComboBox1Index;
                CBSuperGuest.SelectedIndex = showGuest1NotificationsPage.SelectedComboBox2Index;
            }
            else if (page is ShowOwnerReviewsViewModel showOwnerReviewsPage) // 4. MVVM
            {
                CBCreateReview.SelectedIndex = showOwnerReviewsPage.SelectedComboBox1Index;
                CBSuperGuest.SelectedIndex = showOwnerReviewsPage.SelectedComboBox2Index;
            }
            else if (page is ShowReservationsViewModel showReservationsPage) // 1. MVVM
            {
                CBCreateReview.SelectedIndex = showReservationsPage.SelectedComboBox1Index;
                CBSuperGuest.SelectedIndex = showReservationsPage.SelectedComboBox2Index;
            }
            else if (page is Guest1ForumViewModel guest1ForumViewModel) // 3. MVVM
            {
                CBCreateReview.SelectedIndex = guest1ForumViewModel.SelectedComboBox1Index;
                CBSuperGuest.SelectedIndex = guest1ForumViewModel.SelectedComboBox2Index;
            }
        }

    }
}
