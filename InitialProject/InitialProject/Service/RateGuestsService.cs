﻿using InitialProject.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;
using System.Windows;
using System.Xml.Linq;
using InitialProject.IRepository;
using InitialProject.Injector;

namespace InitialProject.Service
{
    public class RateGuestsService
    {
        private readonly IRateGuestRepository rateGuestRepository;

        private readonly ReservationService reservationService;

        private readonly UserService userService;

        private readonly CanceledReservationService canceledReservationService;

        private readonly ForumNotificationsToOwnerService forumNotificationsToOwnerService;

        private string owner;

        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value;
            }
        }

        public RateGuestsService(string owner)
        {
            Owner = owner;
            rateGuestRepository = Injector.Injector.CreateInstance<IRateGuestRepository>();
            //rateGuestRepository = new RateGuestRepository();
            reservationService = new ReservationService(Owner);
            userService = new UserService();
            canceledReservationService = new CanceledReservationService();
            forumNotificationsToOwnerService = new ForumNotificationsToOwnerService();
        }

        public List<RateGuestsDTO> FindAllGuestsToRate()
        {
            return FindRateGuestsDTOs(reservationService.FindOwnerReservations());
        }

        public List<RateGuestsDTO> FindRateGuestsDTOs(List<Reservation> ownerReservations)
        {
            List<RateGuestsDTO> rateGuestsDTOs = new List<RateGuestsDTO>();

            foreach (Reservation temporaryReservation in ownerReservations.ToList())
            {
                RateGuest temporaryRateGuest = rateGuestRepository.FindOwnerRateGuestByReservationId(Owner, temporaryReservation.ReservationId);

                if(temporaryRateGuest == null)
                {
                    RateGuestsDTO rateGuestDTO = IsValidToAdd(temporaryReservation);
                    if (rateGuestDTO != null)
                    {
                        rateGuestsDTOs.Add(rateGuestDTO);
                    }
                }
            }

            return rateGuestsDTOs;
        }

        public RateGuestsDTO IsValidToAdd(Reservation temporaryReservation)
        {
            int days = DateTime.Now.Subtract(temporaryReservation.EndDate).Days;

            if (days <= 5 && days >= 0)
            {
                string deadline = "This is the last day to rate a guest.";

                if (5 - days > 0)
                {
                    deadline = "You have " + (5 - days) + " more days to rate the guest.";
                }

                return new RateGuestsDTO(temporaryReservation.ReservationId, temporaryReservation.GuestUsername, deadline);
            }

            return null;
        }

        public void SaveNewRateGuest(SaveNewRateGuestDTO saveNewRateGuestDTO)
        {
            rateGuestRepository.Add(new RateGuest(reservationService.FindById(saveNewRateGuestDTO.ReservationId), saveNewRateGuestDTO));
        }

        public List<RateGuest> FindOwnerRateGuests(string ownerUsername)
        {
            return rateGuestRepository.FindByOwnerUsername(ownerUsername);
        }

        public int FindNumberOfUnratedGuests(string ownerUsername)
        {
            return FindAllGuestsToRate().Count;
        }

        public List<CancelledReservationsNotificationDTO> FindUnreadCancelledReservations(string ownerUsername)
        {
            return userService.FindUnreadCancelledReservations(ownerUsername);
        }




        public RateGuest FindRateGuestByReservation(int reservationid)
        {
            return rateGuestRepository.FindRateGuestByReservation(reservationid);
        }

        public void MarkAsReadNotificationsCancelledReservations(List<CancelledReservationsNotificationDTO> unreadCancelledReservations)
        {
            canceledReservationService.MarkAsReadNotificationsCancelledReservations(unreadCancelledReservations);
        }

        public int FindNumberOfNewForums(string ownerUsername)
        {
            return forumNotificationsToOwnerService.FindNumberOfNewForums(ownerUsername);
        }

        public void MarkAsReadNotificationsForums(string ownerUsername)
        {
            forumNotificationsToOwnerService.MarkAsReadNotificationsForums(ownerUsername);
        }
    }
}
