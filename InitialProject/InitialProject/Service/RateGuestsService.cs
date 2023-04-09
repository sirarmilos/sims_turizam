using InitialProject.DTO;
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

namespace InitialProject.Service
{
    public class RateGuestsService
    {
        private readonly RateGuestRepository rateGuestRepository;

        private readonly ReservationRepository reservationRepository;

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
            rateGuestRepository = new RateGuestRepository();
            reservationRepository = new ReservationRepository();
        }

        public List<RateGuestsDTO> FindAllGuestsForRate()
        {
            List<Reservation> allReservations = reservationRepository.FindAllReservations();

            List<Reservation> ownerReservations = FindOwnerReservations(allReservations);

            List<RateGuest> allRateGuests = rateGuestRepository.FindAllRateGuests();

            allRateGuests = FindReservationsForRateGuests(allRateGuests, allReservations);

            List<RateGuest> ownerRateGuests = FindOwnerRateGuests(allRateGuests);

            return FindRateGuestsDTOs(ownerReservations, ownerRateGuests);
        }

        public List<Reservation> FindOwnerReservations(List<Reservation> allReservations)
        {
            return allReservations.ToList().FindAll(x => x.Accommodation.OwnerUsername.Equals(Owner) == true);
        }

        public List<RateGuest> FindOwnerRateGuests(List<RateGuest> allRateGuests)
        {
            return allRateGuests.ToList().FindAll(x => x.Reservation.Accommodation.OwnerUsername.Equals(Owner) == true);
        }

        public List<RateGuest> FindReservationsForRateGuests(List<RateGuest> allRateGuests, List<Reservation> allReservations)
        {
            foreach (RateGuest temporaryRateGuest in allRateGuests.ToList())
            {
                temporaryRateGuest.Reservation = allReservations.ToList().Find(x => x.ReservationId == temporaryRateGuest.Reservation.ReservationId);
            }

            return allRateGuests;
        }

        public List<RateGuestsDTO> FindRateGuestsDTOs(List<Reservation> ownerReservations, List<RateGuest> ownerRateGuests)
        {
            List<RateGuestsDTO> rateGuestsDTOs = new List<RateGuestsDTO>();

            foreach (Reservation temporaryReservation in ownerReservations.ToList())
            {
                RateGuest temporaryRateGuest = ownerRateGuests.Find(x => x.Reservation.ReservationId == temporaryReservation.ReservationId);

                if(temporaryRateGuest == null)
                {
                    RateGuestsDTO rateGuestDTO = IsValidToAdd(temporaryReservation, temporaryRateGuest);
                    if (rateGuestDTO != null)
                    {
                        rateGuestsDTOs.Add(rateGuestDTO);
                    }
                }
            }

            return rateGuestsDTOs;
        }

        public RateGuestsDTO IsValidToAdd(Reservation temporaryReservation, RateGuest temporaryRateGuest)
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
            RateGuest rateGuest = new RateGuest(FindReservationByReservationId(saveNewRateGuestDTO.ReservationId), saveNewRateGuestDTO);

            List<RateGuest> allRateGuests = rateGuestRepository.FindAllRateGuests();

            allRateGuests.Add(rateGuest);

            rateGuestRepository.SaveRateGuests(allRateGuests);
        }

        private Reservation FindReservationByReservationId(int reservationId)
        {
            List<Reservation> allReservations = reservationRepository.FindAllReservations();

            return allReservations.Find(x => x.ReservationId == reservationId);
        }
    }
}
