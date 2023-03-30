﻿using InitialProject.Model;
using InitialProject.Repository;
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
        private Accommodation accommodation;
        private Location location;
        private readonly ReservationRepository reservationRepository;
        private int calendarReservationDays;
        private int guestsNumber;
        private List<DateSlot> freeDateSlots;
        private DateTime startDate;
        private DateTime endDate;
        private int actualReservationDays;

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

        public AccommodationReservation(Accommodation accommodation)
        {
            InitializeComponent();
            DataContext = this;
            Accommodation = accommodation;
            Location = accommodation.Location;
            reservationRepository = new ReservationRepository();
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

            List<Reservation> accommodationReservations = reservationRepository.FindAllByAccommodation(Accommodation.Id);

            if (accommodationReservations.Count > 0)
            {
                accommodationReservations.Sort((r1, r2) => r1.StartDate.CompareTo(r2.StartDate));
            }
            else
            {
                if (StartDate.AddDays(CalendarReservationDays) <= EndDate)
                {
                    DateSlot temporary = new DateSlot(startDate, endDate);
                    FreeDateSlots.Add(temporary);
                    return FreeDateSlots;
                }

                return null;
            }

            if (IsDateSlotAfterReservations(accommodationReservations)) return FreeDateSlots;

            if (IsDateSlotBeforeReservations(accommodationReservations)) return FreeDateSlots;

            FindDateSlotsAmongReservations(accommodationReservations);

            return FreeDateSlots;
        }

        private void FindDateSlotsAmongReservations(List<Reservation> accommodationReservations)
        {
            DateSlot dateSlot;

            foreach (Reservation reservation in accommodationReservations)
            {
                bool isFoundGap = (StartDate.AddDays(CalendarReservationDays) < reservation.StartDate) &&
                                  (StartDate.AddDays(CalendarReservationDays) <= EndDate);
                if (isFoundGap)
                {
                    if (accommodationReservations.Last() == reservation)
                    {
                        AddDateSlotsWithinLastReservation(reservation);

                        return;
                    }
                    
                    if (reservation.StartDate > EndDate)
                    {
                        dateSlot = new DateSlot(StartDate, EndDate); 
                    }
                    else
                    {
                        dateSlot = new DateSlot(StartDate, reservation.StartDate.AddDays(-1)); 
                    }
                    
                    FreeDateSlots.Add(dateSlot);
                }

                else
                {
                    bool isFoundGapAfterLastReservation = (accommodationReservations.Last() == reservation) &&
                            (reservation.EndDate.AddDays(CalendarReservationDays + 1) <= EndDate);
                    if (isFoundGapAfterLastReservation)
                    {
                        dateSlot = new DateSlot(reservation.EndDate.AddDays(1), EndDate);
                        FreeDateSlots.Add(dateSlot);
                        return;
                    }
                }


                bool isStartDateOverlappedByReservation = reservation.EndDate.AddDays(1) >= StartDate;
                if (isStartDateOverlappedByReservation)
                {
                    StartDate = reservation.EndDate.AddDays(1);
                }
            }
        }

        private void AddDateSlotsWithinLastReservation(Reservation reservation)
        {
            DateSlot dateSlot;
            if (reservation.EndDate.AddDays(CalendarReservationDays) <= EndDate)
            {
                dateSlot = new DateSlot(StartDate,
                    reservation.StartDate.AddDays(-1)); 
                FreeDateSlots.Add(dateSlot);
                dateSlot = new DateSlot(reservation.EndDate.AddDays(1), EndDate); 
                FreeDateSlots.Add(dateSlot);
            }
            else
            {
                dateSlot = new DateSlot(StartDate, EndDate); 
                FreeDateSlots.Add(dateSlot);
            }
        }

        private bool IsDateSlotBeforeReservations(List<Reservation> accommodationReservations)
        {
            if (accommodationReservations.First().StartDate > EndDate)
            {
                if (StartDate.AddDays(CalendarReservationDays) <= EndDate)
                {
                    DateSlot temporary = new DateSlot(startDate, endDate);
                    FreeDateSlots.Add(temporary);
                    return true;
                }
            }

            return false;
        }

        private bool IsDateSlotAfterReservations(List<Reservation> accommodationReservations)
        {
            if (accommodationReservations.Last().EndDate < StartDate)
            {
                if (StartDate.AddDays(CalendarReservationDays) <= EndDate)
                {
                    DateSlot temporary = new DateSlot(startDate, endDate);
                    FreeDateSlots.Add(temporary);
                    return true;
                }
            }

            return false;
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

                reservationRepository.Save("username", Accommodation, StartDate, EndDate, GuestsNumber);
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
    }
}