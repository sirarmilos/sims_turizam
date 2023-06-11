using GalaSoft.MvvmLight.Command;
using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using static System.Net.Mime.MediaTypeNames;

namespace InitialProject.View
{
    public partial class AccommodationReservation : Page, INotifyPropertyChanged
    {
        private readonly ReservationService reservationService;
        private Accommodation accommodation;
        private int calendarReservationDays;
        private int guestsNumber;
        private List<DateSlot> freeDateSlots;
        private DateTime startDate;
        private DateTime endDate;
        private int actualReservationDays;
        private string guest1;

        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
            }
        }

        public Accommodation Accommodation
        {
            get { return accommodation; }
            set
            {
                accommodation = value;
                OnPropertyChanged();
            }
        }

        public int CalendarReservationDays
        {
            get { return calendarReservationDays; }
            set
            {
                calendarReservationDays = value;
                ActualReservationDays = calendarReservationDays;
                if (calendarReservationDays > 0)
                    calendarReservationDays -= 1;
                OnPropertyChanged();
            }
        }

        public List<DateSlot> FreeDateSlots
        {
            get { return freeDateSlots; }
            set
            {
                freeDateSlots = value;
                OnPropertyChanged();
            }
        }

        public int GuestsNumber
        {
            get { return guestsNumber; }
            set
            {
                guestsNumber = value;
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

        public int ActualReservationDays
        {
            get
            {
                return actualReservationDays;
            }
            set
            {
                actualReservationDays = value;
                OnPropertyChanged();
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

        private ObservableCollection<string> _images;
        private int _currentIndex;
        private string _currentImage;

        public ObservableCollection<string> Images
        {
            get { return _images; }
            set
            {
                _images = value;
                OnPropertyChanged(nameof(Images));
                UpdateCurrentImage();
            }
        }

        public string CurrentImage
        {
            get { return _currentImage; }
            set
            {
                _currentImage = value;
                OnPropertyChanged(nameof(CurrentImage));
            }
        }

        public Page Page
        {
            get;
            set;
        }

        public ICommand PreviousImageCommand { get; private set; }
        public ICommand NextImageCommand { get; private set; }

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

        public AccommodationReservation(Accommodation accommodation, string username, Page page)
        {
            InitializeComponent();
            DataContext = this;
            Guest1 = username;
            reservationService = new ReservationService(Guest1);
            Accommodation = accommodation;
            FreeDateSlots = new List<DateSlot>();

            SetUsernameHeader();

            SetImagePreviewer();

            SetComboBoxes(page);

            SetMessageShow("", Visibility.Collapsed);

            if (page is Guest1AnywhereAnytime)
            {
                LoadAnytimeAnywhereReservationWindow((Guest1AnywhereAnytime)page);
            }

            Page = page;    
        }

        private void LoadAnytimeAnywhereReservationWindow(Guest1AnywhereAnytime guest1AnywhereAnytime)
        {
            GuestsNumber = (int)guest1AnywhereAnytime.MaxGuests;
            ActualReservationDays = (int)guest1AnywhereAnytime.ReservationDays;
            CalendarReservationDays = (int)guest1AnywhereAnytime.ReservationDays;

            if (guest1AnywhereAnytime.StartDatePicker != null || guest1AnywhereAnytime.EndDatePicker != null)
            {
                StartDatePicker.SelectedDate = guest1AnywhereAnytime.StartDatePicker;
                EndDatePicker.SelectedDate = guest1AnywhereAnytime.EndDatePicker;

                Search(null,null);
            }
            else
            {
                StartDatePicker.SelectedDate = DateTime.Today.AddDays(1);
                EndDatePicker.SelectedDate = StartDatePicker.SelectedDate.Value.AddDays(30);

                Search(null,null);

                SetMessageShow("We have offered you first available period(s).", Visibility.Visible);
            }

            GuestNumberTB1.Visibility = Visibility.Collapsed;
            GuestNumberTB2.Visibility = Visibility.Collapsed;
            GuestNumberTB3.Visibility = Visibility.Collapsed;
            GuestNumberAndReservationDaysTB.Visibility = Visibility.Visible;

            //Search();

        }

        private void SetImagePreviewer()
        {
            Images = new ObservableCollection<string>(Accommodation.Images);
            _currentIndex = 0;
            PreviousImageCommand = new RelayCommand(PreviousImage);
            NextImageCommand = new RelayCommand(NextImage);
        }

        private void SetUsernameHeader()
        {
            Notification = reservationService.Guest1HasNotification();
            CheckNotification();
            usernameAndSuperGuest.Text = $"{Guest1}";
            superGuest.Text = $"{CheckSuperType()}";
        }


        private void PreviousImage()
        {
            _currentIndex--;
            if (_currentIndex < 0)
            {
                _currentIndex = Images.Count - 1;
            }
            UpdateCurrentImage();
        }

        private void NextImage()
        {
            _currentIndex++;
            if (_currentIndex >= Images.Count)
            {
                _currentIndex = 0;
            }
            UpdateCurrentImage();
        }

        private void UpdateCurrentImage()
        {
            if (Images != null && Images.Count > 0)
            {
                CurrentImage = Images[_currentIndex];
            }
            else
            {
                CurrentImage = null;
            }
            OnPropertyChanged(nameof(CurrentImage));
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            if (Page is Guest1AnywhereAnytime)
                GoToAnywhereAnytime(sender, e);
            else
                BackToFirstWindow(sender, e);   
        }
        public void BackToFirstWindow(object sender, RoutedEventArgs e)
        {
            SecondWindow.Visibility = Visibility.Collapsed;
            FirstWindow.Visibility = Visibility.Visible;
        }


        private void AllowOnlyDigits(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private string CheckSuperType()
        {
            string superType = string.Empty;

            if (reservationService.IsSuperGuest(Guest1))
            {
                superType = " (Super guest)";
            }

            return superType;
        }

        private void Search(object sender, RoutedEventArgs e)
        {

            SuggestedDatesMessage.Text = "";
            SuggestedDatesMessage.Visibility = Visibility.Collapsed;

            if (StartDatePicker.SelectedDate != null && EndDatePicker.SelectedDate != null)
            {
                if (!IsSearchInputValid()) return;

                FreeDateSlots = FindAvailable();
                if ((FreeDateSlots == null) || (FreeDateSlots.Count == 0))
                {
                    FreeDateSlots = new List<DateSlot>();
                    DateSlot temporaryDateSlot = FirstAvailableDate();
                    if (temporaryDateSlot == null)
                    {
                        SetMessageShow("Unfortunately, there are no available dates in the next one year. Try again.", Visibility.Visible);
                        return;
                    }
                }
            }
            else
            {
                SetMessageShow("No dates are selected.", Visibility.Visible);

                return;
            }

            ShowDateSlots();

            FirstWindow.Visibility = Visibility.Collapsed;
            SecondWindow.Visibility = Visibility.Visible;
        }

        private bool IsSearchInputValid()
        {
            StartDate = StartDatePicker.SelectedDate.Value;
            EndDate = EndDatePicker.SelectedDate.Value;

            if (StartDate.Date < DateTime.Now.Date || EndDate.Date < DateTime.Now.Date)
            {
                SetMessageShow("You can't choose dates from the past. Try again.", Visibility.Visible);
                return false;
            }

            if (CalendarReservationDays == 0 && ActualReservationDays == 0 || ActualReservationDays < 0)
            {
                SetMessageShow("Invalid number of reservation days. Try again.", Visibility.Visible);
                return false;
            }

            if (ActualReservationDays < Accommodation.MinDaysReservation)
            {
                SetMessageShow($"Number of reservation days couldn't be less than minimal days of reservation which is: {Accommodation.MinDaysReservation}. Try again.", Visibility.Visible);
                return false;
            }

            if (StartDate > EndDate)
            {
                SetMessageShow("Start date is greater than end date. Try again.", Visibility.Visible);
                return false;
            }

            if (StartDate.AddDays(ActualReservationDays) > EndDate)
            {
                SetMessageShow("Number of reservation days couldn't be more than selected dates. Try again.", Visibility.Visible);
                return false;
            }

            return true;
        }

        private void ShowDateSlots()
        {
            DateSlotsListBox.Items.Clear();
            foreach (var dateSlot in FreeDateSlots)
            {
                DateSlotsListBox.SelectionMode = SelectionMode.Single;
                DateSlotsListBox.IsHitTestVisible = false;
                DateSlotsListBox.Items.Add(dateSlot.StartDate.ToString("dd.MM.yyyy.") + " - " +
                                           dateSlot.EndDate.ToString("dd.MM.yyyy."));
            }
        }

        // research for available dates slots up to one year in advance
        private DateSlot FirstAvailableDate()
        {
            for (int i = 0; i < 12; i++)
            {
                StartDate = EndDate.AddDays(1);
                EndDate = StartDate.AddDays(30);
                List<DateSlot> temporaryDateSlots = FindAvailable();
                if (temporaryDateSlots.Count != 0)
                {
                    SetMessageShow("All our appointments are reserved. We have offered you the first available period.", Visibility.Visible);
                    return temporaryDateSlots[0];
                }

            }

            return null;

        }

        private void SetMessageShow(string message, Visibility visibilityStatus, SolidColorBrush messageColor = null)
        {
            if (messageColor == null)
                LabelColor = Brushes.Red;
            else
                LabelColor = messageColor;
         
            SuggestedDatesMessage.Text = message;
            SuggestedDatesMessage.Visibility = visibilityStatus;
        }

        private List<DateSlot> FindAvailable()
        {
            FreeDateSlots.Clear();

            FreeDateSlots = reservationService.FindAvailableDateSlots(FreeDateSlots, Accommodation, StartDate, EndDate, CalendarReservationDays); 
            
            return FreeDateSlots;
        }

        
        private void CreateReservation(object sender, RoutedEventArgs e)
        {
            SetMessageShow("", Visibility.Collapsed);

            if (GuestsNumber > Accommodation.MaxGuests)
            {
                SetMessageShow($"The number of guests couldn't be more than the maximum number of guests: {Accommodation.MaxGuests}. Try again.", Visibility.Visible);
                return;
            }

            if (GuestsNumber == 0)
            {
                SetMessageShow("The number of guests can't be zero. Try again.", Visibility.Visible);
                return;
            }

            if (ReservationStartDatePicker.SelectedDate != null && ReservationEndDatePicker.SelectedDate != null)
            {
                if (!IsCreateReservationInputValid()) return;

                // the user can choose only within offered dates
                if (!IsUsedOfferedDate()) return;

                reservationService.Save(new ShowReservationDTO(Accommodation, StartDate, EndDate, GuestsNumber));

                SetMessageShow("A reservation has been successfully created.", Visibility.Visible, Brushes.Green);
                MyReservationsButton.Visibility = Visibility.Visible;
                SecondWindow.Visibility = Visibility.Collapsed;
            }
            else
            {
                SetMessageShow("You need to select start and end date. Try again.", Visibility.Visible);
            }
        }
        
        private bool IsCreateReservationInputValid()
        {
            StartDate = ReservationStartDatePicker.SelectedDate.Value;
            EndDate = ReservationEndDatePicker.SelectedDate.Value;

            if ((FreeDateSlots == null) || (FreeDateSlots.Count == 0))
            {
                SetMessageShow("Firstly you need to look up available dates. Try again.", Visibility.Visible);
                return false;
            }

            if (StartDate > EndDate)
            {   
                SetMessageShow("Start date is greater than end date. Try again.", Visibility.Visible);
                return false;
            }

            if (StartDate.AddDays(CalendarReservationDays) != EndDate)
            {
                SetMessageShow("Invalid input: The start date and end date are not suitable for the number of reservation days. Try again.", Visibility.Visible);
                return false;
            }

            return true;
        }

        private bool IsUsedOfferedDate()
        {
            foreach (var dateSlot in FreeDateSlots)
            {
                if ((dateSlot.StartDate <= StartDate) && (dateSlot.EndDate >= EndDate))
                {
                    return true;
                }
            }

            SetMessageShow("You can only select available dates from the list bellow.", Visibility.Visible);
            return false;
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

        private void GoToAnywhereAnytime(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Guest1AnywhereAnytime(Guest1, this));
        }

        private void GoToShowOwnerReviews(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowOwnerReviews(Guest1, this));
        }

        //private void GoToGuest1Start(object sender, RoutedEventArgs e)
        //{
        //    NavigationService?.Navigate(new Guest1Start(Guest1, this));
        //}

        private void GoToForum(object sender, RoutedEventArgs e)
        {
            //NavigationService?.Navigate(new Guest1Forum(Guest1, this));
        }

        private void GoToSearchAndShowAccommodations(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new SearchAndShowAccommodations(Guest1, this));
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
}
