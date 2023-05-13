using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for SchedulingRenovation.xaml
    /// </summary>
    public partial class SchedulingRenovation : Window
    {
        private readonly RenovationService renovationService;

        public string OwnerUsername
        {
            get;
            set;
        }

        public List<string> AccommodationNames
        {
            get;
            set;
        }

        public string SelectedAccommodationName
        {
            get;
            set;
        }

        public DateTime StartDate
        {
            get;
            set;
        }

        public DateTime EndDate
        {
            get;
            set;
        }

        public int Duration
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public List<DateSlot> AvailableDateSlots
        {
            get;
            set;
        }

        public DateSlot SelectedDateSlot
        {
            get;
            set;
        }

        public SchedulingRenovation(string ownerUsername)
        {
            InitializeComponent();

            OwnerUsername = ownerUsername;

            DataContext = this;

            renovationService = new RenovationService(OwnerUsername);

            SetComboBox();

            SetDefaultValue();

            labelErrorEndDate.Visibility = Visibility.Hidden;
        }

        public void SetComboBox()
        {
            AccommodationNames = new List<string>();
            AccommodationNames = renovationService.FindOwnerAccommodations(OwnerUsername);
            cbAccommodationNames.SelectedItem = null;
            SelectedAccommodationName = null;

            cbAccommodationNames.Focus();
        }

        private void SetDefaultValue()
        {
            AvailableDateSlots = new List<DateSlot>();
            SelectedDateSlot = null;

            buttonRenovate.IsEnabled = false;
            tbDescription.IsEnabled = false;
            dgFreeDates.IsEnabled = false;

            dpStartDate.SelectedDate = DateTime.Now.AddDays(1);
            dpEndDate.SelectedDate = null;
            dgFreeDates.Items.Refresh();
            dgFreeDates.ItemsSource = AvailableDateSlots;
            tbDuration.Text = string.Empty;
            tbDescription.Text = string.Empty;

            dpStartDate.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now));
            dpEndDate.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now));
        }

        private void RenovateAccommodation_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(SelectedDateSlot != null)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void RenovateAccommodation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Renovation renovation = new Renovation(renovationService.FindNextId(), renovationService.FindAccommodationByAccommodationName(SelectedAccommodationName), SelectedDateSlot.StartDate, SelectedDateSlot.EndDate, Description);
            renovationService.AddRenovation(renovation);

            Close();
        }

        private void Cancel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Cancel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void FindAvailableDates_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void FindAvailableDates_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (CheckErrorAllFieldsFilled() == true)
            {
                MessageBox.Show("You must fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(labelErrorEndDate.Visibility == Visibility.Visible)
            {
                MessageBox.Show("You must change dates", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(CheckErrorDuration() == true)
            {
                MessageBox.Show("The duration must not be longer than the distance between the dates.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                tbDuration.Text = string.Empty;
                tbDuration.Focus();
            }
            else if(labelErrorEndDate.Visibility == Visibility.Hidden)
            {
                AvailableDateSlots = renovationService.FindAvailableDateSlotsToRenovation(SelectedAccommodationName, StartDate, EndDate, Duration);

                dgFreeDates.Items.Refresh();
                dgFreeDates.ItemsSource = AvailableDateSlots;
                dgFreeDates.IsEnabled = true;
            }
        }

        private bool CheckErrorAllFieldsFilled()
        {
            return string.IsNullOrEmpty(SelectedAccommodationName) || dpStartDate.SelectedDate == null || dpEndDate.SelectedDate == null || Duration <= 0;
        }

        private bool CheckErrorDate()
        {
            StartDate = dpStartDate.SelectedDate.Value;
            EndDate = dpEndDate.SelectedDate.Value;

            return DateTime.Compare(StartDate, EndDate) >= 0;
        }

        private bool CheckFutureDate()
        {
            return DateTime.Compare(StartDate, DateTime.Now) <= 0;
        }

        private bool CheckErrorDuration()
        {
            return Duration > EndDate.Subtract(StartDate).Days;
        }

        private void RenovateButtonEnable(object sender, SelectionChangedEventArgs e)
        {
            if(SelectedDateSlot != null)
            {
                buttonRenovate.IsEnabled = true;
                tbDescription.IsEnabled = true;
            }
            else
            {
                buttonRenovate.IsEnabled = false;
                tbDescription.IsEnabled = false;
            }
        }

        private void LoadingRowForDgFreeDates(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void labeltbFocus(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                var label = (Label)sender;
                Keyboard.Focus(label.Target);
            }
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dgFreeDates.IsEnabled == true)
            {
                SetDefaultValue();
            }
        }

        private void dpEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(CheckErrorDate() == true)
            {
                labelErrorEndDate.Visibility = Visibility.Visible;
                dpEndDate.Focus();
            }
            else
            {
                labelErrorEndDate.Visibility = Visibility.Hidden;
            }
        }
    }
}
