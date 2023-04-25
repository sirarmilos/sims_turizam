using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
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

        public string AccommodationName
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

            SetDefaultValue();
        }

        private void SetDefaultValue()
        {
            AvailableDateSlots = new List<DateSlot>();
            SelectedDateSlot = null;

            buttonRenovate.IsEnabled = false;
            tbDescription.IsEnabled = false;

            tbAccommodationName.Text = string.Empty;
            dpStartDate.SelectedDate = null;
            dpEndDate.SelectedDate = null;
            dgFreeDates.Items.Refresh();
            dgFreeDates.ItemsSource = AvailableDateSlots;
            tbDuration.Text = string.Empty;
            tbDescription.Text = string.Empty;
        }

        private void Renovate(object sender, RoutedEventArgs e)
        {
            Renovation renovation = new Renovation(renovationService.FindNextId(), renovationService.FindAccommodationByAccommodationName(AccommodationName), SelectedDateSlot.StartDate, SelectedDateSlot.EndDate, Description);
            renovationService.AddRenovation(renovation);

            SetDefaultValue();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void FindAvailableDates(object sender, RoutedEventArgs e)
        {
            if(CheckErrorAllFieldsFilled() == true)
            {
                MessageBox.Show("You must fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(renovationService.IsAccommodationNameExist(AccommodationName) == false)
            {
                MessageBox.Show("Accommodation with this name not exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                tbAccommodationName.Text = string.Empty;
                tbAccommodationName.Focus();
            }
            else if(CheckErrorDate() == true)
            {
                MessageBox.Show("Start date is greater than end date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                dpStartDate.SelectedDate = null;
                dpEndDate.SelectedDate = null;
                dpStartDate.Focus();
            }
            else if(CheckFutureDate() == true)
            {
                MessageBox.Show("The start date must not be earlier than today's date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                dpStartDate.SelectedDate = null;
                dpStartDate.Focus();
            }
            else if(CheckErrorDuration() == true)
            {
                MessageBox.Show("The duration must not be longer than the distance between the dates.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                tbDuration.Text = string.Empty;
                tbDuration.Focus();
            }
            else
            {
                AvailableDateSlots = renovationService.FindAvailableDateSlotsToRenovation(AccommodationName, StartDate, EndDate, Duration);

                dgFreeDates.Items.Refresh();

                dgFreeDates.ItemsSource = AvailableDateSlots;
            }
        }

        private bool CheckErrorAllFieldsFilled()
        {
            return string.IsNullOrEmpty(AccommodationName) || dpStartDate.SelectedDate == null || dpEndDate.SelectedDate == null || Duration <= 0;
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
    }
}
