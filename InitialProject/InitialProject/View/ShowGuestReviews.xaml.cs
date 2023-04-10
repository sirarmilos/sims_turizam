using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for ShowGuestReviews.xaml
    /// </summary>
    public partial class ShowGuestReviews : Window
    {
        private readonly ReviewService reviewService;

        private List<ShowGuestReviewsDTO> showGuestReviewsDTOs;

        private string owner;

        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value;
            }
        }
        public List<ShowGuestReviewsDTO> ShowGuestReviewsDTOs
        {
            get { return showGuestReviewsDTOs; }
            set
            {
                showGuestReviewsDTOs = value;
            }
        }

        public ShowGuestReviews(string owner)
        {
            InitializeComponent();

            Owner = owner;

            DataContext = this;

            reviewService = new ReviewService(Owner);

            ShowGuestReviewsDTOs = new List<ShowGuestReviewsDTO>();

            ShowGuestReviewsDTOs = reviewService.FindAllReviews();

            dgShowGuestReviews.ItemsSource = ShowGuestReviewsDTOs;
        }

        void LoadingRowForDgShowGuestReviews(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void GoToAddNewAccommodation(object sender, RoutedEventArgs e)
        {
            AddNewAccommodation window = new AddNewAccommodation(Owner);
            window.Show();
        }

        private void GoToRateGuests(object sender, RoutedEventArgs e)
        {
            RateGuests window = new RateGuests(Owner);
            if (window.dgRateGuests.Items.Count > 0)
            {
                window.Show();
            }
        }

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            LoginForm window = new LoginForm();
            window.Show();
            Close();
        }

        private void GoToShowGuestReviews(object sender, RoutedEventArgs e)
        {
            ShowGuestReviews window = new ShowGuestReviews(Owner);
            window.Show();
            Close();
        }

        private void GoToShowOwnerManageBookingMoveRequests(object sender, RoutedEventArgs e)
        {
            OwnerManageBookingMoveRequests window = new OwnerManageBookingMoveRequests(Owner);
            window.Show();
            Close();
        }
    }
}
