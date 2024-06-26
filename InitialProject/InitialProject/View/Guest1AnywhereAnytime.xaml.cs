﻿using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using GalaSoft.MvvmLight.Command;
using InitialProject.Service;
using InitialProject.DTO;
using System.DirectoryServices;

namespace InitialProject.View
{
    public partial class Guest1AnywhereAnytime : Page, INotifyPropertyChanged
    {
        AccommodationService accommodationService;
        public Accommodation Accommodation { get; set; }

        private string accommodationName;
        private string country;
        private string city;
        private int? maxGuests;
        private int? reservationDays;
        private int leftCancelationDays;
        private string image;
        private List<string> images;
        private string guest1;

        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
            }
        }

        public string Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged();
            }
        }

        public List<string> Images
        {
            get;
            set;
        }

        public int? MaxGuests
        {
            get { return maxGuests; }
            set
            {
                maxGuests = value;
                OnPropertyChanged();
            }
        }

        public int? ReservationDays
        {
            get { return reservationDays; }
            set
            {
                reservationDays = value;
                OnPropertyChanged();
            }
        }






        private DateTime startDate;
        private DateTime endDate;
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

        public List<Accommodation> Accommodations
        {
            get;
            set;
        }

        private List<DateSlot> freeDateSlots;
        public List<DateSlot> FreeDateSlots
        {
            get { return freeDateSlots; }
            set
            {
                freeDateSlots = value;
                OnPropertyChanged();
            }
        }

        private DateTime? startDatePicker;
        public DateTime? StartDatePicker
        {
            get { return startDatePicker; }
            set
            {
                startDatePicker = value;

                OnPropertyChanged(nameof(StartDatePicker));

                
                //// Ažurirajte vrednost EndDatePicker na istu vrednost kao StartDatePicker
                //Izlaz = Ulaz;
            }
        }

        //private DateTime? jokerIzlaz;
        //public DateTime? JokerIzlaz
        //{
        //    get { return jokerIzlaz; }
        //    set
        //    {
        //        jokerIzlaz = value;
        //        StartDatePicker.SelectedDate = value;

        //        OnPropertyChanged(nameof(Joker));


        //        //// Ažurirajte vrednost EndDatePicker na istu vrednost kao StartDatePicker
        //        //Izlaz = Ulaz;
        //    }
        //}

        private DateTime? endDatePicker;
        public DateTime? EndDatePicker
        {
            get { return endDatePicker; }
            set
            {
                endDatePicker = value;


                OnPropertyChanged(nameof(EndDatePicker));

                //// Ažurirajte vrednost EndDatePicker na istu vrednost kao StartDatePicker
                //Izlaz = Ulaz;
            }
        }




        public ICommand SeeAvailabilityCommand { get; set; }

        private void AllowOnlyCharacters(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsLetter(e.Text, 0) && !char.IsWhiteSpace(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void AllowOnlyDigits(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Guest1AnywhereAnytime(string username, Page page)
        {
            InitializeComponent();

            FirstSearchWindow.Visibility = Visibility.Visible;
            SecondSearchWindow.Visibility = Visibility.Collapsed;

            Guest1 = username;
            DataContext = this;
            accommodationService = new AccommodationService(Guest1);

            SetUsernameHeader();

            Images = new List<string>();
            SeeAvailabilityCommand = new RelayCommand<Accommodation>(SeeAvailability);

            SetComboBoxes(page);

            if (page is AccommodationReservation accommodationReservationPage)
            {
                FirstSearchWindow.Visibility = Visibility.Collapsed;

                SecondSearchWindow.Visibility = Visibility.Visible;
            }



            FreeDateSlots = new List<DateSlot>();

        }

        private void SetUsernameHeader()
        {
            Notification = accommodationService.Guest1HasNotification();
            CheckNotification();
            usernameAndSuperGuest.Text = $"{Guest1}";
            superGuest.Text = $"{CheckSuperType()}";
        }

        private string CheckSuperType()
        {
            string superType = string.Empty;

            if (accommodationService.IsSuperGuest(Guest1))
            {
                superType = "(Super guest)";
            }

            return superType;
        }


        private void Search(object sender, RoutedEventArgs e)
        {
            FirstSearchWindow.Visibility = Visibility.Collapsed;

            SecondSearchWindow.Visibility = Visibility.Visible;

            if (!AreRequiredFieldsFilled()) return;


            SearchAndShowAccommodationDTO searchShowAndAccommodationDTO =
                new SearchAndShowAccommodationDTO("", "", "", "", MaxGuests, ReservationDays);

            Accommodations = accommodationService.FindAll(searchShowAndAccommodationDTO);

            FilterOutRemoved();

            if (Accommodations == null)
            {
                ListAccommodations.ItemsSource = null;
                SuggestedDatesMessage.Text = "No accommodation satisfies your requirements.";
                ListAccommodations.Visibility = Visibility.Collapsed;
                SuggestedDatesMessage.Visibility = Visibility.Visible;

                return;
            }

            ListAccommodations.ItemsSource = new ObservableCollection<Accommodation>(Accommodations);
            ListAccommodations.Visibility = Visibility.Visible;
            SuggestedDatesMessage.Visibility = Visibility.Collapsed;


            if (IsSearchDatePickersInputValid())
                FilterAccommodations();


        }

        private void FilterOutRemoved()
        {
            List<Accommodation> resultsToKeep = new List<Accommodation>();

            if (Accommodations == null) return;

            foreach (var result in Accommodations)
            {
                if (result.Removed != true)
                    resultsToKeep.Add(result);
            }

            Accommodations = resultsToKeep;
        }

        private bool AreRequiredFieldsFilled()
        {
            if ((MaxGuests != null) && (MaxGuests == 0))
            {
                SuggestedDatesMessage.Text = "You can't use zero as number of guests.";
                ListAccommodations.Visibility = Visibility.Collapsed;
                SuggestedDatesMessage.Visibility = Visibility.Visible;
                ListAccommodations.ItemsSource = null;

                return false;
            }

            if (MaxGuests == null)
            {
                SuggestedDatesMessage.Text = "You need to input number of guests.";
                ListAccommodations.Visibility = Visibility.Collapsed;
                SuggestedDatesMessage.Visibility = Visibility.Visible;
                ListAccommodations.ItemsSource = null;

                return false;
            }

            if (ReservationDays == null)
            {
                SuggestedDatesMessage.Text = "You need to input number of reservation days.";
                ListAccommodations.Visibility = Visibility.Collapsed;
                SuggestedDatesMessage.Visibility = Visibility.Visible;
                ListAccommodations.ItemsSource = null;

                return false;
            }

            return true;
        }


        // filtriranje uz odabran raspon datuma
        private void FilterAccommodations()
        {

            //SuggestedDatesMessage.Text = "";
            //SuggestedDatesMessage.Visibility = Visibility.Collapsed;

            foreach (var accommodation in Accommodations)
            {
                FreeDateSlots = FindAvailable(accommodation);
                if ((FreeDateSlots == null) || (FreeDateSlots.Count == 0))
                {
                    Accommodations.Remove(accommodation);
                }

                if (Accommodations.Count == 0)
                    break;
            }

            ListAccommodations.ItemsSource = new ObservableCollection<Accommodation>(Accommodations);
            ListAccommodations.Visibility = Visibility.Visible;
            SuggestedDatesMessage.Visibility = Visibility.Collapsed;
        }

        private List<DateSlot> FindAvailable(Accommodation accommodation)
        {
            FreeDateSlots.Clear();

            FreeDateSlots = accommodationService.FindAvailableDateSlots(FreeDateSlots, accommodation, StartDate, EndDate, (int)ReservationDays);

            return FreeDateSlots;
        }



        private bool IsSearchDatePickersInputValid()
        {
            if (StartDatePicker == null && EndDatePicker == null) return false;

            if ((StartDatePicker != null && EndDatePicker == null) ||
                (StartDatePicker == null && EndDatePicker != null))
            {
                SuggestedDatesMessage.Text = "You can't choose only one date. Try again.";
                ListAccommodations.Visibility = Visibility.Collapsed;
                SuggestedDatesMessage.Visibility = Visibility.Visible;
                ListAccommodations.ItemsSource = null;

                return false;
            }

            StartDate = (DateTime)StartDatePicker;
            EndDate = (DateTime)EndDatePicker;

            if (StartDate.Date < DateTime.Now.Date || EndDate.Date < DateTime.Now.Date)
            {
                //SetMessageShow("You can't choose dates from the past. Try again.", Visibility.Visible);
                SuggestedDatesMessage.Text = "You can't choose dates from the past. Try again.";
                ListAccommodations.Visibility = Visibility.Collapsed;
                SuggestedDatesMessage.Visibility = Visibility.Visible;
                ListAccommodations.ItemsSource = null;

                return false;
            }

            //if (CalendarReservationDays == 0 && ActualReservationDays == 0 || ActualReservationDays < 0)
            //{
            //    SetMessageShow("Invalid number of reservation days. Try again.", Visibility.Visible);
            //    return false;
            //}

            //if (ActualReservationDays < Accommodation.MinDaysReservation)
            //{
            //    SetMessageShow($"Number of reservation days couldn't be less than minimal days of reservation which is: {Accommodation.MinDaysReservation}. Try again.", Visibility.Visible);
            //    return false;
            //}

            if (StartDate > EndDate)
            {
                //SetMessageShow("Start date is greater than end date. Try again.", Visibility.Visible);
                SuggestedDatesMessage.Text = "Start date is greater than end date. Try again.";
                ListAccommodations.Visibility = Visibility.Collapsed;
                SuggestedDatesMessage.Visibility = Visibility.Visible;
                ListAccommodations.ItemsSource = null;

                return false;
            }

            if (StartDate.AddDays((double)ReservationDays) > EndDate)
            {
                //SetMessageShow("Number of reservation days couldn't be more than selected dates. Try again.", Visibility.Visible);
                SuggestedDatesMessage.Text = "Number of reservation days couldn't be more than selected dates. Try again.";
                ListAccommodations.Visibility = Visibility.Collapsed;
                SuggestedDatesMessage.Visibility = Visibility.Visible;
                ListAccommodations.ItemsSource = null;

                return false;
            }

            return true;
        }









        private bool comboBoxClicked = false;
        private bool itemClicked = false;

        private void CBPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            comboBoxClicked = true;
        }


        public string SelectedCreateReviewCBItem { get; set;} // za azurnu verziju comboboxova
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

        private void GoToAnywhereAnytime(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Guest1AnywhereAnytime(Guest1, this));
        }

        private void GoToShowSuperGuest(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowSuperGuest(Guest1, this));
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
