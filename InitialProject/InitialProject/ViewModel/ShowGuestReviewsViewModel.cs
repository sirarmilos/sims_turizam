﻿using InitialProject.DTO;
using InitialProject.Service;
using InitialProject.View;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
        public List<string> UnreadCancelledReservations
        {
            get;
            set;
        }

        public List<CancelledReservationsNotificationDTO> UnreadCancelledReservationsToDelete
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

        public DelegateCommand GoToOwnerHomePageLoginCommand
        {
            get;
        }

        public DelegateCommand GoToAccommodationStartCommand
        {
            get;
        }

        public DelegateCommand GoToShowOwnerManageBookingMoveRequestsCommand
        {
            get;
        }

        public DelegateCommand GoToShowAndCancellationRenovationCommand
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

        public DelegateCommand GoToOwnerForumCommand
        {
            get;
        }

        public DelegateCommand GoToOwnerHomePageNotLoginCommand
        {
            get;
        }

        public DelegateCommand ReadCancelledReservationNotificationCommand
        {
            get;
        }

        public ShowGuestReviews Form
        {
            get;
            set;
        }

        public ShowGuestReviewsViewModel(ShowGuestReviews form, string ownerUsername, string ownerHeader)
        {
            GoToOwnerHomePageLoginCommand = new DelegateCommand(GoToOwnerHomePageLogin);
            GoToAccommodationStartCommand = new DelegateCommand(GoToAccommodationStart);
            GoToShowOwnerManageBookingMoveRequestsCommand = new DelegateCommand(GoToShowOwnerManageBookingMoveRequests);
            GoToShowAndCancellationRenovationCommand = new DelegateCommand(GoToShowAndCancellationRenovation);
            GoToRateGuestsCommand = new DelegateCommand(GoToRateGuests);
            GoToShowGuestReviewsCommand = new DelegateCommand(GoToShowGuestReviews);
            GoToOwnerForumCommand = new DelegateCommand(GoToOwnerForum);
            GoToOwnerHomePageNotLoginCommand = new DelegateCommand(GoToOwnerHomePageNotLogin);
            // ReadCancelledReservationNotificationCommand = new DelegateCommand(ReadCancelledReservationNotification);

            OwnerUsername = ownerUsername;

            reviewService = new ReviewService(OwnerUsername);

            ShowGuestReviewsDTOs = new List<ShowGuestReviewsDTO>();

            ShowGuestReviewsDTOs = reviewService.FindAllReviews();

            SetMenu(ownerHeader);

            Form = form;
        }

        private void SetMenu(string ownerHeader)
        {
            UsernameAndSuperOwner = ownerHeader;

            RateGuestsNotifications = "Number of unrated guests: " + reviewService.FindNumberOfUnratedGuests(OwnerUsername) + ".";

            UnreadCancelledReservations = new List<string>();

            UnreadCancelledReservationsToDelete = reviewService.FindUnreadCancelledReservations(OwnerUsername);

            foreach (CancelledReservationsNotificationDTO temporaryCanceledReservationsNotificationDTO in UnreadCancelledReservationsToDelete.ToList())
            {
                UnreadCancelledReservations.Add(temporaryCanceledReservationsNotificationDTO.AccommodationName + ": " + temporaryCanceledReservationsNotificationDTO.ReservationStartDate.ToShortDateString() + " - " + temporaryCanceledReservationsNotificationDTO.ReservationEndDate.ToShortDateString());
            }
        }

        private void GoToOwnerHomePageLogin()
        {
            OwnerHomePageLogin window = new OwnerHomePageLogin(OwnerUsername, UsernameAndSuperOwner);
            window.Show();
            Form.Close();
            // Close();
        }

        private void GoToAccommodationStart()
        {
            AccommodationStart window = new AccommodationStart(OwnerUsername);
            window.Show();
            Form.Close();
            // Close();
        }

        private void GoToShowOwnerManageBookingMoveRequests()
        {
            OwnerManageBookingMoveRequests window = new OwnerManageBookingMoveRequests(OwnerUsername, UsernameAndSuperOwner);
            window.Show();
            Form.Close();
            // Close();
        }

        private void GoToShowAndCancellationRenovation()
        {
            ShowAndCancellationRenovation window = new ShowAndCancellationRenovation(OwnerUsername, UsernameAndSuperOwner);
            window.Show();
            Form.Close();
            // Close();
        }

        private void GoToRateGuests()
        {
            RateGuests window = new RateGuests(OwnerUsername, UsernameAndSuperOwner);
            window.Show();
            Form.Close();
            // Close();
        }

        private void GoToShowGuestReviews()
        {
            ShowGuestReviews window = new ShowGuestReviews(OwnerUsername, UsernameAndSuperOwner);
            window.Show();
            Form.Close();
            // Close();
        }

        private void GoToOwnerForum()
        {
            OwnerForum window = new OwnerForum(OwnerUsername, UsernameAndSuperOwner);
            window.Show();
            Form.Close();
            // Close();
        }

        private void GoToOwnerHomePageNotLogin()
        {
            if (GlobalOwnerClass.NotificationRead == true)
            {
                reviewService.MarkAsReadNotificationsCancelledReservations(UnreadCancelledReservationsToDelete);
            }

            LoginForm window = new LoginForm();
            window.Show();
            Form.Close();
            // Close();
        }
    }
}
