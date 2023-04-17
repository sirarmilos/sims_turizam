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

        public ShowGuestReviewsViewModel(string owner, string ownerHeader)
        {
            GoToAddNewAccommodationCommand = new DelegateCommand(GoToAddNewAccommodation);
            GoToRateGuestsCommand = new DelegateCommand(GoToRateGuests);
            GoToShowGuestReviewsCommand = new DelegateCommand(GoToShowGuestReviews);
            GoToShowOwnerManageBookingMoveRequestsCommand = new DelegateCommand(GoToShowOwnerManageBookingMoveRequests);
            GoToLogoutCommand = new DelegateCommand(GoToLogout);

            Owner = owner;

            reviewService = new ReviewService(Owner);

            ShowGuestReviewsDTOs = new List<ShowGuestReviewsDTO>();

            ShowGuestReviewsDTOs = reviewService.FindAllReviews();

            UsernameAndSuperOwner = ownerHeader;

            RateGuestsNotifications = "Number of unrated guests: " + reviewService.FindNumberOfUnratedGuests(Owner);
        }

        public void GoToAddNewAccommodation()
        {
            AddNewAccommodation window = new AddNewAccommodation("Owner1");
            window.ShowDialog();
        }

        private void GoToRateGuests()
        {
            RateGuests window = new RateGuests("Owner1", UsernameAndSuperOwner);
            window.Show();
            Close();
        }

        private void GoToShowGuestReviews()
        {
            ShowGuestReviews window = new ShowGuestReviews("Owner1", UsernameAndSuperOwner);
            window.Show();
            Close();
        }

        private void GoToShowOwnerManageBookingMoveRequests()
        {
            OwnerManageBookingMoveRequests window = new OwnerManageBookingMoveRequests("Owner1", UsernameAndSuperOwner);
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
