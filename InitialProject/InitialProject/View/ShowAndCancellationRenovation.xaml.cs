using InitialProject.DTO;
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
    /// Interaction logic for ShowAndCancellationRenovation.xaml
    /// </summary>
    public partial class ShowAndCancellationRenovation : Window
    {
        private readonly RenovationService renovationService;

        public string OwnerUsername
        {
            get;
            set;
        }

        public ShowRenovationDTO SelectedRenovation
        {
            get;
            set;
        }

        public List<ShowRenovationDTO> ShowRenovationDTOs
        {
            get;
            set;
        }

        public ShowAndCancellationRenovation(string ownerUsername, string ownerHeader)
        {
            InitializeComponent();

            OwnerUsername = ownerUsername;

            DataContext = this;

            renovationService = new RenovationService(ownerUsername);

            ShowRenovationDTOs = new List<ShowRenovationDTO>();

            ShowRenovationDTOs = renovationService.FindAllRenovations(ownerUsername);

            SetDefaultValue();

            SetMenu(ownerHeader);
        }

        private void SetMenu(string ownerHeader)
        {
            usernameAndSuperOwner.Header = ownerHeader;

            rateGuestsNotifications.Header = "Number of unrated guests: " + renovationService.FindNumberOfUnratedGuests(OwnerUsername) + ".";
        }

        private void SetDefaultValue()
        {
            buttonCancelRenovation.IsEnabled = false;
            SelectedRenovation = null;
        }

        private void RenovateAccommodation(object sender, RoutedEventArgs e)
        {
            SchedulingRenovation window = new SchedulingRenovation(OwnerUsername);

            window.ShowDialog();
        }

        private void CancelRenovation(object sender, RoutedEventArgs e)
        {
            ShowRenovationDTOs = renovationService.RemoveRenovation(SelectedRenovation, OwnerUsername);

            dgRenovations.Items.Refresh();

            dgRenovations.ItemsSource = ShowRenovationDTOs;
        }

        private void CancelRenovationButtonEnable(object sender, SelectionChangedEventArgs e)
        {
            if(SelectedRenovation != null && SelectedRenovation.Status.Equals("Can be cancelled") == true)
            {
                buttonCancelRenovation.IsEnabled = true;
            }
            else
            {
                buttonCancelRenovation.IsEnabled = false;
            }
        }

        private void LoadingRowForDgRenovations(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void GoToAddNewAccommodation(object sender, RoutedEventArgs e)
        {
            AddNewAccommodation window = new AddNewAccommodation(OwnerUsername);
            window.ShowDialog();
        }

        private void GoToRateGuests(object sender, RoutedEventArgs e)
        {
            RateGuests window = new RateGuests(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void GoToShowGuestReviews(object sender, RoutedEventArgs e)
        {
            ShowGuestReviews window = new ShowGuestReviews(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void GoToShowOwnerManageBookingMoveRequests(object sender, RoutedEventArgs e)
        {
            OwnerManageBookingMoveRequests window = new OwnerManageBookingMoveRequests(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            LoginForm window = new LoginForm();
            window.Show();
            Close();
        }

        private void GoToShowAndCancellationRenovation(object sender, RoutedEventArgs e)
        {
            ShowAndCancellationRenovation window = new ShowAndCancellationRenovation(OwnerUsername, usernameAndSuperOwner.Header.ToString());
            window.Show();
            Close();
        }
    }
}
