using InitialProject.DTO;
using InitialProject.Service;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.ViewModel
{
    public class ShowGuestReviewsViewModel
    {
        private readonly ReviewService reviewService;

        private string owner;

        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value;
            }
        }

        public string UsernameAndSuperOwner
        {
            get;
            set;
        }

        public string RateGuestsNotifications
        {
            get;
            set;
        }

        public List<ShowGuestReviewsDTO> ShowGuestReviewsDTOs
        {
            get;
            set;
        }

        public ShowGuestReviewsViewModel(string owner, string ownerHeader)
        {
            Owner = owner;

            reviewService = new ReviewService(Owner);

            ShowGuestReviewsDTOs = new List<ShowGuestReviewsDTO>();

            ShowGuestReviewsDTOs = reviewService.FindAllReviews();

            // usernameAndSuperOwner.Header = ownerHeader;

            UsernameAndSuperOwner = ownerHeader;

            RateGuestsNotifications = "Number of unrated guests: " + reviewService.FindNumberOfUnratedGuests(Owner);

            // rateGuestsNotifications.Header = "Number of unrated guests: " + reviewService.FindNumberOfUnratedGuests(Owner);
        }

        public void GoToAddNewAccommodation(object sender, RoutedEventArgs e)
        {
            AddNewAccommodation window = new AddNewAccommodation("Owner1");
            window.ShowDialog();
        }

        private void GoToRateGuests(object sender, RoutedEventArgs e)
        {
            RateGuests window = new RateGuests("Owner1", UsernameAndSuperOwner);
            window.Show();
            // Close();
        }

        private void GoToShowGuestReviews(object sender, RoutedEventArgs e)
        {
            ShowGuestReviews window = new ShowGuestReviews("Owner1", UsernameAndSuperOwner);
            window.Show();
            // Close();
        }

        private void GoToShowOwnerManageBookingMoveRequests(object sender, RoutedEventArgs e)
        {
            OwnerManageBookingMoveRequests window = new OwnerManageBookingMoveRequests("Owner1", UsernameAndSuperOwner);
            window.Show();
            // Close();
        }

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            LoginForm window = new LoginForm();
            window.Show();
            // Close();
        }
    }
}
