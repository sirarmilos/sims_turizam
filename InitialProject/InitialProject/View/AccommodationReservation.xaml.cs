using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InitialProject.View
{
    public partial class AccommodationReservation : Window
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
                NotificationMenuItem.Visibility = Visibility.Visible;
            }
            else
            {
                NotificationMenuItem.Visibility = Visibility.Collapsed;
            }
        }

        public AccommodationReservation(Accommodation accommodation, string username)
        {
            InitializeComponent();
            DataContext = this;
            Guest1 = username;
            reservationService = new ReservationService(Guest1);
            Notification = reservationService.Guest1HasNotification();
            CheckNotification();

            Accommodation = accommodation;
            Location = accommodation.Location;
            FreeDateSlots = new List<DateSlot>();

            AccommodationLabel.Content = accommodation.AccommodationName;
            AccommodationLabelMinReservationDays.Content = accommodation.MinDaysReservation;
            AccommodationLabelMaxGuests.Content = accommodation.MaxGuests;
            List<Accommodation> list = new List<Accommodation>();
            list.Add(accommodation);
            listAccommodations.ItemsSource = list;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AllowOnlyDigits(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            SuggestedDatesMessage.Content = "";

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
                        MessageBox.Show($"Unfortunately, there are no available dates in the next one year. Try again.");
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show($"No dates are selected.");
                return;
            }

            ShowDateSlots();
        }

        private bool IsSearchInputValid()
        {
            StartDate = StartDatePicker.SelectedDate.Value;
            EndDate = EndDatePicker.SelectedDate.Value;

            if (ActualReservationDays < 0)
            {
                MessageBox.Show($"Invalid number of reservation days. Try again.");
                return false;
            }

            if (ActualReservationDays < Accommodation.MinDaysReservation)
            {
                MessageBox.Show(
                    $"Number of reservation days couldn't be less than minimal days of reservation which is: {Accommodation.MinDaysReservation}. Try again.");
                return false;
            }

            if (StartDate > EndDate)
            {
                MessageBox.Show($"Start date is greater than end date. Try again.");
                return false;
            }

            if (StartDate.AddDays(ActualReservationDays) > EndDate)
            {
                MessageBox.Show($"Number of reservation days couldn't be more than selected dates. Try again.");
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
                    SuggestedDatesMessage.Content = "All our appointments are reserved. We have offered you the first available period.";
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
            if (GuestsNumber > Accommodation.MaxGuests)
            {
                MessageBox.Show($"The number of guests couldn't be more than the maximum number of guests: {Accommodation.MaxGuests}. Try again.");
                return;
            }

            if (ReservationStartDatePicker.SelectedDate != null && ReservationEndDatePicker.SelectedDate != null)
            {
                if (!IsCreateReservationInputValid()) return;

                // the user can choose only within offered dates
                if (!IsUsedOfferedDate()) return;

                reservationService.Save(new ShowReservationDTO(Accommodation, StartDate, EndDate, GuestsNumber));
                MessageBox.Show($"A reservation has been successfully created.");
            }
            else
            {
                MessageBox.Show($"You need to select start and end date. Try again.");
            }
        }

        private bool IsCreateReservationInputValid()
        {
            StartDate = ReservationStartDatePicker.SelectedDate.Value;
            EndDate = ReservationEndDatePicker.SelectedDate.Value;

            if ((FreeDateSlots == null) || (FreeDateSlots.Count == 0))
            {
                MessageBox.Show($"Firstly you need to look up available dates. Try again.");
                return false;
            }

            if (StartDate > EndDate)
            {
                MessageBox.Show($"Start date is greater than end date. Try again.");
                return false;
            }

            if (StartDate.AddDays(CalendarReservationDays) != EndDate)
            {
                MessageBox.Show( $"Invalid input: The start date and end date are not suitable for the number of reservation days. Try again.");
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

            MessageBox.Show($"You can only select available dates from the list above.");
            return false;
        }

        private void GoToGuest1Start(object sender, RoutedEventArgs e)
        {
            Guest1Start window = new Guest1Start(Guest1);
            window.Show();
            Close();
        }
        private void GoToSearchAndShowAccommodations(object sender, RoutedEventArgs e)
        {
            SearchAndShowAccommodations window = new SearchAndShowAccommodations(Guest1);
            window.Show();
            Close();
        }

        private void GoToCreateReview(object sender, RoutedEventArgs e)
        {
            CreateReview window = new CreateReview(Guest1);
            window.Show();
            Close();
        }

        private void GoToShowReservations(object sender, RoutedEventArgs e)
        {
            ShowReservations window = new ShowReservations(Guest1);
            window.Show();
            Close();
        }

        private void GoToGuest1Requests(object sender, RoutedEventArgs e)
        {
            Guest1Requests window = new Guest1Requests(Guest1);
            window.Show();
            Close();
        }


        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            LoginForm window = new LoginForm();
            window.Show();
            Close();
        }
    }
}
