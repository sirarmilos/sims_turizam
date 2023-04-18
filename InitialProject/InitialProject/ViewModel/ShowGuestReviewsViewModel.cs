using InitialProject.DTO;
using InitialProject.Service;
using InitialProject.View;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.ViewModel
{
    public class ShowGuestReviewsViewModel : Window
    {
        private readonly ReviewService reviewService;

        public string OwnerUsername
        {
            get;
            set;
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

        public DelegateCommand GoToAddNewAccommodationCommand
        {
            get;
        }

        public DelegateCommand GoToRateGuestsCommand
        {
            get;
        }

        public DelegateCommand GoToShowGuestReviewsCommand
        {
            get;
        }

        public DelegateCommand GoToShowOwnerManageBookingMoveRequestsCommand 
        {
            get;
        }

        public DelegateCommand GoToLogoutCommand 
        {
            get;
        }

        public ShowGuestReviewsViewModel(string ownerUsername, string ownerHeader)
        {
            GoToAddNewAccommodationCommand = new DelegateCommand(GoToAddNewAccommodation);
            GoToRateGuestsCommand = new DelegateCommand(GoToRateGuests);
            GoToShowGuestReviewsCommand = new DelegateCommand(GoToShowGuestReviews);
            GoToShowOwnerManageBookingMoveRequestsCommand = new DelegateCommand(GoToShowOwnerManageBookingMoveRequests);
            GoToLogoutCommand = new DelegateCommand(GoToLogout);

            OwnerUsername = ownerUsername;

            reviewService = new ReviewService(OwnerUsername);

            ShowGuestReviewsDTOs = new List<ShowGuestReviewsDTO>();

            ShowGuestReviewsDTOs = reviewService.FindAllReviews();

            SetMenu(ownerHeader);
        }

        private void SetMenu(string ownerHeader)
        {
            UsernameAndSuperOwner = ownerHeader;

            RateGuestsNotifications = "Number of unrated guests: " + reviewService.FindNumberOfUnratedGuests(OwnerUsername) + ".";
        }

        public void GoToAddNewAccommodation()
        {
            AddNewAccommodation window = new AddNewAccommodation(OwnerUsername);
            window.ShowDialog();
        }

        private void GoToRateGuests()
        {
            RateGuests window = new RateGuests(OwnerUsername, UsernameAndSuperOwner);
            window.Show();
            Close();
        }

        private void GoToShowGuestReviews()
        {
            ShowGuestReviews window = new ShowGuestReviews(OwnerUsername, UsernameAndSuperOwner);
            window.Show();
            Close();
        }

        private void GoToShowOwnerManageBookingMoveRequests()
        {
            OwnerManageBookingMoveRequests window = new OwnerManageBookingMoveRequests(OwnerUsername, UsernameAndSuperOwner);
            window.Show();
            Close();
        }

        private void GoToLogout()
        {
            LoginForm window = new LoginForm();
            window.Show();
            Close();
        }
    }
}
