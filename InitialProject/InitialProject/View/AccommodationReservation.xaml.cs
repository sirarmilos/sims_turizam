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
        private Location location;
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

        public Location Location
        {
            get { return location; }
            set { }
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

        public AccommodationReservation(Accommodation accommodation, string username, Page page)
        {
            InitializeComponent();
            DataContext = this;
            Guest1 = username;
            reservationService = new ReservationService(Guest1);
            Notification = reservationService.Guest1HasNotification();
            CheckNotification();

            usernameAndSuperGuest.Text = $"{Guest1}";
            superGuest.Text = $"{CheckSuperType()}";

            Accommodation = accommodation;
            Location = accommodation.Location;
            FreeDateSlots = new List<DateSlot>();

            //AccommodationLabel.Content = accommodation.AccommodationName;
            //AccommodationLabelMinReservationDays.Content = accommodation.MinDaysReservation;
            //AccommodationLabelMaxGuests.Content = accommodation.MaxGuests;
            List<Accommodation> list = new List<Accommodation>();
            list.Add(accommodation);
            //listAccommodations.ItemsSource = list;

            Images = new ObservableCollection<string>(Accommodation.Images);
            _currentIndex = 0;
            PreviousImageCommand = new RelayCommand(PreviousImage);
            NextImageCommand = new RelayCommand(NextImage);

            SetComboBoxes(page);

            LabelColor = Brushes.Red;
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

        public ICommand PreviousImageCommand { get; private set; }
        public ICommand NextImageCommand { get; private set; }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                        SuggestedDatesMessage.Text = "Unfortunately, there are no available dates in the next one year. Try again.";
                        SuggestedDatesMessage.Visibility = Visibility.Visible;
                        //MessageBox.Show($"Unfortunately, there are no available dates in the next one year. Try again.");
                        return;
                    }
                }
            }
            else
            {
                SuggestedDatesMessage.Text = "No dates are selected.";
                SuggestedDatesMessage.Visibility = Visibility.Visible;
                //MessageBox.Show($"No dates are selected.");
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
                SuggestedDatesMessage.Text = "You can't choose dates from the past. Try again.";
                SuggestedDatesMessage.Visibility = Visibility.Visible;
                return false;
            }

            if (CalendarReservationDays == 0 && ActualReservationDays == 0)
            {
                SuggestedDatesMessage.Text = "Invalid number of reservation days. Try again.";
                SuggestedDatesMessage.Visibility = Visibility.Visible;
                return false;
            }

            if (ActualReservationDays < 0)
            {
                SuggestedDatesMessage.Text = "Invalid number of reservation days. Try again.";
                SuggestedDatesMessage.Visibility = Visibility.Visible;
                //MessageBox.Show($"Invalid number of reservation days. Try again.");
                return false;
            }

            if (ActualReservationDays < Accommodation.MinDaysReservation)
            {
                SuggestedDatesMessage.Text = $"Number of reservation days couldn't be less than minimal days of reservation which is: {Accommodation.MinDaysReservation}. Try again.";
                SuggestedDatesMessage.Visibility = Visibility.Visible;
                //MessageBox.Show(
                //    $"Number of reservation days couldn't be less than minimal days of reservation which is: {Accommodation.MinDaysReservation}. Try again.");
                return false;
            }

            if (StartDate > EndDate)
            {
                SuggestedDatesMessage.Text = "Start date is greater than end date. Try again.";
                SuggestedDatesMessage.Visibility = Visibility.Visible;
                //MessageBox.Show($"Start date is greater than end date. Try again.");
                return false;
            }

            if (StartDate.AddDays(ActualReservationDays) > EndDate)
            {
                SuggestedDatesMessage.Text = "Number of reservation days couldn't be more than selected dates. Try again.";
                SuggestedDatesMessage.Visibility = Visibility.Visible;

                //MessageBox.Show($"Number of reservation days couldn't be more than selected dates. Try again.");
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
                    SuggestedDatesMessage.Text = "All our appointments are reserved. We have offered you the first available period.";
                    SuggestedDatesMessage.Visibility = Visibility.Visible;
                    return temporaryDateSlots[0];
                }

            }

            return null;

        }

        private List<DateSlot> FindAvailable()
        {
            FreeDateSlots.Clear();

            FreeDateSlots = reservationService.FindAvailableDateSlots(FreeDateSlots, Accommodation, StartDate, EndDate, CalendarReservationDays); 
            
            return FreeDateSlots;
        }

        
        private void CreateReservation(object sender, RoutedEventArgs e)
        {
            SuggestedDatesMessage.Visibility = Visibility.Collapsed;

            if (GuestsNumber > Accommodation.MaxGuests)
            {
                SuggestedDatesMessage.Text = $"The number of guests couldn't be more than the maximum number of guests: {Accommodation.MaxGuests}. Try again.";
                SuggestedDatesMessage.Visibility = Visibility.Visible;

                //MessageBox.Show($"The number of guests couldn't be more than the maximum number of guests: {Accommodation.MaxGuests}. Try again.");
                return;
            }

            if (GuestsNumber == 0)
            {
                SuggestedDatesMessage.Text = "The number of guests can't be zero. Try again.";
                SuggestedDatesMessage.Visibility = Visibility.Visible;

                return;
            }

            if (ReservationStartDatePicker.SelectedDate != null && ReservationEndDatePicker.SelectedDate != null)
            {
                if (!IsCreateReservationInputValid()) return;

                // the user can choose only within offered dates
                if (!IsUsedOfferedDate()) return;

                reservationService.Save(new ShowReservationDTO(Accommodation, StartDate, EndDate, GuestsNumber));

                SuggestedDatesMessage.Text = "A reservation has been successfully created.";
                LabelColor = Brushes.Green;
                SuggestedDatesMessage.Visibility = Visibility.Visible;
                MyReservationsButton.Visibility = Visibility.Visible;
                SecondWindow.Visibility = Visibility.Collapsed;


                //MessageBox.Show($"A reservation has been successfully created.");
            }
            else
            {
                SuggestedDatesMessage.Text = "You need to select start and end date. Try again.";
                SuggestedDatesMessage.Visibility = Visibility.Visible;

                //MessageBox.Show($"You need to select start and end date. Try again.");
            }
        }
        
        private bool IsCreateReservationInputValid()
        {
            StartDate = ReservationStartDatePicker.SelectedDate.Value;
            EndDate = ReservationEndDatePicker.SelectedDate.Value;

            if ((FreeDateSlots == null) || (FreeDateSlots.Count == 0))
            {
                SuggestedDatesMessage.Text = "Firstly you need to look up available dates. Try again.";
                SuggestedDatesMessage.Visibility = Visibility.Visible;

                //MessageBox.Show($"Firstly you need to look up available dates. Try again.");
                return false;
            }

            if (StartDate > EndDate)
            {
                SuggestedDatesMessage.Text = "Start date is greater than end date. Try again.";
                SuggestedDatesMessage.Visibility = Visibility.Visible;

                //MessageBox.Show($"Start date is greater than end date. Try again.");
                return false;
            }

            if (StartDate.AddDays(CalendarReservationDays) != EndDate)
            {
                SuggestedDatesMessage.Text = "Invalid input: The start date and end date are not suitable for the number of reservation days. Try again.";
                SuggestedDatesMessage.Visibility = Visibility.Visible;

                //MessageBox.Show( $"Invalid input: The start date and end date are not suitable for the number of reservation days. Try again.");
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

            SuggestedDatesMessage.Text = "You can only select available dates from the list above.";
            SuggestedDatesMessage.Visibility = Visibility.Visible;

            //MessageBox.Show($"You can only select available dates from the list above.");
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
