using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for SchedulingSelectedRenovation.xaml
    /// </summary>
    public partial class SchedulingSelectedRenovation : Window
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

        public string DurationCheck
        {
            get;
            set;
        }

        public SchedulingSelectedRenovation(string ownerUsername, string accommodationName)
        {
            InitializeComponent();

            OwnerUsername = ownerUsername;
            AccommodationName = accommodationName;

            DataContext = this;

            renovationService = new RenovationService(OwnerUsername);

            SetDefaultValue();

            labelErrorDate.Visibility = Visibility.Hidden;
            textBlockErrorDuration0.Visibility = Visibility.Hidden;
            textBlockErrorDuration1.Visibility = Visibility.Hidden;
        }

        private void SetDefaultValue()
        {
            AvailableDateSlots = new List<DateSlot>();
            SelectedDateSlot = null;

            buttonRenovate.IsEnabled = false;
            tbDescription.IsEnabled = false;
            dgFreeDates.IsEnabled = false;

            dpStartDate.SelectedDate = DateTime.Now.AddDays(1);
            dpStartDate.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now));

            dpEndDate.SelectedDate = DateTime.Now.AddDays(2);
            dpEndDate.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now));

            dgFreeDates.Items.Refresh();
            dgFreeDates.ItemsSource = AvailableDateSlots;

            tbDuration.Text = string.Empty;
            tbDescription.Text = string.Empty;
        }

        private void RenovateAccommodation_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedDateSlot != null)
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
            Renovation renovation = renovationService.CreateRenovationToAdd(AccommodationName, SelectedDateSlot.StartDate, SelectedDateSlot.EndDate, Description);
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
            if (CheckErrorDuration() == true && labelErrorDate.Visibility == Visibility.Hidden)
            {
                textBlockErrorDuration1.Visibility = Visibility.Visible;
                tbDuration.Focus();
            }
            if (CheckErrorAllFieldsFilled() == true)
            {
                MessageBox.Show("You must fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (labelErrorDate.Visibility == Visibility.Hidden && textBlockErrorDuration0.Visibility == Visibility.Hidden && textBlockErrorDuration1.Visibility == Visibility.Hidden)
            {
                AvailableDateSlots = renovationService.FindAvailableDateSlotsToRenovation(AccommodationName, StartDate, EndDate, Duration);

                dgFreeDates.Items.Refresh();
                dgFreeDates.ItemsSource = AvailableDateSlots;
                dgFreeDates.IsEnabled = true;
            }
        }

        private bool CheckErrorAllFieldsFilled()
        {
            return dpStartDate.SelectedDate == null || dpEndDate.SelectedDate == null || string.IsNullOrEmpty(tbDuration.Text);
        }

        private bool CheckErrorDuration()
        {
            return Duration > EndDate.Subtract(StartDate).Days;
        }

        private void RenovateButtonEnable(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedDateSlot != null)
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

        private void CheckErrorDurationTextChanged(object sender, TextChangedEventArgs e)
        {
            textBlockErrorDuration1.Visibility = Visibility.Hidden;

            if (Duration.Equals(string.Empty) == false)
            {
                int checkOut;

                bool check = int.TryParse(DurationCheck, out checkOut);

                if (check == false || checkOut <= 0)
                {
                    textBlockErrorDuration0.Visibility = Visibility.Visible;
                    tbDuration.Focus();
                }
                else
                {
                    textBlockErrorDuration0.Visibility = Visibility.Hidden;
                    Duration = Convert.ToInt32(DurationCheck.ToString());
                }
            }
        }

        private void EndDateSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CheckErrorDate() == true)
            {
                labelErrorDate.Visibility = Visibility.Visible;
                dpEndDate.Focus();
            }
            else
            {
                labelErrorDate.Visibility = Visibility.Hidden;
            }
        }

        private void StartDateSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CheckErrorDate() == true)
            {
                labelErrorDate.Visibility = Visibility.Visible;
                dpStartDate.Focus();
            }
            else
            {
                labelErrorDate.Visibility = Visibility.Hidden;
            }
        }

        private bool CheckErrorDate()
        {
            if (dpStartDate.SelectedDate == null || dpEndDate.SelectedDate == null)
            {
                return true;
            }

            StartDate = dpStartDate.SelectedDate.Value;
            EndDate = dpEndDate.SelectedDate.Value;

            return DateTime.Compare(StartDate, EndDate) >= 0;
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
